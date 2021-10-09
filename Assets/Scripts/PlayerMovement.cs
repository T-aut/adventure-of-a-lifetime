using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;

    private Vector2 _movement = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _movement.y = Input.GetAxisRaw("Vertical");
        _movement.x = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate() 
    {
        rb.MovePosition(rb.position + _movement * speed * Time.fixedDeltaTime);
    }
}
