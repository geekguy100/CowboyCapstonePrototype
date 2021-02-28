using UnityEngine;
using Pathfinding;
using System;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(CharacterMovement))]
public abstract class EnemyAI : MonoBehaviour
{
    private CharacterMovement characterMovement;
    private EnemyBehaviour enemyBehaviour;

    private Transform target;

    // How close the enemy needs to be to a waypoint before it moves on to the next one.
    [SerializeField] private float nextWaypointDistance = 3f;

    private Path path;
    private int currentWaypoint = 0;

    private Seeker seeker;

    // The distance the target must be from the enemy for the enemy to detect them.
    [SerializeField] private float playerRange;

    // The player.
    private Transform player;

    // Invoked when the enemy is at their destination.
    protected event Action OnPathfindingComplete;

    private bool isMoving = false;
    protected bool IsMoving { get { return isMoving; } }


    protected virtual void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        seeker = GetComponent<Seeker>();
    }

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    /// <summary>
    /// Starts pathfinding towards a target.
    /// </summary>
    /// <param name="target">The target to pathfind towards</param>
    protected void StartPathfinding(Transform target)
    {
        if (!isMoving)
            isMoving = true;

        this.target = target;
        CancelInvoke("UpdatePath");
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    /// <summary>
    /// Start calculating a new path if the enemy is done calculating the current one.
    /// </summary>
    void UpdatePath()
    {
        if (seeker.IsDone() && target != null)
        {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }
    }

    /// <summary>
    /// Callback function for when the Seeker script generates a path.
    /// </summary>
    /// <param name="p">The completed path.</param>
    private void OnPathComplete(Path p)
    {
        //If we got no errors in the generation, set this enemy's path to 
        //what was generated, and reset its waypoint.
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    /// <summary>
    /// Updates the enemy's target.
    /// </summary>
    /// <param name="target">The enemy's new target.</param>
    protected void UpdateTarget(Transform target)
    {
        this.target = target;
    }

    Vector2 dir;
    private bool playerInRange = false;
    protected virtual void Update()
    {
        #region --- Player In Range Actions ---
        // Perform unique action if player is in range (e.g. start shooting).
        if (player != null && Vector2.Distance(transform.position, player.position) < playerRange)
        {
            if (!playerInRange)
                playerInRange = true;
            PlayerRangeAction(player);
        }
        else if (playerInRange)
        {
            playerInRange = false;
            PlayerLeftRangeAction(player);
        }
        #endregion

        if (path == null || target == null)
            return;

        //If we reached the end of the path.
        if (currentWaypoint >= path.vectorPath.Count)
        {
            print(gameObject.name + ": Finished pathfinding.");
            CancelInvoke("UpdatePath");

            path = null;
            target = null;
            dir = Vector2.zero;     // Zero the enemy's direction of movement so it won't continue moving in its previous direction.
            isMoving = false;
            OnPathfindingComplete?.Invoke();
            return;
        }

        //Calculating a direction from enemy's current position to the position of the current waypoint.
        dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;

        //Get the distance to the next waypoint in the path, and update our current waypoint if we're within range.
        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
            currentWaypoint++;

    }

    protected abstract void PlayerRangeAction(Transform player);
    protected virtual void PlayerLeftRangeAction(Transform player) { }

    /// <summary>
    /// Returns true if the enemy has a path.
    /// </summary>
    /// <returns>True if the enemy has a path.</returns>
    protected bool HasPath()
    {
        return (path != null);
    }

    protected virtual void FixedUpdate()
    {
        characterMovement.Move(dir);
    }
}
