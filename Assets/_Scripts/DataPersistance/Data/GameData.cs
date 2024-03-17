using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // values to save are listed here
    public int testInt;
    public bool testBool;
    //public Vector3 playerPosition;

    // the values defined in this constructor will be the default values when no data to load
    public GameData()
    {
        // default values
        this.testInt = 0;
        this.testBool = false;
        //playerPosition = new Vector3(151, 0, -93);
    }
}
