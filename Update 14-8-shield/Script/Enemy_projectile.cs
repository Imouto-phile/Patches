using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_projectile : MonoBehaviour
{
    public float speed = 3f;
    public int damage = 1;
    GameObject enemy;
    public float range=10;
    GameObject boss;

    // Update is called once per frame
    private void Update()
    {
        if(range>=0)
        {
            range-=Time.deltaTime;
        }
        else{
            Destroy(gameObject);
        }
        transform.position += transform.up * Time.deltaTime * speed;
    }
    private void OnTriggerEnter2D(Collider2D oth) {
        Debug.Log("Hit"); 
        if (oth.gameObject.tag == "Player"){           
                        
            oth.gameObject.GetComponent<playerctrl>().TakeDamageHero(1);
            Destroy (gameObject);           
        }        
    }

}
