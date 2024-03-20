using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirenSong : MonoBehaviour
{
    [SerializeField] private float emitFrequency;
    private float broadcastRange;
    private string channel;


    private IEnumerator emissionCoroutine;

    public void SetParameters(float freq, float range, string ch)
    {
        emitFrequency = freq;
        broadcastRange = range;
        channel = ch;
        emissionCoroutine = CoEmit();
        StartCoroutine(emissionCoroutine);
    }

    private IEnumerator CoEmit()
    {
        while (true)
        {
            Collider[] targets = Physics.OverlapSphere(transform.position, broadcastRange);
            foreach (Collider c in targets)
            {
                ISubscriber[] subs = c.GetComponents<ISubscriber>();
                if (subs != null)
                {
                    foreach (ISubscriber sub in subs)
                    {
                        sub.ReceiveMessage(channel);
                        if (c.gameObject.GetComponent<Charmable>() != null)
                            c.gameObject.GetComponent<Charmable>().Siren = transform;
/*                        if (c.gameObject.GetComponent<PlayerHealth>() != null)
                        { 
                            if 
                        }*/
                    }
                }
            }
            yield return new WaitForSeconds(emitFrequency);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, broadcastRange);
    }
}
