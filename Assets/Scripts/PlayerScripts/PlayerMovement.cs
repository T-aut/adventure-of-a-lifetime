using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    private bool _isControlEnabled = true;

    private Vector2 _movement = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        animator.SetFloat("FacingDirection", 2);
    }

    // Update is called once per frame
    void Update()
    {
        _movement.y = _isControlEnabled ? Input.GetAxisRaw("Vertical") : 0;
        _movement.x = _isControlEnabled ? Input.GetAxisRaw("Horizontal") : 0;

        animator.SetBool("IsMoving", _movement.sqrMagnitude > 0);
        animator.SetFloat("Vertical", _movement.y);
        animator.SetFloat("Horizontal", _movement.x);

        if (_movement.y > 0) animator.SetFloat("FacingDirection", 0);
        else if (_movement.y < 0) animator.SetFloat("FacingDirection", 2);

        if (_movement.x > 0) animator.SetFloat("FacingDirection", 1);
        else if (_movement.x < 0) animator.SetFloat("FacingDirection", 3);
    }

    void FixedUpdate() 
    {
        if (!_isControlEnabled) return;

        rb.MovePosition(rb.position + _movement * speed * Time.fixedDeltaTime);
    }

    public void SetControlEnabled(bool enabled) {
        _isControlEnabled = enabled;
    }
}
