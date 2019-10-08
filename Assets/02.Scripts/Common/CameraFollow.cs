using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject Target = null;

    private void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player");
    }

    private void LateUpdate()
    {
        MoveCam();
    }

    void MoveCam()
    {
        if (Target.transform.position.x <= -12.5 || Target.transform.position.x >= 13.1)
        {
            Vector3 fixCam = new Vector3(transform.position.x, transform.position.y, -6f);
            transform.position = Vector3.Lerp(transform.position, fixCam, 0.1f);
            return;
        }

        Vector3 MoveCamera = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z - 6f);
        transform.position = Vector3.MoveTowards(transform.position, MoveCamera, 10 * Time.deltaTime);
    }
}
