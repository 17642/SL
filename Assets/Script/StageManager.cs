using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
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
    }
    #endregion
    void Start()
    {
        isStageOn = true;
        //stageCoinNum = GameManager.instance.stageMaxCoins[stageNumber - 1];
    }

    // Update is called once per frame
    void Update()
    {
        if (isStageOn)
        {
            internalTime += Time.deltaTime;
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

    public void StageEnd(bool stageEndType, int coinAmount)//True - 스테이지 완료, False - 스테이지 실패
    {
        isStageOn = false;
        stageEnd = true;
        this.stageEndType = stageEndType;
        this.obtainedCoin = coinAmount;
        
        
    }
}
