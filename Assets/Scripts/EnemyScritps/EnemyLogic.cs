using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyLogic : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    // Damage struct 
    public float maxHealth = 10;
    public float pushRecoverySpeed = 0.2f;
    public float currentHealth;
    protected float deathTime = 0;
    public Animator myAnimator;
    public PlayerMovement playerMovement;
    public float attackAnimationDuration = 0.4f;
    public float attackCooldown;
    // Immunity
    public float immuneTime = 0.1f;
    protected float lastImmune;
    public float attackRange;
    protected Rigidbody2D rb;
    protected Transform target;
    protected bool isDead = false;
   // Push
    protected Vector2 pushDirection;
    public virtual void FixedUpdate()
    {

        float distance_between_player = Vector2.Distance(rb.position, target.position);
        if (distance_between_player < attackRange)
        {
            SoundManagerScript.PlaySound("wolfAttack");
            StartCoroutine(WaitForAttackAnimation());
        }
    }

    public void TakeDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTime)
        {
            SoundManagerScript.PlaySound("wolfHurt");
            lastImmune = Time.time;
            currentHealth = Mathf.Clamp(currentHealth - dmg.damageAmount, 0, maxHealth);
            
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;
            
            
            if (currentHealth == 0)
            {
                SoundManagerScript.PlaySound("enemyDeath");
                Death();
            }
        }
    }

    protected void Death()
    {
        playerMovement.ResetVelocity();
        pushDirection = Vector2.zero;
        rb.velocity = Vector3.zero;
        rb.mass = 10000;
        isDead = true;
        StartCoroutine(WaitForDeathAnimation());
    }

    public IEnumerator WaitForAttackAnimation()
    {
        myAnimator.SetBool("Attacking", true);

        yield return new WaitForSeconds(attackAnimationDuration);

        myAnimator.SetBool("Attacking", false);
    }
    public IEnumerator WaitForDeathAnimation()
    {
        myAnimator.SetTrigger("Dying");

        yield return new WaitForSeconds(0.5f);

        myAnimator.SetBool("Dead", true);

        yield return new WaitForSecondsRealtime(3f);
        Destroy(gameObject);



    }
}
