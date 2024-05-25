using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{

    [SerializeField]
    private NavMeshAgent m_Agent;
    
    public GameObject m_player;

    [SerializeField]
    private EnemyFOV m_fov;

    [Header("Searching Phase Data")]
    private Vector3 playerLastSeenLocation;
    
    private Vector3 playerWhereabouts;

    private float playerLastSeenTime;
    private Time searchTime;

    [SerializeField]
    private int mediumSoundThreshold = 5;

    [SerializeField]
    private int loudSoundThreshold = 7;

    [SerializeField]
    private float walkSpeed = 0.5f;

    [SerializeField]
    private float runSpeed = 3.5f;

    [SerializeField]
    private GameObject m_enemy;

    [SerializeField]
    private float m_waitTime = 3f;

    private float cantReachTimer = 0f;

    private float timer = 0f;

    private float attackCooldown = 1.0f;
    private float timeLastAttacked = 0.0f;

    public Animator anim;

    private bool isPatrolling = true;

    private bool isAreadyPatroling = false;

    private bool isWaiting = false;

    // to check if the player is within range after attack animation
    private bool isWithinAttackRange = false;

    private Vector3 startPosition;

    public EnemySoundManager enemySoundManager;

    public DeathManager deathManager;

    [SerializeField]
    private List<Waypoint> m_waypoint = new();

    private int counter = 0;

    private void Awake()
    {
        startPosition = m_enemy.transform.position;
    }

    void Update()
    {
        MovementHandler();
    }

    private void MovementHandler()
    {
        if (Vector3.Distance(m_player.transform.position, m_fov.transform.position) <= m_Agent.stoppingDistance + 1)            
        {
            if (Time.time > timeLastAttacked + attackCooldown)
            {
                timeLastAttacked = Time.time;
                AttackThePlayer();
            }
            else
            {
                isWithinAttackRange = false;
            }
            return;
        }

        if (m_fov.seesPlayer)
        {
            ChaseThePlayer(m_fov.player.transform.position);
        }

        Vector3 positionToChase = Vector3.zero;
        float intensityToChase = 0.0f;

        PlayerInformation playerState = m_player.GetComponent<PlayerInformation>();
        if (IsPlayerInAudableRange() && IsThrownObjectInAudableRange()) {
            bool isPlayerLouderThanObject = playerState.GetSoundIntensity() >= playerState.GetThrownObjectIntensity();
            positionToChase = isPlayerLouderThanObject ? m_player.transform.position : playerState.GetThrownObjectPosition();
            intensityToChase = isPlayerLouderThanObject ? m_fov.GetCurrentSoundIntensity() : playerState.GetThrownObjectIntensity();
        } else if (IsPlayerInAudableRange()) {
            positionToChase = m_player.transform.position;
            intensityToChase = m_fov.GetCurrentSoundIntensity();
        } else if (IsThrownObjectInAudableRange()) {
            positionToChase = playerState.GetThrownObjectPosition();
            intensityToChase = playerState.GetThrownObjectIntensity();
        }
        

        if (IsPlayerInAudableRange() || IsThrownObjectInAudableRange()) {
            if ((intensityToChase >= mediumSoundThreshold) && (intensityToChase < loudSoundThreshold))
            {
                LookAroundForPlayer(positionToChase);
            }
            else if (intensityToChase >= loudSoundThreshold)
            {
                ChaseThePlayer(positionToChase);
            }
        } 
        

        if(isWaiting)
        {
            if (timer < m_waitTime)
            {
                timer += Time.deltaTime;
                return;
            }
            else
            {
                timer = 0f;
                SetIsWaiting(false);
            }
        }

        if (isPatrolling && !isAreadyPatroling)
        {
            Patrol();
        }

        // Be idle, you are no longer moving.
        if (isIdle())
        {
            BeIdle();
        }
    }

    private void BeIdle()
    {
        //Debug.Log("I am idle");
        enemySoundManager.PlayIdelSound();
        m_Agent.Stop();
        m_Agent.ResetPath();
        PlayIdleAnimation();
        //wait for 2 sec before patrolling
        SetIsWaiting(true);
        SetIsPatrolling(true);
        SetIsAlreadyPatrolling(false);
        
    }   

    private void Patrol()
    {
        // Debug.Log("I am patrolling");
        SetIsAlreadyPatrolling(true);
        enemySoundManager.PlayWalkingSound();
        PlayWalkingAnimation();
        SetAgentSpeed(walkSpeed/2);

        if (m_waypoint.Count < 1)
        {
            // Debug.Log("No waypoints");

            Vector3 randomPosition = GetRandomPositionAroundPosition(startPosition, 10f);

            bool val = NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 2f, NavMesh.AllAreas);

            Vector3 pos;
            // Debug.Log("NavMeshHit: " + hit);
            // Debug.Log("Looking Around" + hit.position);
            if (val)
            {
                pos = hit.position;
                m_Agent.SetDestination(pos);
            }
        } else {
            if (counter >= m_waypoint.Count) counter = 0;  
            // Debug.Log("Going to Waypoint: " + counter);
            m_Agent.SetDestination(m_waypoint[counter].Position);
            counter++;
        }

    }

    private Vector3 GetRandomPositionAroundPosition(Vector3 position, float radius)
    {
        Vector2 unitCirclePos = Random.insideUnitCircle * radius;
        //cast thi vector to a 3d vector with y = z and y = 0
        Vector3 randomPosition = position + (new Vector3(unitCirclePos.x, 0, unitCirclePos.y));
        return randomPosition;
    }


    private void LookAroundForPlayer(Vector3 pos)
    {
        //Debug.Log("We heard something");
        playerLastSeenLocation = pos;
        enemySoundManager.PlayAlertSound();
        SetAgentSpeed(walkSpeed/2);
        PlayWalkingAnimation();
        timer = 0;
        SetIsWaiting(false);
        SetIsPatrolling(false);
        SearchAroundPlayerLocation();
    }

    private void ChaseThePlayer(Vector3 pos)
    {
        //Debug.Log("There you are!!");
        enemySoundManager.PlayRunningSound();
        SetAgentSpeed(runSpeed);
        PlayRunningAnimation();
        timer = 0;
        SetIsWaiting(false);
        FollowPlayer(pos);
        SetIsPatrolling(false);
    }

    private void AttackThePlayer()
    {
        //Debug.Log("You are dead!!");
        enemySoundManager.PlayAttackSound();
        PlayAttackAnimation();
        // set a delayed check after the animation plays to see if the player is still in range
        if(!isWithinAttackRange)
        {
            StartCoroutine(DelayedCheck(0.9f));
        }
        // set player back to spawn, iterate through all zombies and use their starting position (since we already save it) and transform.position them to it
        // FUTURE TODO: reset the sound traps on death
    }
 
    private void FollowPlayer(Vector3 pos) {
        playerLastSeenLocation = pos;
        SetAgentTarget(playerLastSeenLocation);
    }

    private bool IsPlayerInAudableRange()
    {
        float distance = Vector3.Distance(m_player.transform.position, m_fov.transform.position);
        return distance <= m_fov.GetSoundRadius();
    }
    
    private bool IsThrownObjectInAudableRange() {
        float distance = Vector3.Distance(m_player.GetComponent<PlayerInformation>().GetThrownObjectPosition(), m_fov.transform.position);
        return distance <= m_fov.GetSoundRadius();
    }

    private void SearchAroundPlayerLocation()
    {
        Vector3 randomPositionAroundPlayer = GetRandomPositionAroundPosition(playerLastSeenLocation, 5f);

        bool val = NavMesh.SamplePosition(randomPositionAroundPlayer, out NavMeshHit hit, 2f, NavMesh.AllAreas);

        //Debug.Log("NavMeshHit: " + hit);
        //Debug.Log("Looking Around" + location);
        if (val)
        {
            if (m_fov.seesPlayer)
                playerWhereabouts = m_player.transform.position;
            else
                playerWhereabouts = hit.position;

            m_Agent.SetDestination(playerWhereabouts);
        }
    }

    private void SetAgentTarget(Vector3 targetPosition) {
        m_Agent.SetDestination(targetPosition);
    }

    private void PlayWalkingAnimation()
    {
        anim.SetBool("isRunning", false);
        anim.SetBool("IsWalking", true);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isIdle", false);
        anim.SetBool("isAlert", false);
    }

    private void PlayRunningAnimation()
    {
        anim.SetBool("isRunning", true);
        anim.SetBool("IsWalking", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isIdle", false);
        anim.SetBool("isAlert", false);
    }

    private void PlayIdleAnimation()
    {
        anim.SetBool("isRunning", false);
        anim.SetBool("IsWalking", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isIdle", true);
        anim.SetBool("isAlert", false);
    }

    private void PlayAlertAnimation()
    {
        anim.SetBool("isRunning", false);
        anim.SetBool("IsWalking", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isIdle", false);
        anim.SetBool("isAlert", true);
    }

    private void PlayAttackAnimation()
    {
        anim.SetBool("isRunning", false);
        anim.SetBool("IsWalking", false);
        anim.SetBool("isAttacking", true);
        anim.SetBool("isIdle", false);
        anim.SetBool("isAlert", false);
    }

    public bool GetIsPatrolling()
    {
        return isPatrolling;
    }

    public void SetIsPatrolling(bool val)
    {
        isPatrolling = val;
    }

    public void SetIsAlreadyPatrolling(bool val)
    {
        isAreadyPatroling = val;
    }

    public void SetAgentSpeed(float speed)
    {
        m_Agent.speed = speed;
    }

    public void SetIsWaiting(bool val)
    {
        isWaiting = val;
    }

    private bool isIdle()
    {
        bool cantReachDest = false;
        if (cantReachTimer < m_waitTime) {
            cantReachTimer += Time.deltaTime;
        } else {
            cantReachTimer = 0f;
            cantReachDest = m_Agent.pathStatus == NavMeshPathStatus.PathPartial || m_Agent.pathStatus == NavMeshPathStatus.PathInvalid;
        }
        bool zombieReachedDest = (m_Agent.remainingDistance <= m_Agent.stoppingDistance) && (!m_Agent.hasPath || m_Agent.velocity.sqrMagnitude == 0f);
        return !m_Agent.pathPending && (cantReachDest || zombieReachedDest);
    }

    // checks if the player is within attack range after a short delay
    IEnumerator DelayedCheck(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if(attackCheck())
        {
            // deathManager.ShowDeathMessage();
            // deathManager.ShowDamagedScreen();
            deathManager.handleAttackResult();
        }
        
    }

    // abstracted distance check
    bool attackCheck()
    {
        return Vector3.Distance(m_player.transform.position, m_fov.transform.position) <= m_Agent.stoppingDistance + 1;
    }

    private void resetScene()
    {
        string currentSceneName = SceneManager. GetActiveScene(). name;
        SceneManager.LoadScene(currentSceneName);
    }

}
