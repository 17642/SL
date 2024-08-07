using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

[System.Serializable]
public struct StageData
{
    public bool hasCleared;
    public float time;
    public int maxCoinNumber;
    public int obtainedCoinNumber;


}
[System.Serializable]
public struct PlayerData
{
    public bool tutorialFinished;
    public int maxStageNumber;
    public float volumeValue;
    public bool fullScreen;
    public bool touchInput;
}

[System.Serializable]
public class SaveData
{
    public PlayerData pd;
    public StageData[] sd;

    public SaveData(PlayerData pd, StageData[] sd)
    {
        this.pd = pd;
        this.sd = sd;
    }

}

public class GameManager : MonoBehaviour
{
    readonly string saveName = "save.json";
    
    [SerializeField]
    public int endStageNumber;

    public StageData[] stages;
    [SerializeField]
    public int[] stageMaxCoins = { 3, 3, 4 };

    public PlayerData playerData;




    #region SINGLETON
    public static GameManager instance;
    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(instance);
        } else {
            Destroy(gameObject);
        }


        playerData = new PlayerData();
        stages = new StageData[endStageNumber];
        playerData.fullScreen = false;
        playerData.touchInput = false;
        
        ResetStageData();
        LoadStageData();

        AudioListener.volume = playerData.volumeValue;
        Screen.fullScreen = playerData.fullScreen;

    }
    #endregion


    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public static void GoToMenuScene()
    {
        SceneManager.LoadScene("InitialScene");
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResetStageData()
    {
        for(int i=0; i<stages.Length; i++)
        {
            stages[i].hasCleared = false;
            stages[i].maxCoinNumber = stageMaxCoins[i];
            stages[i].obtainedCoinNumber = 0;
            stages[i].time = 0;
        }

        playerData.tutorialFinished = false;
        playerData.maxStageNumber = endStageNumber;
        playerData.volumeValue = 0.5f;
    }

    void LoadStageData()
    {
        if (!File.Exists(saveName))
        {
            Debug.Log("저장 파일이 없습니다.");
            return;
        }

        SaveData save = new SaveData(playerData, stages);
        string json = File.ReadAllText(saveName);

        if(json == "")
        {
            Debug.Log("세이브 형식에 오류가 있습니다.");
            return;
        }

        save = JsonUtility.FromJson<SaveData>(json);

        if(save.pd.maxStageNumber > playerData.maxStageNumber)
        {
            Debug.Log("세이브 형식에 오류가 있습니다.");
            return;
        }

        int index = 0;
        foreach (StageData stageData in save.sd)
        {
            stages[index] = stageData;
            index++;
        }
        playerData = save.pd;
    }

    public void SaveStageData()
    {

        SaveData save = new SaveData(playerData, stages);

        string json = JsonUtility.ToJson(save);

        File.WriteAllText(saveName, json);
    }

    public void RecordStage(int stageNum, float time, int obtainedCoin)
    {
        if (stageNum > endStageNumber)
        {
            Debug.LogWarning("스테이지 번호가 마지막 스테이지 번호보다 더 큽니다.");
            return;
        }


        if (obtainedCoin == stages[stageNum - 1].maxCoinNumber)
        {
            if (stages[stageNum - 1].time == 0 || time < stages[stageNum - 1].time)
            {
                stages[stageNum - 1].time = time;
            }

            stages[stageNum - 1].obtainedCoinNumber = obtainedCoin;

        }
        else
        {
            if (!stages[stageNum - 1].hasCleared)
            {
                stages[stageNum - 1].time = 0;
            }

            if (stages[stageNum - 1].obtainedCoinNumber < obtainedCoin)
            {
                stages[stageNum - 1].obtainedCoinNumber = obtainedCoin;
            }
        }

        stages[stageNum - 1].hasCleared = true;


    }
}
