using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
[Serializable]
public class DropItemAndProbability
{
    public GameObject item;
    public float probability;//Put a value from 0 - 1, 1 indicates 100%;
}

public class Collectible : MonoBehaviour
{
    public float hp;
    public GameObject sureToDrop;
    public List<DropItemAndProbability> DropItemAndProbability_List;
    public void TakeDamage(float damage)
    {
        hp -= damage;
        if(hp<0)
            Destroy(gameObject);
    }
    private void OnDestroy()
    {
        Instantiate(sureToDrop,transform.position,Quaternion.identity);
        foreach (var item in DropItemAndProbability_List)
        {
            if (Random.value < item.probability)
                Instantiate(item.item, transform.position, Quaternion.identity);
        }
    }
}
