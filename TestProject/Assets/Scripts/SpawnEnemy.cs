using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemy;

    public int spawnMax;
    public int spawnCount;

    public float startTime;
    public float timeBetweenSpawns;

    public bool spawnOnDeath;

    public Transform spawnParent;

    public bool spawnInArea;
    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        /// if we are not spawning on death then spawn every time
        if (spawnInArea)
        {
            StartCoroutine(spawnArea(radius));
        }
        else
        {
            InvokeRepeating("spawn", startTime, timeBetweenSpawns);
        }
           
    }

    void spawn(Vector3 pos)
    {
        if (spawnCount < spawnMax)
        {
            GameObject aEnemy = Instantiate(enemy, pos, transform.rotation, spawnParent);
            ///Make sure the enemy knows which spawner spawned it
            aEnemy.GetComponent<Enemy>().spawnedFrom = gameObject;
            aEnemy.SetActive(true);
            spawnCount++;
        }        
    }

    void spawn()
    {
        spawn(transform.position);
    }


    IEnumerator spawnArea(float r)
    {
        while (true)
        {

            Vector2 rpos = Random.insideUnitCircle * r;
            Vector3 sp = transform.position + new Vector3(rpos.x, 0, rpos.y);

            spawn(sp);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }        
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
