using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new NpcInfo", menuName = "GameData/NpcInfo")]
public class NpcInfo : ScriptableObject
{
    [SerializeField] string npcName;
    [SerializeField] ItemInfo[] itemInfos;

    public string NpcName => npcName;
    public ItemInfo[] ItemInfos => itemInfos;
}
