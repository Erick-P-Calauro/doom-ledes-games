using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private Dictionary<CollectablesEnum, int> inventory = new Dictionary<CollectablesEnum, int>();

    void Start()
    {
        inventory.Add(CollectablesEnum.Plastico, 0);
        inventory.Add(CollectablesEnum.Metal, 0);
        inventory.Add(CollectablesEnum.Organico, 0);
        inventory.Add(CollectablesEnum.Vidro, 0);
        inventory.Add(CollectablesEnum.Papel, 0);
    }

    public void AddItem(CollectablesEnum collectable)
    {
        inventory[collectable] += 1;
    }
}