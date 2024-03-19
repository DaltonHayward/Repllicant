using System.Collections;
using UnityEngine;

public class Tool: MonoBehaviour, ISubscriber, Burnable
{
    private bool isBurning = false;
    private float burnTime = 15f;
    public float Damage;

    protected Animator _animator;
    protected PlayerController _playerController;
    
    private void Awake()
    {
        _animator = GetComponentInParent<Animator>();
        _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    public void ReceiveMessage(string channel)
    {
        if (channel == "Burning")
        {
            if (!isBurning)
            {
                TakeFireDamage();
            }

        }
    }

    public void TakeFireDamage()
    {
        float igniteChance = Random.value;

        //Debug.Log(igniteChance);

        if (igniteChance < 0.01)
        {
            Ignite();
        }
    }

    public void Ignite()
    {
        StartCoroutine(BurnTimer());
    }

    IEnumerator BurnTimer()
    {
        isBurning = true;
        Inventory_Item item = GetComponent<Inventory_Item>();
        //Update sprite
        item.UpdateSprite(GetComponent<Inventory_Item>().itemData.sprites[1].sprite);
        // Add dmg
        float prevDmg = Damage;
        Damage = prevDmg * 2;

        yield return new WaitForSeconds(burnTime);

        isBurning = false;
        // reset dmg
        Damage = prevDmg;
        // reset sprite
        item.UpdateSprite(GetComponent<Inventory_Item>().itemData.sprites[0].sprite);
    }

    #region - Colliders -
    public void BeginCollision()
    {
        GetComponent<CapsuleCollider>().enabled = true;
    }

    public void EndCollision()
    {
        GetComponent<CapsuleCollider>().enabled = false;
    }
    #endregion

    #region - Trail -
    public void BeginTrail()
    {
        GetComponentInChildren<TrailRenderer>().enabled = true;
    }

    public void EndTrail()
    {
        GetComponentInChildren<TrailRenderer>().enabled = false;
    }
    #endregion
}
