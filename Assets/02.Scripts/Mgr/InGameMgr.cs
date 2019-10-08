using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    GameReady, GameStart, GameEnd
}


public class InGameMgr : MonoBehaviour
{
    public GameObject MarketPenal = null;    // Market 창
    public Button MarketCloseBtn = null;     // Market에 닫기 버튼
    public Button BeginnerDrawingBtn = null; // 상점에 초급 랜덤 뽑기 버튼

    public Button InvenBtn = null;          // 인벤토리 버튼
    public GameObject InvenPanel = null;    // 인벤토리 창
    public Button InvenCloseBtn = null;     // 인벤토리 닫기 버튼

    public Button FusionBtn = null;         // 조합 버튼
    public GameObject FusionPanel = null;   // 조합 패널
    public Button FusionCloseBtn = null;    // 조합 닫기 버튼
    
    public GameObject InvenItemList = null; // 인벤에 아이템 박스
    public Button[] ItemArray = null;       // 아이템 버튼들

    public GameObject FusionInvenItemList = null;
    public Button[] FusionItemArray = null;

    public GameObject EquipItemList = null;
    public Button[] EquipArray = null;

    public GameObject FusionItemList = null;
    public Button[] FusionArray = null;

    bool noUIMarket = false;                // 인벤토리가 켜져있을 때는 상점 안열림
    int emptyItem = -1;                     // 비어있는 인벤토리

    public Sprite[] WeaponImg = null;       // 무기 이미지
    public static GameObject[] WeaponObj = null;
    public GameObject[] enemyObj = null;

    public GameObject WeaponBuyNext = null;        // 무기 구매 후 보여줄 오브젝트
    public Image WeaponBuyImg = null;              // 무기 구매 후 보여줄 이미지
    public Text WeaponBuyInfo = null;              // 무기 구매 후 보여줄 정보

    [HideInInspector]public  Color weaponActiveColor = new Color(255, 255, 255, 255); // 액티브 켜진 아이템 칼라
    [HideInInspector]public Color weaponDisableColor = new Color(255, 255, 255, 0); // 액티브 꺼진 아이템 칼라

    public Transform EnemySpawner = null; //enemy 스폰 위치
    int Round = 0; // 라운드
    bool RoundView = false;
    float RoundViewTime = 1.5f; // 1.5초 동안 보여줄 Round
    public Text RoundText = null; // 라운드 시작 될 때 나올 텍스튼
    public GameObject MonsterList = null; // 몬스터들의 몇마리 남았는지 알기 위해서
    public Button GameStartBtn = null; // 게임 스타트 버튼
    public GameObject EndPoint = null;
    int Life = 20;
    public Text LifeText = null;


    private void Start()
    {
        ItemArray = InvenItemList.transform.GetComponentsInChildren<Button>(true);
        FusionItemArray = FusionInvenItemList.transform.GetComponentsInChildren<Button>(true);
        EquipArray = EquipItemList.transform.GetComponentsInChildren<Button>(true);
        FusionArray = FusionItemList.transform.GetComponentsInChildren<Button>(true);
        WeaponImg = Resources.LoadAll<Sprite>("WeaponImg");
        WeaponObj = Resources.LoadAll<GameObject>("WeaponObj");
        enemyObj = Resources.LoadAll<GameObject>("EnemyObj");

        MarketCloseBtn.onClick.AddListener(() =>
        {
            MarketPenal.SetActive(false);
        });

        if(BeginnerDrawingBtn != null)
        {
            BeginnerDrawingBtn.onClick.AddListener(BaseWeaponBuy);
        }

        if(InvenBtn != null)
        {
            InvenBtn.onClick.AddListener(() => {
                InvenPanel.SetActive(true);
                noUIMarket = true;
            });
        }

        if(InvenCloseBtn != null)
        {
            InvenCloseBtn.onClick.AddListener(() =>
            {
                InvenPanel.SetActive(false);
                noUIMarket = false;
            });
        }

        if(FusionBtn != null)
        {
            FusionBtn.onClick.AddListener(() =>
            {
                FusionInven();
                FusionPanel.SetActive(true);
            });
        }

        if(FusionCloseBtn != null)
        {
            FusionCloseBtn.onClick.AddListener(() =>
            {
                FusionMaterialCancel();
                FusionPanel.SetActive(false);
            });
        }

        if(GameStartBtn != null)
        {
            GameStartBtn.onClick.AddListener(() =>
            {
                GameStartFuc();
            });
        }
    }

    private void Update()
    {
        MonsterChk();
        MarketActive();

        EndPointChk();

        if(RoundView)
        {
            RoundViewTime -= Time.deltaTime;

            if(RoundViewTime <= 0)
            {
                RoundView = false;
                RoundText.gameObject.SetActive(false);
                RoundViewTime = 1.5f;
            }
        }
    }

    //마켓 액티브 켜주는 함수
    void MarketActive()
    {
        if (!noUIMarket)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Market")))
                {
                    //Market 활성화
                    MarketPenal.SetActive(true);
                }
            }
        }
    }

    void BaseWeaponBuy()
    { 
        // 인벤토리에 빈 공간을 찾는다.
        for(int i = 0; i < InvenMgr.inventoryList.Count; i++)
        {
            if (InvenMgr.inventoryList[i].WeaponId == -1)
            {
                emptyItem = i;
                break;
            }
        }

        if (emptyItem == -1)
        {
            //인벤토리 꽉참
            Debug.Log("가득 참");
            return;
        }

        // 초급은 0~4
        int RandomBaseWeapon = Random.Range(0, 5); // 랜덤 뽑기 용 인덱스
        InvenMgr.inventoryList[emptyItem] = WeaponMgr.weaponList[RandomBaseWeapon]; // 랜덤 무기를 비어있는 인벤토리 창에 넣어준다.
        //ItemArray[emptyItem].gameObject.SetActive(true); // 아이템 활성화
        ItemArray[emptyItem].GetComponent<Image>().sprite = WeaponImg[InvenMgr.inventoryList[emptyItem].WeaponId]; // 아이템의 인덱스가 이미지의 인덱스와 같다.
        Debug.Log(InvenMgr.inventoryList[emptyItem].WeaponId);
        ItemArray[emptyItem].image.color = weaponActiveColor;
        

        WeaponBuyNext.SetActive(true);
        WeaponBuyImg.sprite = WeaponImg[InvenMgr.inventoryList[emptyItem].WeaponId];
        WeaponBuyInfo.text = "이름 : " + InvenMgr.inventoryList[emptyItem].Name + "\n" +
            "공격력 : " + InvenMgr.inventoryList[emptyItem].Damage + "\n" +
            "공격속도 : " + InvenMgr.inventoryList[emptyItem].FireCool + "\n" +
            "공격범위 : " + InvenMgr.inventoryList[emptyItem].Range;



        emptyItem = -1; // 다시 초기화
    }

    void FusionInven()
    {
        for(int i = 0; i < InvenMgr.inventoryList.Count; i++)
        {
            if(InvenMgr.inventoryList[i].WeaponId == -1)
            {
                FusionItemArray[i].image.color = weaponDisableColor;
                continue;
            }

            //FusionItemArray[i].gameObject.SetActive(true);
            FusionItemArray[i].GetComponent<Image>().sprite = WeaponImg[InvenMgr.inventoryList[i].WeaponId];
            FusionItemArray[i].image.color = weaponActiveColor;
        }
    }

    IEnumerator enemySpawn()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject enemy = Instantiate(enemyObj[0], EnemySpawner.transform.position, EnemySpawner.transform.rotation);
            enemy.transform.SetParent(MonsterList.transform, false);
            yield return new WaitForSeconds(0.7f);
        }
    }
    void MonsterChk()
    {
        if(MonsterList.transform.childCount == 0)
        {
            GameStartBtn.gameObject.SetActive(true);
        }
        else
        {
            GameStartBtn.gameObject.SetActive(false);
        }
    }

    void GameStartFuc()
    {
        Round++;
        GameStartBtn.gameObject.SetActive(false);
        RoundText.text = Round.ToString() + " Round";
        RoundView = true;
        RoundText.gameObject.SetActive(true);
        StartCoroutine(enemySpawn());
    }

    void EndPointChk()
    {
        GameObject[] AllEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < AllEnemy.Length; i++)
        {
            Vector3 dist = AllEnemy[i].transform.position - EndPoint.transform.position;
            if(dist.magnitude < 1.0f)
            {
                Life--;
                LifeText.text = "Life : " + Life.ToString();
                Destroy(AllEnemy[i]);
            }
        }
    }

    void FusionMaterialCancel()
    {
        //if(InvenMgr.FusionList[0].WeaponId == -1 && InvenMgr.FusionList[1].WeaponId == -1)
        //{
        //    return;
        //}

        if(InvenMgr.FusionList[0].WeaponId == -1 && InvenMgr.FusionList[1].WeaponId == -1)
        {
            return;
        }
        else if(InvenMgr.FusionList[0].WeaponId == -1 && InvenMgr.FusionList[1].WeaponId != -1)
        {
            WeaponCancel(1);
        }
        else if(InvenMgr.FusionList[0].WeaponId != -1 && InvenMgr.FusionList[1].WeaponId == -1)
        {
            WeaponCancel(0);
        }
        else if(InvenMgr.FusionList[0].WeaponId != -1 && InvenMgr.FusionList[1].WeaponId != -1)
        {
            WeaponCancel(2);
        }

        for(int i = 0; i < InvenMgr.inventoryList.Count; i++)
        {
            if(InvenMgr.inventoryList[i].WeaponId != -1)
            {
                ItemArray[i].image.color = weaponActiveColor;
                FusionItemArray[i].image.color = weaponActiveColor;
            }
        }
    }

    void WeaponCancel(int value)
    {
        if(value == 0)
        {
            //0 일때는 왼쪽 하나만 들어있을 때
            int emp = 0;
            for(int i = 0; i < InvenMgr.inventoryList.Count; i++)
            {
                if(InvenMgr.inventoryList[i].WeaponId == -1)
                {
                    emp = i;
                    break;
                }
            }

            Weapon temp = InvenMgr.FusionList[0];
            InvenMgr.FusionList[0] = InvenMgr.inventoryList[emp];
            InvenMgr.inventoryList[emp] = temp;

            ItemArray[emp].GetComponent<Image>().sprite = FusionArray[0].GetComponent<Image>().sprite;
            FusionItemArray[emp].GetComponent<Image>().sprite = FusionArray[0].GetComponent<Image>().sprite;

            ItemArray[emp].image.color = weaponActiveColor;
            FusionItemArray[emp].image.color = weaponActiveColor;

            FusionArray[0].image.color = weaponDisableColor;
            FusionArray[1].image.color = weaponDisableColor;
            FusionArray[2].image.color = weaponDisableColor; // 조합 아이템 표시하는 이미지
        }
        else if(value == 1)
        {
            //0 일때는 왼쪽 하나만 들어있을 때
            int emp = 0;
            for (int i = 0; i < InvenMgr.inventoryList.Count; i++)
            {
                if (InvenMgr.inventoryList[i].WeaponId == -1)
                {
                    emp = i;
                    break;
                }
            }

            Weapon temp = InvenMgr.FusionList[1];
            InvenMgr.FusionList[1] = InvenMgr.inventoryList[emp];
            InvenMgr.inventoryList[emp] = temp;

            ItemArray[emp].GetComponent<Image>().sprite = FusionArray[1].GetComponent<Image>().sprite;
            FusionItemArray[emp].GetComponent<Image>().sprite = FusionArray[1].GetComponent<Image>().sprite;

            ItemArray[emp].image.color = weaponActiveColor;
            FusionItemArray[emp].image.color = weaponActiveColor;

            FusionArray[0].image.color = weaponDisableColor;
            FusionArray[1].image.color = weaponDisableColor;
            FusionArray[2].image.color = weaponDisableColor; // 조합 아이템 표시하는 이미지
        }
        else // 2
        {
            //2 일때는 둘다 들어있을 때
            int count = 2;
            int first = -1;
            int second = -1;

            for (int i = 0; i < InvenMgr.inventoryList.Count; i++)
            {
                if (InvenMgr.inventoryList[i].WeaponId == -1)
                {
                    if(count == 2)
                    {
                        first = i;
                        count--;
                    }
                    else if(count == 1)
                    {
                        second = i;
                        count--;
                    }

                    if (count == 0)
                        break;
                }
            }

            Weapon temp = InvenMgr.FusionList[0];
            InvenMgr.FusionList[0] = InvenMgr.inventoryList[first];
            InvenMgr.inventoryList[first] = temp;

            Weapon temp2 = InvenMgr.FusionList[1];
            InvenMgr.FusionList[1] = InvenMgr.inventoryList[second];
            InvenMgr.inventoryList[second] = temp2;

            ItemArray[first].GetComponent<Image>().sprite = FusionArray[0].GetComponent<Image>().sprite;
            FusionItemArray[first].GetComponent<Image>().sprite = FusionArray[0].GetComponent<Image>().sprite;

            ItemArray[second].GetComponent<Image>().sprite = FusionArray[1].GetComponent<Image>().sprite;
            FusionItemArray[second].GetComponent<Image>().sprite = FusionArray[1].GetComponent<Image>().sprite;

            ItemArray[first].image.color = weaponActiveColor;
            ItemArray[second].image.color = weaponActiveColor;
            FusionItemArray[first].image.color = weaponActiveColor;
            FusionItemArray[second].image.color = weaponActiveColor;

            FusionArray[0].image.color = weaponDisableColor;
            FusionArray[1].image.color = weaponDisableColor;
            FusionArray[2].image.color = weaponDisableColor; // 조합 아이템 표시하는 이미지
        }
    }
}
