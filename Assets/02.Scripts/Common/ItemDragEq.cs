using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemDragEq : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    GraphicRaycaster gr;
    PointerEventData ped;

    Vector2 defaultPos = Vector2.zero;
    public int idx = -1;
    InGameMgr gameMgr = null;
    FusionMgr fusionMgr = null;
    private void Start()
    {
        gr = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);

        gameMgr = GameObject.Find("InGameMgr").GetComponent<InGameMgr>();
        fusionMgr = GameObject.Find("InGameMgr").GetComponent<FusionMgr>();
        for (int i = 0; i < gameMgr.ItemArray.Length; i++)
        {
            if (gameMgr.ItemArray[i] == this.GetComponent<Button>())
            {
                idx = i;
            }
        }

        for (int i = 0; i < gameMgr.FusionItemArray.Length; i++)
        {
            if (gameMgr.FusionItemArray[i] == this.GetComponent<Button>()) // 0 ~ 19 => 인벤토리
            {
                idx = i;
            }
        }

        for (int i = 0; i < gameMgr.EquipArray.Length; i++)
        {
            if(gameMgr.EquipArray[i] == this.GetComponent<Button>()) // 20 ~ 25 => 장착한 아이템
            {
                idx = i + 20;
            }
        }

        for(int i = 0; i <gameMgr.FusionArray.Length; i++)
        {
            if (gameMgr.FusionArray[i] == this.GetComponent<Button>()) // 26 ~ 27 => Fusion 재료
            {
                idx = i + 26;
            }
        }

        defaultPos = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        defaultPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentPos = Input.mousePosition;
        this.transform.position = currentPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.position = defaultPos;
        
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);
        if (results.Count != 0)
        {
            GameObject obj = results[0].gameObject;
            if (idx <= 19) // 인벤토리에서 DragDrop
            {
                if (obj.name == "Item")
                {
                    //Debug.Log(obj.GetComponent<ItemDragEq>().idx);
                    int swIdx = obj.GetComponent<ItemDragEq>().idx;
                    SwapInven(swIdx);

                    if (InvenMgr.inventoryList[idx].WeaponId == -1) // 스왑한 대상이 빈 공간이었다면 투명도를 빼준다.
                    {
                        gameMgr.ItemArray[idx].image.color = gameMgr.weaponDisableColor;
                        gameMgr.FusionItemArray[idx].image.color = gameMgr.weaponDisableColor;
                    }
                    else // 그것이 아니라면 활성화
                    {
                        gameMgr.ItemArray[idx].image.color = gameMgr.weaponActiveColor;
                        gameMgr.FusionItemArray[idx].image.color = gameMgr.weaponActiveColor;
                    }

                    if (InvenMgr.inventoryList[swIdx].WeaponId == -1) // 없는 애들 끼리 교환해도 투명도 비활성
                    {
                        gameMgr.ItemArray[swIdx].image.color = gameMgr.weaponDisableColor;
                        gameMgr.FusionItemArray[swIdx].image.color = gameMgr.weaponDisableColor;
                    }
                    else // 그것이 아니라면 활성화
                    {
                        gameMgr.ItemArray[swIdx].image.color = gameMgr.weaponActiveColor;
                        gameMgr.FusionItemArray[swIdx].image.color = gameMgr.weaponActiveColor;
                    }
                }
                else if (obj.name == "EqItem")
                {
                    int swIdx = obj.GetComponent<ItemDragEq>().idx;
                    SwapEquip(swIdx);

                    if (InvenMgr.inventoryList[idx].WeaponId == -1) // 스왑한 대상이 빈 공간이었다면 투명도를 빼준다.
                    {
                        gameMgr.ItemArray[idx].image.color = gameMgr.weaponDisableColor;
                    }
                    else // 그것이 아니라면 활성화
                    {
                        gameMgr.ItemArray[idx].image.color = gameMgr.weaponActiveColor;
                    }

                    if (InvenMgr.EquipList[swIdx-20].WeaponId == -1) // 없는 애들 끼리 교환해도 투명도 비활성
                    {
                        gameMgr.EquipArray[swIdx-20].image.color = gameMgr.weaponDisableColor;
                    }
                    else // 그것이 아니라면 활성화
                    {
                        gameMgr.EquipArray[swIdx-20].image.color = gameMgr.weaponActiveColor;
                    }
                }
                else if(obj.name == "Material")
                {
                    // InvenMgr에 있는 FusionList에 값을 넣어주고 이미지를 넣어준다. 그 후 조합 시 인벤토리에서 조합으로 사용한 재료들을 초기화 시켜주고
                    // 조합된 아이템을 인벤토리로 준다.
                    int Fidx = obj.GetComponent<ItemDragEq>().idx;
                    //Debug.Log("idx:" + idx); //0
                    //Debug.Log("Fidx:" + Fidx); //26
                    SwapFusion(Fidx);

                    if(InvenMgr.inventoryList[idx].WeaponId == -1)
                    {
                        gameMgr.ItemArray[idx].image.color = gameMgr.weaponDisableColor;
                        gameMgr.FusionItemArray[idx].image.color = gameMgr.weaponDisableColor;
                    }
                    else
                    {
                        gameMgr.ItemArray[idx].image.color = gameMgr.weaponActiveColor;
                        gameMgr.FusionItemArray[idx].image.color = gameMgr.weaponActiveColor;
                    }

                    if(InvenMgr.FusionList[Fidx-26].WeaponId == -1)
                    {
                        gameMgr.FusionArray[Fidx-26].image.color = gameMgr.weaponDisableColor;
                    }
                    else
                    {
                        gameMgr.FusionArray[Fidx - 26].image.color = gameMgr.weaponActiveColor;
                    }
                }
            }
            else if(idx > 19 && idx < 26) // 장비창에서 DragDrop , 20 ~ 25
            {
                if(obj.name == "Item")
                {
                    int swIdx = obj.GetComponent<ItemDragEq>().idx;
                    SwapInven(swIdx);

                    if (InvenMgr.EquipList[idx-20].WeaponId == -1) // 스왑한 대상이 빈 공간이었다면 투명도를 빼준다.
                    {
                        gameMgr.EquipArray[idx-20].image.color = gameMgr.weaponDisableColor;
                    }
                    else // 그것이 아니라면 활성화
                    {
                        gameMgr.EquipArray[idx-20].image.color = gameMgr.weaponActiveColor;
                    }

                    if (InvenMgr.inventoryList[swIdx].WeaponId == -1) // 없는 애들 끼리 교환해도 투명도 비활성
                    {
                        gameMgr.ItemArray[swIdx].image.color = gameMgr.weaponDisableColor;
                    }
                    else // 그것이 아니라면 활성화
                    {
                        gameMgr.ItemArray[swIdx].image.color = gameMgr.weaponActiveColor;
                    }
                }
                else if(obj.name == "EqItem")
                {
                    int swIdx = obj.GetComponent<ItemDragEq>().idx;
                    SwapEquip(swIdx);

                    if (InvenMgr.EquipList[idx-20].WeaponId == -1) // 스왑한 대상이 빈 공간이었다면 투명도를 빼준다.
                    {
                        gameMgr.EquipArray[idx-20].image.color = gameMgr.weaponDisableColor;
                    }
                    else // 그것이 아니라면 활성화
                    {
                        gameMgr.EquipArray[idx-20].image.color = gameMgr.weaponActiveColor;
                    }

                    if (InvenMgr.EquipList[swIdx - 20].WeaponId == -1) // 없는 애들 끼리 교환해도 투명도 비활성
                    {
                        gameMgr.EquipArray[swIdx - 20].image.color = gameMgr.weaponDisableColor;
                    }
                    else // 그것이 아니라면 활성화
                    {
                        gameMgr.EquipArray[swIdx - 20].image.color = gameMgr.weaponActiveColor;
                    }
                }
            }
            else if(idx > 25) // Fusion에서 드래그
            {
                if(obj.name == "Item")
                {
                    int swidx = obj.GetComponent<ItemDragEq>().idx;

                    SwapFusion(swidx);

                    if (InvenMgr.inventoryList[swidx].WeaponId == -1)
                    {
                        gameMgr.ItemArray[swidx].image.color = gameMgr.weaponDisableColor;
                        gameMgr.FusionItemArray[swidx].image.color = gameMgr.weaponDisableColor;
                    }
                    else
                    {
                        gameMgr.ItemArray[swidx].image.color = gameMgr.weaponActiveColor;
                        gameMgr.FusionItemArray[swidx].image.color = gameMgr.weaponActiveColor;
                    }

                    if (InvenMgr.FusionList[idx - 26].WeaponId == -1)
                    {
                        gameMgr.FusionArray[idx - 26].image.color = gameMgr.weaponDisableColor;
                    }
                    else
                    {
                        gameMgr.FusionArray[idx - 26].image.color = gameMgr.weaponActiveColor;
                    }
                }
            }

            fusionMgr.FusionItemView();
        }
    }

    public void SwapInven(int swIdx)
    {
        if (idx <= 19)
        {
            Weapon temp = InvenMgr.inventoryList[idx];
            InvenMgr.inventoryList[idx] = InvenMgr.inventoryList[swIdx];
            InvenMgr.inventoryList[swIdx] = temp;

            Sprite temp2 = gameMgr.ItemArray[idx].GetComponent<Image>().sprite;
            gameMgr.ItemArray[idx].GetComponent<Image>().sprite = gameMgr.ItemArray[swIdx].GetComponent<Image>().sprite;
            gameMgr.ItemArray[swIdx].GetComponent<Image>().sprite = temp2;

            Sprite temp3 = gameMgr.FusionItemArray[idx].GetComponent<Image>().sprite;
            gameMgr.FusionItemArray[idx].GetComponent<Image>().sprite = gameMgr.FusionItemArray[swIdx].GetComponent<Image>().sprite;
            gameMgr.FusionItemArray[swIdx].GetComponent<Image>().sprite = temp3;
        }
        else
        {
            Weapon temp = InvenMgr.EquipList[idx-20];
            InvenMgr.EquipList[idx - 20] = InvenMgr.inventoryList[swIdx];
            InvenMgr.inventoryList[swIdx] = temp;

            Sprite temp2 = gameMgr.EquipArray[idx-20].GetComponent<Image>().sprite;
            gameMgr.EquipArray[idx-20].GetComponent<Image>().sprite = gameMgr.ItemArray[swIdx].GetComponent<Image>().sprite;
            gameMgr.ItemArray[swIdx].GetComponent<Image>().sprite = temp2;
        }
    }

    public void SwapEquip(int swIdx)
    {
        if (idx <= 19)
        {
            Weapon temp = InvenMgr.inventoryList[idx];
            InvenMgr.inventoryList[idx] = InvenMgr.EquipList[swIdx - 20];
            InvenMgr.EquipList[swIdx - 20] = temp;

            Sprite temp2 = gameMgr.ItemArray[idx].GetComponent<Image>().sprite;
            gameMgr.ItemArray[idx].GetComponent<Image>().sprite = gameMgr.EquipArray[swIdx - 20].GetComponent<Image>().sprite;
            gameMgr.EquipArray[swIdx - 20].GetComponent<Image>().sprite = temp2;
        }
        else
        {
            Weapon temp = InvenMgr.EquipList[idx-20];
            InvenMgr.EquipList[idx - 20] = InvenMgr.EquipList[swIdx - 20];
            InvenMgr.EquipList[swIdx - 20] = temp;

            Sprite temp2 = gameMgr.EquipArray[idx-20].GetComponent<Image>().sprite;
            gameMgr.EquipArray[idx-20].GetComponent<Image>().sprite = gameMgr.EquipArray[swIdx - 20].GetComponent<Image>().sprite;
            gameMgr.EquipArray[swIdx - 20].GetComponent<Image>().sprite = temp2;
        }
    }

    public void SwapFusion(int Fidx)
    {
        if (Fidx > 25)
        {
            Weapon temp = InvenMgr.FusionList[Fidx - 26];
            InvenMgr.FusionList[Fidx - 26] = InvenMgr.inventoryList[idx];
            InvenMgr.inventoryList[idx] = temp;

            Sprite temp2 = null;
            temp2 = gameMgr.FusionItemArray[idx].GetComponent<Image>().sprite;
            gameMgr.FusionItemArray[idx].GetComponent<Image>().sprite = gameMgr.FusionArray[Fidx - 26].GetComponent<Image>().sprite;
            gameMgr.FusionArray[Fidx - 26].GetComponent<Image>().sprite = temp2;

            //Sprite temp3 = gameMgr.FusionArray[Fidx - 26].GetComponent<Image>().sprite;
            //gameMgr.FusionArray[Fidx - 26].GetComponent<Image>().sprite = gameMgr.ItemArray[idx].GetComponent<Image>().sprite;
            //gameMgr.ItemArray[idx].GetComponent<Image>().sprite = temp3;

            gameMgr.ItemArray[idx].GetComponent<Image>().sprite = gameMgr.FusionItemArray[idx].GetComponent<Image>().sprite;
        }
        else
        {
            Weapon temp = InvenMgr.inventoryList[Fidx];
            InvenMgr.inventoryList[Fidx] = InvenMgr.FusionList[idx - 26];
            InvenMgr.FusionList[idx - 26] = temp;

            Sprite temp2 = null;
            temp2 = gameMgr.FusionItemArray[Fidx].GetComponent<Image>().sprite;
            gameMgr.FusionItemArray[Fidx].GetComponent<Image>().sprite = gameMgr.FusionArray[idx - 26].GetComponent<Image>().sprite;
            gameMgr.FusionArray[idx - 26].GetComponent<Image>().sprite = temp2;

            gameMgr.ItemArray[Fidx].GetComponent<Image>().sprite = gameMgr.FusionItemArray[Fidx].GetComponent<Image>().sprite;
        }
    }
}
