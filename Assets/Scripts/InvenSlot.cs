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
    DragUI dragUI;

    int indexNum;
    static int currentIndex;

    bool isDragging = false;

    private void Start()
    {
        inventory = Inventory.Instance;
        dragUI = DragUI.Instance;
        indexNum = transform.GetSiblingIndex();
    }

    public void SettingSlotInfo(ItemInfo info, int count)
    {
        itemInfo = info;

        if (itemInfo != null)
            OnSlot(info, count);

        else
            OffSlot();
    }

    void OnSlot(ItemInfo info, int count)
    {
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

    void OffSlot()
    {
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
        currentIndex = indexNum;
    }

    public void DragStart()
    {
        if (shop == null)
            shop = Shop.Instance;

        if (itemInfo != null && !shop.isSell && !isDragging)
        {
            inventory.SettingDragUI(indexNum);
            isDragging = true;
        }
    }

    public void Dragging()
    {
        if (isDragging)
            dragUI.MoveDragUI();
    }

    public void DragEnd()
    {
        if (isDragging)
        {
            inventory.MoveItems(indexNum, currentIndex);
            dragUI.OffDragUI();
            isDragging = false;
        }
    }
}
