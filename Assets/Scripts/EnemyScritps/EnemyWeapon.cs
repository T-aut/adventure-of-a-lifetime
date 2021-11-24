using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{    
    //hitbox
    public Collider2D[] hits = new Collider2D[10];
    public BoxCollider2D hitbox;
    public ContactFilter2D filter;
    public int damagePoint;
    public float pushForce;

    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
}
