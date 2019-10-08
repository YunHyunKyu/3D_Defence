using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenMgr : MonoBehaviour
{
    public static List<Weapon> inventoryList = new List<Weapon>();
    public static List<Weapon> EquipList = new List<Weapon>();
    public static List<Weapon> FusionList = new List<Weapon>();

    private void Awake()
    {
        for(int i = 0; i < 20; i++)
        {
            inventoryList.Add(new Weapon());
        }

        for(int i = 0; i < 6; i++)
        {
            EquipList.Add(new Weapon());
        }

        for(int i = 0; i < 3; i++)
        {
            FusionList.Add(new Weapon());
        }
    }
}
