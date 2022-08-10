using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss : MonoBehaviour
{
    public GameObject player;
    public float moveSpeed=2f;
    private Rigidbody2D rb;
    private Vector3 directionToPlayer;
    private Vector3 localScale;
    public GameObject lootDrop;
    public int hp=100;
    float dirX;

private Transform aimTransform;
    [SerializeField]
	GameObject enemyHitbox;    

public projectile ProjectilePrefab;

     
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
    private void Awake() {
        aimTransform = transform.Find("Gon");
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
        FireAtHero();  
    }
    private void FireAtHero(){
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        float angle= -(Mathf.Atan2( directionToPlayer.x,directionToPlayer.y) * Mathf.Rad2Deg);
        aimTransform.eulerAngles=new Vector3(0,0,angle);
        projectile inst=Instantiate(ProjectilePrefab,aimTransform.position,aimTransform.transform.rotation);
        
    }
}
