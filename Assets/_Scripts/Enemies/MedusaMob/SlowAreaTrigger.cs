using UnityEngine;
using UnityEngine.AI;

public class SlowAreaTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ISubscriber subscriber))
        {
            subscriber.ReceiveMessage("SpeedChange:0.93");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ISubscriber subscriber))
        {
            subscriber.ReceiveMessage("SpeedChange:" + (1 / 0.93).ToString());
        }
    }
}