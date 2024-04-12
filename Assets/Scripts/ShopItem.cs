using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] Text itemName;
    [SerializeField] Image itemImage;
    [SerializeField] Image costImage;
    [SerializeField] Text costText;

    ItemInfo itemInfo;

    public void Setting(ItemInfo info)
    {
        itemInfo = info;
        itemName.text = itemInfo.ItemName;
        itemImage.sprite = itemInfo.ItemImage;
        costImage.sprite = itemInfo.CostImage;
        costText.text = itemInfo.Cost.ToString();
    }

    public void BuyItem()
    {
        Inventory.Instance.AddInventory(itemInfo);
    }
}
