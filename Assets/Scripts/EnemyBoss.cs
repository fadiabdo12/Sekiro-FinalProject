using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int maxHealth = 200;
    [SerializeField] int maxPosture = 150;
    [SerializeField] float currentPosture;
    [SerializeField] int currentHealth;
    private static int deathBlowCount = 3;

    [Header("Behaviour")]    
    public float attackDistance; //minimum distance for an attack
    public float enemyMoveSpeed;
    public float attackCooldown; //timer for cooldown
    private float distance; //distance between player and enemy
    private bool attackMode;
    [HideInInspector] public bool inRange; //check if player is in range
    private bool isCooling; //check if enemy is cooling after an attack
    private bool isRecovering;
    private float intTimer;

     [Header("Components")]
    private Rigidbody2D enemyRigidbody;
    private ParticleSystem deathBlowEffect;
    [SerializeField] private Transform target;
    public PlayerBars playerBars;
    private PlayerMovements player;
    PlayerStats playerStats;
    [SerializeField]private GameObject hitBox;
    private Animator enemyAnimator;
    private SpriteRenderer enemySpriteRenderer;
    [SerializeField] private Canvas enemyUI;
    private CapsuleCollider2D enemyCapsuleCollider;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentPosture = 0;
        intTimer = attackCooldown;
        playerBars.setMaxHealth(maxHealth);
        playerBars.setMaxPosture(maxPosture);
        playerStats = FindObjectOfType<PlayerStats>();
        enemyAnimator = GetComponent<Animator>();
        deathBlowEffect = GetComponent<ParticleSystem>();
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovements>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyCapsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isRecovering){
            return;
        }
        playerBars.setPosture((int)currentPosture);
        if(!attackMode){
            Move();
        }
        Flip();
        playerBars.setPosture((int)currentPosture);
        if(currentPosture > 100f){
            player.deathBlow = true;
            if(Input.GetKeyDown(KeyCode.X)){
                deathBlowCount --;
                if(deathBlowCount == 0){
                    Death();
                }
                else{
                    EnemyRecovering();
                }            
            }
            if (!deathBlowEffect.isPlaying)
            {
                deathBlowEffect.Play();
            }
        }
        enemyLogic();
    }

    void FixedUpdate() {
        //to update the posture every time it changes
        currentPosture = Mathf.Max(0f, currentPosture - (5f * Time.fixedDeltaTime));    
    }

    void enemyLogic(){
        //function that is responsible for how the enemy behaves when a player is in range.
        //calculates the player distance and behaves on it, if to attack or to stop attack
        //or if the cooldown of the attack is on
        distance = Vector2.Distance(transform.position, target.position);
        if(distance > attackDistance){
            StopAttack();
        }
        else if(attackDistance >= distance && isCooling == false){
            Attack();
        }
        if(isCooling){
            Cooldown();
            enemyAnimator.SetBool("Attack", false);
        }
    }

    void Move(){
        //function that is responsible for the enemy movements when the player is not discovered by the enemy
        Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, enemyMoveSpeed * Time.deltaTime);
        if(distance > 2f)
            {
                enemyMoveSpeed = 3f;
                enemyAnimator.SetBool("canRun", true);
                enemyAnimator.SetBool("canWalk", false);
            }
        else
            {
                enemyMoveSpeed = 1f;
                enemyAnimator.SetBool("canWalk", true);
                enemyAnimator.SetBool("canRun", false);
            }
    }

    void Attack(){
        //function for the attack of the enemy
        attackCooldown = intTimer;
        attackMode = true;
        enemyAnimator.SetBool("canWalk", false);
        enemyAnimator.SetBool("Attack", true);
    }

    void StopAttack(){
        //function to stop the attack of the enemy
        isCooling = false;
        attackMode = false;
        enemyAnimator.SetBool("Attack", false);
    }

    void Cooldown(){
        //function that controls the cooldown of the attack for the enemy
        attackCooldown -= Time.deltaTime;
        if(attackCooldown <= 0 && isCooling && attackMode){
            isCooling = false;
            attackCooldown = intTimer;
        }
    }

    public void TriggerCooling(){
        //function that simply trigger the cooldown for the enemy
        isCooling = true;
    }

    public void takeDamage(int damage){
        //function that is responsible for what happens to the enemy when he takes damage.
        if(isRecovering){
            return;
        }
        currentHealth -= damage;
        playerBars.setHealth(currentHealth);
        enemyAnimator.SetTrigger("isHurt");
        enemyUI.gameObject.SetActive(true);
        currentPosture += 15;
        playerBars.setPosture((int)currentPosture);

        if(currentHealth <= 0 )
        {
            deathBlowCount--;
            if(deathBlowCount == 0){
                Death();
            }
            else{
                EnemyRecovering();
            }
        }
    }

     public void EnemyRecovering(){
        currentHealth =200;
        currentPosture = 0;
        playerBars.setHealth(currentHealth);
        playerBars.setPosture((int)currentPosture);
        isRecovering = true;
        enemyAnimator.SetTrigger("Tired");
        deathBlowEffect.Stop(); //was commented
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        StartCoroutine(TiredCooldown());
    }

    public void Death(){
        //function that is responsible for the death behaviour of the enemy
        enemyAnimator.SetTrigger("Death");
        playerStats.playerExp += 1000;
        enemyUI.gameObject.SetActive(false);
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        deathBlowEffect.Stop();
        enemyRigidbody.gravityScale = 0;
        enemyCapsuleCollider.enabled = false;
        hitBox.gameObject.SetActive(false);
        this.enabled = false;
    }

     public void Flip(){
        if(this.enabled){
            Vector3 rotation = transform.eulerAngles;
            if(transform.position.x >target.position.x){
                rotation.y = 180f;
            }
            else{
                rotation.y = 0;
            }
            transform.eulerAngles = rotation;
        }
    }

    private IEnumerator TiredCooldown()
    {
    //isInvulnerable = true; // Enemy is invulnerable during recovery
    // Wait for 5 seconds
    yield return new WaitForSeconds(3f);
    //isInvulnerable = false; // Enemy is vulnerable again

    // Re-enable player movement
    GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
    GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

    isRecovering = false;
    }
}
