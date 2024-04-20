using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Shop : MonoBehaviour
{
    [SerializeField] Transform itemParent;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Text shopNpcName;
    [SerializeField] Image sellButton;

    public static Shop Instance;

    [HideInInspector] public bool isSell = false;

    Color selectedColor = new Color(0.3f, 0.6f, 1f, 1f);

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public void SettingShopInfo(int listCount, string name, ItemInfo[] itemInfo)
    {
        shopNpcName.text = name;

        if (itemParent.childCount <= listCount)
        {
            for (int i = 0; i < listCount; i++)
            {
                if (i < itemParent.childCount)
                {
                    itemParent.GetChild(i).GetComponent<ShopItem>().Setting(itemInfo[i]);
                    itemParent.GetChild(i).gameObject.SetActive(true);
                }

                else
                {
                    ShopItem item = Instantiate(itemPrefab, itemParent).GetComponent<ShopItem>();
                    item.Setting(itemInfo[i]);
                }
            }
        }

        else
        {
            for (int i = 0; i < itemParent.childCount; i++)
            {
                if (i < listCount)
                    itemParent.GetChild(i).GetComponent<ShopItem>().Setting(itemInfo[i]);

                else
                    itemParent.GetChild(i).gameObject.SetActive(false);

            }
        }
    }

    public void SellItem()
    {
        isSell = !isSell;
        sellButton.color = isSell ? selectedColor : Color.white;
    }
}
