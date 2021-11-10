using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damagePoint = 1;
    public float pushForce = 2.0f;
    public Animator playerAnimator;
    public ContactFilter2D filter;
    public BoxCollider2D boxCollider;
    public Collider2D[] hits = new Collider2D[10];

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Collision work
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;
            
            OnCollide(hits[i]);
            hits[i] = null;
        }

    }
    void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {   
            if (playerAnimator.GetBool("FireSwordCombo"))
            {
                damagePoint *= 3;
            }
            else if (playerAnimator.GetBool("DeadlyStabCombo"))
            {
                damagePoint *= 5;
            }

            Damage dmg = new Damage
            {
                damageAmount = damagePoint,
                origin = transform.position,
                pushForce = pushForce
            };

            coll.SendMessage("TakeDamage", dmg);

        }
    }
}
