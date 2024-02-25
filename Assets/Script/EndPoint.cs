using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StageManager.instance.StageEnd(true,other.GetComponent<Player>().item_Amount[(int)ItemData.ItemType.Coin]);
        }
    }
}
