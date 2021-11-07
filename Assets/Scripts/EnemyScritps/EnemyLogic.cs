using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyLogic : MonoBehaviour
{
    

    //hitbox

    public BoxCollider2D hitbox;
    public BoxCollider2D boxCollider;
    public Collider2D[] hits = new Collider2D[10];
    public ContactFilter2D filter;
    // Damage struct 
    public float maxHealth = 10;
    public float pushRecoverySpeed = 0.2f;
    public float currentHealth;
    public int damagePoint;
    public float pushForce = 2.0f;


    // Immunity
    public float immuneTime = 0.1f;
    protected float lastImmune;

    // Push
    protected Vector2 pushDirection;

    public virtual void FixedUpdate()
    {
        Debug.Log(currentHealth);
        hitbox.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;
            if (hits[i].tag == "Player")
            {
                OnCollide(hits[i]);
            }

            hits[i] = null;
        }
        
    }

    public void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            Damage dmg = new Damage
            {
                damageAmount = damagePoint,
                origin = transform.position,
                pushForce = pushForce

            };

            coll.SendMessage("TakeDamage", dmg);

        }
    }

    public void TakeDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            currentHealth = currentHealth - dmg.damageAmount;
            Debug.Log(currentHealth);
            //currentHealth = Mathf.Clamp(currentHealth - dmg.damageAmount, 0, maxHealth);
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

}
