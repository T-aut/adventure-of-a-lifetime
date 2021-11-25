using UnityEngine;
 using System.Collections;
 using UnityEngine.SceneManagement;

public class MusicManagerScript : MonoBehaviour
{
    public static AudioClip forestMusic, castleMusic;
    static AudioSource audioSource; 
    public string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene ();
        sceneName = currentScene.name;

        forestMusic = Resources.Load<AudioClip> ("Audio/Music/forestMusic");
        castleMusic = Resources.Load<AudioClip> ("Audio/Music/CastleMusic");


        audioSource = GetComponent<AudioSource> ();  

    }

    // Update is called once per frame
    void Update()
    {   
        if (sceneName == "ForestZone") {
            if(!PauseMenu.GamePaused){
                if(!audioSource.isPlaying){
                    audioSource.PlayOneShot(forestMusic);
                }
            }else{
                audioSource.Stop();
            }
        }
        else if(sceneName == "CastleZone")
        {
            if(!PauseMenu.GamePaused){
                if(!audioSource.isPlaying){
                    audioSource.PlayOneShot(castleMusic);
                }
            }else{
                audioSource.Stop();
            }
        }
        
    }
}
