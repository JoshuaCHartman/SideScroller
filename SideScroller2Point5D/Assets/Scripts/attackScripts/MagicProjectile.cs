using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    public GameObject _damageEffect;

   
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Instantiate(_damageEffect, transform.position, _damageEffect.transform.rotation);
            Destroy(gameObject);
        }
    }
}

    


