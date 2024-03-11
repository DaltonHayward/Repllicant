using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class LevelGenManager : MonoBehaviour
{
    // generators
    [SerializeField] private Transform levelgenerators;
    [SerializeField] private Transform enemygenerators;

    // ai
    [SerializeField] private GameObject nav_Mesh;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Delay navmesh rebuild till after level gen
    private IEnumerator DelayBake()
    {
        yield return new WaitForSeconds(0.5f);
        nav_Mesh.GetComponent<NavMeshSurface>().BuildNavMesh();
    }
}
