using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour
{

    private Animator _playerAnim;

    private void Awake()
    {
        _playerAnim = GetComponentInChildren<Animator>();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Attack(); 
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _playerAnim.SetTrigger("meleeTrigger");
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _playerAnim.SetTrigger("shootTrigger");
        }
    }
}
