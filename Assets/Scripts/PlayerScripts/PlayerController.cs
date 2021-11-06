using System;
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
    public float fireSwordAnimationDuration = 0f;
    public float attackStaminaCost;
    public float regenSecondInterval;
    public float timeLeftUntilRegen = 0f;
    public float healthRegenAmount;
    public float currentMana;
    public float maxMana;
    public ManaBar manaBar;
    public float manaRegenAmount;
    public float currentStamina;
    public float maxStamina;
    public StaminaBar staminaBar;
    public float staminaRegenAmount;
    public bool regenerationEnabled;
    public bool fireSwordComboCanHappen;
    public bool playerIsDead;
    private BoxCollider2D weaponCollider2D;
    public ContactFilter2D filter;

    // Damage struct 
    public float maxHealth;
    public HealthBar healthBar;
    public float pushRecoverySpeed = 0.2f;
    public float currentHealth;

    // Immunity
    public float immuneTime = 0.1f;
    protected float lastImmune;

    // Push
    protected Vector2 pushDirection;



    void Awake()
    {
        playerIsDead = false;
        fireSwordComboCanHappen = false;
    }

    void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        currentStamina = maxStamina;
        UpdateMaxHealth(maxHealth);
        UpdateMaxMana(currentMana);
        UpdateMaxStamina(currentStamina);
        timeLeftUntilRegen = regenSecondInterval;
    }

    void Update()
    {
        if (regenerationEnabled)
        {
            RegenerateResources();
        }

        if (Input.GetButtonDown("Fire1") && animator.GetBool("IsCasting") && !animator.GetBool("FireSwordCombo") && fireSwordComboCanHappen)
        {
            if (currentStamina >= attackStaminaCost)
            {
                fireSwordComboCanHappen = false;
                UseStamina(attackStaminaCost);
                StartCoroutine(WaitForFireSwordAnimation());
            }
        }

        if (!movement.IsControlEnabled()) return;

        if (Input.GetButtonDown("Fire1") && !animator.GetBool("IsAttacking") && !animator.GetBool("IsCasting"))
        {
            if (currentStamina >= attackStaminaCost)
            {
                UseStamina(attackStaminaCost);
                StartCoroutine(WaitForAttackAnimation());
            }
            Attack();
        }
        else if (Input.GetButtonDown("Spell1") && !animator.GetBool("IsAttacking"))
        {
            LaunchFireBall();
        }
      
    }

    void Attack()
    {
        if (currentStamina >= attackStaminaCost)
        {
            UseStamina(attackStaminaCost);
            StartCoroutine(WaitForAttackAnimation());
        }
    }

    void LaunchFireBall()
    {
        if (currentMana >= fireballManaCost)
        {
            UseMana(fireballManaCost);
            StartCoroutine(WaitForFireballAnimation());
        }
    }

    private void RegenerateResources()
    {
        // As time passes, there is less time left until resource regeneration.
        timeLeftUntilRegen -= Time.deltaTime;

        if (timeLeftUntilRegen <= 0f)
        {
            // Clamp ensures that current resource amounts can never regenerate past the maximum amounts.
            currentHealth = Mathf.Clamp(currentHealth + healthRegenAmount, 0, maxHealth);
            currentMana = Mathf.Clamp(currentMana + manaRegenAmount, 0, maxMana);
            currentStamina = Mathf.Clamp(currentStamina + staminaRegenAmount, 0, maxStamina);

            UpdateHealth(currentHealth);
            UpdateMana(currentMana);
            UpdateStamina(currentStamina);

            timeLeftUntilRegen = regenSecondInterval;
        }
    }
     void TakeDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            currentHealth = Mathf.Clamp(currentHealth - dmg.damageAmount, 0, maxHealth);
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;
            movement.Push(pushDirection);
            UpdateHealth(currentHealth);
            if (currentHealth == 0)
            {
                Death();
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


    void Death()
    {
        playerIsDead = true;
    }

    // Use mana resource.
    void UseMana(float manaUsed)
    {
        // Make sure that the mana can't go below zero.
        currentMana = Mathf.Clamp(currentMana - manaUsed, 0, maxMana);

        // Update UI after using mana.
        UpdateMana(currentMana);
    }

    // Use stamina resource.
    void UseStamina(float staminaUsed)
    {
        // Make sure that the stamina can't go below zero.
        currentStamina = Mathf.Clamp(currentStamina - staminaUsed, 0, maxStamina);

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
        animator.SetBool("FireSwordCombo", false);
        animator.SetBool("IsCasting", true);
        movement.SetControlEnabled(false);

        // Wait half of the fireball animation for a fire sword combo.
        fireSwordComboCanHappen = true;
        yield return new WaitForSeconds(fireballCastAnimationDuration/2);

        // Fire sword combo did not happen.
        if (fireSwordComboCanHappen)
        {
            fireSwordComboCanHappen = false;
            yield return new WaitForSeconds(fireballCastAnimationDuration/2);
            CreateFireball(fireballVelocityBeforeAnimation);
            animator.SetBool("IsCasting", false);
            movement.SetControlEnabled(true);
        }
    }

    private IEnumerator WaitForFireSwordAnimation()
    {
        animator.SetBool("FireSwordCombo", true);
        movement.SetControlEnabled(false);

        yield return new WaitForSeconds(fireSwordAnimationDuration);

        animator.SetBool("IsCasting", false);
        animator.SetBool("FireSwordCombo", false);
        movement.SetControlEnabled(true);
    }

    // Creates a fireball at the appropriate location near the player with a computed velocity and angle
    private void CreateFireball(Vector2 fireballVelocity)
    {
        Fireball fireball = Instantiate(fireballProjectile, transform.position, Quaternion.identity)
            .GetComponent<Fireball>();
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