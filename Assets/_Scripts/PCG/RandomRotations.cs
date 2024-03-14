using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script randomizes the rotation of ALL children of the object it is made a component of
public class RandomRotations : MonoBehaviour
{
    [SerializeField] GameObject sureToDrop;
    [SerializeField] GameObject dropWhenStoned;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform t in transform)
        {
            t.transform.rotation = Quaternion.Euler(t.localEulerAngles.x, Random.Range(0, 360), t.localEulerAngles.z);
            if (sureToDrop == null && dropWhenStoned == null) 
            { 
            t.GetComponent<Wood>().sureToDrop = sureToDrop;
            t.GetComponent<Wood>().dropWhenStoned = dropWhenStoned;
            }
        }
    }
}
