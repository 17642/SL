using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
    
public class TouchyInterface : MonoBehaviour
{
    [SerializeField]
    bl_Joystick js;

    [SerializeField]
    float inputDetectionRange;

    public static float hIr;
    public static float vIr;

    public static bool[] btnInput;


    public Button BackButton;
    public Button CtrlButton;
    public Button QButton;




    // Start is called before the first frame update
    void Start()
    {
        btnInput = new bool[3];
        btnInput[0] = false;
        btnInput[1] = false;
        btnInput[2] = false;

    }

    // Update is called once per frame
    void Update()
    {
        hIr = js.Horizontal;
        vIr = js.Vertical;

        hIr = Mathf.Abs(hIr) < inputDetectionRange ? 0 : Mathf.Sign(hIr);
        vIr = Mathf.Abs(vIr) < inputDetectionRange ? 0 : Mathf.Sign(vIr);




    }

    private void LateUpdate()
    {
        btnInput[0] = false;
        btnInput[1] = false;
        btnInput[2] = false;
    }
    public void CtrlButtonEvent()
    {
        btnInput[0] = true;
    }

    public void QButtonEvent()
    {
        btnInput[1] = true;
    }

    public void BackButtonEvent()
    {
        btnInput[2] = true;
    }
}
