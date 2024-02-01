using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Structure", order = 1)]
public class Structure : ScriptableObject
{
    public GameObject structure;
    public int width;
    public int height;
}
