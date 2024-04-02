using Ink.Parsed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 对范围类的 Player 减速
/// </summary>
public class PoisoningBuff : Buff
{
    protected int attack;//攻击伤害
    protected float attackInterval;//攻击间隔

    protected float time;//持续时间
    private float lastAttackTime;
    private float lastTime;
    public void Init(GameObject user,int attack=1,float attackInterval = 1f,float time=10)
    {
        this.user = user;
        this.attack = attack;
        this.attackInterval = attackInterval;
        this.time = time;
    }
    public void Update()
    {
        lastAttackTime += Time.deltaTime;
        if (lastAttackTime>=attackInterval)
        {
            lastAttackTime = 0;
            if (user)
            {
                if (user.TryGetComponent(out ISubscriber subscriber))
                {
                    PlayerHealth playerHealth = user.gameObject.GetComponent<PlayerHealth>();
                    playerHealth.TakeDamage(playerHealth.maxHealth/100);
                }
            }
        }

        lastTime += Time.deltaTime;
        if (lastTime>=time)
        {
            GameObject.Destroy(this);
        }
    }

}
