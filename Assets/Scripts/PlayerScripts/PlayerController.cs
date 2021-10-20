using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public PlayerMovement movement;
    public float attackAnimationDuration = 0f;
    public GameObject fireballProjectile;
    public float fireballCastAnimationDuration = 0f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !animator.GetBool("IsAttacking") && !animator.GetBool("IsCasting")) {
            StartCoroutine(WaitForAttackAnimation());
        }
        else if (Input.GetButtonDown("Spell1") && !animator.GetBool("IsAttacking"))
        {
            StartCoroutine(WaitForFireballAnimation());
        }
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
        animator.SetBool("IsCasting", true);
        movement.SetControlEnabled(false);

        yield return new WaitForSeconds(fireballCastAnimationDuration);

        CreateFireball();
        animator.SetBool("IsCasting", false);
        movement.SetControlEnabled(true);
    }

    // Creates a fireball at the appropriate location near the player with a computed velocity and angle
    private void CreateFireball()
    {
        Vector2 fireballVelocity = movement.GetDirectionVelocity();
        Fireball fireball = Instantiate(fireballProjectile, transform.position, Quaternion.identity).GetComponent<Fireball>();
        fireball.Setup(fireballVelocity, ComputeFireballAngle());
    }

    // Calculates at what angle should the fireball sprite fly
    private Vector3 ComputeFireballAngle()
    {
        Vector2 fireballVelocity = movement.GetDirectionVelocity();
        // Calculate the rotation angle in degrees
        float degrees = Mathf.Atan2(fireballVelocity.y, fireballVelocity.x) * Mathf.Rad2Deg;
        return new Vector3(0, 0, degrees);
    }
}
