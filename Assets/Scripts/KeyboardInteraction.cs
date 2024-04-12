using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInteraction : MonoBehaviour
{
    Inventory inventory;

    private void Start()
    {
        inventory = Inventory.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            inventory.gameObject.SetActive(!inventory.gameObject.activeSelf);
    }
}
