using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using System;

[System.Serializable]
class InstructionField
{
    public int instNumber;
    public TMPro.TMP_SpriteAsset usedSpriteAsset;
    public string instName;
    [TextArea]
    public string inst;
}
public class InstructionUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI instName;
    [SerializeField]
    TextMeshProUGUI instScreen;
    [SerializeField]
    InstructionField[] instField;
    [SerializeField]
    GameObject instSelectButtonField;

    Button[] instButtons;
    private void Start()
    {
        instButtons = instSelectButtonField.GetComponentsInChildren<Button>();

        int index = 0;

        foreach (Button button in instButtons)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = instField[index].instName;
            index++;
        }

        instScreen.text = instField[0].inst;
        if (instField[0].usedSpriteAsset != null)
            instScreen.spriteAsset = instField[0].usedSpriteAsset;
        instName.text = instField[0].instName;

    }


    public void ClickInstSelect(int num)
    {
        instScreen.text = instField[num].inst;
        if (instField[num].usedSpriteAsset != null)
        instScreen.spriteAsset = instField[num].usedSpriteAsset;
        instName.text = instField[num].instName;
    }
    public void ClickExitButton()
    {
        gameObject.SetActive(false);
    }
}
