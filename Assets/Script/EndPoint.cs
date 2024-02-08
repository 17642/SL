using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    StageManager stageManager;
    void Start()
    {
        stageManager = GameObject.FindObjectOfType<StageManager>();

    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            stageManager.StageEnd();
        }
    }
}
