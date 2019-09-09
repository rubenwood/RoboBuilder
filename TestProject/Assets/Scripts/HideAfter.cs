using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAfter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        Invoke("disable", 2);
    }

    void disable()
    {
        gameObject.SetActive(false);
    }
}
