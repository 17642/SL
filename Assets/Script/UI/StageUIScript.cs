using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

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
    GameObject TI;

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
    TMPro.TextMeshProUGUI statusKnife;

    [SerializeField]
    CanvasGroup detectedPanel;

    [SerializeField]
    float startPanelFadeTime;
    [SerializeField]
    float detectImageFadeSpeed;
    [SerializeField]
    float statusSpeed;

    bool tToggle = false;

    public static PopupPanelScript PopupPanel;

    Coroutine StatusCoroutine;

    void Start()
    {
        PopupPanel = GetComponentInChildren<PopupPanelScript>();

        Time.timeScale = 1.0f;
        statusStage.text = "Stage " + StageManager.instance.stageNumber;

        StageNumberText.text = "Stage " + StageManager.instance.stageNumber;
        CoinNumberText.text = "Stars : 0 / " + StageManager.instance.stageCoinNum;

        detectedPanel.alpha = 0;

        if (GameManager.instance.playerData.touchInput)
        {
            TI.SetActive(true);
        }

        StartCoroutine(ManageDetectPanel());
        StartCoroutine(FadeStartPanel());
    }

    // Update is called once per frame
    void Update()
    {
        UIUpdate();

        if (StageManager.instance.isStageOn) {
            if (Input.GetKeyDown(KeyCode.Escape)| TouchyInterface.btnInput[2])
            {
                Pause();
            }
            if (Input.GetKeyDown(KeyCode.Q) || (TouchyInterface.btnInput[1]&&!tToggle))
            {
                if(StatusCoroutine!=null)StopCoroutine(StatusCoroutine);
                StatusCoroutine = StartCoroutine(StatusUp());
            }
            if(Input.GetKeyUp(KeyCode.Q) || (TouchyInterface.btnInput[1] && tToggle))
            {
                if (StatusCoroutine != null) StopCoroutine(StatusCoroutine);
                StatusCoroutine = StartCoroutine(StatusDown());
            }

            
        }

        if (StageManager.instance.stageEnd)
        {
            GameEnd();
        }

        if (TouchyInterface.btnInput[1])
        {
            tToggle = !tToggle;
        }
    }

    private void Pause()
    {
        StageManager.instance.soundManager.StopAllSounds();

        StageManager.instance.isStageOn = false;
        Time.timeScale = 0.0f;
        PausePanel.gameObject.SetActive(true);
    }

    private void GameEnd()
    {
        Time.timeScale = 0.0f;
        FinishPanel.gameObject.SetActive(true);
    }

    private void UIUpdate()
    {
        statusKey.text = "X " + StageManager.instance.player.item_Amount[(int)ItemData.ItemType.Key];
        statusCoin.text = StageManager.instance.player.item_Amount[(int)ItemData.ItemType.Coin].ToString()+ " / " + StageManager.instance.stageCoinNum.ToString();
        int stageTimeRaw = (int)StageManager.instance.internalTime;
        statusTime.text = (stageTimeRaw / 60).ToString() + " : " + (stageTimeRaw % 60).ToString();
        statusKnife.text = "X " + StageManager.instance.player.item_Amount[(int)ItemData.ItemType.Knife];
    }

    IEnumerator StatusUp()
    {
        float alpha = InGameUI.alpha;
        while (alpha < 1.0f)
        {
            alpha += statusSpeed;
            InGameUI.alpha = alpha;
            yield return null;
        }

        InGameUI.alpha = 1;

    }

    IEnumerator StatusDown()
    {
        float alpha = InGameUI.alpha;
        while (alpha > 0f)
        {
            alpha -= statusSpeed;
            InGameUI.alpha = alpha;
            yield return null;
        }

        InGameUI.alpha = 0;
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

        StatusCoroutine = StartCoroutine(StatusDown());
        
    }

    IEnumerator ManageDetectPanel()
    {
        float targetAlpha;

        while (true)
        {

            if (StageManager.instance.detectCount > 0)
            {
                targetAlpha = 1;
            }
            else
            {
                targetAlpha = 0;
            }

            float alpha = detectedPanel.alpha;
            alpha = Mathf.MoveTowards(alpha, targetAlpha, detectImageFadeSpeed * Time.deltaTime);
            detectedPanel.alpha = alpha;
            yield return null;
        }

    }
}
