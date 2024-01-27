using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnClick : MonoBehaviour
{
    [SerializeField] private GameObject rockModel;

    private void OnMouseDown() 
    {
        GameObject rock = Instantiate(rockModel, new Vector3(transform.position.x, -0.3f, transform.position.z), rockModel.transform.rotation);
        Destroy(gameObject);

    }
}
