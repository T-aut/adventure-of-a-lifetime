using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public PlayerMovement movement;
    public float attackAnimationDuration = 0f;
    public GameObject fireballProjectile;
    public float fireballCastDelay = 0f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !animator.GetBool("IsAttacking")) {
            StartCoroutine(WaitForAttackAnimation());
        }
        else if (Input.GetButtonDown("Spell1") && !animator.GetBool("IsAttacking"))
        {
            StartCoroutine(WaitForFireballDelay());
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
    private IEnumerator WaitForFireballDelay()
    {
        movement.SetControlEnabled(false);
        CreateFireball();

        yield return new WaitForSeconds(fireballCastDelay);

        movement.SetControlEnabled(true);
    }

    // Creates a fireball at the appropriate location near the player
    private void CreateFireball()
    {
        Vector2 fireballVelocity = new Vector2();
        // If the player is moving, we can rely on its velocity to not be zero
        if (animator.GetBool("IsMoving"))
        {
            fireballVelocity = new Vector2(animator.GetFloat("Horizontal"), animator.GetFloat("Vertical"));
        }
        // When the player is not moving, we need to get the velocity based on the current direction of the player
        else
        {
            fireballVelocity = movement.GetDirectionVelocity();
        }
        Fireball fireball = Instantiate(fireballProjectile, transform.position, Quaternion.identity).GetComponent<Fireball>();
        fireball.Setup(fireballVelocity, Vector3.zero);
    }
}
