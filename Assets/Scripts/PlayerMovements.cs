using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMovements : MonoBehaviour, IDataPersistence
{
    //this class responsible for every thing the player does

    [Header("move info")]
    [SerializeField] private float jumpSpeed = 6f;
    [SerializeField] private float runSpeed = 8f;
    private const int InitialWallJumps = 1;
    private int wallJumps = InitialWallJumps;
    private int facingDirection = 1;
    public bool deathBlow = false;
    private Vector2 moveInput;
    private bool isRunning;
    private bool isFacingRight = true;

    [Header("Combat")]
    [SerializeField] Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayer;
    PlayerStats playerStats;
    private float attackRate = 2f;
    private float nextAttackTime = 0f;
    private bool blockCooldown = false;

    [Header("collision info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private GameObject interactText;
    [SerializeField] private GameObject ExpBar;
    private bool isTouchingGround;
    [SerializeField] private bool isWallDetected;
    private EnemySoldier enemy;
    
    [Header("Components")]
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private CapsuleCollider2D myCapsuleCollider;
    private SpriteRenderer mySpriteRenderer;
    private InputValue value;
    private AudioManager audioManager;
    [SerializeField] AudioSource SFXsource;
    [SerializeField] private Canvas UpgradeCanvas;
    [SerializeField] private Canvas LetterCanvas;
    [SerializeField] private Canvas DeathCanvas;
    [SerializeField] private Canvas TaskCanvas;
    [SerializeField] private Button reserructOption;

    public PauseMenu PauseMenu;

    public Canvas DeathCanvasObject
    {
        get { return DeathCanvas; }
        set { DeathCanvas = value; }
    }

    public GameObject ExpBarObject
    {
        get { return ExpBar; }
        set { ExpBar = value; }
    }

    public bool blockCooldownTrigger
    {
        get { return blockCooldown; }
        set { blockCooldown = value; }
    }

    public Canvas UpgradeCanvasObject
    {
        get { return UpgradeCanvas; }
        set { UpgradeCanvas = value; }
    }

    public Canvas LetterCanvasObject
    {
        get { return LetterCanvas; }
        set { LetterCanvas = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        DeathCanvas.gameObject.SetActive(false);
        UpgradeCanvas.enabled = false;
        LetterCanvas.enabled = false;
        playerStats = FindObjectOfType<PlayerStats>();
        myAnimator = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        if(SceneManager.GetActiveScene().buildIndex == 2){
            audioManager.playMusic(audioManager.backgroundOneMusic);
        }
        else if(SceneManager.GetActiveScene().buildIndex == 3){
            audioManager.playMusic(audioManager.backgroundTwoMusic);
        }
        else if(SceneManager.GetActiveScene().buildIndex == 4){
            audioManager.playMusic(audioManager.backgroundThreeMusic);
        }
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        
    }

    // Update is called once per frame
    void Update()
    {
        //if the player status is not alive then nothing happens.
        if(!playerStats.isAlive){
            return;
        }
        CheckWallCollision();
        CheckGroundCollision();
        CheckRunning();
        Death();
        OnFire();
        FlipSpriteDirection();
        Potion();
        SetAnimation();
        Blocking();
        DeathBlow();
        touchingObject();
        PosturePunishment();
    }

    public void LoadData(GameData data){
        Debug.Log("loaded from player");
        Debug.Log(data.level);
        //function to load the data from the saved data
        if (data.level == SceneManager.GetActiveScene().buildIndex){
            transform.position = data.playerPosition;
        }
    }

     public void SaveData(GameData data){
         //function to save the data from the progress.
         data.playerPosition = transform.position;
         data.level = SceneManager.GetActiveScene().buildIndex;
     }

    private void CheckGroundCollision(){
        //function that checks if the player is on the ground and not in air.
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        if (isTouchingGround){
            wallJumps = InitialWallJumps;
        }
    }

    public void PlaySFX(AudioClip clip){
        SFXsource.PlayOneShot(clip);
    }

    private void touchingObject(){
        //function that is responsible for how a certain UI's and things in the game behave when the player touches various objects

        //if the player touches the sculptor Idol
        if(myCapsuleCollider.IsTouching(GameObject.FindWithTag("SculptorIdol").GetComponent<BoxCollider2D>()))
            {
                ExpBar.gameObject.SetActive(false);
                interactText.gameObject.SetActive(true);
                if(Input.GetKeyDown(KeyCode.F))
                {
                    playerStats.playerExtraLife = true;
                    playerStats.currentHealth = 100;
                    playerStats.currentPotions = 3;
                    UpgradeCanvas.enabled = true;
                    Time.timeScale = 0f;
                }
            }
        else
        //if the player touches a letter object
        if(myCapsuleCollider.IsTouching(GameObject.FindWithTag("Letter").GetComponent<BoxCollider2D>())){
            ExpBar.gameObject.SetActive(false);
            interactText.gameObject.SetActive(true);
            if(Input.GetKeyDown(KeyCode.F))
            {
                LetterCanvas.enabled = true;
                Time.timeScale = 0f;
            }
        }

        //if the player touches a door
        else if(myCapsuleCollider.IsTouching(GameObject.FindWithTag("Door").GetComponent<BoxCollider2D>())){
            ExpBar.gameObject.SetActive(false);
            interactText.gameObject.SetActive(true);
            if(Input.GetKeyDown(KeyCode.F))
            {
                // Get the position of the door
                Vector3 doorPosition = GameObject.FindWithTag("Door").GetComponent<Transform>().position;

                // Get the current position of the player
                Vector3 currentPlayerPosition = transform.position;

                // Add +1 to the x position of the door and update the player's position
                currentPlayerPosition.x = doorPosition.x + 1f;
                transform.position = currentPlayerPosition;
            }
        }
        else
        {
            //to hide the interact option and return the exp bar on display
            interactText.gameObject.SetActive(false);
            ExpBar.gameObject.SetActive(true);
        }
        
        //if the player presses the main exit button which is escape.
        if (Input.GetKeyDown(KeyCode.Escape)){
            //if the upgrade canvas is on then it closes it
            if(UpgradeCanvas.enabled){
                Time.timeScale = 1f;
                DataPersistenceManager.instance.SaveGame();
                UpgradeCanvas.enabled = false;
            }
            //if the letter canavs is on...
            else if(LetterCanvas.enabled){
                Time.timeScale = 1f;
                LetterCanvas.enabled = false;
            }
            else if(TaskCanvas.enabled){
                Time.timeScale = 1f;
                TaskCanvas.enabled = false;
            }
            //if nothing then the escape button is responsible for pausing and resuming the game
            else {
                if(PauseMenu.GameIsPaused()){
                    PauseMenu.Resume();
                }
                else{
                    PauseMenu.Pause();
                }
            }
        }
    }

    private void CheckWallCollision(){
        //function to check if the player colliding with a wall for the wall jump
        isWallDetected = Physics2D.Raycast(wallCheck.position, transform.right, 0.35f, whatIsGround);
    }

    private void WallJump(){
        //function that triggers the wall jump
        myAnimator.SetTrigger("wallJump");
        myRigidbody.velocity = new Vector2(0f, jumpSpeed+2);
    }

    private void OnDrawGizmos() {
        //functino that draws certain shapes for easier developement of the game
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + 0.35f, wallCheck.position.y, wallCheck.position.z));
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void SetAnimation(){
        //function that is responsible for activating certain animations based on boolean variables
        if (isTouchingGround)
        {
            myAnimator.SetBool("isRunning", isRunning);
            myAnimator.SetBool("isJumping", true);
        }
        myAnimator.SetBool("isJumping", !isTouchingGround);
    }

    void DeathBlow(){
        //function if the player presses the deathblow trigger
        if(deathBlow && Input.GetKeyDown(KeyCode.X)){
            myAnimator.SetTrigger("canDeathBlow");
            deathBlow = false;
        }
    }

    void Potion(){
        //functino if the player drinks potion
        if(Input.GetKeyDown(KeyCode.C)){
            myAnimator.SetTrigger("drinkPotion");
            playerStats.TakePotion();
        }
    }

    void OnFire(){
        //function if the player presses the left control button to attack
        if(!playerStats.isAlive || !isTouchingGround){
            return;
        }
        if(Time.time >= nextAttackTime){
            if(Input.GetKeyDown(KeyCode.LeftControl)){
                myAnimator.SetTrigger("isAttacking");
                Collider2D [] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

                foreach(Collider2D enemyCollider in hitEnemies)
                {
                    if (enemyCollider.CompareTag("Enemy"))
                    {
                        // Get the enemy object from the collider
                        EnemySoldier enemySoldier = enemyCollider.GetComponent<EnemySoldier>();
                        EnemyGeneral enemyGeneral = enemyCollider.GetComponent<EnemyGeneral>();
                        EnemyBoss enemyBoss = enemyCollider.GetComponent<EnemyBoss>();

                        // Check if the enemy object is not null and is not the player's own box collider
                        if (enemySoldier != null && !enemyCollider.CompareTag("EnemyTriggerArea") && !enemyCollider.CompareTag("EnemyDetectionZone"))
                        {
                            // Deal damage to the enemy soldier
                            enemySoldier.takeDamage(playerStats.attackDamage);
                            
                        }
                        else if(enemyGeneral != null && !enemyCollider.CompareTag("EnemyTriggerArea") && !enemyCollider.CompareTag("EnemyDetectionZone"))
                        {
                            // Deal damage to the enemy general
                            enemyGeneral.takeDamage(playerStats.attackDamage);
                        }
                        else if(enemyBoss !=null){
                            enemyBoss.takeDamage(playerStats.attackDamage);
                        } 
                    }
                }
                
                nextAttackTime = Time.time +0.6f / attackRate;
            }
        }
    }

    void Blocking(){
        //function that controls how ceratin states of blocking affect the player
        if(!playerStats.isAlive || isRunning || !isTouchingGround || blockCooldown){
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            myAnimator.SetBool("isBlocking", true);
        }
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            // Continue updating the "isBlocking" parameter as long as the Left Alt key is held down
            myAnimator.SetBool("isBlocking", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            myAnimator.SetBool("isBlocking", false);
        }
    }

    void OnMove(InputValue value){
        //function for when the player is pressing the arrow keys for moving, input function
        //player can't move if he is dead or blocking
        if(!playerStats.isAlive || myAnimator.GetBool("isBlocking")){
            return;
        }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value){
        //function for when the player is pressing the spacebar key for jumping, input function

        //player can't jump if he is dead or blocking
        if(!playerStats.isAlive || myAnimator.GetBool("isBlocking")){
            return;
        }
        // audioManager.PlaySFX(audioManager.jumpSound);
        //if the player jumps on a wall and wall jumps are available then a wall jump is triggered
        if(isWallDetected && wallJumps > 0 && Input.GetKeyDown(KeyCode.Space)){
            wallJumps--;
            WallJump();
        }
        if (!isTouchingGround){
            return;
        }
        if(value.isPressed){
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpSpeed);
        }
    }

    void CheckRunning(){
        //function that checks if the player is running
        Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity =  playerVelocity;
        isRunning = Mathf.Abs(moveInput.x) > Mathf.Epsilon;
    }

    void FlipSpriteDirection(){
        //function that flips the player based on it's movements
        if(myRigidbody.velocity.x > 0 && !isFacingRight){
            SetSpriteDirection();
        }else if(myRigidbody.velocity.x < 0 && isFacingRight){
            SetSpriteDirection();
        }
    }

    void SetSpriteDirection(){
        //function that is related to the flip sprite direction which actively changes the
        //player direction
        facingDirection = facingDirection * -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0,180,0);
    }

    private void PosturePunishment(){
        if(blockCooldown){
            return;
        }
        if(playerStats.currentPosture >= 100){
            playerStats.currentPosture = 0;
            myAnimator.SetBool("isBlocking", false);
            Debug.Log("PUNISHED!");
            StartCoroutine(startBlockCooldown());
        }
    }

    private IEnumerator startBlockCooldown(){
        blockCooldown = true;
        Debug.Log("blockcooldown is true and cooldown started");
        yield return new WaitForSeconds(5);
        blockCooldown = false;
    }

    void Death(){
        //function that is responsible for the player death...

        //if the player dies from falling then death canvas is on but reserruct option is not interactable
        if(myRigidbody.velocity.y < -17f){
            playerStats.isAlive = false;
            print("Death!");
            myAnimator.SetTrigger("Death");
            audioManager.PlaySFX(audioManager.deathSound);
            DeathCanvas.gameObject.SetActive(true);
            reserructOption.interactable = false;
        }
        if(playerStats.currentHealth <= 0){
        //if the player dies from hp loss then death canvas is on amd reserruct option is interactable
            playerStats.isAlive = false;
            print("Death!");
            myAnimator.SetTrigger("Death");
            audioManager.PlaySFX(audioManager.deathSound);
            DeathCanvas.gameObject.SetActive(true);
            if(playerStats.playerExtraLife){
                reserructOption.interactable = true;
            }
            else{
                reserructOption.interactable = false;
            }
        }
    }

}
