using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenSlot : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Text itemCount;

    public void SettingSlotInfo(Sprite image, int count)
    {
        itemImage.sprite = image;
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
        itemImage.gameObject.SetActive(false);
        itemCount.gameObject.SetActive(false);
    }
}
