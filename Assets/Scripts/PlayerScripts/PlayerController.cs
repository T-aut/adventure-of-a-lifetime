using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public PlayerMovement movement;
    public float attackAnimationDuration = 0f;
    public GameObject fireballProjectile;
    public float fireballManaCost;
    public float fireballCastAnimationDuration = 0f;
    public float attackStaminaCost;
    public float currentHealth;
    public float maxHealth;
    public HealthBar healthBar;
    public float currentMana;
    public float maxMana;
    public ManaBar manaBar;
    public float currentStamina;
    public float maxStamina;
    public StaminaBar staminaBar;
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
        UpdateMaxMana(currentMana);
        UpdateMaxStamina(currentStamina);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !animator.GetBool("IsAttacking") && !animator.GetBool("IsCasting")) {
            if(currentStamina >= attackStaminaCost)
            {
                UseStamina(attackStaminaCost);
                StartCoroutine(WaitForAttackAnimation());
            }
        }
        else if (Input.GetButtonDown("Spell1") && !animator.GetBool("IsAttacking"))
        {
            if(currentMana >= fireballManaCost)
            {
                UseMana(fireballManaCost);
                StartCoroutine(WaitForFireballAnimation());
            }
        }
    }

     // Update health bar value.
    void UpdateHealth(float health)
    {
        healthBar.SetHealth(health);
    }

    // Set max health value in health bar and update health bar current health value.
    void UpdateMaxHealth(float maxHealth)
    {
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update mana bar value.
    void UpdateMana(float mana)
    {
        manaBar.SetMana(mana);
    }

    // Set max mana value in mana bar and update mana bar current mana value.
    void UpdateMaxMana(float maxMana)
    {
        manaBar.SetMaxMana(maxMana);
    }

    // Update stamina bar value.
    void UpdateStamina(float stamina)
    {
        staminaBar.SetStamina(stamina);
    }

    // Set max stamina value in stamina bar and update stamina bar current stamina value.
    void UpdateMaxStamina(float maxStamina)
    {
        staminaBar.SetMaxStamina(maxStamina);
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

    private IEnumerator WaitForAttackAnimation() 
    {
        animator.SetBool("IsAttacking", true);
        movement.SetControlEnabled(false);

        yield return new WaitForSeconds(attackAnimationDuration);

        animator.SetBool("IsAttacking", false);
        movement.SetControlEnabled(true);
    }

    // Wait a delay after casting a fireball before moving
    private IEnumerator WaitForFireballAnimation()
    {
        // In order to enable diagonal casting we must save the player's state before the casting animation
        //  disables control
        Vector2 fireballVelocityBeforeAnimation = movement.GetDirectionVelocity();
        animator.SetBool("IsCasting", true);
        movement.SetControlEnabled(false);

        yield return new WaitForSeconds(fireballCastAnimationDuration);

        CreateFireball(fireballVelocityBeforeAnimation);
        animator.SetBool("IsCasting", false);
        movement.SetControlEnabled(true);
    }

    // Creates a fireball at the appropriate location near the player with a computed velocity and angle
    private void CreateFireball(Vector2 fireballVelocity)
    {
        Fireball fireball = Instantiate(fireballProjectile, transform.position, Quaternion.identity).GetComponent<Fireball>();
        fireball.Setup(fireballVelocity, ComputeFireballAngle(fireballVelocity));
    }

    // Calculates at what angle should the fireball sprite fly
    private Vector3 ComputeFireballAngle(Vector2 fireballVelocity)
    {
        // Calculate the rotation angle in degrees
        float degrees = Mathf.Atan2(fireballVelocity.y, fireballVelocity.x) * Mathf.Rad2Deg;
        return new Vector3(0, 0, degrees);
    }
}
