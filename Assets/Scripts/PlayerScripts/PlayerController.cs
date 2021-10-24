using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public PlayerMovement movement;
    public float attackAnimationDuration = 0f;
    public float currentHealth;
    public float maxHealth;
    public float currentMana;
    public float maxMana;
    public float currentStamina;
    public float maxStamina;
    public HealthSystem healthSystem;

    void Awake()
    {
        // Get the resource bar class instance.
        healthSystem = HealthSystem.Instance;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !animator.GetBool("IsAttacking")) {
            StartCoroutine(WaitForAttackAnimation());
        } else if (Input.GetKeyDown("q")) {
            healthSystem.TakeDamage(10);
            healthSystem.UseMana(10);
            healthSystem.UseStamina(10);
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
