using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenArea : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            if(other.GetComponent<Player>().KeyUse()){
                gameObject.SetActive(false);
            }
        }
    }
}
