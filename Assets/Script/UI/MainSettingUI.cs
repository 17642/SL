using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSettingUI : MonoBehaviour
{
    [SerializeField]
    GameObject instructionScreen;
    [SerializeField]
    Slider volumeSlider;
    [SerializeField]
    Toggle fullScreenCheckbox;
    public void InstructionButtonClick()
    {
        instructionScreen.SetActive(true);
    }
    // Start is called before the first frame update
    public void RemoveButtonClick()
    {
        GameManager.instance.ResetStageData();
        GameManager.instance.SaveStageData();
    }

    public void ExitButtonClick()
    {
        gameObject.SetActive(false);
        GameManager.instance.SaveStageData();
    }

    public void SoundSliderChange()
    {
        AudioListener.volume = volumeSlider.value;
        GameManager.instance.playerData.volumeValue = volumeSlider.value;
    }

    public void FullScreenCheckbox()
    {
        Screen.fullScreen = fullScreenCheckbox.isOn;
        GameManager.instance.playerData.fullScreen = fullScreenCheckbox.isOn;
    }

    private void Start()
    {
        volumeSlider.value = GameManager.instance.playerData.volumeValue;
    }

}
