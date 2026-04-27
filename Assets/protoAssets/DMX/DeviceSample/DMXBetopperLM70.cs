using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMXBetopperLM70 : MonoBehaviour
{
    [SerializeField] DMXControllerTask dmxController = null;

    [SerializeField] Color32 color = Color.white;

    //# 9 Channels Mode
    [SerializeField, Range(0, 255)] int xAxis = 0;
    [SerializeField, Range(0, 255)] int yAxis = 0;
    //# 3 Master Control
    //0-8 Light off
    //8-135 Dimming
    //136-240 Strobe
    //241-255 Power ON/OFF
    [SerializeField, Range(0, 255)] int masterControl = 1;
    [SerializeField, Range(0, 255)] int rLed = 0;
    [SerializeField, Range(0, 255)] int gLed = 0;
    [SerializeField, Range(0, 255)] int bLed = 0;
    [SerializeField, Range(0, 255)] int wLed = 0;
    [SerializeField, Range(0, 255)] int speed = 0;//XY Motor Speed

    [Header("Anim")]
    [SerializeField, Range(0, 1)] float h = 0;
    [SerializeField] float time = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    [SerializeField] bool isAnim = true;

    // Update is called once per frame
    void Update()
    {
        if (isAnim)
        {
            h += Time.deltaTime * (1 / time);
            h = Mathf.Repeat(h, 1);

            color = Color.HSVToRGB(h, 1, 1);
            rLed = color.r;
            gLed = color.g;
            bLed = color.b;
            wLed = color.a;
        }
        //
        dmxController.SetData(1, xAxis);
        dmxController.SetData(2, yAxis);
        dmxController.SetData(3, masterControl);
        dmxController.SetData(4, rLed);
        dmxController.SetData(5, gLed);
        dmxController.SetData(6, bLed);
        dmxController.SetData(7, wLed);
        dmxController.SetData(8, speed);

    }
}
