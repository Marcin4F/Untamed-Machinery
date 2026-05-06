using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
//using UnityEditor.XR;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    protected NavMeshAgent agent;
    protected Animator animator;

    // Patrol
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Detection
    public float sightRange = 10.0f;
    protected bool playerInSightRange;

    [SerializeField] protected float walkingSpeed = 1.8f;
    [SerializeField] protected float runningSpeed = 6f;


    [SerializeField] int health = 200;
    public bool isAlive = true;

    protected bool isAggroed = false;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject playerObj = GameObject.Find("character");
            if (playerObj != null)
                player = playerObj.transform;
            else
                Debug.LogError("PlayerObj not found in scene!");
        }
    }

    private void Update()
    {
        if (!isAlive) return;


        if (!isAggroed)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            if (playerInSightRange)
            {
                TakeAggro();
                return;
            }
        }
    }

    public virtual void TakeAggro()
    {
        isAggroed = true;
    }

    // coroutine that waits till agent reaches destination (to be used in coroutines)
    protected IEnumerator ReachAgentDestination()
    {
        while (true)
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        break;
                    }
                }
            }    
            yield return null;
        }

    }

    private void Patrol()
    {
        agent.speed = walkingSpeed;
        agent.enabled = true;
        if (!walkPointSet)
            SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        if (Vector3.Distance(transform.position, walkPoint) < 1f)
            walkPointSet = false;
    }


    protected void SearchWalkPoint()
    {
        while (true)
        {
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);
            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            if (!Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
                break;
        }
    }


    protected void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    public void TakeDamage(int damage)
    {
        TakeAggro();
        health -= damage;
        if (health <= 0)
        {

            Die();
            // DO DODANIA: animacja smierci + moze jakis dzwiek
            //Debug.Log("Smierc");
            //isAlive = false;
            //maleDamage = 0;
            //if (Random.Range(0, 100) < Player.instance.lifeStealChance)
            //    Player.instance.TakeDamage(-Player.instance.lifeSteal);
        }
    }

    private void Die()
    {
        isAlive = false;

        agent.enabled = false;
        // GetComponent<Renderer>().material.color = Color.gray;

        if (Random.Range(0, 100) < Player.instance.lifeStealChance)
            Player.instance.TakeDamage(-Player.instance.lifeSteal);

        Destroy(gameObject);
    }

}
