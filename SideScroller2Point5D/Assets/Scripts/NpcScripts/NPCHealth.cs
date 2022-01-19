using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealthBase = 150;
    [SerializeField] private int _currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _maxHealthBase;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public damage script to access elsewhere
    public void ApplyDamage(int damageReceived)
    {
        _currentHealth -= damageReceived;

        // Struck/wound animation

        if (_currentHealth <=0)
        {
            NpcDie();
        }
    }

    void NpcDie()
    {
        Debug.Log("Target DIED!!");

        // Death animation

        // Turn off enemy
    }
}
