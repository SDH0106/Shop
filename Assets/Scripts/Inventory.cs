using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] Text crystal;
    [SerializeField] Transform menuParent;
    [SerializeField] Transform slotParent;
    [SerializeField] GameObject[] slots;

    int crystalCount;

    public static Inventory Instance;

    Dictionary<int, ItemInfo[]> invenItems;
    ItemInfo[] selectedItems;

    Dictionary<int, int[]> invenCounts;
    int[] selectedCounts;

    int menuNum;
    int beforeMenuNum;

    Color selectedColor = new Color(0.3f, 0.6f, 1f, 1f);

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        menuNum = 0;

        crystalCount = 67890;
        crystal.text = crystalCount.ToString();

        invenItems = new Dictionary<int, ItemInfo[]>()
        {
            { (int)Type.Equipment, new ItemInfo[slotParent.childCount] },
            { (int)Type.Portion, new ItemInfo[slotParent.childCount] },
            { (int)Type.Etc, new ItemInfo[slotParent.childCount] },
        };
        selectedItems = new ItemInfo[slotParent.childCount];

        invenCounts = new Dictionary<int, int[]>()
        {
            { (int)Type.Equipment, new int[slotParent.childCount] },
            { (int)Type.Portion, new int[slotParent.childCount] },
            { (int)Type.Etc, new int[slotParent.childCount] },
        };
        selectedCounts = new int[slotParent.childCount];

        SelectWhichMenu(menuNum);

        gameObject.SetActive(false);
    }

    public void AddInventory(ItemInfo itemInfo)
    {
        bool isSame = false;
        int typeNum = (int)itemInfo.ItemType;
        int emptyArray = invenItems[typeNum].Length;

        for (int i = 0; i < invenItems[typeNum].Length; i++)
        {
            if (emptyArray == invenItems[typeNum].Length)
            {
                if (invenItems[typeNum][i] == null)
                    emptyArray = i;
            }

            if (invenItems[typeNum][i] == itemInfo)
            {
                if (invenCounts[typeNum][i] < itemInfo.MaxCount)
                {
                    isSame = true;
                    invenCounts[typeNum][i]++;
                    break;
                }
            }
        }

        if (!isSame)
        {
            if (emptyArray != invenItems[typeNum].Length)
            {
                invenItems[typeNum][emptyArray] = itemInfo;
                invenCounts[typeNum][emptyArray] = 1;
            }
        }

        crystalCount -= itemInfo.Cost;
        crystal.text = crystalCount.ToString();

        SettingInvenSlot();
    }

    public void SettingInvenSlot()
    {
        for (int i = 0; i < selectedItems.Length; i++)
        {
            if (selectedItems[i] != null)
                slots[i].GetComponent<InvenSlot>().SettingSlotInfo(selectedItems[i].ItemImage, selectedCounts[i]);

            else
                slots[i].GetComponent<InvenSlot>().OffSlot();
        }
    }

    public void SelectWhichMenu(int num)
    {
        menuNum = num;

        selectedItems = invenItems[menuNum];
        selectedCounts = invenCounts[menuNum];

        menuParent.GetChild(menuNum).GetComponent<Image>().color = selectedColor;

        if (menuNum != beforeMenuNum)
            menuParent.GetChild(beforeMenuNum).GetComponent<Image>().color = Color.white;

        beforeMenuNum = menuNum;

        SettingInvenSlot();
    }
}
