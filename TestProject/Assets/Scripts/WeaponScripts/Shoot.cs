using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject projectile;
    public Transform launchPoint, firedParent;
    public Vector3 projOffset;
    public float power;
    public float fireRate;
    public int fireMode;
    public AudioClip[] sounds;

    [HideInInspector]
    public bool trigger; /// Needed to trigger shooting in AI scripts

    private float nextTimeToFire = 0;

    // Start is called before the first frame update
    void Start(){
    }

    // Update is called once per frame
    void Update(){}

    protected GameObject spawnProj()
    {
        GameObject aProj = Instantiate(projectile, launchPoint.position, launchPoint.rotation, firedParent);
        ///aProj.layer = gameObject.layer;
        aProj.transform.rotation *= Quaternion.Euler(projOffset);
        aProj.SetActive(true);

        return aProj;
    }
    
    protected virtual void fire(bool trigger, Vector2 spread, Quaternion launchAngle, float cd, int numProj)
    {
        /// if the game object the shoot script is attached to is the player then set the trigger to be fire1
        /// fire mode 0 = single, else fire mode constant
        if (gameObject.tag == "PlayerWep" && fireMode == 0)
        {
            trigger = Input.GetButtonDown("Fire1");
        }
        else if(gameObject.tag == "PlayerWep" && fireMode != 0)
        {
            trigger = Input.GetButton("Fire1");
        }

        if (nextTimeToFire <= Time.time && trigger)
        {
            for(int i = 0; i < numProj; i++) /// do this for numProj
            {
                GameObject proj = spawnProj();
                /// spread
                proj.transform.position += new Vector3(Random.Range(-spread.x, spread.x), Random.Range(-spread.y, spread.y), Random.Range(0, 1));      
                /// launch angle
                launchPoint.localRotation = launchAngle;
                /// add force
                proj.GetComponent<Rigidbody>().AddForce(launchPoint.forward * power, ForceMode.Impulse);
            }
            /// Play shot audio
            launchPoint.gameObject.GetComponent<AudioSource>().PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
            /// allocate time for next fire
            nextTimeToFire = Time.time + cd;
        }        
    }

    /// Trigger, spread, launchAngle, cd, numProj

    /// SG
    protected virtual void fire(bool button, Vector2 spread, float cooldown, int numProj)
    {
        fire(button, spread, launchPoint.localRotation, cooldown, numProj);
    }
    /// MG
    protected virtual void fire(bool button, Quaternion launchAngle, float cooldown)
    {
        fire(button, Vector2.zero, launchAngle, cooldown, 1);
    }
    /// PShoot
    protected virtual void fire(bool button, float cooldown)
    {
        fire(button, Vector2.zero, launchPoint.localRotation, cooldown, 1);
    }
    /// OTHERS
    protected virtual void fire(bool button, Vector2 spread, float cooldown)
    {
        fire(button, spread, launchPoint.localRotation, cooldown, 1);
    }
    protected virtual void fire(Vector2 spread)
    {
        fire(Input.GetButtonDown("Fire1"), spread, launchPoint.localRotation, 0, 1);
    }
    protected virtual void fire(bool button)
    {
        fire(button, Vector2.zero, launchPoint.localRotation, 0, 1);
    }
    protected virtual void fire()
    {
        fire(Input.GetButtonDown("Fire1"), Vector2.zero, launchPoint.localRotation, 0, 1);
    }

    
}
