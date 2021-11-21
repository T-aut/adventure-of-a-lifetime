using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public PlayerMovement movement;
    public float dashDistance;
    public float dashStaminaCost;
    public float dashAnimationDuration;
    public float dashCooldownTimer = 0f;
    public float dashCooldown;
    public float deadlyStabComboTimeframe;
    public float deadlyStabAnimationDuration;
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
    public bool dashCanBeUsed;
    public bool isDashButtonDown;
    public bool regenerationEnabled;
    public bool fireSwordComboCanHappen;
    public bool deadlyStabComboCanHappen;
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
    public float lastImmune = 0f;

    // Push
    public Vector2 pushDirection;

    void Awake()
    {
        playerIsDead = false;
        dashCanBeUsed = true;
        isDashButtonDown = false;
        fireSwordComboCanHappen = false;
        deadlyStabComboCanHappen = false;
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
        dashCooldownTimer = dashCooldown;
    }

    void Update()
    {
        if (regenerationEnabled)
        {
            RegenerateResources();
        }
        
        if (!dashCanBeUsed)
        {
            CalculateDashCooldownTime();
        }

        // Combo moves.
        if (Input.GetButtonDown("Fire1") && animator.GetBool("IsCasting") && !animator.GetBool("FireSwordCombo") && fireSwordComboCanHappen)
        {
            DoFireSwordCombo();
        }
        else if (Input.GetButtonDown("Fire1") && !animator.GetBool("DeadlyStabCombo") && deadlyStabComboCanHappen)
        {
            DoDeadlyStabCombo();
        }

        if (!movement.IsControlEnabled()) return;

        // Attack, spells and abilities.
        if (Input.GetButtonDown("Fire1") && !animator.GetBool("IsAttacking") && !animator.GetBool("IsCasting"))
        {
            Attack();
        }
        else if (Input.GetButtonDown("Spell1") && !animator.GetBool("IsAttacking"))
        {
            LaunchFireBall();
        }
        else if (Input.GetButtonDown("Dash") && !animator.GetBool("IsAttacking") && !animator.GetBool("IsCasting") && dashCanBeUsed)
        {
            if (currentStamina >= dashStaminaCost)
            {
                isDashButtonDown = true;
                UseStamina(dashStaminaCost);
            }
        }
    }

    private void DoFireSwordCombo()
    {
        if (currentStamina >= attackStaminaCost)
        {
            fireSwordComboCanHappen = false;
            UseStamina(attackStaminaCost);
            StartCoroutine(WaitForFireSwordAnimation());
        }
    }

    private void DoDeadlyStabCombo()
    {
        if (currentStamina >= attackStaminaCost)
        {
            deadlyStabComboCanHappen = false;
            UseStamina(attackStaminaCost);
            StartCoroutine(WaitForDeadlyStabAnimation());
        }
    }

    private void Attack()
    {
        if (currentStamina >= attackStaminaCost)
        {
            UseStamina(attackStaminaCost);
            SoundManagerScript.PlaySound("dagger");
            StartCoroutine(WaitForAttackAnimation());
        }
    }

    private void LaunchFireBall()
    {
        if (currentMana >= fireballManaCost)
        {
            UseMana(fireballManaCost);
            SoundManagerScript.PlaySound("fireball");
            StartCoroutine(WaitForFireballAnimation());
        }
    }

    void FixedUpdate()
    {
        if (isDashButtonDown)
        {
            Vector2 positionAfterDash = movement.rb.position + movement.GetDirectionVelocity() * dashDistance * movement.speed * Time.fixedDeltaTime;
            movement.rb.MovePosition(positionAfterDash);
            StartCoroutine(WaitForDashAnimation());
            isDashButtonDown = false;
        }

        // If TakeDamage() exists and there's no other way for player to has his health decreased
        // Maybe this Fixed update logic is not required anymore?
        if (currentHealth <= 0)
        {
            Death();
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

    private void CalculateDashCooldownTime()
    {
        dashCooldownTimer -= Time.deltaTime;

        if (dashCooldownTimer <= 0f)
        {
            dashCanBeUsed = true;
            dashCooldownTimer = dashCooldown;
        }
    }

    private void TakeDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            currentHealth = Mathf.Clamp(currentHealth - dmg.damageAmount, 0, maxHealth);
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;
            movement.Push(pushDirection);
            UpdateHealth(currentHealth);
            if (currentHealth <= 0)
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
        movement.SetControlEnabled(false);
        animator.SetBool("IsDead", true);
        regenerationEnabled = false;
        UpdateHealth(0); // Maybe UI updates should be moved to another function?

        playerIsDead = true;
        FindObjectOfType<GameManager>().EndGame();
    }

    // Use mana resource.
    private void UseMana(float manaUsed)
    {
        // Make sure that the mana can't go below zero.
        currentMana = Mathf.Clamp(currentMana - manaUsed, 0, maxMana);

        // Update UI after using mana.
        UpdateMana(currentMana);
    }

    // Use stamina resource.
    private void UseStamina(float staminaUsed)
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
        yield return new WaitForSeconds(fireballCastAnimationDuration / 2);

        // Fire sword combo did not happen.
        if (fireSwordComboCanHappen)
        {
            fireSwordComboCanHappen = false;
            yield return new WaitForSeconds(fireballCastAnimationDuration / 2);
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

    private IEnumerator WaitForDashAnimation()
    {
        movement.SetControlEnabled(false);
        animator.SetBool("IsDashing", true);

        yield return new WaitForSeconds(dashAnimationDuration);

        animator.SetBool("IsDashing", false);
        dashCanBeUsed = false;
        movement.SetControlEnabled(true);

        // Wait for deadly stab combo.
        deadlyStabComboCanHappen = true;
        yield return new WaitForSeconds(deadlyStabComboTimeframe);
        deadlyStabComboCanHappen = false;
    }

    private IEnumerator WaitForDeadlyStabAnimation()
    {
        movement.SetControlEnabled(false);
        animator.SetBool("DeadlyStabCombo", true);

        yield return new WaitForSeconds(deadlyStabAnimationDuration);

        animator.SetBool("DeadlyStabCombo", false);
        movement.SetControlEnabled(true);
    }

}