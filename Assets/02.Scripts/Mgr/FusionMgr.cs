using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FusionMgr : MonoBehaviour
{
    InGameMgr gameMgr = null;
    public Button FusionOkBtn = null;
    public Weapon weaponFusionItem = null;
    int emptyItem = -1;

    private void Start()
    {
        gameMgr = GetComponent<InGameMgr>();

        if(FusionOkBtn != null)
        {
            FusionOkBtn.onClick.AddListener(()=> { FusionOkFuc(); });
        }

        //InvokeRepeating("FusionItemView", 0, 0.3f);
    }

    //private void Update()
    //{
    //    FusionItemView();
    //}

    void FusionOkFuc()
    {
        // 2개의 재료를 없애고 조합 아이템을 인벤토리로 올린다.

        if (InvenMgr.FusionList[2].WeaponId == -1)
            return;

        // 인벤토리에 빈 공간을 찾는다.
        for (int i = 0; i < InvenMgr.inventoryList.Count; i++)
        {
            if (InvenMgr.inventoryList[i].WeaponId == -1)
            {
                emptyItem = i;
                break;
            }
        }

        InvenMgr.inventoryList[emptyItem] = InvenMgr.FusionList[2];

        Sprite temp = gameMgr.ItemArray[emptyItem].GetComponent<Image>().sprite;
        gameMgr.ItemArray[emptyItem].GetComponent<Image>().sprite = gameMgr.FusionArray[2].GetComponent<Image>().sprite;
        gameMgr.FusionArray[2].GetComponent<Image>().sprite = temp;
        //gameMgr.ItemArray[emptyItem].GetComponent<Image>().sprite = gameMgr.FusionArray[2].GetComponent<Image>().sprite;

        gameMgr.FusionItemArray[emptyItem].GetComponent<Image>().sprite = gameMgr.ItemArray[emptyItem].GetComponent<Image>().sprite;

        //gameMgr.FusionItemArray[emptyItem].GetComponent<Image>().sprite = gameMgr.FusionArray[2].GetComponent<Image>().sprite;
        gameMgr.ItemArray[emptyItem].image.color = gameMgr.weaponActiveColor;
        gameMgr.FusionItemArray[emptyItem].image.color = gameMgr.weaponActiveColor;

        //2개 재료 삭제
        Weapon weapon= new Weapon();
        InvenMgr.FusionList[0] = weapon;
        weapon = new Weapon();
        InvenMgr.FusionList[1] = weapon;



        gameMgr.FusionArray[0].GetComponent<Image>().sprite = null;
        gameMgr.FusionArray[1].GetComponent<Image>().sprite = null;
        gameMgr.FusionArray[2].GetComponent<Image>().sprite = null;
        gameMgr.FusionArray[0].image.color = gameMgr.weaponDisableColor;
        gameMgr.FusionArray[1].image.color = gameMgr.weaponDisableColor;
        gameMgr.FusionArray[2].image.color = gameMgr.weaponDisableColor;


    }

    public void FusionItemView()
    {
        if (InvenMgr.FusionList[0].WeaponId == -1)
        {
            gameMgr.FusionArray[2].image.color = gameMgr.weaponDisableColor;
            return;
        }
        if (InvenMgr.FusionList[1].WeaponId == -1)
        {
            gameMgr.FusionArray[2].image.color = gameMgr.weaponDisableColor;
            return;
        }

        int FirstId = InvenMgr.FusionList[0].WeaponId;
        int SecondId = InvenMgr.FusionList[1].WeaponId;

        Debug.Log(FirstId);
        Debug.Log(SecondId);

        if(FirstId == 0 && SecondId == 0)
        {
            gameMgr.FusionArray[2].GetComponent<Image>().sprite = gameMgr.WeaponImg[5];
            InvenMgr.FusionList[2] = WeaponMgr.weaponList[5];
        }
        else if(FirstId == 1 && SecondId == 1)
        {
            gameMgr.FusionArray[2].GetComponent<Image>().sprite = gameMgr.WeaponImg[6];
            InvenMgr.FusionList[2] = WeaponMgr.weaponList[6];
        }
        else if (FirstId == 2 && SecondId == 2)
        {
            gameMgr.FusionArray[2].GetComponent<Image>().sprite = gameMgr.WeaponImg[7];
            InvenMgr.FusionList[2] = WeaponMgr.weaponList[7];
        }
        else if (FirstId == 3 && SecondId == 3)
        {
            gameMgr.FusionArray[2].GetComponent<Image>().sprite = gameMgr.WeaponImg[8];
            InvenMgr.FusionList[2] = WeaponMgr.weaponList[8];
        }
        else if (FirstId == 4 && SecondId == 4)
        {
            gameMgr.FusionArray[2].GetComponent<Image>().sprite = gameMgr.WeaponImg[9];
            InvenMgr.FusionList[2] = WeaponMgr.weaponList[9];
        }
        else
        {
            gameMgr.FusionArray[2].image.color = gameMgr.weaponDisableColor;
            InvenMgr.FusionList[2].Init();
            return;
        }


        gameMgr.FusionArray[2].image.color = gameMgr.weaponActiveColor;
    }
}
