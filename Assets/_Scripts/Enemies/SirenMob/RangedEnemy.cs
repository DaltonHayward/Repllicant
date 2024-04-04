using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
public enum RangedEnemyState
{
    idle,
    attack,
    chase,
}
public class RangedEnemy : Enemy
{

    [SerializeField] RangedEnemy birdClone;

    public List<RangedEnemy> activeClones = new();
    public float skillCD;
    public bool canCopy;
    float lastCopyTime;

    float copyDelayRemaining = 0f;

    //int maxCopyCount = 0;
    public RangedEnemyState rangedEnemy;
    public GameObject bullet;
    public override void Start()
    {
        base.Start();
        //lastCopyTime = skillCD - 10;
        copyDelayRemaining = skillCD;
    }


    // check if bird can copy itself
    private bool CanCopy()
    {
        if (canCopy == true && activeClones.Count < 3 && copyDelayRemaining <= 0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Update()
    {
        // update delay remaining on copy ability
        if (copyDelayRemaining > 0f)
        {
            copyDelayRemaining -= Time.deltaTime;
        }

        // remove clones on death
        if (hp <= 0)
        {
            foreach (RangedEnemy clone in activeClones)
            {
                clone.Die();
            }
        }

        // remove dead clones from activeClones list
        foreach (RangedEnemy clone in activeClones)
        {
            if (clone == null) //ReferenceEquals(clone, null) ? false : (clone ? false : true)
            {
                activeClones.Remove(clone);
            }
        }

        /*
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
        }*/

        switch (rangedEnemy)
        {
            case RangedEnemyState.idle:
                if (Vector3.Distance(player.position, transform.position) < chaseRange)
                {
                    rangedEnemy = RangedEnemyState.chase;

                    break;
                }
                //lastCopyTime = -skillCD - 10;
                //navMeshAgent.isStopped = true;
                animator.Play("Idle");
                break;
            case RangedEnemyState.attack:
                if (Vector3.Distance(player.position, transform.position) > attackRange)
                {
                    rangedEnemy = RangedEnemyState.chase;

                    break;
                }
                //navMeshAgent.isStopped = true;
                //transform.LookAt(player.position);
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                if (Time.time - lastAttackTime > attackSpeed)
                {
                    Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Bullet>().player = player;
                    lastAttackTime = Time.time;
                }
                if (CanCopy())
                {
                    //for (int i = 0; i < 3; i++)
                    Vector3 pos = transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
                    RangedEnemy clone = Instantiate(birdClone, pos, Quaternion.identity); //.GetComponent<RangedEnemy>();
                    activeClones.Add(clone);
                    copyDelayRemaining = skillCD;
                    //maxCopyCount++;

                    //lastCopyTime = Time.time;
                }
                animator.Play("Attack");

                break;
            case RangedEnemyState.chase:

                if (Vector3.Distance(player.position, transform.position) <= attackRange)
                {
                    rangedEnemy = RangedEnemyState.attack;
                    navMeshAgent.enabled = false;
                    break;
                }
                if (Vector3.Distance(player.position, transform.position) > chaseRange)
                {
                    rangedEnemy = RangedEnemyState.idle;
                    navMeshAgent.enabled = false;

                    break;
                }
                //navMeshAgent.isStopped = false;
                //navMeshAgent.SetDestination(player.position);
                navMeshAgent.enabled = true;
                Vector3 targetPos = player.position - (player.position - transform.position).normalized * (attackRange - 1);
                targetPos.y = transform.position.y;
                navMeshAgent.SetDestination(targetPos);

                animator.Play("Chase");

                break;
        }
    }

}