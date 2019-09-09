using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SGShoot : Shoot
{
    public int numProj;
    public Vector2 spread;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fire(trigger, spread, 1 / fireRate, numProj);     
    }
}
