using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGuard : Enemy
{
    [Header("Charge knockback distance")]
    public float strokeBackDistance;
    [Header("Shockwave damage")]
    public float WaveAttack;
    [Header("Charge speed")]
    public float chargespeed;
    //Charge distance
    float chargedistance;
    [Header("chargeRange")]
    public float chargeRange;
    [Header("chargeInternal")]
    public float chargeInternal;
    [Header("chargeclock")]
    public float chargeclock;
    [Header("chargeTargetPosition")]
    public Vector3 chargeTargetPosition;
    [Header("PoisoningInjury")]
    public int poisoningInjury;
    [Header("poisoningTime")]
    public float poisoningTime;

    public List<Buff> buffs = new List<Buff>();

    public BoarState state;
    public float skillSpeed;
    public void Die()
    {
        Destroy(gameObject);
    }
    public int lookAccurate = 10;
    float subAngle;
    List<GameObject> target = new List<GameObject>();
    public void LookAround()
    {
        target.Clear();

        RaycastHit hit;
        for (int i = 0; i < lookAccurate; i++)
        {
            if (Physics.Raycast(transform.position, Quaternion.Euler(0, -1 * subAngle * (i + 1), 0) * transform.forward, out hit, attackRange + 0.1f) && !target.Contains(hit.collider.gameObject))
            {

                target.Add(hit.collider.gameObject);
            }
            if (Physics.Raycast(transform.position, Quaternion.Euler(0, subAngle * (i + 1), 0) * transform.forward, out hit, attackRange + 0.1f) && !target.Contains(hit.collider.gameObject))
            {

                target.Add(hit.collider.gameObject);
            }
        }
    }
    public override void Start()
    {
        base.Start();
        subAngle = 45f / lookAccurate;
        gameObject.AddComponent<SlowDownBuff>().Init(gameObject);
    }


    Vector3 chargeDir;
    bool playerIsDamageByCharge = false;
    bool meduIsDamageByCharge = false;
    public override void Update()
    {

        if (Vector3.Distance(new Vector3(player.position.x, 0, player.position.z), new Vector3(transform.position.x, 0, transform.position.z)) < chargeRange)
        {
            chargeclock += Time.deltaTime;
            if (chargeclock > chargeInternal)
            {
                chargeTargetPosition = player.position;
                state = BoarState.charge;
                chargeclock = 0;
                chargeDir = new Vector3((player.position - transform.position).normalized.x, 0, (player.position - transform.position).normalized.z);
                chargedistance = Vector3.Distance(new Vector3(player.position.x, 0, player.position.z), new Vector3(transform.position.x, 0, transform.position.z));
            }
        }


        switch (state)
        {
            case BoarState.idle:

                playerIsDamageByCharge = false;
                if (Vector3.Distance(player.position, transform.position) < chaseRange)
                {
                    state = BoarState.chase;

                    break;
                }
                animator.Play("Idle");
                break;
            case BoarState.chase:
                if (Vector3.Distance(player.position, transform.position) <= attackRange)
                {
                    state = BoarState.attack;
                    navMeshAgent.enabled = false;

                    break;
                }
                if (Vector3.Distance(player.position, transform.position) > chaseRange)
                {
                    state = BoarState.idle;
                    navMeshAgent.enabled = false;

                    break;
                }
                navMeshAgent.enabled = true;
                Vector3 targetPos = player.position - (player.position - transform.position).normalized * (attackRange - 1);
                targetPos.y = transform.position.y;
                navMeshAgent.SetDestination(targetPos);
                animator.Play("Chase");

                break;
            case BoarState.attack:
                if (Vector3.Distance(player.position, transform.position) > attackRange)
                {
                    state = BoarState.chase;

                    break;
                }
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                if (Time.time - lastAttackTime > attackSpeed)
                {
                    Debug.Log($"Player taken damage from minotaur{attack}");

                    player.gameObject.GetComponent<PlayerController>().ReceiveMessage("DamageOnPlayer:" + attack.ToString());
                    player.gameObject.GetComponent<PlayerHealth>().TakeDamage(attack);

                    ///毒buff 添加
                    if (buffs.Count>=3) //最多3成 
                    {
                        GameObject.Destroy(buffs[0]);  //删除最开始的毒  //添加新的毒
                        buffs.Remove(buffs[0]);
                        PoisoningBuff poisoningBuff = player.gameObject.AddComponent<PoisoningBuff>();
                        poisoningBuff.Init(player.gameObject, 1, 1, 10);
                        buffs.Add(poisoningBuff);
                    }
                    else//添加新的毒
                    {
                        PoisoningBuff poisoningBuff = player.gameObject.AddComponent<PoisoningBuff>();
                        poisoningBuff.Init(player.gameObject, 1, 1, 10);
                        buffs.Add(poisoningBuff);
                    }
                    lastAttackTime = Time.time;
                    animator.Play("Attack");

                }

                if (Time.time - lastSkillTime > skillSpeed)
                {

                    //Shock wave
                    LookAround();


                    lastSkillTime = Time.time;
                }

                break;
            case BoarState.charge:

                if (Vector3.Distance(new Vector3(chargeTargetPosition.x, 0, chargeTargetPosition.z), new Vector3(transform.position.x, 0, transform.position.z)) < 0.2f)
                {
                    state = BoarState.idle;

                    return;
                }
                Collider[] target;
                transform.Translate(chargeDir * chargespeed * Time.deltaTime, Space.World);
                target = Physics.OverlapSphere(transform.position, 0.6f);
                for (int i = 0; i < target.Length; i++)
                {
                    if (target[i].tag == "Player" && playerIsDamageByCharge == false)
                    {
                        Debug.Log($"Player taken charge damage{attack}");

                        //target[i].gameObject.GetComponent<PlayerController>().SetState(PlayerController.State.KNOCKBACK);
                        //target[i].gameObject.GetComponent<PlayerController>().strokeBackTargetPosition = target[i].transform.position + new Vector3((Quaternion.Euler(0, 30f, 0) * (target[i].transform.position - transform.position).normalized * strokeBackDistance).x, 0, (Quaternion.Euler(0, 30f, 0) * (target[i].transform.position - transform.position).normalized * strokeBackDistance).z);
                        target[i].gameObject.GetComponent<PlayerController>().ReceiveMessage("DamageOnPlayer:" + attack.ToString());
                        target[i].gameObject.GetComponent<PlayerHealth>().TakeDamage(attack);

                        playerIsDamageByCharge = true;
                    }
                }
                break;
            case BoarState.stone:
                break;
            default:
                break;
        }
    }
    public void Die1()
    {
        Destroy(gameObject);
    }

    public void TakeDamage1(float damage)
    {
        hp -= damage;
        if (hp <= 0)
            Die();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
