using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollCredits : MonoBehaviour
{
    public LevelLoader levelLoader;
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player"))
        {
            levelLoader.LoadGivenScene();
        }
    }
}
