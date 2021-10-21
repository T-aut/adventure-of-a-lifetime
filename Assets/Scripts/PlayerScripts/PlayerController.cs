using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public PlayerMovement movement;
    public float attackAnimationDuration = 0f;
    public HealthSystem health = GameObject.FindGameObjectsWithTag("HealthSprite")[0].GetComponent<HealthSystem>();
    public HealthSystem mana = GameObject.FindGameObjectsWithTag("ManaSprite")[0].GetComponent<HealthSystem>();


    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !animator.GetBool("IsAttacking")) {
            StartCoroutine(WaitForAttackAnimation());
        }
    }

    private IEnumerator WaitForAttackAnimation() {
        animator.SetBool("IsAttacking", true);
        movement.SetControlEnabled(false);

        yield return new WaitForSeconds(attackAnimationDuration);

        animator.SetBool("IsAttacking", false);
        movement.SetControlEnabled(true);
    }
}
