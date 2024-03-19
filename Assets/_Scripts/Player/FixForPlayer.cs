using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixForPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FixPosition());
    }

    public IEnumerator FixPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
        }
    }
}
