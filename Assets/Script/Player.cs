using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float slowMoveSpeedMultiply = 0.6f;

    public bool isCrawling = false;
    public bool isMoving = false;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        toggleCrawl();
        playerMove();


    }

    void toggleCrawl(){
        if(Input.GetKeyDown(KeyCode.LeftShift)) isCrawling = !isCrawling;
    }

    void playerMove(){
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");

        isMoving = hInput != 0 || vInput != 0;

        Vector2 mvDirection = new Vector2(hInput,vInput).normalized;

        if(isCrawling) mvDirection *= slowMoveSpeedMultiply;

        transform.position = (Vector2)transform.position+mvDirection*moveSpeed*Time.deltaTime;
    }



}
