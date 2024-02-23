using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageSelect : MonoBehaviour
{
    [SerializeField]
    GameObject stageButtonLayout;

    StageSelectButtonScript[] stageButtons;
    // Start is called before the first frame update
    void Start()
    {
        stageButtons = stageButtonLayout.GetComponentsInChildren<StageSelectButtonScript>();

        foreach (StageSelectButtonScript stageButton in stageButtons)
        {
            
        }
    }
    public void ClickMainMenuButton()
    {
        GameManager.GoToMenuScene();
    }

    public void StageButtonClick(int number)
    {
        GameManager.instance.ChangeScene("Stage" + number);
    }
}
