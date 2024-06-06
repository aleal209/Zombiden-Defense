using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

// modified version of towercontroller for targeting single enemies
public class TowerControllerSingle : MonoBehaviour
{
    public AudioSource src;
    public AudioClip sfx1;

    public float cooldown = 1.5f;
    public int damage = 1;
    private List<GameObject> enemies = new List<GameObject>();
    private int inProcess = 0;

    public float min = 0;

    [System.NonSerialized] public GameObject player;
    public Behaviour halo;

    private GameObject target;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemies.Add(other.gameObject);
            StartCoroutine(Targeter());
            Debug.Log(enemies.Count);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemies.Remove(other.gameObject);
        }
    }

    private IEnumerator Targeter()
    {
        if (inProcess == 0)
        {
            inProcess = 1;
            while (enemies.Count > 0)
            {
                src.clip = sfx1;
                src.Play();
                // gets the correct target by finding the max time an enemy in range has been alive (variable is named wrong but don't want to break anything/change multiple things)
                foreach (GameObject enemy in enemies)
                {
                    if (enemy.GetComponent<EnemyController>().timer > min) {
                        min = enemy.GetComponent<EnemyController>().timer;
                        target = enemy;
                    }
                }
                target.GetComponent<EnemyController>().Damage(damage);
                min = 0;
                yield return new WaitForSeconds(cooldown);
            }
            inProcess = 0;
        }
    }

    private void Start()
    {
        player = GameObject.Find("Player");
        halo.enabled = player.GetComponent<playerdata>().halo_on;
        Debug.Log(player.GetComponent<playerdata>().halo_on.ToString());
    }

    private void Update()
    {
        if (Input.GetKeyDown("h"))
        {
            if (halo.enabled == true)
            {
                player.GetComponent<playerdata>().halo_on = false;
                halo.enabled = player.GetComponent<playerdata>().halo_on;
            }
            else
            {
                player.GetComponent<playerdata>().halo_on = true;
                halo.enabled = player.GetComponent<playerdata>().halo_on;
            }
        }
        foreach (GameObject enemy in enemies.ToList()) {
            if (enemy.GetComponent<EnemyController>().health <= 0)
            {
                enemies.Remove(enemy);
            }
        }
    }
}



