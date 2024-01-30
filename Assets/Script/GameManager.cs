using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{





#region SINGLETON
    public static GameManager instance;
    void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(instance);
        }else{
            Destroy(gameObject);
        }
    }
#endregion



    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
