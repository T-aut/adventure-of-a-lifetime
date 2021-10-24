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
    public HealthBar healthBar;
    public float currentMana;
    public float maxMana;
    public float currentStamina;
    public float maxStamina;
    public bool playerIsDead;

    void Awake()
    {
        playerIsDead = false;
    }

    void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        currentStamina = maxStamina;
        UpdateMaxHealth(maxHealth);
        UpdateMana(currentMana);
        UpdateStamina(currentStamina);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !animator.GetBool("IsAttacking")) {
            StartCoroutine(WaitForAttackAnimation());
        } else if (Input.GetKeyDown("q")) {
            TakeDamage(10);
            UseMana(10);
            UseStamina(10);
        }
    }

    // Update health bar value.
    void UpdateHealth(float health)
    {
        healthBar.SetHealth(health);
    }

    // Set max health value in health and update health bar current health value.
    void UpdateMaxHealth(float maxHealth)
    {
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update mana bar value.
    void UpdateMana(float mana)
    {

    }

    // Update stamina bar value.
    void UpdateStamina(float stamina)
    {

    }

    // Take damage and lose health.
    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            // If health is below zero, the player is dead and health is set to zero.
            playerIsDead = true;
            currentHealth = 0f;
        }

        // Update UI after taking damage.
        UpdateHealth(currentHealth);
    }

    // Use mana resource.
    void UseMana(float manaUsed)
    {
        currentMana -= manaUsed;
        if (currentMana < 0)
        {
            // If mana is below zero, it is set to zero.
            currentMana = 0f;
        }

         // Update UI after using mana.
        UpdateMana(currentMana);
    }

    // Use stamina resource.
    void UseStamina(float staminaUsed)
    {
        currentStamina -= staminaUsed;
        if (currentStamina < 0)
        {
            // If stamina is below zero, it is set to zero.
            currentStamina = 0f;
        }

        // Update UI after using stamina.
        UpdateStamina(currentStamina);
    }

    private IEnumerator WaitForAttackAnimation() {
        animator.SetBool("IsAttacking", true);
        movement.SetControlEnabled(false);

        yield return new WaitForSeconds(attackAnimationDuration);

        animator.SetBool("IsAttacking", false);
        movement.SetControlEnabled(true);
    }
}
