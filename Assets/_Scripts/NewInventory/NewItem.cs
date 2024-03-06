using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/NewItem", order = 1)]
public class NewItem : ScriptableObject
{
    string ItemName;
    string description;
    GameObject envModel;
    GameObject invModel;
    Sprite invIcon;
    int width;
    int height;
    bool canStack;
    int maxStackSize;
    

}
