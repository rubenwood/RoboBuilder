using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobobayAnim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator play()
    {
        Animator a = GetComponent<Animator>();
        a.SetBool("Play", true);

        while (a.GetCurrentAnimatorStateInfo(0).IsName("Go") && a.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
        {
            yield return null;
        }

        a.SetBool("Play", false);
    }
}
