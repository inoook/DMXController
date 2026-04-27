using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMXControllerTest : MonoBehaviour
{
    [SerializeField] DMXControllerTask dmxController = null;
    [SerializeField] byte[] dmxBytes = null;

    [ContextMenu("Test_SetDMX")]
    private void Test_SetDMX()
    {
        dmxController.SetData(dmxBytes);
    }

}
