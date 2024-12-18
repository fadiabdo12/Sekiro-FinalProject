using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour, IDataPersistence
{
    //this class is responsible for the player stats behaviour
    [SerializeField] public int currentHealth;
    [SerializeField] public float currentPosture;
    [SerializeField] TextMeshProUGUI potions;
    [SerializeField] TextMeshProUGUI exp;
    [SerializeField] public int currentPotions;
    [SerializeField] public int attackDamage;
    [SerializeField] public int playerExp;
    [SerializeField] public bool playerExtraLife;
    [SerializeField] Image ExtraLifeImage;
    AudioManager audioManager;
    public PlayerBars playerBars;
    public bool isAlive = true;
    EnemySoldier enemySoldier;

    // Start is called before the first frame update
    void Start()
    {
        currentPosture = 0;
        enemySoldier = GetComponent<EnemySoldier>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //controlling the player hp and posture bars based on how they are affected also the potions and exp amount
        playerBars.setHealth(currentHealth);
        playerBars.setPosture((int)currentPosture);
        potions.text = currentPotions.ToString();
        exp.text = playerExp.ToString();
        //if player extra life is true then the extra life image for reserruct option is visible but if not then it's not visible
        if(playerExtraLife){
            ExtraLifeImage.enabled = true;
        }
        else{
            ExtraLifeImage.enabled = false;
        }
    }

    public void LoadData(GameData data){
        //function that loads the game data from the saved file and sets it on the player
        this.playerExtraLife = data.extraLife;
        this.currentHealth = data.Health;
        this.currentPotions = data.Potions;
        this.attackDamage = data.attackDamage;
        this.playerExp = data.exp;
    }

    public void SaveData(GameData data){
        //function that saves the game data to a load file
        data.extraLife = this.playerExtraLife;
        data.Health = this.currentHealth;
        data.Potions = this.currentPotions;
        data.attackDamage = this.attackDamage;
        data.exp = this.playerExp;
    }

    void FixedUpdate() {
        //function that controls the posture of the player
        currentPosture = Mathf.Max(0f, currentPosture - (5f * Time.fixedDeltaTime));    
    }

    public void TakeDamage(int damage){
        //function that is responsible for when the player takes damage
        currentHealth -= damage;
        currentPosture += 20;
        
    }

    public void TakePotion(){
        //function that is is responsible for when the player takes potion
        if(currentPotions > 0){
            currentPotions-=1;
            potions.text = currentPotions.ToString();
            currentHealth += 30;
            // audioManager.PlaySFX(audioManager.potionSound);
        }
    }

}
