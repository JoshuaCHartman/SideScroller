using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour
{
    private CharacterController _characterController;
    private Animator _playerAnim;
    private playerMovement _playerMovement;

    // to determine facing direction 
    [SerializeField] private GameObject _face;
    [SerializeField] private GameObject _backOfHead;

    // for projectile attack
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _projectileLeftAttackPoint;
    [SerializeField] private Transform _projectileRightAttackPoint;
    [SerializeField] private float _projectileSpeed = 1500;

   

    //private GameObject launchedProjectile;
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
        // if facing left, fire left projectile

       // _playerMovement = GetComponentInParent<playerMovement>();
       // float playerDirection = _playerMovement.GetPlayerDirection(); // returns -/+ value indicating L/R facing direction

        //GameObject launchedProjectile = new GameObject;
        
        if (_face.transform.position.x < _backOfHead.transform.position.x)
        {
            FireToTheLeft();
        }

        if(_face.transform.position.x > _backOfHead.transform.position.x)
        {
            FireToTheRight();
        }
        //GameObject launchedProjectile = Instantiate(_projectileFireLeft, _projectileAttackPoint.position, Quaternion.identity);
        //launchedProjectile.GetComponent<Rigidbody>().AddForce(_projectileAttackPoint.forward * _projectileSpeed);

      

    }

    private void FireToTheLeft()
    {
        GameObject launchedProjectile = Instantiate(_projectile, _projectileLeftAttackPoint.position, Quaternion.identity);
        launchedProjectile.GetComponent<Rigidbody>().AddForce(_projectileLeftAttackPoint.forward * _projectileSpeed);
    }

    private void FireToTheRight()
    {
        GameObject launchedProjectile = Instantiate(_projectile, _projectileRightAttackPoint.position, Quaternion.identity);
        launchedProjectile.GetComponent<Rigidbody>().AddForce(_projectileRightAttackPoint.forward * _projectileSpeed);
    }
}
