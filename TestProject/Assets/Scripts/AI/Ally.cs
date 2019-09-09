using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Agents
{
    private Transform player;

    [SerializeField]
    private List<string> mods;

    ///Modifiers
    protected int hpMod, projectileMod;
    protected float speedMod, fireRateMod, shotSpreadMod;

    // Start is called before the first frame update
    void Start()
    {
        ///init player as target
        player = GameObject.Find("Player").transform;
        target = player;

        /// Add targetable tags & layers
        enemylayers.Add(14);

        applyMods();

        /// Start running the go routine
        StartCoroutine(go());
    }

    void Update()
    {
        headsLook(getHeads());
    }

    IEnumerator go()
    {
        while (true)
        {            
            yield return new WaitForSeconds(updateRate);
            setAgentDest(target);                       
        }
    }
    ///Add mods to this robo
    public void addMods(List<string> ms)
    {
        mods.AddRange(ms);
    }
    
    private void checkMods()
    {
        ///This function will loop through this robos mods and apply them
        ///Once done the mods list should be cleared (set null?)
    }
    private void applyMods()
    {
        /// ApplyMods
        /// Each robo must have:
        hp += hpMod;
        nma.speed += speedMod;

        /// Each robo could have:
        /// PShoot,MGShoot,SGShoot,LaserShoot
        /// RULE: Ally cannot have laser unless it has a +NumLaser Mod
        /// if totalNumLasers > 0 apply laser mods else ignore laser mods
        /// 
    }

    List<Transform> getHeads()
    {
        List<Transform> heads = new List<Transform>();
        foreach (Transform t in transform)
        {
            if(t.tag == "head")
            {
                heads.Add(t);
            }            
        }
        return heads;
    }
    void headsLook(List<Transform> heads)
    {
        foreach (Transform h in heads)
        {
            h.LookAt(target);
        }
    }

    IEnumerator checkCol(Collider col)
    {
        while (true)
        {
            yield return new WaitForSeconds(updateRate);
            if (col)
            {
                focusTarget(col.transform);
            }            
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (!col.isTrigger && (col.gameObject.layer == 14 || col.gameObject.layer == 15))
        {
            StartCoroutine(checkCol(col));
        }
    }

    void focusTarget(Transform t)
    {
        /// this is for the existing target, not the incoming target 't'
        if (target != null && isEnemyLayer(target)){ 
            /// if we are targetting an enemy and it's hp is above zero then just return
            if(target.GetComponent<Enemy>().hp > 0)
            {
                return;
            }
        }
        /// if we are not targetting an enemy then 't' transform becomes the target
        /// if that target is tagged as an enemy, target enemy, if not then target ally
        if (isEnemyLayer(t))
        {
            target = t;
            StartCoroutine(shootEnemy());
        }
        else
        {
            target = player;
            StartCoroutine(targettingAlly());
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (isEnemyLayer(col.transform))
        {
            takeDamage();
        }
    }
}
