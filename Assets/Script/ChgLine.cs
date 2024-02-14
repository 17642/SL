using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChgLine : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
        StageManager.instance.ChgLight();}
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player")){
        StageManager.instance.ChgLight();}
    }
    
}
