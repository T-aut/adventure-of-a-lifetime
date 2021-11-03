using UnityEngine;
using UnityEngine.Audio;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip playerMoveSound;
    static AudioSource audioSource; 
    // Start is called before the first frame update
    void Start()
    {
        playerMoveSound = Resources.Load<AudioClip> ("Audio/Effects/playerMove");

        audioSource = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound (string clip)
    {
        if(!audioSource.isPlaying){
            switch (clip){
                case "playerMove":
                    audioSource.PlayOneShot (playerMoveSound);
                    break;
                
            }
        }
    }
}
