using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 움직임 관련 변수
    float h;
    float v;
    Vector3 dir;
    float speed = 5.0f;
    // 움직임 관련 변수

    bool isAttack = false;

    Rigidbody MyRigid;
    Animator Anim;

    private void Start()
    {
        MyRigid = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
    }

    private void Update()
    {
        PlayerMove();
        PlayerTurn();
        PlayerAttack();
    }

    void PlayerMove()
    {
        if (!isAttack)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            dir = new Vector3(h, 0, v);

            MyRigid.MovePosition(transform.position + dir.normalized * speed * Time.deltaTime);


            if (h != 0 || v != 0)
            {
                Anim.SetBool("isWalk", true);
            }
            else
            {
                Anim.SetBool("isWalk", false);
            }
        }
    }

    void PlayerTurn()
    {
        if (h == 0 && v == 0)
            return;

        Quaternion LookRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, LookRot, speed * Time.deltaTime);
    }

    void PlayerAttack()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isAttack = true;
            Anim.SetBool("isWalk", false);
            Anim.SetBool("isAttack", true);
        }
    }

    void OnAttackFinish()
    {
        isAttack = false;
        Anim.SetBool("isAttack", false);
    }
}
