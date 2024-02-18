using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    public Transform[] point;
    void Start()
    {
        gameObject.transform.SetParent(null);
    }

}
