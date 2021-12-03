using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryFragment : MonoBehaviour
{
    public Animator animator;
    public GameObject Dialog1;
    public GameObject Dialog2;
    public PlayerMovement PlayerMovement;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!animator.GetBool("IsActivated")) animator.SetBool("IsActivated", true);
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false; // so that the character does not collide with the fading fragment

        Dialog1.SetActive(true);
        PlayerMovement.SetControlEnabled(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (Dialog1.activeSelf)
            {
                Dialog1.SetActive(false);
                Dialog2.SetActive(true);
                return;
            }
            if (Dialog2.activeSelf)
            {
                Dialog2.SetActive(false);
                PlayerMovement.SetControlEnabled(true);
                PlayerMovement.SetAltarUnlocked(true);
                return;
            }
        }
    }
}
