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

    public Animator myAnimator;
    public float attackAnimationDuration = 0.4f;
    public float attackCooldown;
    // Immunity
    public float immuneTime = 0.1f;
    protected float lastImmune;
    public float attackRange;
    protected Rigidbody2D rb;
   protected Transform target;
    // Push
    protected Vector2 pushDirection;
    public virtual void FixedUpdate()
    {

        float distance_between_player = Vector2.Distance(rb.position, target.position);
        if (distance_between_player < attackRange)
        {
            StartCoroutine(WaitForAttackAnimation());
        }
    }

    public void TakeDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            currentHealth = Mathf.Clamp(currentHealth - dmg.damageAmount, 0, maxHealth);
            Debug.Log(currentHealth);
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;
            if (currentHealth == 0)
            {
                Death();
            }
        }
    }

    protected void Death()
    {
        Destroy(gameObject);
    }

    public IEnumerator WaitForAttackAnimation()
    {
        myAnimator.SetBool("Attacking", true);

        yield return new WaitForSeconds(attackAnimationDuration);

        myAnimator.SetBool("Attacking", false);
    }
}
