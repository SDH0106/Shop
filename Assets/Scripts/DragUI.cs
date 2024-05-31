using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragUI : MonoBehaviour
{
    [SerializeField] Image dragItemImage;

    public static DragUI Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        dragItemImage.gameObject.SetActive(false);
    }

    public void OnDragUI(ItemInfo selectedItem)
    {
        dragItemImage.sprite = selectedItem.ItemImage;
        dragItemImage.gameObject.SetActive(true);
    }

    public void MoveDragUI()
    {
        GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public void OffDragUI()
    {
        dragItemImage.gameObject.SetActive(false);
    }
}
