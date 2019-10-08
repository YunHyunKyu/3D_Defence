using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBuyNext : MonoBehaviour
{
    float DisappearTime = 0.0f; // 사라지다

    private void OnEnable()
    {
        DisappearTime = 0.7f;
    }

    private void Update()
    {
        DisappearTime -= Time.deltaTime;

        if(DisappearTime < 0.0f)
        {
            this.gameObject.SetActive(false);
        }
    }
}
