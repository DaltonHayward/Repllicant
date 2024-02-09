using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Collectible
{
    public GameObject dropWhenStoned;//石化时掉落物
    [HideInInspector]public GameObject dropItemStart;//本来必掉落物
    private void Start()
    {
        dropItemStart = sureToDrop;
    }
    public void Stoned()
    {
        sureToDrop = dropWhenStoned;
    }
    public void UnStoned()//解除石化
    {
        sureToDrop = dropItemStart;
    }

}
