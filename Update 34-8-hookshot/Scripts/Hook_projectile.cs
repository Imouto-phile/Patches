using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook_projectile : MonoBehaviour
{
    public float speed = 3f;
    public int damage = 5;
    GameObject enemy;
    public float range=10;
    private GameObject player;
    private Transform to;
    private bool grappled;
    float elapsedtime;
    // Update is called once per frame
    private void Awake(){
        player=GameObject.FindGameObjectWithTag("Player");
    }
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
        if (grappled){
            
            
            
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Walls"){
            
                      
        }        
        if (collision.gameObject.tag == "enemyHitbox"){      
            Debug.Log("deteted"); 

            to=collision.transform;   
            Transform From=player.transform;
                   
            grappled=true;
            
            StartCoroutine(StartGrappling(From ,to));
        }      
        
    

    }

    private IEnumerator StartGrappling(Transform from, Transform to){
        elapsedtime =0;
        while (grappled){
            
            elapsedtime += Time.deltaTime;
            float per=elapsedtime/2;
            Vector2 from_dis=from.transform.position;
            Vector2 to_dis=to.transform.position;
            player.transform.position=Vector3.Lerp(from_dis,to_dis,per);
            
            if ((per >= 1) || (Vector2.Distance(from.position,to.position)<0.2)){
                grappled=false;
                break;
            }
            yield return null;}
    } 
}
