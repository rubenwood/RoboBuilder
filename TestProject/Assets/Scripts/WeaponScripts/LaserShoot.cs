using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShoot : MonoBehaviour
{
    public Transform launchPoint;
    public Vector3 shootAngles;
    public int dmg;
    public float chargeUp;
    public AudioClip chargeSound, fireSound;

    private LineRenderer line;
    private Coroutine beamshooting;

    [HideInInspector]
    public bool charging, trigger;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        charging = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "PlayerWep")
        {
            trigger = Input.GetButton("Fire1");
        }

        fire(trigger);
    }

    void fire(bool fireTrigger)
    {
        if (fireTrigger && !charging) /// if trigger is held and not currently charging
        {
            beamshooting = StartCoroutine(beamShoot());
        }
        else if (beamshooting != null)
        {
            /// if charging but trigger is not held then stop trying to shoot and stop charging, 
            /// only do this for player weapons, if its an ally or enemy weapon then it works differently
            if (gameObject.tag == "PlayerWep" && charging && !fireTrigger) 
            {
                launchPoint.GetComponent<AudioSource>().Stop();
                charging = false;
                StopCoroutine(beamshooting);            
            }            
        }
    }
    IEnumerator beamShoot()
    {
        GetComponent<MatFade>().enabled = false;

        if (!launchPoint.GetComponent<AudioSource>().isPlaying)
        {
            ///Set pitch relative to charge time
            launchPoint.GetComponent<AudioSource>().pitch = 1 / chargeUp;
            launchPoint.GetComponent<AudioSource>().PlayOneShot(chargeSound);
        }
        charging = true;

        yield return new WaitForSeconds(chargeUp);

        Vector3 dest = raycast();
        if (dest != Vector3.zero)
        {
            drawBeam(launchPoint.position, dest);

            ///set pitch back to 1
            launchPoint.GetComponent<AudioSource>().pitch = 1;
            launchPoint.GetComponent<AudioSource>().PlayOneShot(fireSound);
        }
        charging = false;
    }

    void drawBeam(Vector3 start, Vector3 end)
    {
        LineRenderer line = GetComponent<LineRenderer>();
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        line.enabled = true;
    }

    Vector3 raycast()
    {
        RaycastHit hit;
        launchPoint.localRotation = Quaternion.Euler(0, 0, 0);
        launchPoint.localRotation *= Quaternion.Euler(new Vector3(Random.Range(-shootAngles.x, shootAngles.x), Random.Range(-shootAngles.y, shootAngles.y), 0));

        Vector3 dest = Vector3.zero;

        if (Physics.Raycast(launchPoint.position, launchPoint.forward, out hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore))
        {
            dest = hit.point;
            dealDamage(hit.collider, dmg);
        }

        return dest;
    }

    void dealDamage(Collider col, int dmg)
    {
        /// Visual & audio feedback when target hit
        /// Only deal damage to things on a different layer 
        if(col.gameObject.layer != gameObject.layer)
        {
            if (col.GetComponent<Agents>())
            {
                col.GetComponent<Agents>().hp -= dmg;
                if (col.GetComponent<Agents>().hp == 0)
                {
                    Destroy(col.gameObject);
                }
            }

            if (col.tag == "Player")
            {
                col.transform.GetChild(2).gameObject.SetActive(true);
            }
        }
    }
}
