using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponCtrl : MonoBehaviour
{
    public Transform[] FirePos = null;
    int[] kindIdx = new int[6];

    GameObject Target = null;
    bool[] fireChk = new bool[7];

    float[] nextFire = new float[7];

    bool[] isSwordChk = new bool[7];

    private void Start()
    {
        for(int i =0; i < fireChk.Length; i++)
        {
            fireChk[i] = false;
        }
        for (int i = 0; i < nextFire.Length; i++)
        {
            nextFire[i] = 0.0f;
        }
        for (int i = 0; i < isSwordChk.Length; i++)
        {
            isSwordChk[i] = false;
        }

        InvokeRepeating("TargetChk", 0f, 0.3f);
    }

    void TargetChk()
    {
        GameObject[] AllEnemy = GameObject.FindGameObjectsWithTag("Enemy");

        float nearEnemy = Mathf.Infinity;
        foreach (GameObject enemy in AllEnemy)
        {
            Vector3 dist = enemy.transform.position - transform.position;
            if(nearEnemy > dist.magnitude)
            {
                nearEnemy = dist.magnitude;
                Target = enemy;
            }
        }

        //sword chk
        for(int i = 0; i < 6; i++)
        {
            if(InvenMgr.EquipList[i].Kind != WeaponKinds.sword)
            {
                isSwordChk[i] = false;
            }
        }
    }

    private void Update()
    {
        Fire();
    }

    void Fire()
    {
        //무기 종류 : 총, 활, 검, 대포, 지팡이 
        for(int i = 0; i < 6; i++)
        {
            if (InvenMgr.EquipList[i].WeaponId == -1)
                continue;
            else
            {
                BulletFire(i);
            }
        }
    }

    void BulletFire(int i)
    {
        if (Target == null)
            return;

        if (InvenMgr.EquipList[i].Kind != WeaponKinds.sword)
        {
            kindIdx[i] = (int)InvenMgr.EquipList[i].Kind;
            Vector3 dist = Target.transform.position - transform.position;
            if (dist.magnitude < InvenMgr.EquipList[i].Range)
            {
                if (Time.time > nextFire[i])
                {
                    nextFire[i] = InvenMgr.EquipList[i].FireCool + Time.time;
                    GameObject bullet = Instantiate(InGameMgr.WeaponObj[kindIdx[i] - 1], FirePos[i].transform.position, FirePos[i].transform.rotation); // 발사체 생성
                    
                    if (InvenMgr.EquipList[i].Kind == WeaponKinds.cannon)
                        bullet.GetComponent<CannonCtrl>().SightTarget(Target, i);
                    else if (InvenMgr.EquipList[i].Kind != WeaponKinds.cannon)
                        bullet.GetComponent<BulletCtrl>().SightTarget(Target, i);
                }
            }
        }
        else if (InvenMgr.EquipList[i].Kind == WeaponKinds.sword)
        {
            kindIdx[i] = (int)InvenMgr.EquipList[i].Kind;
            if (isSwordChk[i] == false)
            {
                GameObject sword = Instantiate(InGameMgr.WeaponObj[kindIdx[i] - 1], FirePos[i].transform.position, FirePos[i].transform.rotation);
                sword.GetComponent<SwordCtrl>().SightTarget(Target);
                sword.GetComponent<SwordCtrl>().RangeSword(InvenMgr.EquipList[i].Range, i);
                //sword.transform.SetParent(FirePos[i].transform, false);
                isSwordChk[i] = true;
            }
        }
    }
}
