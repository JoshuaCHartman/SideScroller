using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealthBase = 150;
    [SerializeField] private int _currentHealth;

    // for physics
    private bool isHit = false; // for use to determine application of physics, & turning off navmeshagent & kinematic
    public bool hasEnemyCollisionOccured = false; // for use to determine when stop moving after physics applied, & turn above back on
    private Rigidbody _npcBody;
    private NavMeshAgent _npcNavMeshAgent;
    //private NpcState _npcState { get; set; }
    //private NpcState _previousNpcState;
    private EnemyController _npcController;
    private EnemyAnimator _npcAnim;

    public GameObject _fireEffect;
    public GameObject _fireEffect2;
    private GameObject _catchOnFire;

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _maxHealthBase;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        ApplyPhysics();
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
        // get components for physics
        _npcBody = gameObject.GetComponent<Rigidbody>();
        _npcNavMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        // get components for state 
        _npcController = gameObject.GetComponent<EnemyController>();
        _npcController._enemyState = NpcState.SUSPEND_STATE;
        _npcAnim = gameObject.GetComponent<EnemyAnimator>();

        // physics switches :
        isHit = true;
        hasEnemyCollisionOccured = true;
    }

    private void ApplyPhysics()
    {


        if (isHit)
        {
            _fireEffect.SetActive(true);
            _fireEffect2.SetActive(true);
            
            // turn off everything when hit
            _npcNavMeshAgent.enabled = false;
            _npcBody.isKinematic = false;

            // apply physics
            _npcBody.AddForce(700, 500, 0, ForceMode.Force);

            // switch
            isHit = false;
        }
        else if (hasEnemyCollisionOccured)
        {
            // determine velocity - use as means of determining when not moving from physics
            float enemyVelocity = _npcBody.velocity.magnitude;

            if (enemyVelocity < 0.1f)
            {
                _npcAnim.NpcDeath();
                
                //_npcController._enemyState = NpcState.RESET_AFTER_PHYSICS;

                StartCoroutine(CoolDownWhenEnemyKnockedDown());
                // reset everything when stopped
                //hasEnemyCollisionOccured = false;
                //_npcNavMeshAgent.enabled = true;
                //_npcBody.isKinematic = true;

                //_npcController._enemyState = NpcState.PATROL;

                //Destroy(gameObject);
            }
        }

    }

    IEnumerator CoolDownWhenEnemyKnockedDown()
    {
        yield return new WaitForSecondsRealtime(2)  ;
       
        //_npcAnim.NpcDeath();
        Debug.Log("Coroutine");
        //_catchOnFire = Instantiate(_fireEffect, transform.position, _fireEffect.transform.rotation );
        hasEnemyCollisionOccured = false;
        _npcNavMeshAgent.enabled = true;
        _npcBody.isKinematic = true;

        _npcController._enemyState = NpcState.PATROL;
        
        var _catchOnFire2 = Instantiate(_fireEffect2, transform.position, _fireEffect.transform.rotation);
        
        Destroy(gameObject);
        
        GameObject.Destroy(_catchOnFire2, 4);
        
    }
}
