using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonCtrl : MonoBehaviour
{
    GameObject Target = null;
    float speed = 15.0f;
    int idx = -1;
    float Damage = 0;

    public GameObject hitEffect = null;

    private void Start()
    {
        Damage = InvenMgr.EquipList[idx].Damage;
        Debug.Log("bullet : " + Damage);
    }

    private void Update()
    {
        Shot();
        OnColl();
    }

    void Shot()
    {
        if (Target == null)
        {
            Destroy(this.gameObject);
            return;
        }
        Vector3 TargetHeight = new Vector3(Target.transform.position.x, Target.transform.position.y + 0.5f, Target.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, TargetHeight, speed * Time.deltaTime);
        transform.LookAt(Target.transform.position);
    }

    void OnColl()
    {
        if (Target == null)
        {
            return;
        }

        Vector3 TargetHeight = new Vector3(Target.transform.position.x, Target.transform.position.y + 0.5f, Target.transform.position.z);
        Vector3 dist = TargetHeight - transform.position;
        if (dist.magnitude <= 0.5f)
        {
            Collider[] coll = Physics.OverlapSphere(Target.transform.position, 10);
            for(int i = 0; i < coll.Length; i++)
            {
                if(coll[i].transform.CompareTag("Enemy"))
                {
                    coll[i].GetComponent<EnemyController>().TakeDamage(Damage);
                }
            }

            if (hitEffect != null)
            {
                GameObject eff = Instantiate(hitEffect, Target.transform.position, Target.transform.rotation);
                Destroy(eff.gameObject, 2.5f);
            }
            Destroy(gameObject);
        }
    }

    public void SightTarget(GameObject tg, int i)
    {
        Target = tg;
        idx = i;
    }
}
