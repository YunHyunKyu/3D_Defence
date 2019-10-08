using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 10.0f;

    float curhp = 100.0f;

    GameObject PointList = null;
    Transform[] point = null;
    int pointIdx = 1;

    Transform TargetPos = null;

    private void Start()
    {
        PointList = GameObject.FindGameObjectWithTag("PointList");
        point = PointList.GetComponentsInChildren<Transform>();

        TargetPos = point[pointIdx];

        InvokeRepeating("Arrive", 0, 0.5f);
    }

    //도착했는지 판단 하고 다음 위치 지정해주기
    void Arrive()
    {
        if(pointIdx+1 >= point.Length)
        {
            return;
        }

        Vector3 dist = TargetPos.position - transform.position;
        if(dist.magnitude < 1.0f)
        {
            TargetPos = point[++pointIdx];
        }
    }

    private void Update()
    {
        EnemyMove();
    }

    void EnemyMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, TargetPos.position, speed * Time.deltaTime);

        transform.LookAt(TargetPos.transform.position);
    }

    public void TakeDamage(float dg)
    {
        curhp -= dg;

        if(curhp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
