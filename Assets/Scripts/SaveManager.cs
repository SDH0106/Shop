using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int index;
    public string equipItem;
    public int equipItemCount;

    public SaveData(int index, string equipItem, int equipItemCount)
    {
        this.index = index;
        this.equipItem = equipItem;
        this.equipItemCount = equipItemCount;
    }
}

public class SaveManager : MonoBehaviour
{
    [SerializeField] ItemInfo[] itemInfos;
    public SaveData[] saveDatas;
    [HideInInspector] public int saveCrystalData;
    [HideInInspector] public int saveMenuIndexData;
    [HideInInspector] public ItemInfo[] saveItemInfos;

    public static SaveManager Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void OnApplicationQuit()
    {
        Inventory inven = Inventory.Instance;
        inven.SaveInfo();
    }

    class SaveJson
    {
        public int crystal;
        public int menuIndex;
        public SaveData[] saves;

        public SaveJson(int crystal, int menuIndex, SaveData[] saves)
        {
            this.crystal = crystal;
            this.menuIndex = menuIndex;
            this.saves = saves;
        }
    }

    public void MakeJsonFile(int crystal, int menuIndex, ItemInfo[] saveItems, int[] saveItemCounts)
    {
        SaveJson itemJson = new SaveJson(crystal, menuIndex, GetItemArray(saveItems, saveItemCounts));

        string json = JsonUtility.ToJson(itemJson);
        SaveFile("ItemSaveData", json);
    }

    SaveData[] GetItemArray(ItemInfo[] saveItems, int[] saveItemCounts)
    {
        SaveData[] saveData = new SaveData[saveItems.Length];

        for (int i = 0; i < saveData.Length; i++)
        {
            if (saveItems[i] != null)
                saveData[i] = new SaveData(i, saveItems[i].ItemName, saveItemCounts[i]);
        }

        return saveData;
    }

    void SaveFile(string fileName, string text)
    {
        string path = string.Format("{0}/{1}.txt", Application.dataPath, fileName);
        StreamWriter sw = new StreamWriter(path);
        sw.Write(text);
        sw.Close();
    }

    public void LoadJsonFile(ItemInfo[] saveItems, int[] saveItemCounts)
    {
        string json = LoadFile("ItemSaveData", saveItems, saveItemCounts);

        if (json != null)
        {
            object convert = JsonUtility.FromJson(json, typeof(SaveJson));
            SaveJson saveJson = convert as SaveJson;

            saveDatas = saveJson.saves;
            saveCrystalData = saveJson.crystal;
            saveMenuIndexData = saveJson.menuIndex;

            saveItemInfos = new ItemInfo[saveDatas.Length];

            for (int i = 0; i < saveDatas.Length; i++)
            {
                if (saveDatas[i] != null)
                {
                    for (int j = 0; j < itemInfos.Length; j++)
                    {
                        if (saveDatas[i].equipItem == itemInfos[j].ItemName)
                            saveItemInfos[i] = itemInfos[j];
                    }
                }
            }
        }
    }

    string LoadFile(string fileName, ItemInfo[] saveItems, int[] saveItemCounts) 
    {
        string path = string.Format("{0}/{1}.txt", Application.dataPath, fileName);

        if (!File.Exists(path))
            MakeJsonFile(10000, 0, saveItems, saveItemCounts);

        StreamReader sr = new StreamReader(path);
        string readToEnd = sr.ReadToEnd();
        sr.Close();
        return readToEnd;
    }
}