using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float scanRadius;
    [SerializeField]
    private float scanRange;
    RaycastHit2D hit;

    int playerLayerMask;

    void Start(){
        playerLayerMask = 1<<LayerMask.NameToLayer("Player");
    }
    
    // Update is called once per frame
    void Update()
    {
        ScanFront();
        DrawRay();
    }

    void ScanFront(){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, scanRadius, playerLayerMask);

        foreach (Collider2D collider in colliders){
            Vector2 direction = (collider.transform.position - transform.position).normalized;
            float angle = Vector2.Angle(transform.up, direction);
            if(angle<scanRange){
                Debug.Log("시야 범위 접근 각도:"+angle);
            }
        }

    }

    void DrawRay(){
        Vector2 FrontVec = transform.up*scanRadius;
        Debug.DrawRay(transform.position, FrontVec,Color.red);
        Debug.DrawRay(transform.position, Quaternion.Euler(0,0,scanRange)*FrontVec,Color.red);
        Debug.DrawRay(transform.position, Quaternion.Euler(0,0,-scanRange)*FrontVec,Color.red);
    }

    void OnTriggerStay2D(Collider2D other){
        if(other.CompareTag("PlayerSoundA")){
            Debug.Log("소리 범위 A와 접촉");
        }else if(other.CompareTag("PlayerSoundB")){
            Debug.Log("소리 범위 B와 접촉");
        }
    }
    
}
