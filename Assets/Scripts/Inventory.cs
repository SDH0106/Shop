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
    public Image dragUI;

    [HideInInspector] public int crystalCount;

    public static Inventory Instance;

    Dictionary<int, ItemInfo[]> invenItems;
    ItemInfo[] selectedItems;

    Dictionary<int, int[]> invenCounts;
    int[] selectedCounts;

    int menuNum;
    int beforeMenuNum;

    int startDragIndex;
    [HideInInspector] public int endDragIndex;

    Color selectedColor = new Color(0.3f, 0.6f, 1f, 1f);

    SaveManager saveManager;

    ItemInfo[] allItems;
    int[] allItemCounts;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        saveManager = SaveManager.Instance;
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

        allItems = new ItemInfo[slotParent.childCount * invenItems.Count];
        allItemCounts = new int[allItems.Length];

        SelectWhichMenu(menuNum);

        //LoadInfo();

        dragUI.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    void LoadInfo()
    {
        /*int count = 0;

        for (int i = 0; i < allItems.Length; i++)
        {
            if (i < slotParent.childCount)
                count = 0;


            else if (i < slotParent.childCount * 2)
                count = 1;

            else
                count = 2;

            invenItems[count][i - slotParent.childCount * count] = allItems[i];
            invenCounts[count][i - slotParent.childCount * count] = allItemCounts[i];
        }
*/

        saveManager.LoadJsonFile();

        saveManager.crystal = crystalCount;
        crystal.text = crystalCount.ToString();

        saveManager.menuIndex = menuNum;
        SelectWhichMenu(menuNum);

        for (int i = 0; i < allItems.Length; i++)
        {
            if (saveManager.saveItemInfos[i] != null)
                allItems[i] = saveManager.saveItemInfos[i];
        }

        for (int i = 0; i < invenItems.Count; i++)
        {
            for (int j = 0; j < invenItems[i].Length; j++)
            {
                invenItems[i][j] = allItems[j + slotParent.childCount * i];
            }
        }
    }

    void SaveInfo()
    {
        for (int i = 0; i < invenItems.Count ; i++)
        {
            for (int j = 0; j < invenItems[i].Length; j++)
                allItems[j + slotParent.childCount * i] = invenItems[i][j];
        }
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

    public void DelInventory(int indexNum)
    {
        crystalCount += selectedItems[indexNum].Cost;

        selectedCounts[indexNum]--;

        if (selectedCounts[indexNum] == 0)
            selectedItems[indexNum] = null;

        crystal.text = crystalCount.ToString();

        SettingInvenSlot();
    }

    public void SettingInvenSlot()
    {
        for (int i = 0; i < selectedItems.Length; i++)
        {
            if (selectedItems[i] != null)
                slots[i].GetComponent<InvenSlot>().SettingSlotInfo(selectedItems[i], selectedCounts[i]);

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

    public void SettingDragUi(int indexNum)
    {
        startDragIndex = indexNum;
        dragUI.sprite = selectedItems[indexNum].ItemImage;
        dragUI.gameObject.SetActive(true);
    }

    public void DragEnd()
    {
        dragUI.gameObject.SetActive(false);

        if (endDragIndex != startDragIndex)
        {
            ItemInfo startInfo = selectedItems[startDragIndex];
            int startCount = selectedCounts[startDragIndex];

            ItemInfo endInfo = selectedItems[endDragIndex];
            int endCount = selectedCounts[endDragIndex];

            selectedItems[endDragIndex] = startInfo;
            selectedCounts[endDragIndex] = startCount;

            selectedItems[startDragIndex] = endInfo;
            selectedCounts[startDragIndex] = endCount;

            SettingInvenSlot();
        }
    }
}
