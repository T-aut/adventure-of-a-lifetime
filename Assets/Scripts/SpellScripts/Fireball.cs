using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float fireballSpeed;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Position the fireball in the appropriate direction and calculate its velocity
    public void Setup(Vector2 velocity, Vector3 direction)
    {
        rb.velocity = velocity.normalized * fireballSpeed;

        // Find out a 2D object's direction
        transform.rotation = Quaternion.Euler(direction);
    }

    // This method is called when the fireball collides with another object that has a collider
    public void OnTriggerEnter2D(Collider2D otherObject)
    {
        // Destroy the fireball object if it collides with anything except the player or the grid
        if (!otherObject.gameObject.CompareTag("Player") && !otherObject.gameObject.CompareTag("Grid"))
        {
            Destroy(this.gameObject);
        }
    }

}
