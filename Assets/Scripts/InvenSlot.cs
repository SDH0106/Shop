using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenSlot : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Text itemCount;

    ItemInfo itemInfo;
    Inventory inventory;
    Shop shop;

    int indexNum;

    bool isDrag = false;

    private void Start()
    {
        inventory = Inventory.Instance;
        indexNum = transform.GetSiblingIndex();
    }

    public void SettingSlotInfo(ItemInfo info, int count)
    {
        itemInfo = info;

        itemImage.sprite = info.ItemImage;
        itemImage.gameObject.SetActive(true);

        if (count > 1)
        {
            itemCount.text = count.ToString();
            itemCount.gameObject.SetActive(true);
        }

        else
            itemCount.gameObject.SetActive(false);
    }

    public void OffSlot()
    {
        itemInfo = null;

        itemImage.gameObject.SetActive(false);
        itemCount.gameObject.SetActive(false);
    }

    public void SellItem()
    {
        if (shop == null)
            shop = Shop.Instance;

        if (itemInfo != null && shop.isSell)
            inventory.DelInventory(indexNum);
    }

    public void GetIndexNum()
    {
        inventory.endDragIndex = indexNum;
    }

    public void DragStart()
    {
        if (shop == null)
            shop = Shop.Instance;

        if (itemInfo != null && !shop.isSell && !isDrag)
        {
            inventory.SettingDragUi(indexNum);
            isDrag = true;
        }
    }

    public void Dragging()
    {
        if (isDrag)
            inventory.dragUI.rectTransform.position = Input.mousePosition;
    }

    public void DragEnd()
    {
        if (isDrag)
        {
            inventory.MoveItem();
            isDrag = false;
        }
    }
}
