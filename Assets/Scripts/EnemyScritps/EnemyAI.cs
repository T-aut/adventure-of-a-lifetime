using UnityEngine;
using Pathfinding;

public class EnemyAI : EnemyLogic
{


    public Transform graphics;

    public float speed = 1.2f;
    public float nextWaypointDistance = 0.8f;
    public float aggroRange;
    
    Path path;
    int currentWaypoint = 0;
    private float lastAttack;
    public float attackTime = 0.1f;
    Seeker seeker;

    private float previous_x, previous_y;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }

    private void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        // previous_x = 1;
        // previous_y = 1;
    }

    private void Start()
    {
        //lastAttack = Time.time;
        InvokeRepeating("CheckDist", 0, 1f);
        boxCollider = GetComponent<BoxCollider2D>();
    }

   public override void FixedUpdate()
    {   if (isDead) return;
        pushDirection = Vector2.Lerp(pushDirection, Vector2.zero, pushRecoverySpeed);
        rb.MovePosition(rb.position + pushDirection);
        if (myAnimator.GetBool("Attacking"))
            return;
        base.FixedUpdate();
        if (path == null)
            return;
       
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        Vector2 force = direction * speed * Time.deltaTime;

        rb.MovePosition(rb.position + force);

        var delta_x = direction.x + previous_x;
        var delta_y = direction.y + previous_y;

        // dirty fix
        // if the wolf did a complete 180 in either of the directions - DO NOT update the animation
        if (delta_x == 0 || delta_y == 0)
        {
            previous_x = direction.x;
            previous_y = direction.y;
        }
        else
        {
            UpdateAnimation(direction);
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        
    }

    public void UpdateAnimation(Vector2 direction)
    {
        myAnimator.SetFloat("MoveX", direction.x);
        myAnimator.SetFloat("MoveY", direction.y);
    }

    void CheckDist()
    {
        float dist = Vector2.Distance(rb.position, target.position);

        if (dist <= aggroRange)
        {
            
            InvokeRepeating("UpdatePath", 0, 0.5f);
        }
        else
        {
            CancelInvoke("UpdatePath");
            path = null;
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
