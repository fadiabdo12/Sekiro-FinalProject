using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXsource;
    [SerializeField]private AudioClip Cutscene;
    [SerializeField]private AudioClip backgroundOne;
    [SerializeField]private AudioClip backgroundTwo;
    [SerializeField]private AudioClip backgroundThree;
    // [SerializeField]private AudioClip groundTouch;
    // [SerializeField]private AudioClip jump;
    [SerializeField]private AudioClip RocksFalling;
    [SerializeField]private AudioClip death;
    [SerializeField]private AudioClip Block;
    // [SerializeField]private AudioClip hit;
    [SerializeField]private AudioClip getHit;
    // [SerializeField]private AudioClip deathBlow;
    // [SerializeField]private AudioClip potion;

    // public AudioClip jumpSound
    // {
    //     get { return jump; }
    //     set { jump = value; }
    // }
    public AudioClip CutsceneMusic
    {
        get { return Cutscene; }
        set { Cutscene = value; }
    }
    public AudioClip backgroundOneMusic
    {
        get { return backgroundOne; }
        set { backgroundOne = value; }
    }

    public AudioClip RocksFallingSound
    {
        get { return RocksFalling; }
        set { RocksFalling = value; }
    }

    public AudioClip backgroundTwoMusic
    {
        get { return backgroundTwo; }
        set { backgroundTwo = value; }
    }

    public AudioClip backgroundThreeMusic
    {
        get { return backgroundThree; }
        set { backgroundThree = value; }
    }

    public AudioClip deathSound
    {
        get { return death; }
        set { death = value; }
    }

    public AudioClip getHitSound
    {
        get { return getHit; }
        set { getHit = value; }
    }

    public AudioClip blockSound
    {
        get { return Block; }
        set { Block = value; }
    }

    // public AudioClip potionSound
    // {
    //     get { return potion; }
    //     set { potion = value; }
    // }

    // public AudioClip groundSound
    // {
    //     get { return groundTouch; }
    //     set { groundTouch = value; }
    // }

    // public AudioClip deathBlowSound
    // {
    //     get { return deathBlow; }
    //     set { deathBlow = value; }
    // }

    // public AudioClip hitSound
    // {
    //     get { return hit; }
    //     set { hit = value; }
    // }

    public void PlaySFX(AudioClip clip){
        SFXsource.PlayOneShot(clip);

    }

    public void playMusic(AudioClip BGMclip){
        musicSource.clip = BGMclip;
        musicSource.Play();
    }

}
