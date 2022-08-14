using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Takes and handles input and movement for a player character
public class playerctrl : MonoBehaviour
{
    public int exp = 0;
    public int maxExp = 10;
    public int level = 1;
    public float moveSpeed = 2f;
    public float collisionOffset = 0.05f;
    public int Health=100;
    public ContactFilter2D movementFilter;
    public Animator animator;
    public projectile ProjectilePrefab;
    public Transform LaunchOffset;
    private bool fire;
    private Vector3 fireDirection;
    float dirX;
    public GameObject Shield_sprite;
    public Transform staffn;
    public Transform staff_peak;
    private Transform aimTransform;
    public int hp=100;

    bool shield=false;
    [SerializeField]
	GameObject GemMagnet;


    public static Vector3 GetWorldPosition(){
        Vector3 vec = GetWorldPositionwz(Input.mousePosition, Camera.main);
        vec.z=0f;
        return vec;
    }
    public static Vector3 GetWorldPositionwz(Vector3 screenPosition, Camera worldCamera){
        Vector3 worldposition= worldCamera.ScreenToWorldPoint(screenPosition);
        return worldposition;
    }
    
    private void Awake() {
        aimTransform = transform.Find("aim");
        staffn = aimTransform.Find("staff");
        staff_peak=staffn.Find("Staff_peak");
        Debug.Log(staff_peak);
    }


    Vector2 movementInput;
    SpriteRenderer spriterenderer;
    Rigidbody2D rb;
    

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator=GetComponent<Animator>();
        spriterenderer=GetComponent<SpriteRenderer>();
    }
    private void Update(){
        


        //magnet follows player
		dirX = Input.GetAxisRaw ("Horizontal") * moveSpeed;
		GemMagnet.transform.position = new Vector2 (transform.position.x, transform.position.y);
		if (rb.velocity.y == 0) {
			rb.AddForce (Vector2.up * 700f);
		}
        //projectiles
        fire = Input.GetButtonDown("Fire1");
        Vector3 mouseposition = GetWorldPosition();
        Vector3 aimdir= (mouseposition - transform.position).normalized;

        float angle= Mathf.Atan2(aimdir.y, aimdir.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles=new Vector3(0,0,angle);
                    if (fire)
            {

               projectile inst=Instantiate(ProjectilePrefab,staff_peak.position,staff_peak.transform.rotation);
               //inst.transform.LookAt(mousePos);
            }
        shield = Input.GetKey(KeyCode.F);
        


    }


    private void FixedUpdate() {
            // if mvmt input is not 0 , try to move
            if (movementInput != Vector2.zero){
                // check for potential collisions
                int count = rb.Cast(
                    movementInput, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                    movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                    castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                    moveSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

                if(count == 0){
                    rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);

                }
                animator.SetBool("isMoving",true);
            }else{               
                animator.SetBool("isMoving",false);
            }
            if(movementInput.x < 0) {
                spriterenderer.flipX= true;
            }else if(movementInput.x > 0){
                spriterenderer.flipX= false;
            }
            //rb.velocity = new Vector2 (dirX, rb.velocity.y);
            
            
        
            Shield_sprite.SetActive(shield);
        }
    void OnMove(InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();

    }
    void xpGame(Collider2D col){
        if(exp >= maxExp ){
            level +=1;
            maxExp=maxExp*2+(maxExp/2);
        }     
    }
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag.Equals ("Gem")) {
			Destroy (col.gameObject);
            exp += 1;
		}
        xpGame(col);
		
	}   
    public void TakeDamageHero(int damage){
        Health=Health-damage;
        
        if (Health <= 0){
            Destroy(gameObject);
           
        }
    }
}