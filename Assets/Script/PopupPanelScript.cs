using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PopupPanelScript : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_SpriteAsset key;
    [SerializeField]
    TMPro.TMP_SpriteAsset star;
    [SerializeField]
    TMPro.TMP_SpriteAsset knife;

    [SerializeField]
    TMPro.TextMeshProUGUI text;

    [SerializeField]
    float showSpeed;

    public void PopupPanel(ItemData.ItemType itemType, bool num)
    {
        string textBuffer;
        if (num)
        {
            textBuffer = "<sprite=0> + 1";
        }
        else
        {
            textBuffer = "<sprite=0> - 1";
        }

        switch (itemType)
        {
            case ItemData.ItemType.Key:
                text.spriteAsset = key;
                break;
            case ItemData.ItemType.Coin:
                text.spriteAsset = star;
                break;
            case ItemData.ItemType.Knife:
                text.spriteAsset = knife;
                break;
            default:
                Debug.Log("Invaild Icon");
                text.spriteAsset = star;
                break;
        }

        text.text = textBuffer;

        StopAllCoroutines();
        StartCoroutine(SmoothUp() );


    }

    IEnumerator SmoothUp()
    {

        float elapsedTime = 0f;

        while (elapsedTime < showSpeed / 2)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 0.5f, elapsedTime / (showSpeed / 2));
            transform.GetComponent<CanvasGroup>().alpha = alpha;
            yield return null;
        }
        elapsedTime = 0f;
        while (elapsedTime < showSpeed / 2)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0.5f, 0f, elapsedTime / (showSpeed / 2));
            transform.GetComponent<CanvasGroup>().alpha = alpha;
            yield return null;
        }
        yield return null;
    }
}
