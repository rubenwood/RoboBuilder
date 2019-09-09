using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Smelter : MonoBehaviour
{
    public string[] scoreTags;
    public AudioClip[] scoreSounds;

    public int power = 0, partProgess = 0;
    private int maxProgress;

    public Transform unlockedPartsParent;   /// Transform containing all the unlocked parts
    [HideInInspector]
    public Transform selectedPart;          /// currently selected parts transform
    public Transform partSpawnLocation;     /// location to spawn the part when built

    private AudioSource scoreAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        scoreAudioSource = gameObject.GetComponent<AudioSource>();

        foreach (Transform part in unlockedPartsParent)
        {
            Debug.Log(part.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectPart(Transform p)
    {
        try {
            selectedPart = p;
            maxProgress = selectedPart.GetComponent<Part>().getMaxProg();

            Debug.Log("Selected Part is: " + selectedPart.name + " Reqruires " + maxProgress + " parts.");
        } catch (Exception e) {
            Debug.Log("NO SELECTED PART!");
        }
    }

    void OnTriggerEnter(Collider col)
    {
        playCorrespondingSound(col);

        if (col.tag == "battery")
        {
            power++;
        }
        if(col.tag == "junk" && selectedPart != null) /// if its tagged as junk and there is a selected part
        {
            if(partProgess < maxProgress)
            {
                partProgess++;
            }
            checkBuild();
        }        
    }

    void checkBuild() /// check to see if build is complete, if so, spawn the part
    {
        if (partProgess >= maxProgress && power > 0)
        {
            partProgess = 0;

            spawnPart();
        }
    }
    void spawnPart()
    {
        Transform spawnedPart = Instantiate(selectedPart, partSpawnLocation.position, partSpawnLocation.rotation, partSpawnLocation);
        ///spawnedPart.gameObject.AddComponent<Rigidbody>();
        spawnedPart.gameObject.GetComponent<Rigidbody>().useGravity = false;
        spawnedPart.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        ///spawnedPart.gameObject.AddComponent<MeshCollider>(); 
        ///spawnedPart.gameObject.GetComponent<Collider>().convex = true;
        spawnedPart.gameObject.GetComponent<Collider>().isTrigger = true;
        spawnedPart.gameObject.layer = 10; /// add this part to the parts layer
    }

    void playCorrespondingSound(Collider col)
    {
        for (int i = 0; i < scoreTags.Length; i++)
        {
            if (col.tag == scoreTags[i])
            {
                scoreAudioSource.PlayOneShot(scoreSounds[i]);
            }
        }
    }
}
