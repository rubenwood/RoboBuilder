using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGShoot : Shoot
{
    public Vector3 randAngle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /// la will be the new launch rangle, in this case, a randomly generated angle based off of the public variable randAngle
        Quaternion la = Quaternion.Euler(Random.Range(-randAngle.x, randAngle.x), Random.Range(-randAngle.y, randAngle.y), Random.Range(-randAngle.z, randAngle.z));
        fire(trigger, la, 1 / fireRate);
    }
}
