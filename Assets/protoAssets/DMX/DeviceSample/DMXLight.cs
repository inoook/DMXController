using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMXLight : MonoBehaviour
{
    [SerializeField] DMXControllerTask dmxController = null;

    [SerializeField] Color color = Color.white;

    [SerializeField, Range(0, 1)] float value1 = 0;// Brightness
    //[SerializeField, Range(0, 1)] float value2 = 0;// R
    //[SerializeField, Range(0, 1)] float value3 = 0;// G
    //[SerializeField, Range(0, 1)] float value4 = 0;// B

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
        }

        color = Color.HSVToRGB(h, 1, 1);

        // BETOPPER https://cdn.shopify.com/s/files/1/0084/5230/9047/files/BETOPPER_Mini_LED_Flat_PAR_LIGHT_LP005_Usuer_Manual.pdf?v=1630046245
        //dmxController.SetData(1, value1);
        //dmxController.SetData(2, color.r);
        //dmxController.SetData(3, color.g);
        //dmxController.SetData(4, color.b);

        dmxController.SetData(1, color.r);
        dmxController.SetData(2, color.g);
        dmxController.SetData(3, color.b);
        dmxController.SetData(4, value1);

    }

    // ÉÇÅ[Éh
    // single
    // RGB
    // RGBL
    // LRGB
}
