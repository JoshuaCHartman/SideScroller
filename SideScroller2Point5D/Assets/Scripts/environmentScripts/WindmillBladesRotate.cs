using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillBladesRotate : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(0,0,-45f)* Time.deltaTime );

    }
}
