using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSettingUI : MonoBehaviour
{
    // Start is called before the first frame update
    public void RemoveButtonClick()
    {
        GameManager.instance.ResetStageData();
    }

    public void ExitButtonClick()
    {
        gameObject.SetActive(false);
    }
}
