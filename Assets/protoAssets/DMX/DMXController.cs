using UnityEngine;
using System.IO.Ports;
using System.Threading;
using UnityEngine.UI;

// https://note.com/hikohiro/n/n9902fd77293f
public class DMXController : MonoBehaviour
{
    private SerialPort serial;
    byte[] dmxData = new byte[513];
    [SerializeField] string port = "COM1";

    [SerializeField, Range(0, 1)] float value1 = 0;// Brightness
    [SerializeField, Range(0, 1)] float value2 = 0;// R
    [SerializeField, Range(0, 1)] float value3 = 0;// G
    [SerializeField, Range(0, 1)] float value4 = 0;// B
    [SerializeField, Range(0, 1)] float value5 = 0;

    void Start()
    {
        serial = new SerialPort(port, 250000);
        serial.DataBits = 8;
        serial.Parity = Parity.None;
        serial.StopBits = StopBits.Two;
        serial.Open();
    }

    private void SendDMX()
    {
        Debug.LogWarning("Send");
        serial.BreakState = true;
        Thread.Sleep(1);
        serial.BreakState = false;
        Thread.Sleep(1);

        dmxData[1] = (byte)(value1 * 255f);//ch1に0-255の値を設定
        dmxData[2] = (byte)(value2 * 255f);//ch2に0-255の値を設定
        dmxData[3] = (byte)(value3 * 255f);//ch3に0-255の値を設定
        dmxData[4] = (byte)(value4 * 255f);//ch4に0-255の値を設定
        dmxData[5] = (byte)(value5 * 255f);//ch4に0-255の値を設定
        serial.Write(dmxData, 0, dmxData.Length);
    }

    void Update()
    {
        SendDMX();
    }

    private void OnDestroy()
    {
        serial?.Close();
        serial?.Dispose();
    }
}