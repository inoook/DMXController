using UnityEngine;
using System.IO.Ports;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;
using System.Threading;

// USB to DMXコントロールケーブルアダプタードングルインターフェースDMX512 FTDIドライバーRS485 3ピンXLRシリアルコンバータケーブル
// https://www.amazon.co.jp/gp/product/B07L7PZKR8/ref=ppx_yo_dt_b_asin_title_o00_s00?ie=UTF8&th=1
// を使用したDMX制御
public class DMXControllerTask : MonoBehaviour
{
    [System.Serializable]
    public class Setting
    {
        public string port = "";
        public override string ToString()
        {
            return $"port: {port}";
        }

        public void DrawGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("port: ");
            port = GUILayout.TextField(port);
            GUILayout.EndHorizontal();
        }
    }

    private SerialPort serial;
    // 送信byteが多いと、フレームレートが落ちる
    byte[] dmxData = new byte[513];
    //byte[] dmxData = new byte[5];

    [SerializeField] Setting setting = default;


    CancellationTokenSource loopCancellationTokenSource;

    internal void ApplySetting(Setting dmxSetting)
    {
        setting = dmxSetting;
    }
    internal Setting GetSetting()
    {
        return setting;
    }

    void Start()
    {
        try
        {
            Debug.LogWarning($"serial.port: {setting.port}");
            serial = new SerialPort(setting.port, 250000);
            serial.DataBits = 8;
            serial.Parity = Parity.None;
            serial.StopBits = StopBits.Two;
            serial.Open();

            //
            loopCancellationTokenSource = new CancellationTokenSource();
            _ = Loop(loopCancellationTokenSource);
        }
        catch
        {

        }
    }

    async Task Loop(CancellationTokenSource cancellationTokenSource)
    {
        int rate = Mathf.FloorToInt( 1000 * (1 / 30f) );
        try
        {
            while (true)
            {
                // send -----
                await Task.Delay(rate, cancellationTokenSource.Token);
                //SendDMX();
                await SendAsync(cancellationTokenSource.Token);
            }
        }
        catch (OperationCanceledException e)
        {
            Debug.LogWarning(e);
        }catch(Exception e)
        {
            Debug.LogError(e);
        }
    }

    // dmxDataの配列数に限らず安定して動く
    public async Task SendAsync(CancellationToken cancellationToken = default)
    {
        await Task.Run(() =>
        {
            SendDMX();
        }, cancellationToken);
    }

    // dmxDataの配列数が多いと重くなる
    public void Send()
    {
        SendDMX();
    }

    // https://qiita.com/ossyaritoori/items/53c3dd438d4232515c18
    private void SendDMX()
    {
        if (serial.IsOpen)
        {
            serial.BreakState = true;
            MicroSecDelay(176);// 176usec
            serial.BreakState = false;
            MicroSecDelay(16);// 16usec

            //dmxData[1] = (byte)(value1 * 255f);//ch1に0-255の値を設定
            //dmxData[2] = (byte)(value2 * 255f);//ch2に0-255の値を設定
            //dmxData[3] = (byte)(value3 * 255f);//ch3に0-255の値を設定
            //dmxData[4] = (byte)(value4 * 255f);//ch4に0-255の値を設定
            //dmxData[5] = (byte)(value5 * 255f);//ch4に0-255の値を設定
            serial.Write(dmxData, 0, dmxData.Length);
        }
        else
        {
            Debug.Log("serial is not open");
        }
    }

    private void MicroSecDelay(int time)
    {
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        while (true)
        {
            sw.Stop();
            var elapsed = sw.ElapsedTicks * 1000 * 1000 / System.Diagnostics.Stopwatch.Frequency;
            if (elapsed > time)
            {
                break;
            }
            else
            {
                sw.Start();
            }
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < dmxData.Length; i++)
        {
            dmxData[i] = 0;
        }
        SendDMX();

        serial?.Close();
        serial?.Dispose();

        loopCancellationTokenSource?.Cancel();
    }

    public void SetData(int ch, float value)
    {
        dmxData[ch] = (byte)(value * 255f);//ch1に0-255の値を設定
    }
    public void SetData(int ch, byte value)
    {
        dmxData[ch] = value;
    }
    public void SetData(byte[] values)
    {
        for(int i = 0; i < values.Length; i++)
        {
            dmxData[i+1] = values[i];
        }
    }


    [ContextMenu("Cancel")]
    void Cancel()
    {
        loopCancellationTokenSource?.Cancel();
    }


    internal void DrawGUI()
    {
        GUILayout.Label(this.gameObject.name);
        GUILayout.Label(setting.ToString());
        GUILayout.Label($"IsOpen: {serial.IsOpen}");
    }
}