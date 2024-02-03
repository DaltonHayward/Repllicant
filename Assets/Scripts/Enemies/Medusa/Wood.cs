using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Collectible
{
    public GameObject dropWhenStoned; //Items dropped when stoned
    [HideInInspector]public GameObject dropItemStart; // Items that were supposed to drop
    private void Start()
    {
        dropItemStart = sureToDrop;
    }
    public void Stoned()
    {
        sureToDrop = dropWhenStoned;
    }
    public void UnStoned() //Stone removed
    {
        sureToDrop = dropItemStart;
    }

}
