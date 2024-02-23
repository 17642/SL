using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageSelectButtonScript : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI stageNumberText;
    [SerializeField]
    TMPro.TextMeshProUGUI notClearedText;
    [SerializeField]
    GameObject stageRecordField;
    [SerializeField]
    TMPro.TextMeshProUGUI timeRec;
    [SerializeField]
    TMPro.TextMeshProUGUI starRec;

    [SerializeField]
    int stageNumber;

    Button btn;

    readonly string notCleared = "Not Cleared";
    readonly string noAccesable = "Not Accesable";

    string noClear;

    private void Start()
    {
        btn = GetComponent<Button>();

        stageNumberText.text = "Stage " + stageNumber;

        if (stageNumber-1 == 0 || GameManager.instance.stages[stageNumber - 2].hasCleared)
        {
            noClear = notCleared;
        }
        else
        {
            noClear = noAccesable;
            btn.enabled = false;
        }

        notClearedText.text = noClear;

        if (GameManager.instance.stages[stageNumber - 1].hasCleared)
        {
            stageRecordField.SetActive(true);
            notClearedText.gameObject.SetActive(false);
        }
        
        int timeRaw = (int)GameManager.instance.stages[stageNumber-1].time;
        if (timeRaw == 0)
        {
            timeRec.text = "-- : --";
        }
        else
        {
            timeRec.text = (timeRaw / 60).ToString() + " : " + (timeRaw % 60).ToString();
        }
        starRec.text = GameManager.instance.stages[stageNumber - 1].obtainedCoinNumber.ToString() + " / " + GameManager.instance.stages[stageNumber - 1].maxCoinNumber.ToString();

        
    }

}
