using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [SerializeField] NpcInfo npcInfo;
    [SerializeField] TextMeshPro npcName;
    [SerializeField] GameObject menu;

    private void Start()
    {
        npcName.text = npcInfo.NpcName;
        menu.SetActive(false);
    }

    private void OnMouseUp()
    {
        menu.transform.localPosition = transform.right * 2;
        menu.SetActive(true);
    }

    public void TransferNpcInfoToShop()
    {
        Shop shop = Shop.Instance;
        shop.SettingShopInfo(npcInfo.ItemInfos.Length, npcInfo.NpcName, npcInfo.ItemInfos);
    }
}
