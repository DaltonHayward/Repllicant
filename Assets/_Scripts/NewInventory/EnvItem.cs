using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvItem : MonoBehaviour 
{
    // Start is called before the first frame update
    public NewItem itemType;
    

    void Start()
    {
        
    }

    public void Construct(NewItem itemType)
    {
        this.itemType = itemType;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
