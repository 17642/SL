using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float scanRadius;
    [SerializeField]
    private float scanRange;
    [SerializeField]
    private float moveSpeed;
    private bool isDetected;
    [SerializeField]
    private bool isIrrtated;
    [SerializeField]
    private float detectRange;
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private float scanSensitivity;
    private Vector2 originLocation;
    private Vector2 lastKnownPLocation;
    private Quaternion originDirection;
    private NavMeshAgent nav;
    [SerializeField]
    LayerMask ignoreLayer;
    RaycastHit2D hit;

[SerializeField]
    private GameObject enemySight;

    int playerLayerMask;

    void Start(){
        //playerLayerMask = 1<<LayerMask.NameToLayer("Player");
        originLocation = transform.position;
        originDirection = transform.rotation;
        Debug.Log("원위치: "+ originLocation+" 방향: "+originDirection);
        nav = GetComponent<NavMeshAgent>();
        if(detectRange>scanRadius){
            Debug.LogWarning("적 개체의 경계 범위가 시야 범위보다 넓습니다.");
        }
        nav.updateRotation = false;
		nav.updateUpAxis = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        ScanFront2();
        DrawRay();
        if(isIrrtated){
        if(isDetected){
            MoveToDetectedLocation();
        }else{
            MoveToOriginLocation();
        }
        }
        enemySight.SetActive(StageManager.instance.stageLight);
    }

    void ScanFront(){//Raycast로 수정 필요 
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, scanRadius);//, playerLayerMask);

        foreach (Collider2D collider in colliders){
            if(collider.CompareTag("Player")){
            Vector2 direction = collider.transform.position - transform.position;
            float angle = Vector2.Angle(transform.up, direction);
            float var = direction.magnitude;
            if(angle<scanRange){
                Debug.Log("시야 범위 접근 각도:"+angle+"거리: "+var);
                if(var>detectRange){
                    isDetected = true;
                    lastKnownPLocation = collider.transform.position;
                    Debug.Log("경계 범위 접근. 마지막 추적 위치: "+lastKnownPLocation);
                }
            }
            }
            break;
        }

    }

    void ScanFront2(){
        float startAngle = -scanRange; // 시작 각도
        float endAngle = scanRange; // 종료 각도

        for(float angle = startAngle;angle<=endAngle;angle+=scanSensitivity){
            Vector2 rayDir=Quaternion.Euler(0,0,angle)*transform.up;

            hit = Physics2D.Raycast(transform.position,rayDir,scanRadius, ~ignoreLayer);
            //Debug.Log("접촉한 물체: "+hit.transform.name);
            if(hit&&hit.collider.CompareTag("Player")){
                Vector2 detectedPlayerPosition = hit.collider.transform.position;
                float var =Vector2.Distance(detectedPlayerPosition,transform.position);

                Debug.Log("플레이어를 감지했습니다. 거리: " + var);
                if(var>detectRange){
                    isDetected = true;
                    isIrrtated = true;
                    lastKnownPLocation = hit.transform.position;
                    Debug.Log("경계 범위 접근. 마지막 추적 위치: "+lastKnownPLocation);
                }
                break;
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

    void MoveToDetectedLocation(){//Unity.A
        nav.SetDestination(lastKnownPLocation);
        FaceTarget();
        if(((Vector2)transform.position-lastKnownPLocation).magnitude<0.1f){
            isDetected = false;
        }
    }

    void MoveToOriginLocation(){
        if(((Vector2)transform.position-originLocation).magnitude>0.05f){
            nav.SetDestination(originLocation);
            FaceTarget();}
        else if(Quaternion.Angle(transform.rotation, originDirection) > 0.05f){
            transform.rotation = Quaternion.RotateTowards(transform.rotation, originDirection,5f);
            //transform.rotation = Quaternion.LookRotation(Vector3.forward,originDirection.eulerAngles);
            //isIrrtated = false;
        }else{
            isIrrtated = false;
        }
    }

    void FaceTarget() {
        var vel = nav.velocity;
        vel.z = 0;

        if (vel != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(Vector3.forward,vel);
        }
}
 
    
}
