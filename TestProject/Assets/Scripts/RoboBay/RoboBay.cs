using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/** TODO
 * Consume parts when spawned
 * Make it so that we dont have to put everything back into the trigger when we're done spawning
 */
public class RoboBay : MonoBehaviour
{
    public int mode, bp; /// determine build mode 1 = bp, 2 = free; bp is the id for the blueprint
    public InputField bpInputField;
    public Dropdown dd;

    public Transform robobayArms;

    private List<Transform> partsInBay;
    private Transform selectedBP;

    List<string> totalMods = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        mode = 1; /// initially in bp mode
        partsInBay = new List<Transform>();

        selectBP(bp); /// Initially select blueprint
    }

    void OnTriggerEnter(Collider col)
    {
        /// when a part enters the robobay, add it to the list of parts 
        if(col.gameObject.layer == 10)
        {
            Debug.Log("==== A PART ENTERD THE BAY!");
            if (!alreadyIn(col))
            {
                partsInBay.Add(col.transform);
            }
            ///logParts();
            checkParts();
        }        
    }

    void OnTriggerExit(Collider col)
    {        
        /// when a part leaves the robobay remove it from the list
        if (col.gameObject.layer == 10)
        {
            ///Debug.Log("PART REMOVED FROM BAY!!!");
            partsInBay.Remove(col.transform);
            ///logParts();
            checkParts();
        }        
    }

    /// for using UI elements to set and select a blueprint
    public void selectBP(int i)
    {
        bp = i;
        selectedBP = GameObject.Find("BluePrints").transform.GetChild(i);
    }

    void checkParts()
    {
        /// Is there at least 1 head, 1 connector and 1 leg
        /// If so, could spawn a robot, but we will need to do further checks
        int heads = 0;
        int connectors = 0;
        int legs = 0;
        /// for each transform in bay check its tags and increase where relevant
        foreach (Transform p in partsInBay) 
        {
            if (p.tag == "head")
            {
                heads++;
            }
            else if (p.tag == "connector")
            {
                connectors++;
            }
            else if (p.tag == "leg")
            {
                legs++;
            }
        }

        Debug.Log("heads: " + heads + " connectors: " + connectors + " legs: " + legs);

        if (heads > 0 && connectors > 0 && legs > 0)
        {
            /// We could spawn a robot, so check if its parts match the blueprint
            if (checkMatchBP(selectedBP))
            {
                StartCoroutine(spawnRobot(selectedBP));
            }
        }
    }    

    bool checkMatchBP(Transform bp)
    {
        int matches = 0; /// this is used to track how many matches we have, if we have as many matches as things in the bp then we can spawn
        /// If the parts match the blueprint then we can spawn and instance of this robot
        /// First check to see if we have the same amount of parts in the bay as in the bp
        if (partsInBay.Count == bp.childCount)
        {
            /// If the number of parts in the bay matches the number of parts in the bp
            /// then store these list in a temporary form, we'll be removing from these lists
            List<Transform> tempPnB = partsInBay;
            List<Transform> tempBP = childrenToList(bp);            

            bool remove = false; /// temporary boolean, used to check if we should remove this element from the tempPnB list

            for (int i = tempPnB.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < tempBP.Count; j++)
                {
                    if (tempPnB[0].GetComponent<Part>().getName() == tempBP[j].GetComponent<Part>().getName())
                    {
                        ///Add the mods on this part to total mods
                        totalMods.AddRange(tempPnB[0].GetComponent<Part>().getMods());
                        matches++;                   
                        remove = true; 
                        break; /// as soon as we have a match break out of this loop
                    }
                }
                if (remove)
                {
                    tempPnB.RemoveAt(0);
                    remove = false;
                }                
            }

            if (matches == bp.childCount)
            {
                return true;
            }
        }
        
        return false;
    }

    IEnumerator spawnRobot(Transform bp)
    {
        playAnims();

        /// TODO: Remove all parts relating to this robot from the robobay
        /// Make an instance of the robot from the blueprint
        Transform spawnParent = GameObject.Find("SpawnedRobos").transform;
        Transform spawnedRobo = Instantiate(bp, spawnParent.position, spawnParent.rotation, spawnParent);

        /// When we spawn a robo check its parts for mods and add those modifiers to the spawned robo
        if (spawnedRobo.GetComponent<Ally>())
        {
            spawnedRobo.GetComponent<Ally>().addMods(totalMods);
        }
        totalMods.Clear(); ///Clear total mods after we spawn this robo or we will maintain these mods for future robos

        yield return new WaitForSeconds(1.0f); /// TODO: Change this to use the length of longest animation

        spawnedRobo.gameObject.SetActive(true);
    }

    void playAnims() ///This function is responsible for playing the animations of the robo bay arms
    {
        RobobayAnim[] ra = robobayArms.GetComponentsInChildren<RobobayAnim>();
        foreach(RobobayAnim r in ra)
        {
            StartCoroutine(r.play());
        }
    }

    bool alreadyIn(Collider c) /// checks to see if this transform has been added to the list of parts in the bay before
    {
        foreach(Transform p in partsInBay)
        {
            if (c.transform == p)
            {
                return true;
            }
        }
        return false;
    }
    void logParts()
    {
        Debug.Log("_____PARTS IN BAY COUNT: " + partsInBay.Count);
        foreach (Transform p in partsInBay)
        {
            Debug.Log("PART NAME: " + p.name + " part tag: " + p.tag);
        }
    }
    string LogNameList(List<Transform> a)
    {
        string outStr = "";
        foreach (Transform ax in a)
        {
            outStr += " " + ax.name;
        }
        return outStr;
    }
    List<Transform> childrenToList(Transform parent)
    {
        List<Transform> outList = new List<Transform>();
        foreach (Transform child in parent)
        {
            outList.Add(child);
        }
        return outList;
    }
}
