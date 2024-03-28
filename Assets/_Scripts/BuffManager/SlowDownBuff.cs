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
public class SlowDownBuff:Buff
{
    protected float dis;//减速范围  //半径
    protected float intervalTime;//间隔时间
    protected Collider[] colliders=new Collider[10];
    protected float slowDown;//减速数值 %

    public GameObject player;

    protected float IntervalTime;
    public void Init(GameObject user,float slowDown=0.7f, float dis=10,float intervalTime=0.5f)
    {
        this.user = user;
        this.dis = dis;
        this.intervalTime = intervalTime;
        this.slowDown = slowDown;
    }
    public void Update()
    {
        IntervalTime -= Time.deltaTime;
        if (IntervalTime <= 0)
        {
            IntervalTime = intervalTime;
            if (player)
            {
                float juli = Vector3.Distance(player.transform.position, user.transform.position);
                if (juli > this.dis)
                {
                    if (player.TryGetComponent(out ISubscriber subscriber))
                    {
                        subscriber.ReceiveMessage("SpeedChange:" + (1 / slowDown).ToString());
                    }
                    player = null;
                }
            }
            else
            {
                colliders = Physics.OverlapSphere(user.transform.position, this.dis);
                foreach (Collider collider in colliders)
                {
                    if (collider == null) continue;
                    if (collider.TryGetComponent(out ISubscriber subscriber))
                    {
                        if (collider.gameObject == user)
                        {
                            continue;
                        }
                        if (collider.tag == "Player")
                        {
                            subscriber.ReceiveMessage("SpeedChange:" + slowDown);
                            player = collider.gameObject;
                        }
                    }

                }
            }
        }
    }

    public void OnDestroy()
    {
        if (player)
        {
            if (player.TryGetComponent(out ISubscriber subscriber))
            {
                subscriber.ReceiveMessage("SpeedChange:" + (1 / slowDown).ToString());
            }
        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position,dis);
    }
}
