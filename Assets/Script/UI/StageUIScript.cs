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

    //���� UI ����
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
    float statusSpeed;

    Coroutine StatusCoroutine;

    void Start()
    {
        Time.timeScale = 1.0f;
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
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if(StatusCoroutine!=null)StopCoroutine(StatusCoroutine);
                StatusCoroutine = StartCoroutine(StatusUp());
            }
            if(Input.GetKeyUp(KeyCode.Q))
            {
                if (StatusCoroutine != null) StopCoroutine(StatusCoroutine);
                StatusCoroutine = StartCoroutine(StatusDown());
            }

            if (StageManager.instance.detectCount > 0)
            {
                //-������- ǥ��
                //�ӽ�
                detectedPanel.alpha = 1;
            }
            else
            {
                detectedPanel.alpha = 0;
            }
        }

        if (StageManager.instance.stageEnd)
        {
            GameEnd();
        }
    }

    private void Pause()
    {
        StageManager.instance.isStageOn = false;
        Time.timeScale = 0.0f;
        PausePanel.gameObject.SetActive(true);
    }

    private void GameEnd()
    {
        Time.timeScale = 0.0f;
        FinishPanel.gameObject.SetActive(true);
    }

    static public void PopupStatusElement(string option)
    {
        switch (option)
        {
            case "key":
                // ���� ���� ��Ҹ� �˾��մϴ�.
                // ���� ���, ���� ��� UI�� Ȱ��ȭ�ϰ� ���� ���� ������Ʈ�մϴ�.
                // statusKey.gameObject.SetActive(true);
                // statusKey.text = "X " + StageManager.instance.player.item_Amount[(int)ItemData.ItemType.Key];
                break;
            case "coin":
                // ���� ���� ��Ҹ� �˾��մϴ�.
                // ���� ���, ���� ��� UI�� Ȱ��ȭ�ϰ� ���� ���� ������Ʈ�մϴ�.
                // statusCoin.gameObject.SetActive(true);
                // statusCoin.text = StageManager.instance.player.item_Amount[(int)ItemData.ItemType.Coin] + " / " + StageManager.instance.stageCoinNum;
                break;
            case "knife":
                break;
            // �ٸ� �ɼǿ� ���� ���� ��� �˾��� �����մϴ�.
            default:
                // �ùٸ��� ���� �ɼ��̰ų� ó������ ���� �ɼ��� ��� ó���մϴ�.
                Debug.LogWarning("Invalid option for PopupStatusElement: " + option);
                break;
        }
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
}
