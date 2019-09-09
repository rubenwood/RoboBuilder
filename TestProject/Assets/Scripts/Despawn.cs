using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour
{
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("despawn", time);
    }

    void despawn()
    {
        Destroy(gameObject);
    }
}
