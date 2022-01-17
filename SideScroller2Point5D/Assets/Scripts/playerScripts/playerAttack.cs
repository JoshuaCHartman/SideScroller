using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour
{
    private CharacterController _characterController;
    private Animator _playerAnim;

    // for projectile attack
    public GameObject projectile;
    public Transform projectileAttackPoint;

    private void Awake()
    {
        _characterController = GetComponentInParent<CharacterController>();
        _playerAnim = GetComponent<Animator>();

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
        if (_characterController.isGrounded) // cannot attack initiate attack in air
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

    void ProjectileAttack()
    {
        Instantiate(projectile, projectileAttackPoint.position, Quaternion.identity);




    }
}
