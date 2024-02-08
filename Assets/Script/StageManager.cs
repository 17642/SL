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
    void Start()
    {
        isStageOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isStageOn){
            internalTime += Time.deltaTime;
        }
    }

    public void ChgLight(){
        stageLight = !stageLight;
    }

    public void StageEnd(){
        isStageOn = false;
    }
}