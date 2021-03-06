using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public bool _isControlEnabled = true;
    private Vector2 _movement = new Vector2();

    private bool _isAltarUnlocked = false;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        animator.SetFloat("FacingDirection", 2);
        playerController = gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        _movement.y = _isControlEnabled ? Input.GetAxisRaw("Vertical") : 0;
        _movement.x = _isControlEnabled ? Input.GetAxisRaw("Horizontal") : 0;

        animator.SetBool("IsMoving", _movement.sqrMagnitude > 0);
        animator.SetFloat("Vertical", _movement.y);
        animator.SetFloat("Horizontal", _movement.x);

        if (animator.GetBool("IsMoving"))
        {
            SoundManagerScript.PlaySound("playerMove");
        }

        if (_movement.y > 0) animator.SetFloat("FacingDirection", 0);
        else if (_movement.y < 0) animator.SetFloat("FacingDirection", 2);

        if (_movement.x > 0) animator.SetFloat("FacingDirection", 1);
        else if (_movement.x < 0) animator.SetFloat("FacingDirection", 3);
    }

    public void ResetVelocity()
    {
        rb.velocity = Vector3.zero;
    }

    public void Push(Vector2 pushVector)
    {
        UpdatePosition(pushVector);
    }

    private void UpdatePosition(Vector2 positionVector)
    {
        positionVector.y = _isControlEnabled ? Input.GetAxisRaw("Vertical") : 0;
        positionVector.x = _isControlEnabled ? Input.GetAxisRaw("Horizontal") : 0;

        animator.SetBool("IsMoving", positionVector.sqrMagnitude > 0);
        animator.SetFloat("Vertical", positionVector.y);
        animator.SetFloat("Horizontal", positionVector.x);

        if (positionVector.y > 0) animator.SetFloat("FacingDirection", 0);
        else if (positionVector.y < 0) animator.SetFloat("FacingDirection", 2);

        if (positionVector.x > 0) animator.SetFloat("FacingDirection", 1);
        else if (positionVector.x < 0) animator.SetFloat("FacingDirection", 3);
    }

    void FixedUpdate()
    {
        if (!_isControlEnabled) return;
        playerController.pushDirection = Vector2.Lerp(playerController.pushDirection, Vector2.zero, playerController.pushRecoverySpeed);
        rb.MovePosition(rb.position + _movement * speed * Time.fixedDeltaTime + playerController.pushDirection);
    }

    public void SetControlEnabled(bool enabled)
    {
        _isControlEnabled = enabled;
    }

    // Returns the velocity vector of the current direction the player is facing/moving towards
    public Vector2 GetDirectionVelocity()
    {
        Vector2 directionVelocity = new Vector2();

        if (animator.GetBool("IsMoving"))
        {
            // The player is moving and its velocity vector is not zero
            directionVelocity = new Vector2(animator.GetFloat("Horizontal"), animator.GetFloat("Vertical"));
        }
        else
        {
            // The player is not moving and its velocity vector is zero, so we need to return a velocity vector
            //  based on the direction the player is facing
            float currentDirection = animator.GetFloat("FacingDirection");

            // Character is facing upwards
            if (currentDirection == 0)
            {
                directionVelocity = new Vector2(0, 1);
            }
            // Character is facing to the right
            else if (currentDirection == 1)
            {
                directionVelocity = new Vector2(1, 0);
            }
            // Character is facing downwards
            else if (currentDirection == 2)
            {
                directionVelocity = new Vector2(0, -1);
            }
            // Character is facing to the left
            else if (currentDirection == 3)
            {
                directionVelocity = new Vector2(-1, 0);
            }
        }

        return directionVelocity;
    }

    public bool IsControlEnabled() => _isControlEnabled;

    public void SetAltarUnlocked(bool value) => _isAltarUnlocked = value;

    public bool IsAltarUnlocked => _isAltarUnlocked;
}
