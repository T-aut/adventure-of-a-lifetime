using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public Animator animator;
    public float fireballSpeed;
    public float fireballExplosionAnimationDuration;
    public Vector3 currentDirection;
    public Rigidbody2D rb;

    // Awake is called when the object is created, but before Start
    // If we don't set the IsFlying value here, the object will never stay in the fireball_fly state
    //  because by default the boolean value at creation won't be true or false
    void Awake()
    {
        animator.SetBool("IsFlying", true);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Position the fireball in the appropriate direction and calculate its velocity
    public void Setup(Vector2 velocity, Vector3 direction)
    {
        rb.velocity = velocity.normalized * fireballSpeed;
        // Rememebr the direction of the fireball for when we need to
        currentDirection = direction;
        // Find out a 2D object's direction
        transform.rotation = Quaternion.Euler(direction);
    }

    // This method is called when the fireball collides with another object that has a collider
    public void OnTriggerEnter2D(Collider2D otherObject)
    {
        // Destroy the fireball object if it collides with anything except the player or the grid
        if (!otherObject.gameObject.CompareTag("Player") && !otherObject.gameObject.CompareTag("Grid"))
        {
            StartCoroutine(WaitForExplosionAnimation());
            Destroy(this.gameObject);
        }
    }

    private IEnumerator WaitForExplosionAnimation()
    {
        animator.SetBool("IsFlying", false);
        // Stop the fireball but keep its rotation the same as previously
        Setup(new Vector2(0, 0), currentDirection);

        yield return new WaitForSeconds(fireballExplosionAnimationDuration);
    }

}
