using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicStateController : MonoBehaviour
{
    [SerializeField] private string _currentState = "IdleState";
    private Animator _enemyAnim;

    // target (player) & chase
    private Transform _targetTF; // player's transform to target
    private float _distance; // distance between player & enemy
    [SerializeField] private float _chaseDistance = 20f;
    [SerializeField] private float _chaseSpeed = 12f;

    private void Awake()
    {
        _enemyAnim = GetComponent<Animator>();
        _targetTF = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _distance = Vector3.Distance(transform.position, _targetTF.position);

        if (_currentState == "IdleState")
        {
            if (_distance < _chaseDistance)
            {
                _currentState = "ChaseState";
            }
        }

        else if (_currentState == "IdleState")
        {
            // play animation
            _enemyAnim.SetTrigger("Chase");

            // move towards player, L or R
            // Right
            if (transform.position.x < _targetTF.position.x)
            {
                transform.Translate(transform.right * _chaseSpeed * Time.deltaTime);
            }
            // Left
            else
            {
                transform.Translate(-transform.right * _chaseSpeed * Time.deltaTime);
            }

        }
    }

    private float GetDistanceBetweenEnemyAndPlayer()
    {
        float distance = Vector3.Distance(transform.position, _targetTF.position);
        return distance;
    }
}
