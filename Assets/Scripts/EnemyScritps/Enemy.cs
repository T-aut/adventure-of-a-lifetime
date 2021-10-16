using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   public enum EnemyStates
    {
        idle,
        walk
            //attack, stagger little bit later
    }
    public EnemyStates currentState;
   public Animator myAnimator;
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;
    public float moveSpeed;
    public Rigidbody2D myRigidBody;
    private  Vector3 directionVector;
    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyStates.idle;
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckDistance();
    }

    public virtual void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        { if (currentState == EnemyStates.idle || currentState == EnemyStates.walk)
            {
                UpdateAnimation();
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                myRigidBody.MovePosition(temp);
                ChangeState(EnemyStates.walk);
            }
        }
        else
        {
            ChangeState(EnemyStates.idle);
        }
        
    }
    public void ChangeState(EnemyStates newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }
    protected void UpdateAnimation()
    {
        myAnimator.SetFloat("MoveX", directionVector.x);
        myAnimator.SetFloat("MoveY", directionVector.y);
    }
    protected void changeAnim(Vector3 direction)
    {
        direction = direction.normalized;
        myAnimator.SetFloat("MoveX", direction.x);
        myAnimator.SetFloat("MoveY", direction.y);
    }

}
