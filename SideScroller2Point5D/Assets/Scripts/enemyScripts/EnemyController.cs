using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// ENUM
public enum NpcState { IDLE, PATROL, CHASE, ATTACK, FLEE }

public class EnemyController : MonoBehaviour
{

    private Animator _enemyAnim;
    private NavMeshAgent _navAgent;
    private NpcState _enemyState;

    private float _idleSpeed = 0;
    public float walkSpeed = .25f;
    public float runSpeed = 1f;

    // chase variables
    public float chaseDistance = 7f;
    private float _currentChaseDistance;
    public float attackDistance = 1.8f;
    public float chaseAfterAttackDistance = 2f;

    // patrol variables - patrol timer,
    public float patrolRadiusMin = 20f, patrolRadiusMax = 60f;
    public float patrolForThisTime = 15f;
    private float _patrolTimer;

    //idle variables, timer
    public float idleForThisTime = 5f;
    private float _idleTimer;

    // attack variable, timer
    public float waitBeforAttack; // gives time "cushion" between enemy getting to player and attacking
    private float _attackTimer;

    // target (player)
    private Transform _targetTF; // player's transform to target

    public GameObject meleeAttackPoint;

  

    private void Awake()
    {
        // get references - animator, navagent, player transform.position,
        _enemyAnim = GetComponent<Animator>();
        _navAgent = GetComponent<NavMeshAgent>();

        _targetTF = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;

    }

    // Start is called before the first frame update
    void Start()
    {
        // random between idle / patrol to start



    }

    // Update is called once per frame
    void Update()
    {
        // if tree with enums for state 
        if (_enemyState == NpcState.IDLE)
        {
            Idle();
        }
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


    private void Idle()
    {
        throw new NotImplementedException();
    }
    private void Patrol()
    {
        throw new NotImplementedException();
    }
    private void Chase()
    {
        throw new NotImplementedException();
    }

    private void Attack()
    {
        throw new NotImplementedException();
    }
}
