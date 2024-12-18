using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeMenu : MonoBehaviour
{
    //this function is for the upgrade menu screen on the sculptor idol
    PlayerStats playerStats;
    [SerializeField] TextMeshProUGUI AttackPrice;
    private int startingAttackPrice = 50;
    private int startingVitalityPrice = 100;
    [SerializeField] TextMeshProUGUI VitalityPrice;
    [SerializeField] TextMeshProUGUI Success;
    [SerializeField] TextMeshProUGUI Failed;
    // Start is called before the first frame update
    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        AttackPrice.text = startingAttackPrice.ToString();
        VitalityPrice.text = startingVitalityPrice.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        AttackPrice.text = startingAttackPrice.ToString();
        VitalityPrice.text = startingVitalityPrice.ToString();
    }

    public void UpgradeAttackDamage(){
        //if the player presses on the upgrade attack damage option.
        if(playerStats.playerExp >= startingAttackPrice){
            playerStats.playerExp -= startingAttackPrice;
            playerStats.attackDamage = playerStats.attackDamage + 10;
            Success.gameObject.SetActive(true);
            StartCoroutine(FadeInAndOut(Success.gameObject, 1f));
            startingAttackPrice += 200;
        }
        //if the player doesn't have enough exp
        else{
            Failed.gameObject.SetActive(true);
            StartCoroutine(FadeInAndOut(Failed.gameObject, 1f));
        }
    }

    public void UpgradeVitalityPoints(){
        //if the player presses on the upgrade vitality damage option.
        if(playerStats.playerExp >= startingVitalityPrice){
            playerStats.playerExp -= startingVitalityPrice;
            playerStats.currentHealth = playerStats.currentHealth + 10;
            Success.gameObject.SetActive(true);
            StartCoroutine(FadeInAndOut(Success.gameObject, 1f));
            startingVitalityPrice += 300;
        }
        //if the player doesn't have enough exp
        else{
            Failed.gameObject.SetActive(true);
            StartCoroutine(FadeInAndOut(Failed.gameObject, 1f));
        }
    }

    private IEnumerator FadeInAndOut(GameObject targetObject, float duration)
    {
        //function for the fading of the result of upgrading text
        TextMeshProUGUI textComponent = targetObject.GetComponent<TextMeshProUGUI>();
        float startAlpha = textComponent.alpha;
        float targetAlpha = 1f;

        float elapsedTime = 0f;

        // Fade in
        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            textComponent.alpha = alpha;

            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        
        // Wait for delay
        yield return new WaitForSecondsRealtime(duration);

        startAlpha = textComponent.alpha;
        targetAlpha = 0f;
        elapsedTime = 0f;

        // Fade out
        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            textComponent.alpha = alpha;

            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        // Deactivate after fade out
        targetObject.SetActive(false);

        Debug.Log("Fading coroutine finished for: " + targetObject.name);
    }
}
