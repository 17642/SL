using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseUIScript : MonoBehaviour
{

    [SerializeField]
    TMPro.TextMeshProUGUI coinNum;
    [SerializeField]
    TMPro.TextMeshProUGUI internalTime;
    // Start is called before the first frame update
    private void OnEnable()
    {
        coinNum.text = StageManager.instance.player.item_Amount[(int)ItemData.ItemType.Coin].ToString() + " / " + StageManager.instance.stageCoinNum.ToString();
        int stageTimeRaw = (int)StageManager.instance.internalTime;
        internalTime.text = (stageTimeRaw / 60).ToString() + " : " + (stageTimeRaw % 60).ToString();
    }


    public void ExitMenuButtonClick()
    {
        GameManager.GoToMenuScene();
    }

    public void ResumeButtonClick()
    {
        StageManager.instance.isStageOn = true;
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }

    public void RestartButtonClick()
    {
        GameManager.ReloadScene();
    }
}
