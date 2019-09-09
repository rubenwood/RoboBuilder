using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PartSelector : MonoBehaviour
{
    private GameObject smelter;
    private Transform selectedPart;
    private List<Transform> partsInSelector;

    // Start is called before the first frame update
    void Start()
    {
        smelter = GameObject.Find("Smelter");
        partsInSelector = new List<Transform>();
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 10 && !alreadyIn(col)) /// if its a part and not already in the collider then
        {
            partsInSelector.Add(col.transform); /// I think this may be adding all transforms
            selectPart(col);
        }
    }
    void OnTriggerExit(Collider col)
    {
        ///Debug.Log(col.transform.name + " left the part selector");
        partsInSelector.Remove(col.transform);
        selectPart(col);
    }


    void selectPart(Collider col)
    {
        if (partsInSelector.Count == 1) /// if there is only 1 part in the selector then set that to be the selected part
        {
             smelter.GetComponent<Smelter>().selectPart(col.transform);
        }
        else /// if there are 0 or more than 1 parts then set the selected part to null and remove all parts in the array
        {
            smelter.GetComponent<Smelter>().selectedPart = null;            
        }
    }

    bool alreadyIn(Collider c) /// checks to see if this transform has been added to the list of parts in the bay before
    {
        foreach (Transform p in partsInSelector)
        {
            if (c.transform == p)
            {
                return true;
            }
        }
        return false;
    }
}
