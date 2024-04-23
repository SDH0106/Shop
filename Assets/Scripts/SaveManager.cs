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

    public SaveData(int index, string equipItems, int equipItemCount)
    {
        this.index = index;
        this.equipItem = equipItems;
        this.equipItemCount = equipItemCount;
    }
}

public class SaveManager : MonoBehaviour
{
    [SerializeField] ItemInfo[] itemInfos;
    public SaveData[] saveDatas;
    public int crystal;
    public int menuIndex;
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
            crystal = saveJson.crystal;
            menuIndex = saveJson.menuIndex;

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

    /*[SerializeField] ItemInfo[] itemInfos;

        int count;

        class SaveJson
        {
            public ItemInfo[] items;

            public SaveJson(ItemInfo[] _items)
            {
                items = _items;
            }
        }

        [ContextMenu("MakeJsonFile")]
        public void MakeJsonFile()
        {
            count = 0;
            SaveJson itemJson = new SaveJson(GetItemArray());

            string json = JsonUtility.ToJson(itemJson);
            SaveFile("ItemSaveData", json);
        }

        ItemInfo[] GetItemArray()
        {
            ItemInfo[] items = new ItemInfo[count];

            for(int i = 0; i < items.Length; i++)
                items[i] = itemInfos[0];

            return items;
        }

        public void AddJsonFile()
        {
            SaveJson itemJson = new SaveJson(AddStatArray(1));

            string json = JsonUtility.ToJson(itemJson);
            SaveFile("ItemSaveData", json);
        }

        private ItemInfo[] AddStatArray(int add)
        {
            count += add;

            ItemInfo[] stats = new ItemInfo[count];

            string json = LoadFile("monsterStatData");
            object convert = JsonUtility.FromJson(json, typeof(SaveJson));
            SaveJson itemJson = convert as SaveJson;
            ItemInfo[] beforeStats = itemJson.items;

            for (int i = 0; i < beforeStats.Length; i++)
            {
                stats[i] = beforeStats[i];
            }

            for (int j = beforeStats.Length; j < stats.Length; j++)
            {
                ItemInfo newStat = new ItemInfo("¸ó½ºÅÍ", 0, 0, 0, 0, 0, 0);
                stats[j] = newStat;
            }

            return stats;
        }

        void SaveFile(string fileName, string text)
        {
            string path = string.Format("{0}/{1}.txt", Application.dataPath, fileName);
            StreamWriter sw = new StreamWriter(path);
            sw.Write(text);
            sw.Close();
        }

        [ContextMenu("LoadJsonFile")]
        public void LoadJsonFile()
        {
            string json = LoadFile("itemSaveData");
            object convert = JsonUtility.FromJson(json, typeof(SaveJson));
            SaveJson itemJson = convert as SaveJson;
            ItemInfo[] items = itemJson.items;

            itemInfos = items;
        }

        string LoadFile(string fileName) 
        {
            string path = string.Format("{0}/{1}.txt", Application.dataPath, fileName);
            StreamReader sr = new StreamReader(path);
            sr.Close();

            return sr.ReadToEnd();
        }
     */
}