using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldier : MonoBehaviour
{
    //this class is responsible for the enemy soldier behavior(enemy general ai)
    [Header("Stats")]
    [SerializeField] int maxHealth = 100;
    [SerializeField] int maxPosture = 100;
    [SerializeField] float currentPosture;
    private Coroutine uiTimerCoroutine;
    private float uiTimerDuration = 10f;
    [SerializeField] int currentHealth;

    [Header("Behaviour")]    
    public float attackDistance; //minimum distance for an attack
    public float enemyMoveSpeed;
    public float attackCooldown; //timer for cooldown
    private float distance; //distance between player and enemy
    private bool attackMode;
    [HideInInspector] public bool inRange; //check if player is in range
    private bool isCooling; //check if enemy is cooling after an attack
    private float intTimer;

    [Header("Components")]
    private Rigidbody2D enemyRigidbody;
    private ParticleSystem deathBlowEffect;
    [HideInInspector] public Transform target;
    public PlayerBars playerBars;
    private PlayerMovements player;
    PlayerStats playerStats;
    public GameObject DetectionZone;
    public GameObject TriggerArea;
    private Animator enemyAnimator;
    [SerializeField] Transform leftPoint;
    [SerializeField] Transform rightPoint;
    private SpriteRenderer enemySpriteRenderer;
    [SerializeField] private Canvas enemyUI;
    private CapsuleCollider2D enemyCapsuleCollider;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        SelectTarget();
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
        if(!attackMode){
            Move();
        }
        if(!InsidePatrolRadius() && !inRange && !enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack")){// if the player got out of radius enemy returns to patrolling
            SelectTarget();
        }
        playerBars.setPosture((int)currentPosture);
        playerinRange();

        if(currentPosture > 100f){
            player.deathBlow = true;
            if(Input.GetKeyDown(KeyCode.X)){
                // audioManager.PlaySFX(audioManager.deathBlowSound);
                Death();
                deathBlowEffect.Stop();
            }
            if (!deathBlowEffect.isPlaying)
            {
                deathBlowEffect.Play();
            }
        }
    }
    void FixedUpdate() {
        //to update the posture every time it changes
        currentPosture = Mathf.Max(0f, currentPosture - (5f * Time.fixedDeltaTime));    
    }

    void playerinRange(){
        //function that checks if player in range and if in range then it activates the enemy logic
        //function
        if(inRange == true){
            enemyLogic();
        }
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
        if(!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("enemyAttackOne"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
            {
                if(distance >= 2f && distance <=9)
                {
                    enemyMoveSpeed = 2f;
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
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, enemyMoveSpeed * Time.deltaTime);
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
        //function that controls the cooldown of the attck for the enemy
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
        currentHealth -= damage;
        playerBars.setHealth(currentHealth);
        enemyAnimator.SetTrigger("isHurt");
        enemyUI.gameObject.SetActive(true);
        currentPosture += 30;
        playerBars.setPosture((int)currentPosture);

        if (uiTimerCoroutine != null)
        {
            StopCoroutine(uiTimerCoroutine);
        }

        uiTimerCoroutine = StartCoroutine(DisableUITimer());

        if(currentHealth <= 0 )
        {
            Death();
        }
    }

    private bool InsidePatrolRadius(){
        //function that returns if the player is currently inside the patrol radius of the two points
        return transform.position.x > leftPoint.position.x && transform.position.x < rightPoint.position.x;
    }

    public void SelectTarget(){
        //function that updates the target for the enemy if it's the player or the patrol points that
        // moves betweeen them
        float DistanceToLeft = Vector2.Distance(transform.position, leftPoint.position);
        float DistanceToRight = Vector2.Distance(transform.position, rightPoint.position);

        if(DistanceToLeft > DistanceToRight){
            target = leftPoint;
            Flip();
        }
        else{
            target = rightPoint;
            Flip();
        }
    }

    public void Death(){
        //function that is responsible for the death behaviour of the enemy
        enemyAnimator.SetTrigger("Death");
        playerStats.playerExp += 100;
        enemyUI.gameObject.SetActive(false);
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        enemyRigidbody.gravityScale = 0;
        enemyCapsuleCollider.enabled = false;
        this.enabled = false;
    }

    public void Flip(){
        //function that flips the enemy object based on it's movements
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

    private IEnumerator DisableUITimer()
    //funciton that is responsible for turning off the ui if the enemy is not attacked by the player for a
    //certain amount of time
    {
        yield return new WaitForSeconds(uiTimerDuration);
        enemyUI.gameObject.SetActive(false);
    }
}
