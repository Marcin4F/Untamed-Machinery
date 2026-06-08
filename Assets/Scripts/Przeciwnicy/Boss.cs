using System.Collections;
using UnityEngine;

public class Boss : Enemy
{
    public float attackRange = 5.0f;
    [SerializeField] float speedUpTime = 2.0f;
    AoeAttack aoeAttack;
    Blast blast;

    // zapisuje ja w zmiennej zeby moc ja potem zatrzymac z zewnatrz
    private IEnumerator patrolCoroutine;
    private IEnumerator walkCoroutine;

    private void OnDestroy()
    {
        int rewardAmount;
        switch (GameManagement.instance.rewardIndex)
        {
            case 0:
                rewardAmount = Random.Range(-Player.instance.minHealing, -Player.instance.maxHealing);
                Player.instance.TakeDamage(rewardAmount);
                break;
            case 1:
                rewardAmount = Random.Range(Player.instance.minReward, Player.instance.maxReward) * 10;
                GameManagement.instance.currency1 += rewardAmount;
                InGameUI.instance.SetCurr1();
                break;
            case 2:
                rewardAmount = Random.Range(Player.instance.minReward, Player.instance.maxReward) * 10;
                GameManagement.instance.currency2 += rewardAmount;
                InGameUI.instance.SetCurr2();
                break;
            case 3:
                rewardAmount = Random.Range(Player.instance.minReward, Player.instance.maxReward) * 10;
                GameManagement.instance.currency3 += rewardAmount;
                InGameUI.instance.SetCurr3();
                break;
            default:
                Debug.LogError("Reward number exceeded. Around line 57 in 'RewardSystemR'");
                break;
        }

        InGameUI.instance.GameWon();
    }

    void Start()
    {
        aoeAttack = GetComponentInChildren<AoeAttack>();
        blast = GetComponentInChildren<Blast>();

        patrolCoroutine = PatrolCycle();
        walkCoroutine = WalkToRandomPoint();
        StartCoroutine(patrolCoroutine);
    }

    private bool CanAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        // Debug.Log($"Distance: {distanceToPlayer}, AttackRange: {attackRange}");

        return distanceToPlayer <= attackRange;
    }

    public override void TakeAggro()
    {
        if (isAggroed) return;
        base.TakeAggro();
        StopCoroutine(patrolCoroutine);
        StopCoroutine(walkCoroutine);
        StartCoroutine(EnemyCycle());
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

    IEnumerator EnemyCycle()
    {
        // main execution loop (after taking aggro)
        while (true)
        {
            yield return StartCoroutine(Run());
            yield return StartCoroutine(Attack());
        }
    }

    IEnumerator Run()
    {
        if (CanAttack()) yield break;

        float time = 0.0f;
        animator.SetBool("isRunning", true);

        // run towards the player until you're close enough
        while (!CanAttack())
        {
            animator.SetFloat("runningSpeed", time);
            agent.speed = walkingSpeed + time * (runningSpeed - walkingSpeed);

            time += Time.deltaTime / speedUpTime;

            if (time > 1.0f)
            {
                animator.SetFloat("runningSpeed", 1.0f);
                agent.speed = runningSpeed;
            }

            ChasePlayer();
            yield return null;
        }
    }

    IEnumerator Attack()
    {
        agent.enabled = false;

        animator.SetBool("isRunning", false);
        animator.SetTrigger("attack");

        aoeAttack.StartWindup();

        yield return null;

        // wait for the attack animation to start
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("attack")
        );

        // wait for the attack animation to stop
        yield return new WaitWhile(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("attack")
        );

        aoeAttack.Attack();
        blast.Fire();

        // wait for the recovery animation to start
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("attack_recovery")
        );

        // wait for the recovery animation to stop
        yield return new WaitWhile(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("attack_recovery")
        );


        agent.enabled = true;

        yield return new WaitForSeconds(0.5f);

    }

    IEnumerator WalkToRandomPoint()
    {
        animator.SetBool("isRunning", true);
        animator.SetFloat("runningSpeed", 0.0f);

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

        animator.SetBool("isRunning", false);
    }

}
