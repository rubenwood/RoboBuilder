using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sweeper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            snap();
        }
    }

    void snap()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100, ~0))
        {
            Vector3 newRot = transform.localRotation.eulerAngles;
            newRot.z = hit.normal.y;
            transform.localRotation = Quaternion.Euler(newRot);
        }
    }
}
