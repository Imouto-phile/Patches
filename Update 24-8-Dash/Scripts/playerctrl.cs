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
    float movement_cells=0.0025f;
    private bool fire;
    private Vector3 fireDirection;
    float dirX;
    public GameObject Shield_sprite;
    public Transform staffn;
    public Transform staff_peak;
    private Transform aimTransform;
    public GameObject trail;
    public float dash_cd = 2f;
    bool dash_loaded=true;
    public float dashamount=40f;
    private enum State{
        Idle,
        Moving,
        Dashing,
    }
    private Vector3 movedir;
    private State state=State.Idle;

    private Vector3 rolldir;
    private Rigidbody2D body;
    [SerializeField]
	GameObject GemMagnet;
    private float cd_shield=3f;
    public float decelerator=30f;

    bool shield_loaded = true ;
    [SerializeField]public LayerMask dashing_mask;
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
        body=GetComponent<Rigidbody2D>();
        state=State.Moving;
        
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
        
        switch(state){
            case State.Idle:
                animator.SetBool("isMoving",false);

                if(Input.GetKey(KeyCode.W)){
                    state=State.Moving;
                    
                }
                if(Input.GetKey(KeyCode.S)){
                    state=State.Moving;
                    
                }
                if(Input.GetKey(KeyCode.D)){
                    state=State.Moving;
                    
                }
                if ((Input.GetKey(KeyCode.A))){
                    state=State.Moving;
                }
                firing();
                break;
            case State.Moving:
                float moveX= 0f;
                float moveY= 0f;
                animator.SetBool("isMoving",true);
                if(Input.GetKey(KeyCode.W)){
                    moveY=1f;
                    
                }
                if(Input.GetKey(KeyCode.S)){
                    moveY=-1f;
                }
                if(Input.GetKey(KeyCode.A)){
                    moveX=-1f;
                    spriterenderer.flipX=true;
                    
                }
                if(Input.GetKey(KeyCode.D)){
                    moveX=1f;
                    spriterenderer.flipX=false;
                }
                movedir=new Vector3(moveX,moveY).normalized;
                    //magnet follows player
                dirX = Input.GetAxisRaw ("Horizontal") * moveSpeed;
                GemMagnet.transform.position = new Vector2 (transform.position.x, transform.position.y);
                //if (rb.velocity.y == 0) {
                //	rb.AddForce (Vector2.up * 700f);
                //    
                //}
                //projectiles
                firing();


                if (!(Input.GetKey(KeyCode.W)) && !(Input.GetKey(KeyCode.S)) && !(Input.GetKey(KeyCode.A)) && !(Input.GetKey(KeyCode.D))){
                    state=State.Idle;
                }
                
                if ((Input.GetKeyDown(KeyCode.Space)) && (dash_loaded==true)){
                    Debug.Log("it passed");
                    rolldir=(GetWorldPosition() - transform.position).normalized;
                    dashamount=5f;
                    state=State.Dashing;
                }
                
                
        
            break;
            case State.Dashing:
                
                trail.SetActive(true);
                dashamount-=dashamount*decelerator*Time.deltaTime;
                float mindash = 3f;
                if (dashamount<mindash){
                    
                    state=State.Moving;
                    trail.SetActive(false);
                }
                break;
            
        }


    }
    private void FixedUpdate() {
            // if mvmt input is not 0 , try to move
        switch(state){
            case State.Idle:
                rb.velocity=new Vector2(0f,0f);
                break;
            case State.Moving:
                rb.velocity=movedir*moveSpeed;
                shielding();
                //dashing();
                
                break;
            case State.Dashing:
                dashing();
                break;
        }

        }

    private void firing(){
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
    
    private void shielding(){
        if (Input.GetKey(KeyCode.F) && shield_loaded){
            StartCoroutine(shield_available());
            StartCoroutine(Cd());
        } 
    }
    public void TakeDamageHero(int damage){
        Health=Health-damage;
        
        if (Health <= 0){
            Destroy(gameObject);
           
        }
    }
    
    IEnumerator Cd(){
        yield return new WaitForSeconds(cd_shield);
        shield_loaded = true;
    }
    IEnumerator shield_available(){
        Shield_sprite.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Shield_sprite.SetActive(false);
        shield_loaded = false;
    }
    IEnumerator Cd_dash(){
        yield return new WaitForSeconds(dash_cd);
        dash_loaded = true;
    }
    void dash_use(){
            if ((Input.GetKey(KeyCode.Space))&&(dash_loaded)){
            body.velocity= rolldir * dashamount;
            dash_loaded=false;
            }
            //Vector3 dashPos=transform.position + (aimdir*dashamount);
            
            // RaycastHit2D R=Physics2D.Raycast(transform.position,aimdir,dashamount,dashing_mask);
            // if (R.collider != null){
            //   dashPos=R.point;   
            //   Debug.Log($"collision {R.collider.tag}");
              
            // }
            //rb.MovePosition(dashPos);
            //rb.MovePosition(dashPos);
            
            }
            
            
    
    void dashing(){
        if (dash_loaded){
            dash_use();
            StartCoroutine(Cd_dash());
            }
        
        
    }
    
}
