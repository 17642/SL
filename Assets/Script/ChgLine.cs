using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChgLine : MonoBehaviour
{
    
    StageManager stageManager;
    void Start()
    {
        stageManager = GameObject.FindObjectOfType<StageManager>();

    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
        stageManager.ChgLight();}
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player")){
        stageManager.ChgLight();}
    }
    
}
