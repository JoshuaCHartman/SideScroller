using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectsAfter : MonoBehaviour
{
    [SerializeField] private float lifespan;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifespan );
    }

   
}
