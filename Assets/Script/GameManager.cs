using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct StageData
{
    public bool hasCleared;
    public float time;
    public int maxCoinNumber;
    public int obtainedCoinNumber;

    
    
}

public class GameManager : MonoBehaviour
{
    
    [SerializeField]
    public int endStageNumber;

    public StageData[] stages;
    public int[] stageMaxCoins = { 3, 3, 4 };




    #region SINGLETON
    public static GameManager instance;
    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(instance);
        } else {
            Destroy(gameObject);
        }
    }
    #endregion



    void Start()
    {
        stages = new StageData[endStageNumber];
        ResetStageData();
        LoadStageData();
    }

    // Update is called once per frame
    void Update()
    {

    }

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
            StageData data = stages[i];
            data.hasCleared = false;
            data.maxCoinNumber = stageMaxCoins[i];
            data.obtainedCoinNumber = 0;
            data.time = 0;
        }
    }

    void LoadStageData()
    {

    }

    void SaveStageData()
    {

    }
}
