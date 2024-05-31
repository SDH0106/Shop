using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] Text crystal;
    [SerializeField] Transform menuParent;
    [SerializeField] Transform slotParent;
    [SerializeField] GameObject[] slots;

    [HideInInspector] public int crystalCount;

    public static Inventory Instance;

    Dictionary<int, ItemInfo[]> invenItems;
    ItemInfo[] selectedItems;

    Dictionary<int, int[]> invenCounts;
    int[] selectedCounts;

    int menuNum;
    int beforeMenuNum;

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

        LoadInfo();

        gameObject.SetActive(false);
    }

    void LoadInfo()
    {
        saveManager.LoadJsonFile(allItems, allItemCounts);

        LoadItemToArray();
        ArrayToDictionary();

        crystalCount = saveManager.saveCrystalData;
        crystal.text = crystalCount.ToString();

        SelectWhichMenu(saveManager.saveMenuIndexData);
    }

    void LoadItemToArray()
    {
        for (int i = 0; i < allItems.Length; i++)
        {
            if (saveManager.saveItemInfos[i] != null)
            {
                allItems[i] = saveManager.saveItemInfos[i];
                allItemCounts[i] = saveManager.saveDatas[i].equipItemCount;
            }
        }
    }

    void ArrayToDictionary()
    {
        for (int i = 0; i < invenItems.Count; i++)
        {
            for (int j = 0; j < invenItems[i].Length; j++)
            {
                invenItems[i][j] = allItems[j + slotParent.childCount * i];
                invenCounts[i][j] = allItemCounts[j + slotParent.childCount * i];
            }
        }
    }

    public void SaveInfo()
    {
        DictionaryToArray();
        saveManager.MakeJsonFile(crystalCount, menuNum, allItems, allItemCounts);
    }

    void DictionaryToArray()
    {
        for (int i = 0; i < invenItems.Count; i++)
        {
            for (int j = 0; j < invenItems[i].Length; j++)
            {
                allItems[j + slotParent.childCount * i] = invenItems[i][j];
                allItemCounts[j + slotParent.childCount * i] = invenCounts[i][j];
            }
        }
    }

    /*public void AddInventory(ItemInfo itemInfo)
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
    }*/

    public void AddInventory(ItemInfo itemInfo)
    {
        int typeNum = (int)itemInfo.ItemType;

        if (!CheckSameItemInInven(typeNum, itemInfo))
        {
            int emptyIndex = FindEmptyIndex(typeNum);

            if (emptyIndex == -1)
                return;

            invenItems[typeNum][emptyIndex] = itemInfo;
            invenCounts[typeNum][emptyIndex] = 1;
        }

        UpdateCrystalCount(-itemInfo.Cost);

        UpdateInvetory();
    }

    bool CheckSameItemInInven(int typeNum, ItemInfo itemInfo)
    {
        for (int i = 0; i < invenItems[typeNum].Length; i++)
        {
            if (invenItems[typeNum][i] == itemInfo && invenCounts[typeNum][i] < itemInfo.MaxCount)
            {
                invenCounts[typeNum][i]++;
                return true;
            }
        }

        return false;
    }

    int FindEmptyIndex(int typeNum)
    {
        for (int i = 0; i < invenItems[typeNum].Length; i++)
        {
            if (invenItems[typeNum][i] == null)
                return i;
        }

        return -1;
    }

    void UpdateCrystalCount(int cost)
    {
        crystalCount += cost;
        crystal.text = crystalCount.ToString();
    }

    public void DelInventory(int indexNum)
    {
        selectedCounts[indexNum]--;
        UpdateCrystalCount(selectedItems[indexNum].Cost);

        if (selectedCounts[indexNum] == 0)
            selectedItems[indexNum] = null;

        UpdateInvetory();
    }

    public void UpdateInvetory()
    {
        for (int i = 0; i < selectedItems.Length; i++)
            slots[i].GetComponent<InvenSlot>().SettingSlotInfo(selectedItems[i], selectedCounts[i]);
    }

    public void SelectWhichMenu(int num)
    {
        menuNum = num;

        selectedItems = invenItems[menuNum];
        selectedCounts = invenCounts[menuNum];

        UpdateMenuColors();
        UpdateInvetory();
    }

    private void UpdateMenuColors()
    {
        menuParent.GetChild(menuNum).GetComponent<Image>().color = selectedColor;

        if (menuNum != beforeMenuNum)
            menuParent.GetChild(beforeMenuNum).GetComponent<Image>().color = Color.white;

        beforeMenuNum = menuNum;
    }

    public void SettingDragUI(int indexNum)
    {
        DragUI.Instance.OnDragUI(selectedItems[indexNum]);
    }

    public void MoveItems(int startIndex, int endIndex)
    {
        if (endIndex == startIndex)
            return;

        /*if (startInfo != endInfo)
        {
            selectedItems[endIndex] = startInfo;
            selectedCounts[endIndex] = startCount;

            selectedItems[startIndex] = endInfo;
            selectedCounts[startIndex] = endCount;
        }

        else
        {
            int totalCount = startCount + endCount;

            if (totalCount <= startInfo.MaxCount)
            {
                selectedItems[startIndex] = null;
                selectedCounts[startIndex] = 0;

                selectedCounts[endIndex] = totalCount;
            }

            else
            {
                if (selectedCounts[endIndex] != selectedItems[endIndex].MaxCount && selectedCounts[startIndex] != selectedItems[startIndex].MaxCount)
                {
                    selectedCounts[endIndex] = selectedItems[endIndex].MaxCount;
                    selectedCounts[startIndex] = totalCount - selectedCounts[endIndex];
                }

                else
                {
                    selectedCounts[endIndex] = startCount;
                    selectedCounts[startIndex] = endCount;
                }
            }
        }*/

        if (selectedItems[startIndex] != selectedItems[endIndex])
            SwapItems(startIndex, endIndex);

        else
            MergeItems(startIndex, endIndex);

        UpdateInvetory();
    }

    void SwapItems(int startIndex, int endIndex)
    {
        ItemInfo tempItem = selectedItems[startIndex];
        int tempCount = selectedCounts[startIndex];

        selectedItems[startIndex] = selectedItems[endIndex];
        selectedCounts[startIndex] = selectedCounts[endIndex];

        selectedItems[endIndex] = tempItem;
        selectedCounts[endIndex] = tempCount;
    }

    void MergeItems(int startIndex, int endIndex)
    {
        int totalCount = selectedCounts[startIndex] + selectedCounts[endIndex];
        int maxCount = selectedItems[startIndex].MaxCount;

        if (totalCount <= maxCount)
        {
            selectedItems[startIndex] = null;
            selectedCounts[startIndex] = 0;

            selectedCounts[endIndex] = totalCount;
        }

        else
        {
            selectedCounts[endIndex] = Mathf.Clamp(totalCount - selectedCounts[endIndex], 1, maxCount);
            selectedCounts[startIndex] = totalCount - selectedCounts[endIndex];
        }
    }
}