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

    [SerializeField]
    private int key_Amount = 0;

    private GameObject soundRadius;

    void Start()
    {
        soundRadius = transform.Find("SoundRadius").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        toggleCrawl();
        playerMove();
        soundRadius.SetActive(isMoving && !StageManager.instance.stageLight);
    }

    void toggleCrawl()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) isCrawling = !isCrawling;
        if (isCrawling) soundRadius.transform.localScale = new Vector2(0.75f, 0.75f);
        else soundRadius.transform.localScale = new Vector2(1, 1);
    }

    void playerMove()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        isMoving = hInput != 0 || vInput != 0;

        Vector2 mvDirection = new Vector2(hInput, vInput).normalized;

        if (isCrawling) mvDirection *= slowMoveSpeedMultiply;

        transform.position = (Vector2)transform.position + mvDirection * moveSpeed * Time.deltaTime;
    }

    public void GetItem(GameObject input)
    {
        ItemData.ItemType itemType = input.GetComponent<ObtainableItem>().GetItemType();
        if (itemType == ItemData.ItemType.Key)
        {
            key_Amount++;
        }
        Destroy(input);
    }

    public bool KeyUse(){
        if(key_Amount>0){
            key_Amount--;
            return true;
        }
        return false;
    }

}
