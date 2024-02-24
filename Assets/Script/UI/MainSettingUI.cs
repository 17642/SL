using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSettingUI : MonoBehaviour
{
    [SerializeField]
    GameObject instructionScreen;
    public void InstructionButtonClick()
    {
        instructionScreen.SetActive(true);
    }
    // Start is called before the first frame update
    public void RemoveButtonClick()
    {
        GameManager.instance.ResetStageData();
        GameManager.instance.SaveStageData();
    }

    public void ExitButtonClick()
    {
        gameObject.SetActive(false);
        GameManager.instance.SaveStageData();
    }
}
