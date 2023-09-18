using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] float delayBeforeDestroy = 2f;

    void Start() 
    {
        SelfDestructDelayed();
    }

    void SelfDestructDelayed()
    {
        Destroy(gameObject, delayBeforeDestroy);
    }
}
