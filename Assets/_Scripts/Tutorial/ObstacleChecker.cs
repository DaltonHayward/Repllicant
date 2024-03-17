using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private int previousNumberOfChildren;

    private void Start()
    {
        previousNumberOfChildren = this.transform.childCount;
    }
    // Update is called once per frame
    void Update()
    {
        int currentNumberOfChildren = this.transform.childCount;

        if(currentNumberOfChildren == 0 && previousNumberOfChildren != 0)
        {
            Debug.Log("All obstacles are gone");
        }

        previousNumberOfChildren = currentNumberOfChildren;

        
    }
}
