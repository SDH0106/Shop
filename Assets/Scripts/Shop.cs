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

    public static Shop Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public void SettingShopInfo(int count, string name, ItemInfo[] itemInfo)
    {
        shopNpcName.text = name;

        if (itemParent.childCount <= count)
        {
            for (int i = 0; i < count; i++)
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
                if (i < count)
                    itemParent.GetChild(i).GetComponent<ShopItem>().Setting(itemInfo[i]);

                else
                    itemParent.GetChild(i).gameObject.SetActive(false);

            }
        }
    }
}
