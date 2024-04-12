using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [SerializeField] NpcInfo npcInfo;
    [SerializeField] TextMeshPro npcName;
    [SerializeField] GameObject menu;

    bool isMenuOpen;

    private void Start()
    {
        npcName.text = npcInfo.NpcName;
        isMenuOpen = false;
        menu.SetActive(isMenuOpen);
    }

    private void OnMouseUp()
    {
        menu.transform.localPosition = transform.right * 2;
        isMenuOpen = !isMenuOpen;
        menu.SetActive(isMenuOpen);
    }

    public void TransferNpcInfoToShop()
    {
        Shop shop = Shop.Instance;
        shop.SettingShopInfo(npcInfo.ItemInfos.Length, npcInfo.NpcName, npcInfo.ItemInfos);
    }
}
