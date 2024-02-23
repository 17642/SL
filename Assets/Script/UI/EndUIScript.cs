using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net.Mail;

public class EndUIScript : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI stageName;
    [SerializeField]
    TMPro.TextMeshProUGUI endMessage;
    [SerializeField]
    TMPro.TextMeshProUGUI internalTime;
    [SerializeField]
    TMPro.TextMeshProUGUI starCounter;

    [SerializeField]
    Image star;

    [SerializeField]
    Button NextLevelButton;

    public string endMessageText = "Cleared";

    private void OnEnable()
    {
        stageName.text ="Stage " + StageManager.instance.stageNumber.ToString();
        
        int internalTimeRaw = (int)StageManager.instance.internalTime;
        internalTime.text=(internalTimeRaw/60).ToString()+" : "+(internalTimeRaw%60).ToString();
        

        if (!StageManager.instance.stageEndType)
        {
            endMessageText = "Failed";
            NextLevelButton.gameObject.SetActive(false);
            star.gameObject.SetActive(false);
        }
        else
        {
            endMessageText = "Cleared";
            starCounter.text = StageManager.instance.obtainedCoin.ToString() + " / " + StageManager.instance.stageCoinNum.ToString();
        }

        endMessage.text = "- " + endMessageText + " -";

        if(StageManager.instance.stageNumber == GameManager.instance.endStageNumber)
        {
            NextLevelButton.enabled = false;
        }
        
    }

    public void NextLevelButtonClick()
    {
        Time.timeScale = 1.0f;
        Debug.Log("Go To Next Level");
        GameManager.instance.ChangeScene("Stage"+(StageManager.instance.stageNumber+1));
    }
    public void RestartButtonClick()
    {
        Time.timeScale = 1.0f;
        GameManager.ReloadScene();
    }

    public void MenuButtonClick()
    {
        Time.timeScale = 1.0f;
        GameManager.GoToMenuScene();
    }
}
