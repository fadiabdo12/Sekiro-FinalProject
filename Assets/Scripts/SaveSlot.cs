using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI profileName;

    public void SetData(GameData data){
        //there's no profile data for the specific profileId
        if(data == null)
        {  
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        //there's profile data for the specific profileId
        else
        {
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);

            profileName.text = profileId;
        }
    }

    public string GetProfileId(){
        return this.profileId;
    }
}
