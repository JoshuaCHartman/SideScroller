using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPhysics : MonoBehaviour
{
    // FUNCTIONALITY MOVED TO PLAYERATTACK SCRIPT


    private Transform hitPosition;
    private bool isHit;
    private bool hasEnemyCollisionOccured;
    private EnemyController collisionEnemy;

    // detect collision
    private void OnCollisionEnter(Collision collision)
    {
        // get position of the hit from sword
        hitPosition.position = gameObject.transform.position;

        // if hits enemy tag, set condition bools to true & get the EnemyController to turn on/off navmesh & kinematic for physics application
        if (collision.gameObject.tag == "Enemy")
        {
            if (!isHit)
            {
                isHit = true;
                hasEnemyCollisionOccured = true;
                collisionEnemy = collision.gameObject.GetComponent<EnemyController>();

                //StartCoroutine(main game method)

            }
        }
    }

    //private void FixedUpdate()
    //{
    //    if (hasEnemyCollisionOccured)
    //    {
    //        if (collisionEnemy != null)
    //        {
    //            collisionEnemy._navAgent.enabled = false;
    //            collisionEnemy._enemyRigidbody.isKinematic = false;

    //            collisionEnemy._enemyRigidbody.AddForce(200, 200, 0, ForceMode.Force);

    //            hasEnemyCollisionOccured = true;
    //        }
    //    }

    //    else if (collisionEnemy != null)
    //    {
    //        float enemyVelocity = collisionEnemy._enemyRigidbody.velocity.magnitude;

    //        if (enemyVelocity < 0.5f)
    //        {
    //            collisionEnemy._navAgent.enabled = true;
    //            collisionEnemy._enemyRigidbody.isKinematic = true;

    //            collisionEnemy = null;
    //        }
    //    }
    //}
}
