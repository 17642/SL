using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    bool DebugStage;

    public bool stageLight = false;//false - dark, true - bright
    [SerializeField]
    public float internalTime = 0.0f;
    [SerializeField]
    public bool isStageOn;

    [SerializeField]
    public Player player;

    //private bool stageHasCoin;
    [SerializeField]
    public int stageCoinNum;
    [SerializeField]
    public int stageNumber;

    [SerializeField]
    public SoundManager soundManager;

    public bool stageEnd = false;
    public bool stageEndType = false;
    public int obtainedCoin = 0;

    public int detectCount = 0;


    #region SINGLETON
    public static StageManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }

        if (!DebugStage)
            stageCoinNum = GameManager.instance.stageMaxCoins[stageNumber - 1];
    }
    #endregion
    void Start()
    {
        isStageOn = true;
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isStageOn)
        {
            internalTime += Time.deltaTime;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
        detectCount = Enemy.detectEnemyCount;
        //Debug.Log(Enemy.detectEnemyCount);
        Enemy.detectEnemyCount = 0;
        //Debug.Log(detectCount);



    }

    public void ChgLight()
    {
        stageLight = !stageLight;

    }

    public void StageEnd(bool stageEndType, int coinAmount)//True - �������� �Ϸ�, False - �������� ����
    {
        soundManager.StopAllSounds();

        isStageOn = false;
        stageEnd = true;
        this.stageEndType = stageEndType;
        this.obtainedCoin = coinAmount;

        if (stageEndType)
        {
            GameManager.instance.RecordStage(stageNumber, internalTime, coinAmount);
            GameManager.instance.SaveStageData();
        }
        
        
    }
}
