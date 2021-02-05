/*****************************************************************************
// File Name :         EnemyBehaviour.cs
// Author :            Kyle Grenier
// Creation Date :     #CREATIONDATE#
//
// Brief Description : ADD BRIEF DESCRIPTION OF THE FILE HERE
*****************************************************************************/
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(CharacterMovement))]
public abstract class EnemyBehaviour : MonoBehaviour
{
    private CharacterMovement characterMovement;

    #region Pathfinding
    [Header("Pathfinding")]
    //How close the enemy needs to be to a waypoint before it moves on to the next one.
    [SerializeField] private float nextWaypointDistance = 3f;

    protected GameObject player;
    private Seeker seeker;
    private Path path;
    protected bool playerInRange = false;

    private int currentWaypoint = 0;
    private float pathUpdateInterval = 0.5f;

    Vector2 dir = Vector2.zero;




    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(transform.position, player.transform.position, OnPathComplete);
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    #endregion

    /// <summary>
    /// The unique action this behaviour performs.
    /// </summary>
    protected abstract void PerformAction();

    protected virtual void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        seeker = GetComponent<Seeker>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected virtual void Start()
    {
        //Perform action on start.
        PerformAction();
    }

    protected virtual void Update()
    {
        //Calculating direction towards next waypoint in path to player IF they are in range of the enemy.

        //TODO: Use a field of view system instead of just having the player in range.
        if (Vector2.Distance(transform.position, player.transform.position) < 10f)
        {
            //Once the player gets in range, start repeatedly updating the enemy's path.
            if (!playerInRange)
            {
                playerInRange = true;
                InvokeRepeating("UpdatePath", 0f, pathUpdateInterval);
            }

            if (path == null)
                return;

            //If we reached the end of the path.
            if (currentWaypoint >= path.vectorPath.Count)
                return;

            //Calculating a direction from enemy's current position to the position of the current waypoint.
            dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;

            //Get the distance to the next waypoint in the path, and update our current waypoint if we're within range.
            float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDistance)
                currentWaypoint++;

        }
        //Once the player leaves the enemy's range, reset it and resume performing its action.
        else if (playerInRange)
        {
            playerInRange = false;
            dir = Vector3.zero;
            CancelInvoke("UpdatePath");
            PerformAction();
        }
    }

    protected virtual void FixedUpdate()
    {
        //Move the enemy using FixedUpdate for physics' sake.
        characterMovement.Move(dir);
    }
}
