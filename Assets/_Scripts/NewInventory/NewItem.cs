using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/NewItem", order = 1)]
public class NewItem : ScriptableObject
{
    
    public string ItemName;
    [field: TextArea]
    public string description;
    public GameObject envModel;
    public GameObject invModel;
    public Sprite invIcon;
    public int width;
    public int height;
    public bool canStack;
    public int maxStackSize;

    public void construct(){
        invModel.GetComponent<NewInventoryItem>().Construct(this);
        envModel.GetComponent<EnvItem>().Construct(this);
    }
    

}
