using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WepSwitch : MonoBehaviour
{
    public Transform weapons;

    // Start is called before the first frame update
    void Start()
    {
        hideAll();
        weapons.GetChild(0).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            hideAll();
            weapons.GetChild(0).gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            hideAll();
            weapons.GetChild(1).gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            hideAll();
            weapons.GetChild(2).gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            hideAll();
            weapons.GetChild(3).gameObject.SetActive(true);
        }else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            hideAll();
            weapons.GetChild(4).gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            hideAll();
            weapons.GetChild(5).gameObject.SetActive(true);
        }
    }

    void hideAll()
    {
        foreach (Transform child in weapons)
        {
            child.gameObject.SetActive(false);
        }
    }
}
