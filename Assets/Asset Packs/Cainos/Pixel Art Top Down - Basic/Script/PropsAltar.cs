using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

//when something get into the alta, make the runes glow
namespace Cainos.PixelArtTopDown_Basic
{

    public class PropsAltar : MonoBehaviour
    {
        public List<SpriteRenderer> runes;
        public LevelLoader _levelLoader;
        public float lerpSpeed;

        private Color curColor;
        private Color targetColor;
        private bool isStanding = false;
        private float timeRemaining = 2;

        private void OnTriggerEnter2D(Collider2D other)
        {
            targetColor = new Color(1, 1, 1, 1);
            timeRemaining = 2;
            isStanding = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            targetColor = new Color(1, 1, 1, 0);
            timeRemaining = 2;
            isStanding = false;
        }

        private void Update()
        {
            curColor = Color.Lerp(curColor, targetColor, lerpSpeed * Time.deltaTime);

            foreach (var r in runes)
            {
                r.color = curColor;
            }

            if (isStanding && timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else if(isStanding && timeRemaining <= 0)
            {
                LoadNextLevel();
            }
        }

        public void LoadNextLevel()
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }

        IEnumerator LoadLevel(int levelIndex)
        {
            //Play animation
            // transition.SetTrigger("Start");

            //Wait
            yield return new WaitForSeconds(1f);

            //Load scene
            SceneManager.LoadScene(levelIndex);
        }
    }
}
