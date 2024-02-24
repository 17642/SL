using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    //시야 요소
    [SerializeField]
    private float scanRadius;//시야 거리
    [SerializeField]
    private float scanRange;//시야 반경
    [SerializeField]
    private float moveSpeed;//적의 기본 이동속도
    private bool isDetected;//적 감지 및 해당 방향으로 이동중.
    [SerializeField]
    protected bool isIrrtated;//isDetected+ 원래 위치로 복귀중.
    [SerializeField]
    private float detectRange;//경계 거리(이 거리 내부로 들어올 시 즉시 발각)
    [SerializeField]
    private float rotateSpeed;//회전 속도
    [SerializeField]
    private float scanSensitivity;//Ray의 수
    [SerializeField]
    private float alertSpeed;//경계 상태에 들어간 적의 속도

    //플레이어 및 원래 위치 기억
    protected Vector2 originLocation;
    private Vector2 lastKnownPLocation;
    protected Quaternion originDirection;
    protected NavMeshAgent nav;
    [SerializeField]
    LayerMask ignoreLayer;
    RaycastHit2D hit;

    [SerializeField]
    private GameObject enemySight;
    //플레이어 소리 측정 시 사용
    [SerializeField]
    float soundListenCooldown;
    float soundListenTimer = 0f;

    [SerializeField]
    float enemyWaitingTime;

    [SerializeField]
    float navMeshReloadTime;

    float navMeshTimer;

    public Renderer unitRenderer;

    public static int detectEnemyCount = 0;

    AudioSource audioSource;



    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        unitRenderer = GetComponent<Renderer>();
        //playerLayerMask = 1<<LayerMask.NameToLayer("Player");
        originLocation = transform.position;
        originDirection = transform.rotation;
        Debug.Log("원위치: " + originLocation + " 방향: " + originDirection);
        nav = GetComponent<NavMeshAgent>();
        if (detectRange > scanRadius)
        {
            Debug.LogWarning("적 개체의 경계 범위가 시야 범위보다 넓습니다.");
        }
        nav.updateRotation = false;
        nav.updateUpAxis = false;

        nav.speed = moveSpeed;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        SetUnitAlpha();

        if (StageManager.instance.stageLight)
        {
            ScanFront2();
            DrawRay();
        }
        if (isIrrtated)
        {
            nav.speed = alertSpeed;
            if (isDetected)
            {
                MoveToDetectedLocation();
            }
            else
            {
                //MoveToOriginLocation();
            }
        }
        else
        {
            nav.speed = moveSpeed;
        }
        enemySight.SetActive(StageManager.instance.stageLight);
        TimerManager();


        PlaySoundOnWalking();
        
    }

    void PlaySoundOnWalking()
    {
        if (nav.velocity.magnitude > 0 && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (nav.velocity.magnitude == 0 && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void LateUpdate()
    {
        if (isDetected)
        {
            detectEnemyCount++;
        }

        ReloadNavMesh();
        
    }

    void ReloadNavMesh()
    {
        if (nav.velocity.magnitude <= 0.01f && !nav.hasPath && nav.pathStatus == NavMeshPathStatus.PathComplete)
        {
            navMeshTimer += Time.deltaTime;
            if (navMeshTimer >= navMeshReloadTime)
            {
                Debug.Log("Character stuck");
                nav.enabled = false;
                nav.enabled = true;
                Debug.Log("navmesh re enabled");
                // navmesh agent will start moving again
                navMeshTimer = 0;
            }
        }
        else
        {
            navMeshTimer = 0;
        }
    }


    void ScanFront2()
    {
        float startAngle = -scanRange; // 시작 각도
        float endAngle = scanRange; // 종료 각도

        for (float angle = startAngle; angle <= endAngle; angle += scanSensitivity)
        {
            Vector2 rayDir = Quaternion.Euler(0, 0, angle) * transform.up;

            hit = Physics2D.Raycast(transform.position, rayDir, scanRadius, ~ignoreLayer);
            //Debug.Log("접촉한 물체: "+hit.transform.name);
            if (hit && hit.collider.CompareTag("Player"))
            {
                Vector2 detectedPlayerPosition = hit.collider.transform.position;
                float var = Vector2.Distance(detectedPlayerPosition, transform.position);

                Debug.Log("플레이어를 감지했습니다. 거리: " + var);
                if (var > detectRange)
                {
                    isDetected = true;
                    isIrrtated = true;
                    lastKnownPLocation = hit.transform.position;
                    Debug.Log("경계 범위 접근. 마지막 추적 위치: " + lastKnownPLocation);

                }
                else
                {
                    AttackPlayer(hit.collider);
                }
                break;
            }
        }
    }

    void ScanBySound(Collider2D input)
    {
        if (soundListenTimer < 0.01f)
        {
            soundListenTimer = soundListenCooldown;
            isDetected = true;
            isIrrtated = true;
            lastKnownPLocation = input.transform.position;
            Debug.Log("적 감지(소음) 거리: " + Vector2.Distance(lastKnownPLocation, transform.position));

        }
    }

    void DrawRay()
    {
        Vector2 FrontVec = transform.up * scanRadius;
        Debug.DrawRay(transform.position, FrontVec, Color.red);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, 0, scanRange) * FrontVec, Color.red);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, 0, -scanRange) * FrontVec, Color.red);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("PlayerSoundA"))
        {
            AttackPlayer(other.GetComponentInParent<Player>().transform.GetComponent<Collider2D>());
            Debug.Log("소리 범위 A와 접촉");
            return;
        }
        else if (other.CompareTag("PlayerSoundB"))
        {
            Debug.Log("소리 범위 B와 접촉");
            if (!StageManager.instance.stageLight) ScanBySound(other);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player와 직접 접촉");
            AttackPlayer(collision.collider);
        }
    }

    void MoveToDetectedLocation()
    {//Unity.A
        nav.SetDestination(lastKnownPLocation);
        FaceTarget();
        if (((Vector2)transform.position - lastKnownPLocation).magnitude < 0.1f)
        {
            StartCoroutine(WaitAndResetDetection());
        }
    }

    IEnumerator MoveToOriginLocation()
    {
        while (isIrrtated && !isDetected)
        {
            if (((Vector2)transform.position - originLocation).magnitude > 0.05f)
            {
                nav.SetDestination(originLocation);
                FaceTarget();
            }
            else if (Quaternion.Angle(transform.rotation, originDirection) > 0.05f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, originDirection, 5f);
                //transform.rotation = Quaternion.LookRotation(Vector3.forward,originDirection.eulerAngles);
                //isIrrtated = false;
            }
            else
            {
                isIrrtated = false;
            }
            yield return null;
        }
    }

    protected void FaceTarget()
    {
        var vel = nav.velocity;
        vel.z = 0;

        if (vel != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, vel);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, originDirection, 5f);
        }

        Debug.Log("FaceTarget Called");
    }

    protected IEnumerator FaceCoroutine(){

        while (true)
        {
            var vel = nav.velocity;
            vel.z = 0;

            if (vel != Vector3.zero)
            {
                //transform.rotation = Quaternion.LookRotation(Vector3.forward, vel);
                transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.LookRotation(Vector3.forward,vel), 2f);
                yield return null;

            }
            else
            {
                break;
            }


        }
        Debug.Log("FaceCoroutine Called");

    }


    IEnumerator WaitAndResetDetection()
    {
        isDetected = false;
        yield return new WaitForSeconds(enemyWaitingTime);

        StartCoroutine(MoveToOriginLocation());
    }

    void TimerManager()
    {
        if (!isDetected)
        {
            soundListenTimer -= Time.deltaTime; // 타이머 감소
            soundListenTimer = Mathf.Max(soundListenTimer, 0f); // 타이머가 음수가 되지 않도록 보정
        }

    }

    void SetUnitAlpha()
    {
        Color color = unitRenderer.material.color;
        color.a = StageManager.instance.stageLight ? 1.0f : 0.0f; // isVisible 값에 따라 투명도를 설정합니다.
        unitRenderer.material.color = color;
    }

    void AttackPlayer(Collider2D other)
    {
        Debug.Log("ATTACK!");
        other.GetComponent<Player>().showDamageEffect(transform);
        if (other.GetComponent<Player>().UseItem(ItemData.ItemType.Knife)){
            //적 처치 스크립트
            Destroy(gameObject);
        }
        else
        {
            //플레이어 사망 스크립트
            StageManager.instance.StageEnd(false, 0);
        }
    }
}
