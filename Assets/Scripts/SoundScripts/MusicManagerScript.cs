using UnityEngine;

public class MusicManagerScript : MonoBehaviour
{
    public static AudioClip forestMusic;
    static AudioSource audioSource; 
    // Start is called before the first frame update
    void Start()
    {
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
}
