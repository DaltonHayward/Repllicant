using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtMinotaur : MonoBehaviour, ISubscriber
{
    private Color _originalMaterialColor;
    public void Awake()
    {
        _originalMaterialColor = GetComponentInChildren<SkinnedMeshRenderer>().materials[0].GetColor("_BaseColor");
    }
    public void ReceiveMessage(string channel)
    {
        if (channel.StartsWith("Attacked"))
        {
            string[] parts = channel.Split(':');

            float damage;
            if (float.TryParse(parts[1].Trim(), out damage))
            {
                GetComponent<COW>().TakeDamage(damage);
                GetComponentInChildren<SkinnedMeshRenderer>().materials[0].SetColor("_BaseColor", Color.red);
                StartCoroutine(Timeout(0.1f));
            }
        }
    }

    IEnumerator Timeout(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GetComponentInChildren<SkinnedMeshRenderer>().materials[0].SetColor("_BaseColor", _originalMaterialColor);
    }

}
