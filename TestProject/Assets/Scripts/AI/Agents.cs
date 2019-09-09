using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agents : MonoBehaviour
{
    public float updateRate; /// Used to control how often certain things get called

    public int hp;
    ///public float range; /// Used for OverlapSphere and drawing the gizmo

    public Transform drops;
    public Transform dropsParent;
    public AudioClip[] gotHitSound;

    protected Transform target; /// This agents target
    protected List<string> enemyTags = new List<string>();
    protected List<int> enemylayers = new List<int>();
    //protected List<string> allyTags = new List<string>();
    //protected List<string> allyLayers = new List<string>();

    protected NavMeshAgent nma;

    private Vector3 lookPos;    

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()
    {
        /// Cant get this component in Start
        if (GetComponent<NavMeshAgent>())
        {
            nma = GetComponent<NavMeshAgent>();
        }
    }


    protected void takeDamage()
    {
        ///play got hit sound
        GetComponent<AudioSource>().PlayOneShot(gotHitSound[Random.Range(0, gotHitSound.Length-1)]);
        hp--;
        if(hp == 0)
        {
            onDeath();
        }
    }

    ///Uses OverlapSphere to check if an enemy is still within range
    ///This is not currently used but may be used in the future
    //protected bool detectEnemy()
    //{
    //    Collider[] cols = Physics.OverlapSphere(transform.position, range);
    //    foreach(Collider c in cols)
    //    {
    //        if (isEnemyLayer(c.transform))
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    ///Use this to draw gizmos when using OverlapSphere or similar
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, range);
    //}

    protected void setAgentDest(Transform target)
    {
        if (target != null)
        {
            nma.destination = target.position;
        }        
    }
    protected void setLookPos(Transform target)
    {
        /// Set look position, but only use x and z of the targets position, not the Y otherwise the robos will tilt
        /// We will set the Y of the head of the robo instead, making only the head tilt to face the target
        if (transform.tag == "ally")
        {
            lookPos = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.LookAt(lookPos);
        }
        else
        {
            transform.LookAt(target);
        }       
    }
    protected void goToLookAtTarget(Transform target)
    {
        /// Set destination
        setAgentDest(target);
        /// Set look pos
        setLookPos(target);
    }
    protected bool isTaggedEnemy(Transform t)
    {
        if (enemyTags.Contains(t.tag))
        {
            return true;
        }
        return false;
    }
    protected bool isEnemyLayer(Transform t)
    {
        if (enemylayers.Contains(t.gameObject.layer))
        {
            return true;
        }
        return false;
    }

    protected IEnumerator shootEnemy()
    {
        yield return new WaitForSeconds(updateRate);        
        if (GetComponent<Shoot>())
        {
            foreach(Shoot s in GetComponents<Shoot>())
            {
                s.trigger = true;
            }
        }
    } 
    protected void laserShoot() /// Maybe need to be coroutine
    {
        if (GetComponent<LaserShoot>())
        {
            foreach(LaserShoot l in GetComponents<LaserShoot>())
            {
                l.trigger = true;
            }
        }
    }

    protected void stopShoot()
    {
        if (GetComponent<Shoot>())
        {
            foreach (Shoot s in GetComponents<Shoot>())
            {
                s.trigger = false;
            }
        }
    }

    protected void stopLaser()
    {
        if (GetComponent<LaserShoot>())
        {
            foreach (LaserShoot l in GetComponents<LaserShoot>())
            {
                l.trigger = false;
            }
        }
    }

    protected IEnumerator targettingAlly()
    {
        yield return new WaitForSeconds(updateRate);
        /// put functionality concerning targetting allied units here
        stopShoot();
        stopLaser();
    }

    protected virtual void onDeath()
    {
        Destroy(gameObject);
    }
    protected float getDistance(Transform target)
    {
        float dist = Vector3.Distance(transform.position, target.position);
        ///Debug.Log("Distance from me to the target is: " + dist);
        return dist;
    }
}
