using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanController : MonoBehaviour
{
    [SerializeField] DMXControllerTask dmxController = null;

    [SerializeField] TimerAct sendTimerAct = default;
    
    [Header("出力補正用のCurve")]
    [SerializeField] AnimationCurve fanValueCurve = default;

    [Header("送風量 (0~1.0f)")]
    [SerializeField, Range(0, 1)] float fanV = 0;

    public float FanValue => fanV;

    /// <summary>
    /// 送風量をセットする (0~1.0f)
    /// </summary>
    /// <param name="v">送風量 (0~1.0f)</param>
    public void SetValue(float v)
    {
        fanV = v;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        sendTimerAct.SendAct = SendDMX;
    }

    void SendDMX()
    {
        float fanOutput = fanValueCurve.Evaluate(fanV);
        // DMXの送信indexは0始まりではない
        dmxController.SetData(1, fanOutput);
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        sendTimerAct.Process(deltaTime);
    }
}
