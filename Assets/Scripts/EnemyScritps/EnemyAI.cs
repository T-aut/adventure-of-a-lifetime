using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    Transform target;
    public Transform graphics;

    public float speed = 1.2f;
    public float nextWaypointDistance = 0.8f;
    public float aggroRange;
    public Animator myAnimator;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath;
    private int count = 0;
    Seeker seeker;
    Rigidbody2D rb;
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
        InvokeRepeating("CheckDist", 0, 1f);
    }

    void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }


        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        Vector2 force = direction * speed * Time.deltaTime;

        rb.MovePosition(rb.position + force);

        var delta_x = direction.x + previous_x;
        var delta_y = direction.y + previous_y;

        // dirty fix
        // if the wolf did a complete 180 in either of the directions - DO NOT update the animation
        //if ((delta_x < 0.01f && delta_x > -0.01f) || (delta_y < 0.01f && delta_y > -0.01f))
        if (delta_x == 0 || delta_y == 0)
        {
            previous_x = direction.x;
            previous_y = direction.y;
            Debug.Log("turned around");
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
