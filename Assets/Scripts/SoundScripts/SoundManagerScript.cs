using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip playerMoveSound, fireballSound, daggerSound, fireballExplodeSound, skeletonHurtSound, skeletonAttackSound,
     wolfHurtSound, enemyDeathSound, wolfAttackSound, playerHurtSound, dashSound, comboAttackSound;
    public static AudioSource audioSource, audio1, audio2, audio3, audio4, audio5; 

    public static float delay = 5f;
    public static float lastDelay;

    // Start is called before the first frame update
    void Start()
    {
        playerMoveSound = Resources.Load<AudioClip> ("Audio/Effects/playerMove");
        fireballSound = Resources.Load<AudioClip> ("Audio/Effects/fireball");
        daggerSound = Resources.Load<AudioClip> ("Audio/Effects/dagger");
        fireballExplodeSound = Resources.Load<AudioClip> ("Audio/Effects/fireballExplode");
        wolfHurtSound = Resources.Load<AudioClip> ("Audio/Effects/wolfHurt");
        skeletonHurtSound = Resources.Load<AudioClip> ("Audio/Effects/skeletonHurt");
        enemyDeathSound = Resources.Load<AudioClip> ("Audio/Effects/enemyDeath");
        wolfAttackSound = Resources.Load<AudioClip> ("Audio/Effects/wolfAttack");
        playerHurtSound = Resources.Load<AudioClip> ("Audio/Effects/playerHurt");
        dashSound = Resources.Load<AudioClip> ("Audio/Effects/dash");
        comboAttackSound = Resources.Load<AudioClip> ("Audio/Effects/comboAttack");
        skeletonAttackSound = Resources.Load<AudioClip> ("Audio/Effects/skeletonAttack");

        audioSource = GetComponent<AudioSource> ();  
        audio1 = gameObject.AddComponent<AudioSource>();
        audio2 = gameObject.AddComponent<AudioSource>();
        audio3 = gameObject.AddComponent<AudioSource>();
        audio4 = gameObject.AddComponent<AudioSource>();
        audio5 = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {   
    }

    public static void PlaySound (string clip)
    {  
        switch (clip){  
            case "playerMove":
                if(!audioSource.isPlaying){
                    audioSource.PlayOneShot(playerMoveSound);
                }
                break;
            case "fireball":
                audioSource.PlayOneShot(fireballSound);
                break;
            case "dagger":
                audioSource.PlayOneShot(daggerSound);
                break;
            case "fireballExplode":
                audioSource.PlayOneShot(fireballExplodeSound);
                break;
            case "wolfHurt":  
                if(!audio3.isPlaying){
                    audio3.PlayOneShot(wolfHurtSound, 0.7f);
                }   
                break;
            case "skeletonHurt":  
                if(!audio4.isPlaying){
                    audio4.PlayOneShot(skeletonHurtSound);
                }   
                break;
            case "enemyDeath":
                audioSource.PlayOneShot(enemyDeathSound, 3.5f); 
                break;
            case "wolfAttack":
                if(!audio2.isPlaying){
                    audio2.PlayOneShot(wolfAttackSound, 0.6f);
                }
                break;
            case "playerHurt":
                if(!audio1.isPlaying){
                    audio1.PlayOneShot(playerHurtSound, 0.6f);
                }
                break;
            case "dash":
                audioSource.PlayOneShot(dashSound);
                break;
            case "comboAttack":
                audioSource.PlayOneShot(comboAttackSound);
                break;
            case "skeletonAttack":
                if(!audio5.isPlaying){
                    audioSource.PlayOneShot(skeletonAttackSound);
                }
                break;
        }
    }
}
