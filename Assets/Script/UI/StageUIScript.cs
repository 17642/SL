using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageUIScript : MonoBehaviour
{
    [SerializeField]
    CanvasGroup StartPanel;
    [SerializeField]
    GameObject PausePanel;
    [SerializeField]
    CanvasGroup InGameUI;
    [SerializeField]
    CanvasGroup FinishPanel;

    [SerializeField]
    TMPro.TMP_Text StageNumberText;
    [SerializeField]
    TMPro.TMP_Text CoinNumberText;

    //상태 UI 관리
    [SerializeField]
    TMPro.TextMeshProUGUI statusKey;
    [SerializeField]
    TMPro.TextMeshProUGUI statusCoin;
    [SerializeField]
    TMPro.TextMeshProUGUI statusTime;
    [SerializeField]
    TMPro.TextMeshProUGUI statusStage;

    [SerializeField]
    float startPanelFadeTime;


    [SerializeField]
    float statusSpeed;

    Coroutine StatusCoroutine;

    void Start()
    {
        statusStage.text = "Stage " + StageManager.instance.stageNumber;

        StageNumberText.text = "Stage " + StageManager.instance.stageNumber;
        CoinNumberText.text = "Stars : 0 / " + StageManager.instance.stageCoinNum;


        StartCoroutine(FadeStartPanel());
    }

    // Update is called once per frame
    void Update()
    {
        UIUpdate();

        if (StageManager.instance.isStageOn) {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                StopCoroutine(StatusCoroutine);
                StatusCoroutine = StartCoroutine(StatusUp());
            }
            if(Input.GetKeyUp(KeyCode.Tab))
            {
                StopCoroutine(StatusCoroutine);
                StatusCoroutine = StartCoroutine(StatusDown());
            }
        }
    }

    private void Pause()
    {
        StageManager.instance.isStageOn = false;
        Time.timeScale = 0.0f;
        PausePanel.gameObject.SetActive(true);
    }

    private void PopupUI()
    {

    }

    private void UIUpdate()
    {
        statusKey.text = "X " + StageManager.instance.player.item_Amount[(int)ItemData.ItemType.Key];
        statusCoin.text = StageManager.instance.player.item_Amount[(int)ItemData.ItemType.Coin].ToString()+ " / " + StageManager.instance.stageCoinNum.ToString();
        int stageTimeRaw = (int)StageManager.instance.internalTime;
        statusTime.text = (stageTimeRaw / 60).ToString() + " : " + (stageTimeRaw % 60).ToString();
    }

    IEnumerator StatusUp()
    {
        yield return null;
    }

    IEnumerator StatusDown()
    {
        yield return null;
    }

    IEnumerator FadeStartPanel()
    {
        //Time.timeScale = 0.0f;
        StageManager.instance.isStageOn = false;
        float timer = 0.0f;
        while (timer < startPanelFadeTime)
        {
            yield return null;
            timer += Time.deltaTime;
            Debug.Log(timer);
        }

        timer = 0f;
        while (timer < startPanelFadeTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / startPanelFadeTime);

            StartPanel.alpha = alpha;
            StageNumberText.alpha = alpha;
            CoinNumberText.alpha = alpha;


            yield return null;
            timer += Time.deltaTime;
        }

        StageManager.instance.isStageOn = true;
        StartPanel.gameObject.SetActive(false);
        
    }
}
