using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public enum PatrolType
{
    Linear, Circle
}

public class PatrolEnemy : Enemy
{
    [SerializeField]
    PatrolPoint patrolPoint;

    [SerializeField]
    PatrolType patrolType;

    private bool isReturning = false;//Linear 순찰에서만 사용
    private bool patrolCounter = false;


    int currentPatrolIndex = 0;


    protected override void Start()
    {
        //Debug.Log(nav);
        base.Start();
        //Debug.Log(nav);

        Patrol();
    }

    protected override void Update()
    {
        base.Update();
        //Debug.Log("isUpdating");
        if (!isIrrtated)
         {
            //Debug.Log("Attempting Patrolling");
            if (!patrolCounter)
            {
                patrolCounter = true;
                //Patrol();
            }
             SetOriginPos();
             MoveToNextLocation();
        }
        patrolCounter = false;
    }

    void SetOriginPos()
    {
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
        if (!isReturning)
        {
            currentPatrolIndex++;
            if (currentPatrolIndex == patrolPoint.point.Length - 1)
            {
                isReturning = true;
            }
        }
        else
        {
            currentPatrolIndex--;
            if (currentPatrolIndex == 0)
            {
                isReturning = false;
            }
        }
        SetDestination(patrolPoint.point[currentPatrolIndex].position);
    }

    void PatrolCircle()
    {
        currentPatrolIndex++;

        if (currentPatrolIndex == patrolPoint.point.Length)
        {
            currentPatrolIndex = 0;
        }
        else
        {
            
        }
        SetDestination(patrolPoint.point[currentPatrolIndex].position);
    }

    void SetDestination(Vector3 destination)
    {
        Debug.Log("Destination Set to "+destination);
        nav.SetDestination(destination);
    }

    void MoveToNextLocation()
    {
        FaceTarget();
        //StartCoroutine(FaceCoroutine());
        if (((Vector2)transform.position - (Vector2)patrolPoint.point[currentPatrolIndex].position).magnitude < 0.1f)
        {
            Debug.Log("Moving to next Location");
            Patrol();
        }
    }

}
