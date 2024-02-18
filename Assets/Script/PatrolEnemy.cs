using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public enum PatrolType{
    Linear, Circle
}

public class PatrolEnemy : Enemy
{
    [SerializeField]
    PatrolPoint patrolPoint;

    [SerializeField]
    PatrolType patrolType;

    private bool isReturning=false;//Linear 순찰에서만 사용


    int currentPatrolIndex=0;

    protected override void Start()
    {
        base.Start();


    }

    protected override void Update()
    {
        base.Update();

        if(!isIrrtated){
            SetOriginPos();
            moveToNextLocation();
        }
    }

    void SetOriginPos(){
        originLocation = transform.position;
        originDirection = transform.rotation;
    }

    void Patrol()
    {
        switch (patrolType)
        {
            case PatrolType.Linear:
                PatrolLinear();
                break;
            case PatrolType.Circle:
                PatrolCircle();
                break;
        }
    }

    void PatrolLinear()
    {
        if(!isReturning){
            currentPatrolIndex++;
            if(currentPatrolIndex==patrolPoint.point.Length-1){
                isReturning = true;
            }
        }else{
            currentPatrolIndex--;
            if(currentPatrolIndex==0){
                isReturning = false;
            }
        }
        SetDestination(patrolPoint.point[currentPatrolIndex].position);
    }

    void PatrolCircle()
    {
        if (currentPatrolIndex == patrolPoint.point.Length - 1)
        {
            currentPatrolIndex = 0;
        }
        else
        {
            currentPatrolIndex++;
        }
        SetDestination(patrolPoint.point[currentPatrolIndex].position);
    }

    void SetDestination(Vector3 destination)
    {
        nav.SetDestination(destination);
    }

    void moveToNextLocation(){
        FaceTarget();
        if (((Vector2)transform.position - (Vector2)patrolPoint.point[currentPatrolIndex].position).magnitude < 0.1f)
        {
            Patrol();
        }
    }

}
