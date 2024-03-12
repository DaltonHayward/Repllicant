using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class LevelGenManager : MonoBehaviour
{
    // generators
    [SerializeField] private Transform levelGenerators;
    [SerializeField] private Transform enemyGenerators;

    // ai
    [SerializeField] private GameObject nav_Mesh;

    // THIS SHOULD BE ADDED TO GAME MANAGER OR SOMETHING
    [SerializeField] private GameObject ui;


    // Start is called before the first frame update
    void Start()
    {
        ui.SetActive(true);
        Generate();
        GenerateEnemies();
        StartCoroutine(DelayBake());
    }

    // Activate all level generators
    private void Generate()
    {
        foreach (Transform levelgenerator in levelGenerators) 
        {
            levelgenerator.GetComponent<LevelGenerator>().GenerateLevel();
        }
    }

    // Activate all enemy generators
    private void GenerateEnemies()
    {
        foreach (Transform levelgenerator in enemyGenerators)
        {
            levelgenerator.GetComponent<LevelGenerator>().GenerateLevel();
        }
    }

    // Delay navmesh rebuild till after level gen
    private IEnumerator DelayBake()
    {
        yield return new WaitForSeconds(1f);
        nav_Mesh.GetComponent<NavMeshSurface>().BuildNavMesh();
    }
}
