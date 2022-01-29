using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.AI;

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
    [SerializeField] private GameObject _meleeAttackPoint; // turn on / off
    [SerializeField] private LayerMask _npcLayers; // melee will only apply to objects on that layer
                                                   // [SerializeField] private LayerMask _enemyLayers; 
    [SerializeField] private float _meleeAttackRange = 1f; // radius of attackpoint sphere for detection
    [SerializeField] private int _attackDamage = 50;

    public GameObject FinishingPowerUpMeleeAttack;

    // for enemy health stae
    private NPCHealth _npcHealth;

    // for physics
    private bool isHit = false; // for use to determine application of physics, & turning off navmeshagent & kinematic
    public bool hasEnemyCollisionOccured = false; // for use to determine when stop moving after physics applied, & turn above back on
    private Rigidbody _npcBody;
    private NavMeshAgent _npcNavMeshAgent;
    //private NpcState _npcState { get; set; }
    //private NpcState _previousNpcState;
    private EnemyController _npcController;


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

    //private void FixedUpdate()
    //{
    //    ApplyPhysics();
    //}

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
                Collider[] hitNpc = Physics.OverlapSphere(_meleeAttackPoint.transform.position, _meleeAttackRange, _npcLayers);

                //apply damage & physics
                foreach (Collider npc in hitNpc)
                {
                    // console log a hit of the target
                    Debug.Log("HIT" + npc.name);


                    _npcHealth = npc.GetComponent<NPCHealth>();
                    _npcHealth.ApplyDamage(_attackDamage);

                    var npcHealth = _npcHealth.CurrentNpcHealth();

                    if (npcHealth >0 && npcHealth <= 50)
                    {
                        TurnOnFinishingMeleeFx();
                    }
                    if (npcHealth <= 0)
                    {
                        TurnOffFinishingMeleeFx();
                    }
                    


                    // MOVED BELOW TO HEALTH / DAMAGE SCRIPT
                    // access components for application of physics

                    //_npcBody = npc.GetComponent<Rigidbody>();
                    //_npcNavMeshAgent = npc.GetComponent<NavMeshAgent>();

                    // apply damage

                    // if health <=0, apply physics

                    // change state when hit

                    //_npcController = npc.GetComponent<EnemyController>();
                    //_npcController._enemyState = NpcState.SUSPEND_STATE;



                    // physics switches :

                    //isHit = true;
                    //hasEnemyCollisionOccured = true;

                }
            }

        }
    }

    private void ApplyPhysics()
    {


        if (isHit)
        {
            // turn off everything when hit
            _npcNavMeshAgent.enabled = false;
            _npcBody.isKinematic = false;

            // apply physics
            _npcBody.AddForce(500, 500, 0, ForceMode.Force);

            // switch
            isHit = false;
        }
        else if (hasEnemyCollisionOccured)
        {
            // determine velocity - use as means of determining when not moving from physics
            float enemyVelocity = _npcBody.velocity.magnitude;

            if (enemyVelocity < 0.5f)
            {
                _npcController._enemyAnimator.NpcDeath();
                //_npcController._enemyState = NpcState.RESET_AFTER_PHYSICS;
                
                StartCoroutine(CoolDownWhenEnemyKnockedDown());
                // reset everything when stopped
                //hasEnemyCollisionOccured = false;
                //_npcNavMeshAgent.enabled = true;
                //_npcBody.isKinematic = true;
                
                //_npcController._enemyState = NpcState.PATROL;
            }
        }

    }

   IEnumerator CoolDownWhenEnemyKnockedDown()
    {

        yield return new WaitForSeconds(5f);
        hasEnemyCollisionOccured = false;
        _npcNavMeshAgent.enabled = true;
        _npcBody.isKinematic = true;
        _npcController._enemyState = NpcState.PATROL;

    }

    private void OnDrawGizmosSelected()
    {
        if (_meleeAttackPoint == null)
        {
            return; // prevents errors if no attack point assigned
        }

        Gizmos.DrawWireSphere(_meleeAttackPoint.transform.position, _meleeAttackRange);
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

        if (_face.transform.position.x > _backOfHead.transform.position.x)
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

    // use the following as animation events for melee attack - prevents multi hit in one swing
    public void TurnOnMeleeAttackPoint()
    {
        _meleeAttackPoint.SetActive(true);
    }
    public void TurnOffMeleeAttackPoint()
    {
        if (_meleeAttackPoint.activeInHierarchy)
        {
            _meleeAttackPoint.SetActive(false);
        }
    }

    public void TurnOnFinishingMeleeFx()
    {
        FinishingPowerUpMeleeAttack.SetActive(true);
    }
    public void TurnOffFinishingMeleeFx()
    {
        if (FinishingPowerUpMeleeAttack.activeInHierarchy)
        {
            FinishingPowerUpMeleeAttack.SetActive(false);
        }
    }
}
