using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

// ENUM
public enum NpcState { SUSPEND_STATE, RESET_AFTER_PHYSICS, IDLE, PATROL, CHASE, ATTACK, FLEE }

public class EnemyController : MonoBehaviour
{
    public NpcState NpcState { get; set; }

    [SerializeField] Vector3 patrolTarget;
    [SerializeField] private Vector3 _randomDestination;

    public EnemyAnimator _enemyAnimator; // EnemyAnimator script - with instructions to activate animation controller / trigger animations
    private NavMeshAgent _navAgent;
    private Rigidbody _enemyRigidbody; // for usa in physics


    /*[SerializeField] private NpcState _enemyState;*/ // serialized to check state in inspector
    public NpcState _enemyState;

    // private float _idleSpeed = 0; // changed to vector3 velocity.zero
    public float walkSpeed = 6f;
    public float runSpeed = 12f;

    // chase variables
    public float chaseDistance = 20f;
    private float _currentChaseDistance;
    public float attackDistance = .8f; // is enemy close enough to attack player?
    public float chaseAfterAttackDistance = 2f;

    // patrol variables ,
    public float patrolRadiusMin = 2f, patrolRadiusMax = 4f;

    // timer for patrol / idle
    public float stateTime = 15f; // max time to patrol / idle
    private float _stateTimer; // timer for patrol / idle

    // attack variable, timer
    public float waitBeforAttack; // gives time "cushion" between enemy getting to player and attacking
    private float _attackTimer;

    // target (player)
    private Transform _targetTF; // player's transform to target

    public GameObject meleeAttackPoint;

    // variables to check if player off 0Z and set to 0Z
    private Vector3 _tempPos;

    private playerAttack playerAttackForPhysics;


    private void Awake()
    {
        // get references - animator, navagent, player transform.position,
        _enemyAnimator = GetComponent<EnemyAnimator>();
       
        _navAgent = GetComponent<NavMeshAgent>();

        _enemyRigidbody = GetComponent<Rigidbody>();

        _targetTF = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;

    }
    private void FixedUpdate() // physics, end of frame
    {
        // if player z axis is greater than  1 or less than -1 change z value to 0
        ConfineNPCMovementToAroundZedAxis();
    }



    // Start is called before the first frame update
    void Start()
    {
        _enemyState = NpcState.PATROL;
        _stateTimer = stateTime;
        _attackTimer = waitBeforAttack;
        _currentChaseDistance = chaseDistance;


    }

    // Update is called once per frame
    void Update()
    { 
        // if tree with enums for state 
        if (_enemyState == NpcState.SUSPEND_STATE)
        {
            SuspendStateForPhysics();
        }

        if (_enemyState == NpcState.RESET_AFTER_PHYSICS)
        {
            KnockbackGetUp();
        }
        
        //if (_enemyState == NpcState.IDLE)
        //{
        //    Idle();
        //}
        if (_enemyState == NpcState.PATROL)
        {
            Patrol();
        }
        if (_enemyState == NpcState.CHASE)
        {
            Chase();
        }
        if (_enemyState == NpcState.ATTACK)
        {
            Attack();
        }

    }

    private void KnockbackGetUp()
    {
        _enemyAnimator.NpcDeath();
        StartCoroutine(CoolDownWhenEnemyKnockedDown());

    }
    IEnumerator CoolDownWhenEnemyKnockedDown()
    {
        yield return new WaitForSeconds(4f);
        //hasEnemyCollisionOccured = false;
        //_npcNavMeshAgent.enabled = true;
        //_npcBody.isKinematic = true;
        //_npcController._enemyState = NpcState.PATROL;

    }

    private void SuspendStateForPhysics()
    {
        // _navAgent.enabled = false;
        _enemyAnimator.LaunchedInAir();
    }


    //private void Idle()
    //{
    //    // stop moving
    //    _navAgent.velocity = Vector3.zero;
    //    _navAgent.isStopped = true;

    //    // set timer
    //    _stateTimer += Time.deltaTime;
    //    if (_stateTimer <= stateTime)
    //    {
    //        _enemyState = NpcState.PATROL;
    //        _stateTimer = 0f;
    //    }

    //    if (_navAgent.velocity.sqrMagnitude == 0)
    //    {
    //        _enemyAnimator.Idle(true);
    //        _enemyAnimator.Walk(false);
    //        _enemyAnimator.Run(false);
    //    }
    //    else
    //    {
    //        _enemyAnimator.Idle(false);
    //    }

    //    // test: if (enemy position - player position) <= chase distance, then chase (turn off walk anim, turn on chase STATE)
    //    if (Vector3.Distance(transform.position, _targetTF.position) <= chaseDistance)
    //    {
    //        _enemyAnimator.Idle(false);
    //        _enemyState = NpcState.CHASE;

    //        // sound to alert player enemy is chasing
    //    }
    //}
    private void Patrol()
    {
        // start moving - turn on navmeshagent @ walk speed

        _navAgent.isStopped = (false);
        _navAgent.speed = walkSpeed;

        // set timer for amount of time to patrol
        _stateTimer += Time.deltaTime;
        if (_stateTimer > stateTime)
        {

            // set a random position (the confine movement to zed axis method should limit player movement away from Z)
            //SetNewRandomDestination();
            // OR
            GetRandomXAxisPosition();


            // reset patrol timer
            _stateTimer = 0f;
        }

        // walk (for timer amount)
        if (_navAgent.velocity.sqrMagnitude > 0)
        {
            _enemyAnimator.Walk(true);
        }
        else
        {
            _enemyAnimator.Idle();
        }

        // test: if (enemy position - player position) <= chase distance, then chase (turn off walk anim, turn on chase STATE)
        if (Vector3.Distance(transform.position, _targetTF.position) <= chaseDistance)
        {
            _enemyAnimator.Walk(false);
            _enemyState = NpcState.CHASE;

            // sound to alert player enemy is chasing
        }

    }

    private void PatrolXAxis()
    {
        // start moving - turn on navmeshagent @ walk speed
        _navAgent.isStopped = (true);
        // _navAgent.speed = walkSpeed;

        // set timer for amount of time to patrol
        _stateTimer += Time.deltaTime;
        if (_stateTimer <= stateTime)
        {
            // set a random position (the confine movement to zed axis method should limit player movement away from Z)
            // SetNewRandomDestination();

            var npcPosition = transform.position.x;
            // get a randomx value with random range +- 20 from that position.x
            var patrolRandomX = Random.Range(npcPosition - 20f, npcPosition + 20f);

            // set new target position as the randomx, with self y, 0z
            patrolTarget = new Vector3(patrolRandomX, transform.position.y, 0);

            // var patrolPath = Vector3.MoveTowards(transform.position, patrolTarget, 0);

            //_navAgent.SetDestination(patrolPath);
            transform.position = patrolTarget * walkSpeed * Time.deltaTime;
            // reset patrol timer
            _stateTimer = 0f;
        }

        // walk (for timer amount)

        _enemyAnimator.Walk(true);


        // test: if (enemy position - player position) <= chase distance, then chase (turn off walk anim, turn on chase STATE)
        if (Vector3.Distance(transform.position, _targetTF.position) <= chaseDistance)
        {
            _enemyAnimator.Walk(false);
            _enemyState = NpcState.CHASE;

            // sound to alert player enemy is chasing
        }

    }

    private void SetNewRandomDestination()
    {
        // find a random point, later will set z=0

        float _randomRadius = Random.Range(patrolRadiusMin, patrolRadiusMax);

        Vector3 _randomDirection = Random.insideUnitSphere * _randomRadius; // source position -> random radius makes a sphere, point then chosen at random

        //check if navigable/inside bounds, store point info
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(_randomDirection, out navMeshHit, _randomRadius, -1);

        _randomDestination = navMeshHit.position; // navmeshHit cannot be altered, so must use new vector3 to hold/modify. if no axis is constrained, can use Hit.position

        //check for z and set to 0 if out of bounds)
        if (_randomDestination.z > 0 || _randomDestination.z < 0)
        {
            _randomDestination.z = 0;
        }

        _navAgent.SetDestination(_randomDestination);
    }

    private void GetRandomXAxisPosition()
    {

        // get enemy position.x
        var npcPosition = transform.position.x;
        // get a randomx value with random range +- 20 from that position.x
        var patrolRandomX = Random.Range(npcPosition - 50f, npcPosition + 50f);

        // set new target position as the randomx, with self y, 0z
        patrolTarget = new Vector3(patrolRandomX, transform.position.y, 0);
        _navAgent.SetDestination(patrolTarget);


        //var patrolPath = Vector3.MoveTowards(transform.position, patrolTarget, 0);
        //_navAgent.SetDestination(patrolPath);
        //transform.position = patrolPath * walkSpeed * Time.deltaTime;
    }

    private void Chase()
    {
        // turn on & set speed to run

        _navAgent.isStopped = false;
        _navAgent.speed = runSpeed;

        // set destination position to player's
        _navAgent.SetDestination(_targetTF.position);

        // run to player
        if (_navAgent.velocity.sqrMagnitude > 0)
        {
            _enemyAnimator.Run(true);
        }
        else
        {
            _enemyAnimator.Run(false);
        }

        // check distance between enemy & player for melee attack
        if (Vector3.Distance(transform.position, _targetTF.position) - .6f <= attackDistance)
        {
            // if within distance, stop run/walk & change state to attack
            _enemyAnimator.Walk(false);
            _enemyAnimator.Run(false);
            // _enemyAnimator.Idle(false);
            _enemyState = NpcState.ATTACK;

            // reset chase distance to stored value
            ResetChaseDistance();
        }
        // if within distance, stop run/walk & change state to attack

        // reset chase distance to stored value

        // if player out of range, stop run & reset to patrol
        else if (Vector3.Distance(transform.position, _targetTF.position) >= chaseDistance)
        {
            _enemyAnimator.Run(false);
            _enemyState = NpcState.PATROL;
            _stateTimer = stateTime;
            ResetChaseDistance();
        }
    }
    private void Attack()
    {
        // stop moving / stop navagent
        _navAgent.velocity = Vector3.zero;
        _navAgent.isStopped = true;

        // attack timer = pause between attacks
        _attackTimer += Time.deltaTime;
        if (_attackTimer > waitBeforAttack)
        {
            // attack animation
            _enemyAnimator.Attack();

            // reset attack timer
            _attackTimer = 0f;

            // attack audio
        }

        // test player distance
        if (Vector3.Distance(transform.position, _targetTF.position) > (attackDistance + chaseAfterAttackDistance))
        {
            _enemyState = NpcState.CHASE;
        }
    }


    void TurnOnMeleeAttackPoint()
    {
        meleeAttackPoint.SetActive(true);
    }
    void TurnOffMeleeAttackPoint()
    {
        if (meleeAttackPoint.activeInHierarchy)
        {
            meleeAttackPoint.SetActive(false);
        }
    }
    private void ResetChaseDistance()
    {
        if (chaseDistance != _currentChaseDistance)
        {
            chaseDistance = _currentChaseDistance;

        }
    }

    private void ConfineNPCMovementToAroundZedAxis()
    {
        _tempPos = transform.position;

        if (transform.position.z != 0)
        {
            _tempPos.z = 0;
        }

        transform.position = _tempPos;
    }

}

