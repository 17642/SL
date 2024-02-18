using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public bool stageLight = false;//false - dark, true - bright
    [SerializeField]
    public float internalTime = 0.0f;
    [SerializeField]
    private bool isStageOn;

    //private bool stageHasCoin;
    [SerializeField]
    private int stageCoinNum;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (isStageOn)
        {
            internalTime += Time.deltaTime;
        }
    }

    public void ChgLight()
    {
        stageLight = !stageLight;

    }

    public void StageEnd(bool stageEndType, int coinAmount)
    {
        if(coinAmount>=stageCoinNum){
            
        }
        isStageOn = false;
    }
}
