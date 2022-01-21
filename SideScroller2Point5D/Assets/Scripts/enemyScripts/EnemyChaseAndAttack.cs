using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyChaseAndAttack : MonoBehaviour
{
    private Vector3 _tempPos;
    public NpcState NpcState { get; set; }

    //private EnemyAnimator _enemyAnimator;
    private Animator _enemyAnim;

    [SerializeField] private NpcState _enemyState;

    private Transform _targetTF;
    
    private float _distance; // distance between player & enemy
    public float chaseDistance = 20f;

    [SerializeField] private float chaseSpeed = 12f;

    private void Awake()
    {
        _enemyAnim = GetComponent<Animator>();
        _targetTF = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;
    }

    private void FixedUpdate()
    {
        ConfineNPCMovementToAroundZedAxis();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _distance = GetDistanceBetweenEnemyAndPlayer();

        if (_enemyState == NpcState.IDLE)
        {
           if(_distance <= chaseDistance)
            {
                _enemyState = NpcState.CHASE;
                _enemyAnim.SetTrigger("Chase");
            }
            else if (_enemyState == NpcState.CHASE)
            {
                _enemyAnim.SetTrigger("Chase");

                if (_targetTF.position.x > transform.position.x)
                {
                    // move Right
                    transform.Translate(transform.right * chaseDistance * Time.deltaTime);
                }
                else
                {
                    // move Left
                    transform.Translate(-transform.right * chaseDistance * Time.deltaTime);
                }
            }
        }
    }

    private float GetDistanceBetweenEnemyAndPlayer()
    {
        float distance = Vector3.Distance(transform.position, _targetTF.position);
        return distance;
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
