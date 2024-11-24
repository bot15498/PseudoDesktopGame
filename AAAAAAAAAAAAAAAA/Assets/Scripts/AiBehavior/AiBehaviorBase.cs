using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyAiType
{
    Idle, // Don't do anything, not even attack if a player is near
    Lazy, // Only attack if a player is in range / can be seen. Don't try to move towards the player
    Normal, // If the player is in range, move towards the player and start attacking
    Patrol, // Move a series of waypoints. Chase player if you see them
    Lure, // When seeing the player, provoke the player with an attack, then hide to get the player closer. When the player is closer, start attacking again. 
}

public enum EnemyAiStunState
{
    Normal,
    Stun,
    RunAway,
    Stagger
}

public enum EnemyAiChaseState
{
    Idle,
    AttackingAndChasing,
    AttackingAndIdle,
    ChasingForLineOfSight,
    AttackingButStayingStillForever
}

public abstract class AiBehaviorBase : MonoBehaviour
{
    public EnemyAiType enemyAiType = EnemyAiType.Idle;

    public GameObject player;
    public float speed = 5f;
    public float runawaySpeed = 5f;
    public SkinnedMeshRenderer skinRenderer;
    public Material staggeredMaterial;
    [Header("AI Agent Avoidance settings")]
    public NavMeshAgent agent;
    public float avoidCircleRadius = 5f;
    public float lineOfSightWidth = 2f;
    public float avoidSpeedMultiplier = 2f;
    public float avoidSpeedSmoother = 5f;
    public List<Transform> agentsInAvoidanceCircle;
    public List<Transform> agentsInLineOfSight;
    [Header("Attacking Player settings")]
    [Tooltip("The max distance that the enemy will use to look at a player.")]
    public float maxViewDistance = 5f;
    [Tooltip("How close the enemy will get to the player. ")]
    public float minApproachDistance = 0f;
    [Tooltip("How far away the enemy must be before they start chasing. ")]
    public float maxChaseDistance = 5f;
    [Tooltip("How far an enemy has to before they 'forget' the player is there. ")]
    public float forgetDuration = 10f;
    public EnemyAiStunState stunState;
    public bool canSeeBullet = false;
    [Header("General attack stuff.")]
    public float attackInterval = 1f;
    public float randomInterval = 0.1f;
    [Header("anime")]
    public Animator anime;

    // For patroling
    public Transform[] waypoints;
    public float patrolWaitTimeSec = 3f;
    private int currWaypointIndex = 0;
    private bool isIncreasingIndex = false;

    private Rigidbody rb;
    [SerializeField]
    private LayerMask playerLayerMask;
    private AiAvoidanceCircle avoidCircle;
    private AiLineOfSight lineOfSight;
    private AiViewTrigger maxViewCircle;
    [SerializeField]
    public EnemyAiChaseState chaseState = EnemyAiChaseState.Idle;
    [SerializeField]
    private float timeSinceLastSawPlayer = 0f;
    [SerializeField]
    private float timeSinceLastAttack = 0f;
    private bool addedStaggerMaterial = false;
    [SerializeField]
    private EnemyAiStunState prevStunState;


    public abstract void Attack();

    public abstract bool IsObjectInAttackRange();

    protected void Start()
    {
        // Get rigid body
        rb = GetComponent<Rigidbody>();

        // Get the nav mesh agent
        agent = GetComponent<NavMeshAgent>();
        if(agent != null)
        {
            agent.enabled = true;
            agent.speed = speed;
        }

        anime = GetComponent<Animator>();
        if(anime != null)
        {
            anime.SetBool("isChasingPlayer", false);
            anime.SetBool("isAttackingPlayer", false);
            anime.SetBool("isStaggered", false);
        }

        // Set up lists
        agentsInAvoidanceCircle = new List<Transform>();
        agentsInLineOfSight = new List<Transform>();

        // Create a child game object with the alert field of view
        GameObject maxViewObject = new GameObject("MaxViewCircle");
        maxViewObject.transform.parent = transform;
        maxViewObject.transform.localPosition = Vector3.zero;
        maxViewCircle = maxViewObject.AddComponent<AiViewTrigger>();
        maxViewCircle.CapsuleRadius = maxViewDistance;

        // Create a child game object with the avoidance circle, and get the reference to it
        GameObject avoidObject = new GameObject("AvoidCircle");
        avoidObject.transform.parent = transform;
        avoidObject.transform.localPosition = Vector3.zero;
        avoidCircle = avoidObject.AddComponent<AiAvoidanceCircle>();
        avoidCircle.CapsuleRadius = avoidCircleRadius;

        // Create a child game object with the line of sight box
        GameObject sightObject = new GameObject("LineOfSight");
        sightObject.transform.parent = transform;
        sightObject.transform.localPosition = Vector3.zero;
        lineOfSight = sightObject.AddComponent<AiLineOfSight>();
        lineOfSight.LineOfSightWidth = lineOfSightWidth;
    }

    protected void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault();
        }

        if (stunState == EnemyAiStunState.Normal)
        {
            // Handle movement 
            switch (enemyAiType)
            {
                case EnemyAiType.Idle:
                    // Don't do anything. 
                    break;
                case EnemyAiType.Lazy:
                    chaseState = EnemyAiChaseState.AttackingButStayingStillForever;
                    if (CanSeePlayer() || canSeeBullet)
                    {
                        FacePlayer();
                    }
                    break;
                case EnemyAiType.Normal:
                    ApplyAvoidance();
                    ApplyFollow();
                    break;
                case EnemyAiType.Patrol:
                    if (chaseState != EnemyAiChaseState.Idle)
                    {
                        ApplyAvoidance();
                        ApplyFollow();
                    }
                    else if (waypoints.Length > 0)
                    {
                        Vector3 currWaypoint = waypoints[currWaypointIndex].position;
                        // normalize distance in 2d only
                        currWaypoint = new Vector3(currWaypoint.x, transform.position.y, currWaypoint.z);
                        if (Vector3.Distance(transform.position, currWaypoint) > 0.5f)
                        {
                            agent.SetDestination(currWaypoint);
                        }
                        else
                        {
                            // delay starting a new point to give it the illusion of thinking
                            if (!isIncreasingIndex)
                            {
                                StartCoroutine(DelayIndexIncrease(patrolWaitTimeSec));
                            }
                        }
                    }
                    break;
                case EnemyAiType.Lure:
                    break;
            }

            // Handle attack
            if (timeSinceLastAttack >= attackInterval + Random.Range(0, randomInterval) && chaseState != EnemyAiChaseState.Idle && IsObjectInAttackRange())
            {
                Attack();
                timeSinceLastAttack = 0;
            }
            else
            {
                timeSinceLastAttack += Time.deltaTime;
            }

            // Anime
            if (anime != null)
            {
                if (IsObjectInAttackRange() && chaseState != EnemyAiChaseState.Idle)
                {
                    anime.SetBool("isAttackingPlayer", true);
                }
                else
                {
                    anime.SetBool("isAttackingPlayer", false);
                }
            }
        }
        else if (stunState == EnemyAiStunState.Stagger)
        {
            if (!addedStaggerMaterial)
            {
                // Staggered. so set the material
                List<Material> currmaterials = skinRenderer.materials.ToList();
                currmaterials.Add(staggeredMaterial);
                skinRenderer.materials = currmaterials.ToArray();
                addedStaggerMaterial = true;
                if(anime != null)
                {
                    anime.SetBool("isStaggered", true);
                }
            }
        }
        else if (stunState == EnemyAiStunState.RunAway)
        {
            RunAwayFromPlayer();
        }
        else
        {
            StopChasePlayer();
        }
    }

    protected void FixedUpdate()
    {

    }

    public bool CanSeePlayer(float maxView = float.PositiveInfinity)
    {
        bool cansee = false;
        if (player == null)
        {
            return false;
        }
        // draw raycast to see if you hit the player
        Vector3 playerDirection = player.transform.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDirection, out hit, Mathf.Infinity, playerLayerMask))
        {
            if (hit.collider.gameObject.tag == "Player" && (hit.point - transform.position).magnitude < maxView)
            {
                cansee = true;
            }
        }
        return cansee;
    }

    public float DistanceToPlayer()
    {
        return Mathf.Abs(Vector3.Distance(transform.position, player.transform.position));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            canSeeBullet = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet") && !CanSeePlayer())
        {
            canSeeBullet = false;
        }
    }

    public virtual void FacePlayer()
    {
        // dumb just look at player
        Vector3 direction = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
    }

    public virtual void FaceAwayFromPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(-direction);
        transform.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
    }

    public void ChasePlayer()
    {
        if(agent != null)
        {
            rb.isKinematic = true;
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
            if(anime != null)
            {
                anime.SetBool("isChasingPlayer", true);
            }
        }
    }

    public void RunAwayFromPlayer()
    {
        if(agent != null)
        {
            rb.isKinematic = true;
            agent.isStopped = false;
            Vector3 direction = player.transform.position - transform.position;
            direction = -direction;
            FaceAwayFromPlayer();
            agent.SetDestination(transform.position + direction.normalized * 1.5f);
            if (anime != null)
            {
                anime.SetBool("isChasingPlayer", true);
            }
        }
    }

    public void StopChasePlayer()
    {
        if(agent != null)
        {
            agent.isStopped = true;
            if(anime != null)
            {
                anime.SetBool("isChasingPlayer", false);
            }
        }
    }

    private IEnumerator DelayIndexIncrease(float seconds)
    {
        isIncreasingIndex = true;
        yield return new WaitForSeconds(seconds);
        currWaypointIndex = (currWaypointIndex + 1) % waypoints.Length;
        isIncreasingIndex = false;
    }

    public IEnumerator DelayStunStateChange(EnemyAiStunState newstate, float waitduration)
    {
        yield return new WaitForSeconds(waitduration);
        stunState = newstate;
        if (newstate == EnemyAiStunState.Normal)
        {
            // We are becoming unstunned / unstaggered, so reenable navmesh stuff.
            rb.isKinematic = true;
        }
        yield return null;
    }

    public void TempSetStunState(EnemyAiStunState newstate)
    {
        prevStunState = stunState;
        stunState = newstate;
    }

    public void TempRestoreStunState()
    {
        stunState = prevStunState;
    }

    public void ApplyAvoidance()
    {
        if(agent != null)
        {
            if (agentsInAvoidanceCircle.Count == 0 && agentsInLineOfSight.Count == 0)
            {
                return;
            }

            // Figure out vector to move in to avoid nearby enemies.
            // Do this by adding all the vectors together, then normalize it and flip it on the xz plane.
            Vector3 directionToNearbyEnemies = Vector3.zero;
            float maxDistance = 0f;
            foreach (Transform t in agentsInAvoidanceCircle)
            {
                directionToNearbyEnemies += t.position - transform.position;
                float currDistance = Vector3.Distance(t.position, transform.position);
                maxDistance = maxDistance < currDistance ? currDistance : maxDistance;
            }

            // Now add in line of sight
            //foreach(Transform t in agentsInLineOfSight)
            //{
            //    // vector of enemy to nearby enemy relative to face orientation
            //    directionToNearbyEnemies += (t.position - transform.position).normalized - (transform.rotation * Vector3.right).normalized;
            //    float currDistance = directionToNearbyEnemies.magnitude;
            //    maxDistance = maxDistance < currDistance ? currDistance : maxDistance;
            //}
            directionToNearbyEnemies = directionToNearbyEnemies.normalized;

            // Apply to velocity
            // Lerp it so it slows down as the enemies get further from each other. 
            agent.velocity = Vector3.Lerp(
                agent.desiredVelocity,
                -directionToNearbyEnemies * agent.speed * avoidSpeedMultiplier,
                Mathf.Clamp01((avoidCircleRadius - maxDistance) / avoidSpeedSmoother)
            );
        }
    }

    public void ApplyFollow()
    {
        if (CanSeePlayer())
        {
            FacePlayer();
        }

        switch (chaseState)
        {
            case EnemyAiChaseState.Idle:
                StopChasePlayer();
                if (CanSeePlayer(maxViewDistance) || canSeeBullet)
                {
                    chaseState = EnemyAiChaseState.AttackingAndChasing;
                }
                break;
            case EnemyAiChaseState.AttackingAndChasing:
                if (!CanSeePlayer() && !canSeeBullet)
                {
                    // Can't see the player at all, so go to chasing for line of sight
                    chaseState = EnemyAiChaseState.ChasingForLineOfSight;
                }
                else if (DistanceToPlayer() <= minApproachDistance)
                {
                    // Close enough, stop chasing and be still
                    chaseState = EnemyAiChaseState.AttackingAndIdle;
                }
                else
                {
                    // Too far, chase
                    ChasePlayer();
                }
                break;
            case EnemyAiChaseState.AttackingAndIdle:
                StopChasePlayer();
                if (!CanSeePlayer())
                {
                    // Can't see the player at all, so go to chasing for line of sight
                    chaseState = EnemyAiChaseState.ChasingForLineOfSight;
                }
                else if (DistanceToPlayer() >= maxChaseDistance)
                {
                    // Too far, chase
                    ChasePlayer();
                    chaseState = EnemyAiChaseState.AttackingAndChasing;
                }
                break;
            case EnemyAiChaseState.ChasingForLineOfSight:
                if (CanSeePlayer())
                {
                    chaseState = EnemyAiChaseState.AttackingAndChasing;
                    timeSinceLastSawPlayer = 0;
                }
                else
                {
                    ChasePlayer();
                    FacePlayer();
                    timeSinceLastSawPlayer += Time.deltaTime;
                }

                if (timeSinceLastSawPlayer > forgetDuration)
                {
                    // We have gone long enough without seeing the player, I guess they are gone?
                    chaseState = EnemyAiChaseState.Idle;
                    timeSinceLastSawPlayer = 0;
                }
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxViewDistance);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, avoidCircleRadius);
    }
}
