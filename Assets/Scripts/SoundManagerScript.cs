using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip playerMoveSound, fireballSound, daggerSound, forestMusic;
    static AudioSource audioSource; 
    // Start is called before the first frame update
    void Start()
    {
        playerMoveSound = Resources.Load<AudioClip> ("Audio/Effects/playerMove");
        fireballSound = Resources.Load<AudioClip> ("Audio/Effects/fireball");
        daggerSound = Resources.Load<AudioClip> ("Audio/Effects/dagger");
        forestMusic = Resources.Load<AudioClip> ("Audio/Music/forestMusic");

        audioSource = GetComponent<AudioSource> ();  
    }

    // Update is called once per frame
    void Update()
    {   
        if(!PauseMenu.GamePaused){
            if(!audioSource.isPlaying){
                audioSource.PlayOneShot(forestMusic);
            }
        }else{
            audioSource.Stop();
        }
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
        }
    }
}
