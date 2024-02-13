using UnityEngine;

public class Tool : MonoBehaviour
{
    private Animator _animator;
    private PlayerController _playerController;

    private void Awake()
    {
        _animator = GetComponentInParent<Animator>();
        _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("hit tool");
            ISubscriber subscriber = other.GetComponent<ISubscriber>();
            if (subscriber != null)
            {
                subscriber.ReceiveMessage("Tool");
                _animator.CrossFade("Blend Tree", 0.07f, 0);
                _playerController.SetState(PlayerController.State.STANDING);
            }
        }
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
