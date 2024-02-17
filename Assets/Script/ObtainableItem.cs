using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData{
    public enum ItemType{
        Key,Knife,Coin
    }

    public ItemType itemType;
}
public class ObtainableItem : MonoBehaviour
{
    [SerializeField]
    ItemData itemType;
    public ItemData.ItemType GetItemType(){
        return itemType.itemType;
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            other.GetComponent<Player>().GetItem(gameObject);
        }
    }
}
