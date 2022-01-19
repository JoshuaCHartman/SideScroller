using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator _enemyAnimator;
    private void Awake()
    {
        _enemyAnimator = GetComponent<Animator>();
    }

    // idle

    // walk / bool
    public void Walk(bool walk)
    {
        _enemyAnimator.SetBool(AnimationTags.WALK_PARAMETER, walk);
    }


    // run / bool
    public void Run(bool run)
    {
        _enemyAnimator.SetBool(AnimationTags.RUN_PARAMETER, run);
    }

    // melee attack / trigger
    public void MeleeAttack()
    {
        _enemyAnimator.SetTrigger(AnimationTags.MELEE_TRIGGER);
    }

    // death / trigger

    public void NpcDeath()
    {
        _enemyAnimator.SetTrigger(AnimationTags.DEATH_TRIGGER);
    }


}
