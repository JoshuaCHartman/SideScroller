using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class playerAttack : MonoBehaviour
{
    private CharacterController _characterController;
    private Animator _playerAnim;
    //private playerMovement _playerMovement;

    // to determine facing direction 
    [SerializeField] private GameObject _face;
    [SerializeField] private GameObject _backOfHead;

    // for projectile attack
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _projectileLeftAttackPoint;
    [SerializeField] private Transform _projectileRightAttackPoint;
    [SerializeField] private float _projectileSpeed = 1500;

    // for melee attack
    [SerializeField] private Transform _meleeAttackPoint;
    [SerializeField] private LayerMask _npcLayers; // melee will only apply to objects on that layer
   // [SerializeField] private LayerMask _enemyLayers; 
    [SerializeField] private float _meleeAttackRange = 0.5f; // radius of attackpoint sphere for detection
    [SerializeField] private int _attackDamage = 50;
    [SerializeField] private float _meleePhysicsForce = 1000;
   

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
        // projectile

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _playerAnim.SetTrigger("shootTrigger");
        }

        if (_characterController.isGrounded) // cannot attack initiate attack in air
        {

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // animation
                _playerAnim.SetTrigger("meleeTrigger");

                //detect enemies
                Collider[] hitNpc = Physics.OverlapSphere(_meleeAttackPoint.position, _meleeAttackRange, _npcLayers);

                //apply damage
                foreach (Collider npc in hitNpc)
                {
                    Debug.Log("HIT" + npc.name);
                   Rigidbody npcBody = npc.GetComponent<Rigidbody>();
                    npc.GetComponent<NPCHealth>().ApplyDamage(_attackDamage);
                    npcBody.AddForce(500, 500, 0);
                }
            }
            
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_meleeAttackPoint == null)
        {
            return; // prevents errors if no attack point assigned
        }

        Gizmos.DrawWireSphere(_meleeAttackPoint.position, _meleeAttackRange);
    }

    // Projectile Attack TRIGGERED by ANIMATION EVENT (Animator window : projectile animation). 
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
