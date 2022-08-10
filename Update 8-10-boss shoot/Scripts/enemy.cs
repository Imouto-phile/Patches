using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public GameObject player;
    public float moveSpeed=2f;
    private Rigidbody2D rb;
    private Vector3 directionToPlayer;
    private Vector3 localScale;
    public GameObject lootDrop;
    public int hp=10;
    float dirX;

    public int damage=5;
    [SerializeField]
	GameObject enemyHitbox;    


     
    public int hpmax;  
    // Start is called before the first frame update
    public void TakeDamage(int damage){
        hp=hp-damage;
        if (hp <= 0){
            Destroy(gameObject);
            Instantiate(lootDrop, transform.position, Quaternion.identity);


        }
    }
    void Start()
    {
        player=GameObject.Find("player");
        rb = GetComponent<Rigidbody2D>();
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void FixedUpdate(){
        MoveEnemy();
    }
    void MoveEnemy(){
        directionToPlayer = (player.transform.position - transform.position).normalized;
        rb.velocity = new Vector2(directionToPlayer.x, directionToPlayer.y) * moveSpeed;
    }
    private void LateUpdate(){
       if(rb.velocity.x > 0){
        transform.localScale = new Vector3(-localScale.x,localScale.y,localScale.z);
       }
       else if( rb.velocity.x < 0){
         transform.localScale = new Vector3(localScale.x,localScale.y,localScale.z);
       }

    }
    private void Update(){
        //hitbox follows enemy
		dirX = Input.GetAxisRaw ("Horizontal") * moveSpeed;
		enemyHitbox.transform.position = new Vector2 (transform.position.x, transform.position.y);
		if (rb.velocity.y == 0) {
			rb.AddForce (Vector2.up * 700f);
		}    
    }
     private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.tag == "Player"){                        
            collision.gameObject.GetComponent<playerctrl>().TakeDamageHero(damage);
            
     }
}
}