using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCtrl : MonoBehaviour
{
    GameObject Target = null;
    float Range = 0.0f;
    float speed = 10.0f;
    float Damage = 0.0f;

    Rigidbody rigid;
    Vector3 originPos = Vector3.zero;
    bool isHit = false;

    float swordCool = 1.0f;
    PlayerWeaponCtrl weaponCtrl;

    int idx = -1;
    float RandomX;
    float RandomZ;

    private void Start()
    {
        weaponCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWeaponCtrl>();
        rigid = GetComponent<Rigidbody>();
        originPos = transform.position;

        Damage = InvenMgr.EquipList[idx].Damage;
        Debug.Log("sword : " + Damage);

        RandomX = Random.Range(-2, 3);
        RandomZ = +Random.Range(-1, 3);
        InvokeRepeating("OnSwordChk", 0, 0.5f);
    }

    void OnSwordChk()
    {
        if (InvenMgr.EquipList[idx].Kind != WeaponKinds.sword)
        {
            Destroy(gameObject);
        }

        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        float nearEnemy = Mathf.Infinity;
        foreach (GameObject enemy in enemys)
        {
            Vector3 dist = enemy.transform.position - weaponCtrl.transform.position;
            if (nearEnemy > dist.magnitude)
            {
                nearEnemy = dist.magnitude;
                Target = enemy;
            }
        }
        
        if (Target == null)
        {
            transform.position = weaponCtrl.transform.position;
            return;
        }

    }

    private void Update()
    {
        if (Target == null)
            return;

        Vector3 dist = Target.transform.position - weaponCtrl.transform.position;

        if (dist.magnitude < Range)
        {
            sting();
            HitFuc();
        }
        else
        {
            transform.position = weaponCtrl.transform.position;
        }
    }

    void HitFuc()
    {
        if (Target == null)
            return;

        if (isHit)
        {
            Vector3 TargetHeight = new Vector3(Target.transform.position.x + RandomX, Target.transform.position.y + 3.0f, Target.transform.position.z+RandomZ);
            transform.position = Vector3.MoveTowards(transform.position, TargetHeight, speed * Time.deltaTime);

            swordCool -= Time.deltaTime;

            if (swordCool <= 0f)
            {
                swordCool = 1.0f;
                isHit = false;
            }
        }
    }

    void sting()
    {
        if (Target == null)
        {
            //원래 자리로
            transform.position = originPos;
            return;
        }

        if (!isHit)
        {
            Vector3 TargetHeight = new Vector3(Target.transform.position.x, Target.transform.position.y + 1.0f, Target.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, TargetHeight, speed * Time.deltaTime);
            transform.LookAt(Target.transform.position);
        }
    }

    public void SightTarget(GameObject tg)
    {
        Target = tg;
    }

    public void RangeSword(float range, int value)
    {
        Range = range;
        idx = value;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Enemy"))
        {
            isHit = true;
            coll.GetComponent<EnemyController>().TakeDamage(Damage);
        }
    }
}
