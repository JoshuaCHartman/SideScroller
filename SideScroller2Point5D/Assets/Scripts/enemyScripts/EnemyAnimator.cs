using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator _enemyAnimator;
    private void Awake()
    {
        _enemyAnimator = GetComponentInChildren<Animator>();
        //_enemyAnimator = GetComponent<Animator>();
    }

    // idle / bool
    public void Idle()
    {
        _enemyAnimator.Play("Idle");
    }

    // walk / bool
    public void Walk(bool walk)
    {
        _enemyAnimator.SetBool("Walk", walk);
    }


    // run / bool
    public void Run(bool run)
    {
        _enemyAnimator.SetBool("Run", run);
    }

    // CHASE (run) / trigger

    //public void Chase()
    //{
    //    _enemyAnimator.SetTrigger("Chase");
    //}

    // melee attack / trigger
    public void Attack()
    {
        _enemyAnimator.SetTrigger("Attack");
    }

    // death / trigger

    public void NpcDeath()
    {
        _enemyAnimator.SetTrigger("Death");
    }


}
