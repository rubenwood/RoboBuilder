using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjHit : MonoBehaviour
{
    public float timeToDespawnIfNotHit;
    private bool hit = false;

    // Start is called before the first frame update
    void Start()
    {
        /// if timeToDespawnIfNotHit seconds has elapsed then despawn anyway
        Invoke("despawnIfNotHit", timeToDespawnIfNotHit);
    }

    void despawnIfNotHit()
    {
        if (!hit)
        {
            despawn(true);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        /// Called when this projectile hits something
        /// By default when it hits something it will:
        /// enable gravity
        /// enable attached despawn script
        if (col.gameObject.tag != "projectile")
        {
            ///Debug.Log("A PROJECTILE HIT SOMETHING!");
            hit = true;

            grav(true);
            despawn(true);
        }        
    }

    void grav(bool v)
    {
        GetComponent<Rigidbody>().useGravity = v;
    }

    void coll(bool v)
    {
        GetComponent<Collider>().enabled = v;
    }

    void despawn(bool v)
    {
        GetComponent<Despawn>().enabled = v;
    }
}
