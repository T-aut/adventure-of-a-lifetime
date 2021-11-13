using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public string sceneNameToLoad;
    public float transitionTime;

    public void LoadGivenScene()
    {
        StartCoroutine(TransitionToScene());
    }

    private IEnumerator TransitionToScene()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneNameToLoad);
    }
}
