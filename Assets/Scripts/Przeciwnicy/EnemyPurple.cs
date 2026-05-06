using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyPurple : Enemy
{
    public float attackRange = 5.0f;

    Vector3 direction;
    Quaternion targetRotation;
    [SerializeField] EnemyShooting muzzleStraight;
    [SerializeField] EnemyShooting muzzleRight;
    [SerializeField] EnemyShooting muzzleLeft;

    // tak samo jak w enemy hammer
    private IEnumerator patrolCoroutine;
    private IEnumerator walkCoroutine;

    void Start()
    {
        // muzzle = GetComponentInChildren<EnemyShooting>();
        // if (muzzle == null)
        //     Debug.Log("muzzle not found - enemy!!!");

        patrolCoroutine = PatrolCycle();
        walkCoroutine = WalkToRandomPoint();
        StartCoroutine(patrolCoroutine);
    }

    public override void TakeAggro()
    {
        if (isAggroed) return;
        base.TakeAggro();
        StopCoroutine(patrolCoroutine);
        StopCoroutine(walkCoroutine);
        StartCoroutine(EnemyCycle());
    }

    IEnumerator EnemyCycle()
    {
        animator.SetBool("isAggroed", true);

        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("guarded_idle")
        );

        while (true)
        {
            yield return StartCoroutine(Fire());
            yield return StartCoroutine(Reposition());
        }
    }

    IEnumerator PatrolCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            walkCoroutine = WalkToRandomPoint();
            yield return StartCoroutine(walkCoroutine);
        }
    }

    protected void Shoot()
    {
        muzzleStraight.FireAShot();
        muzzleRight.FireAShot();
        muzzleLeft.FireAShot();
    }

    IEnumerator Fire()
    {
        agent.enabled = false;

        // rotate towards player (1 second)
        float progress = 0.0f;
        while (progress < 1.0f)
        {
            direction = player.position - transform.position;
            targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, progress);
            progress += Time.deltaTime;

            yield return null;
        }
        
        // prepare to shoot
        animator.SetTrigger("fire");

        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("fire_setup")
        );

        yield return new WaitWhile(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("fire_setup")
        );

        // shoot
        Shoot();

        // animate recoil
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("fire_recoil")
        );

        yield return new WaitWhile(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("fire_recoil")
        );


        agent.enabled = true;

        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator Reposition()
    {
        animator.SetBool("isRunning", true);
        agent.speed = runningSpeed;

        // Find a point to walk to
        SearchWalkPoint();
        agent.SetDestination(walkPoint);

        // walk there
        // yield return StartCoroutine(ReachAgentDestination());
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


        animator.SetBool("isRunning", false);
    }

    IEnumerator WalkToRandomPoint()
    {
        animator.SetBool("isWalking", true);
        agent.speed = walkingSpeed;

        // Find a point to walk to
        SearchWalkPoint();
        agent.SetDestination(walkPoint);

        // walk there
        // yield return StartCoroutine(ReachAgentDestination());
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

        animator.SetBool("isWalking", false);
    }

}

