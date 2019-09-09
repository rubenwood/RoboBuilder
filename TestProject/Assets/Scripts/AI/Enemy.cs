using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Agents
{
    [HideInInspector]
    public GameObject spawnedFrom;

    ///Changing these to use a proper loot table
    public float chanceToDropItem, chanceToDropMod;

    public Vector2 shootAngles;
    public Transform launchPoint;

    /// Start is called before the first frame update
    void Start()
    {
        /// Set target to some random transform first
        target = GameObject.Find("Player").transform;
        /// Add targetable tags & layers
        enemylayers.Add(15);

        StartCoroutine(go());
    }

    /// Update is called once per frame
    void Update()
    {
        setLookPos(target); /// have to do the look at here otherwise it will freak out
    }

    /// Coroutine for stuff we need to update frequently but shouldn't put in update
    IEnumerator go()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateRate);
            setAgentDest(target);
        }        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.isTrigger == false)
        {
            if (isEnemyLayer(col.transform))
            {
                target = col.transform;
                laserShoot();               
            }
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (isEnemyLayer(col.transform))
        {
            stopLaser();          
        }
    }

    /// When something hits the enemy
    void OnCollisionEnter(Collision collider)
    {
        /// check to see if we got hit by a projectile that is not on the same layer as this enemy
        if (collider.gameObject.tag == "projectile" && collider.gameObject.layer != gameObject.layer)
        {
            takeDamage();
        }        
    }

    protected override void onDeath()
    {
        base.onDeath();
        /// triggered when this enemy dies
        if(Random.value >= 0.5)
        {
            dropRandomItem();
        }

        if(spawnedFrom.GetComponent<SpawnEnemy>().spawnOnDeath == true)
        {
            /// decrease spawn count
            spawnedFrom.GetComponent<SpawnEnemy>().spawnCount--;
        }

    }    
    
    /// Item dropping 
    void dropRandomItem()
    {
        /// drops a random item from drop list
        GameObject aDrop = Instantiate(drops.GetChild(Random.Range(0, drops.childCount)).gameObject, transform.position, transform.rotation, dropsParent);
        /// if we are dropping a part...
        if (aDrop.GetComponent<Part>())
        {
            dropPart(aDrop.GetComponent<Part>());
        }
        aDrop.SetActive(true);
    }

    /// When dropping a part we need to do some extra stuff
    void dropPart(Part p)
    {
        if(Random.value <= chanceToDropMod) /// chance for a dropped part to have a mod
        {
            string[] modList = { "test", "test2", "test3", "test4" };
            int roll = Random.Range(0, modList.Length);
            p.addMod(modList[roll]);
        }   
    }
}
