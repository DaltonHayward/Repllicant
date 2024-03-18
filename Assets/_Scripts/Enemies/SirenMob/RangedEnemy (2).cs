using UnityEngine;
public enum RangedEnemyState
{
    idle,
    attack,
    chase,
}
public class RangedEnemy : Enemy
{
    public float skillCD;
    bool canCopy = true;
    float lastCopyTime = 0;
    public RangedEnemyState rangedEnemy;
    public GameObject bullet;

    public override void Update()
    {
        if (canCopy && Time.time - lastCopyTime > skillCD)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 pos = transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
                RangedEnemy rangedEnemy = Instantiate(gameObject, pos, Quaternion.identity).GetComponent<RangedEnemy>();
                rangedEnemy.hp = 1;
                rangedEnemy.canCopy = false;
            }
            lastCopyTime = Time.time;
        }
        if (Vector3.Distance(player.position, transform.position) > chaseRange)
        {
            rangedEnemy = RangedEnemyState.idle;
        }
        if (Vector3.Distance(player.position, transform.position) < chaseRange)
        {
            rangedEnemy = RangedEnemyState.chase;
        }
        if (Vector3.Distance(player.position, transform.position) < attackRange)
        {
            rangedEnemy = RangedEnemyState.attack;
        }

        switch (rangedEnemy)
        {
            case RangedEnemyState.idle:
                navMeshAgent.isStopped = true;
                break;
            case RangedEnemyState.attack:
                navMeshAgent.isStopped = true;
                transform.LookAt(player.position);
                if (Time.time - lastAttackTime > attackSpeed)
                {
                    Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Bullet>().player = player;
                    lastAttackTime = Time.time;
                }
                break;
            case RangedEnemyState.chase:
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(player.position);
                break;
        }
    }
}
