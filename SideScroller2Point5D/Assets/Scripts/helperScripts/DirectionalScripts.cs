using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalScripts : MonoBehaviour
{

    public float GetPlayerMovementDirection() // return a - or + value float to indicate player facing direction ONLY WHEN MOVING
                                      // negative = left
                                      // positive = right
    {
        float playerDirection;

        // assign input, and use to determine which direciton player is facing, and rotate accordingly
        playerDirection = Input.GetAxis("Horizontal"); // GetAxis returns values between -1 & 0 (left) or between 0 & 1 (right)
        return playerDirection;

    }





}
