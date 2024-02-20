using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainUI : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshPro mainTitle;

    public Button StartButton;
    public Button SettingButton;
    public Button ExitButton;

    [SerializeField]
    GameObject SettingScreen;



    public void ExitButtonClick()
    {
        Application.Quit();
    }

    public void SettingButtonClick()
    {
        SettingScreen.SetActive(true);
    }

    public void StartButtonClick()
    {
        GameManager.instance.ChangeScene("SampleScene");
    }





}
