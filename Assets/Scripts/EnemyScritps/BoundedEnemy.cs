using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundedEnemy : Enemy
{
    public Collider2D boundary;

    public override void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius && boundary.bounds.Contains(target.transform.position) )
        {
            if (currentState == EnemyStates.idle || currentState == EnemyStates.walk)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
                myRigidBody.MovePosition(temp);
                ChangeState(EnemyStates.walk);
            }
        }

        else
        {
            currentState = EnemyStates.idle;
        }

    }
}
