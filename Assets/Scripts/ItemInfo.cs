using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Equipment,
    Portion,
    Etc,
    Count,
}

[CreateAssetMenu(fileName = "new ItemInfo", menuName = "GameData/ItemInfo")]
public class ItemInfo : ScriptableObject
{
    [SerializeField] Type itemType;
    [SerializeField] string itemName;
    [SerializeField] Sprite itemImage;
    [SerializeField] Sprite costImage;
    [SerializeField] int cost;
    [SerializeField] int maxCount;

    public Type ItemType => itemType;
    public string ItemName => itemName;
    public Sprite ItemImage => itemImage;
    public Sprite CostImage => costImage;
    public int Cost => cost;
    public int MaxCount => maxCount;
}
