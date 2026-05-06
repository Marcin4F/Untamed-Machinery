using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    public GameObject player;
    public float attackRange = 5.0f;
    public float attackCooldown = 1.0f;
    public LayerMask hitLayerMask;
    public int damage = 3;
    private Enemy enemyScript;
    private float lastAttackTime;


    void Start()
    {
        player = GameObject.FindWithTag("Player");
        enemyScript = GetComponent<Enemy>();
    }

    void Update()
    {
        
        if (CanAttack())
        {
            Debug.Log("Trying to attack!");
            TryAttack();
        }
    }

    private bool CanAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        //Debug.Log($"Distance: {distanceToPlayer}, AttackRange: {attackRange}");

        return distanceToPlayer <= attackRange;
    }

    private void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null && playerScript.alive && enemyScript.isAlive)
        {
            Debug.Log("hit player");
            playerScript.TakeDamage(damage);
        }

        lastAttackTime = Time.time;
    }

}
