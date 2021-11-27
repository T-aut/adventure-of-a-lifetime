using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryFragment : MonoBehaviour
{
    public Animator animator;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!animator.GetBool("IsActivated")) animator.SetBool("IsActivated", true);
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false; // so that the character does not collide with the fading fragment
    }
}
