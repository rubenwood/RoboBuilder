using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PShoot : Shoot
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fire(trigger, 1/fireRate);
    }
}
