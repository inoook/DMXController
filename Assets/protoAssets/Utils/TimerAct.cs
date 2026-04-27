using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimerAct
{
    [SerializeField] bool enable = true;
    [Tooltip("Sec この秒数ごとに送信")]
    [SerializeField] float sendRate = 1;

    float time = 0;

    public System.Action SendAct = null;

    public void Enable(bool v)
    {
        enable = v;
        time = 0;
    }

    public void Process(float deltaTime)
    {
        if (!enable) { return; }

        float delta = sendRate;
        time += deltaTime;
        if (time > delta)
        {
            time = time - delta;
            SendAct?.Invoke();
        }
    }
}
