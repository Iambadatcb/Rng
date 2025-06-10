using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Canvas inventory;

    void Start()
    {
        inventory.enabled = false;
    }
    public void InventoryOpen()
    {
        inventory.enabled = true;

    }
    public void InventoryClose()
    {
        inventory.enabled = false;
    }
    
}
