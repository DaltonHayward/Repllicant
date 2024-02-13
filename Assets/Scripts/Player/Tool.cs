using UnityEngine;

public class Tool: MonoBehaviour
{
    protected Animator _animator;
    protected PlayerController _playerController;

    public float Damage;
    
    private void Awake()
    {
        _animator = GetComponentInParent<Animator>();
        _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
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
