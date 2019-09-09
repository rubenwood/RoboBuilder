using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** TODO
 * Refactor this code, it's too messy/confusing
 * Make it more generic so we can reuse this pickup and drop functionality else where
 * comment everything to help make it clearer
 */
public class PickUp : MonoBehaviour
{
    private bool pickedUp; /// flag set when something is picked up
    private GameObject target;/// variable to store our target object

    private List<int> pickupLayers;

    // Start is called before the first frame update
    void Start()
    {
        target = null;
        pickedUp = false;

        pickupLayers = new List<int>{9,10,11}; /// set pickup-able layers
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E)) {
            gameObject.GetComponent<Collider>().enabled = true; /// enable the grab collider only when the button is pressed

            if (target != null)
            {
                pickUp(target); /// if target is not null then pick it up
            }
        }
        else if (Input.GetKeyUp(KeyCode.E) && pickedUp) /// if we release the button and had something picked up 
        {
            drop(target); /// drop the target
        }
        else /// if all else fails then set target to null and turn of the grab collider
        {
            target = null; 
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("~~~~~~ENTERED GRAB COLLIDER");

        if (pickupLayers.Contains(col.gameObject.layer) && !pickedUp)
        {
            target = col.gameObject;
        }   
    }
    void OnTriggerExit(Collider col)
    {
        Debug.Log("EXITED GRAB COLLIDER~~~~~~~");

        if (pickupLayers.Contains(col.gameObject.layer))
        {
            drop(target);
        }           
    }

    /// string prevTag = ""; /// variable to store the previous tag
    void pickUp(GameObject t)
    {        
        if (pickupLayers.Contains(t.layer))
        {
            if (pickedUp)
            {
                t.transform.position = transform.position;
                if(t.layer == 11) /// this is specific to picked up bots, might change this
                {
                    t.transform.rotation = transform.rotation;
                }
            }
            else if (!pickedUp)
            {
                /// prevTag = t.tag; /// assign the current tag to the previous tag variable
                /// t.tag = "grabbed"; /// set the current tag to grabbed
                changePhysics(t, false);

                pickedUp = true;
                gameObject.GetComponent<Collider>().enabled = false;
            }
        }
    }

    void drop(GameObject grabbedObj)
    {
        if (grabbedObj != null)
        {
            if (grabbedObj.layer == 9 || grabbedObj.layer == 10) /// only renable physics if its on layer 9 or 10, but not layer 11
            {
                changePhysics(grabbedObj, true);
            }
            pickedUp = false;
            gameObject.GetComponent<Collider>().enabled = true;
        }

    }

    /// Turns on or off the physics and collider attch to the object
    void changePhysics(GameObject o, bool v)
    {
        Rigidbody[] rbs = o.GetComponents<Rigidbody>();
        foreach(Rigidbody r in rbs)
        {
            r.useGravity = v;
            r.isKinematic = !v;
        }
        /// Do we really need to disbale/enable the collider if we do above?
        /// o.GetComponent<Collider>().enabled = v;
        /// Maybe we can just turn it into a trigger temporarily
        Collider[] cols = o.GetComponents<Collider>();
        foreach(Collider c in cols)
        {
            c.isTrigger = !v;
        }        

        /// if the component has a trigger then set the trigger to false
        /// this is only really the case when we spawn a part, it starts out as a trigger,
        /// because we dont want it to collide with any other parts that spawn
        /// only when we grab it do we want to make it NOT a trigger
        /// after that it should always be a regular collider (i.e we never change trigger back to true)
        ///if (o.GetComponent<Collider>().isTrigger == true)
        ///{
        ///    o.GetComponent<Collider>().isTrigger = false;
        ///}
    }

    bool checkLayer(int layerToCheck)
    {
        foreach(int intLayer in pickupLayers)
        {
            if (layerToCheck == intLayer)
            {
                return true;
            }
        }
        return false;
    }
}
