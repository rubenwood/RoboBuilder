using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatFade : MonoBehaviour
{
    public Material mat;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnEnable()
    {        
        Invoke("FadeToZero", 3);
    }

    void FadeToZero()
    {
        ///t needs to increase gradually
        mat.color = Color.Lerp(mat.color, new Color(mat.color.r, mat.color.g, mat.color.b, 0), 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
