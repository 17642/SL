using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float slowMoveSpeedMultiply = 0.6f;

    Camera cam;
    [SerializeField]
    GameObject ps;
    

    public bool isCrawling = false;
    public bool isMoving = false;


    public int[] item_Amount = { 0, 0, 0 };

    private GameObject soundRadius;

    private Rigidbody2D rb;

    void Start()
    {
        soundRadius = transform.Find("SoundRadius").gameObject;
        rb=GetComponent<Rigidbody2D>();
        cam = transform.Find("LightCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (StageManager.instance.isStageOn)
        {
            toggleCrawl();
            
            soundRadius.SetActive(isMoving && !StageManager.instance.stageLight);
        }
    }

    private void FixedUpdate()
    {
        if(StageManager.instance.isStageOn)
        playerMove();
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

        //transform.position = (Vector2)transform.position + mvDirection * moveSpeed * Time.deltaTime;
        rb.MovePosition((Vector2)transform.position + mvDirection * moveSpeed * Time.deltaTime);
    }

    public void GetItem(GameObject input)
    {
        StageManager.instance.soundManager.PlaySound(Sound.Pickup);
        ItemData.ItemType itemType = input.GetComponent<ObtainableItem>().GetItemType();
        StageUIScript.PopupPanel.PopupPanel(itemType, true);
        item_Amount[(int)itemType]++;
        Destroy(input);
    }

    public bool UseItem(ItemData.ItemType type)
    {
        if (item_Amount[(int)type] > 0)
        {
            StageUIScript.PopupPanel.PopupPanel(type, false);
            item_Amount[(int)type]--;
            return true;
        }


        return false;
    }

    public void showDamageEffect(Transform Object)
    {
        StageManager.instance.soundManager.PlaySound(Sound.Attack);
        cam.GetComponent<CameraScript>().ShakeCam();
        Vector3 midPoint = (transform.position + Object.position) / 2;

        Instantiate(ps, midPoint, Quaternion.identity);
    }


}
