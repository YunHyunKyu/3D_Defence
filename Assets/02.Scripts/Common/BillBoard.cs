using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    Transform m_CameraTr = null;

    private void Start()
    {
        m_CameraTr = GameObject.FindGameObjectWithTag("SubCamera").GetComponent<Camera>().transform;
    }

    private void LateUpdate()
    {
        this.transform.forward = m_CameraTr.forward; // 빌보드
    }
}
