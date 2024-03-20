using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
[Serializable]

public class DropItemAndProbability
{
    public GameObject item;
    public float probability; // determines drop rate
}

public class Collectible : MonoBehaviour
{
    public float hp = 100;
    // the item that is guaranteed to drop
    public GameObject sureToDrop;
    // contains all possible item drops with probability
    public List<DropItemAndProbability> DropItemAndProbability_List;

    virtual public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0) { Destroy(gameObject); }
    }

    public void InstantiateLoot()
    {
        Instantiate(sureToDrop, new Vector3(transform.position.x, InventoryController.instance.LookUpItem(sureToDrop.name).worldModel.transform.position.y, transform.position.z), Quaternion.identity);
    }

    private void OnDestroy()
    {
        // drop guaranteed item
        // rolls for each possible drop
        foreach (var item in DropItemAndProbability_List)
        {
            if (Random.value < item.probability) { Instantiate(item.item, transform.position, Quaternion.identity); }
        }
    }
}
