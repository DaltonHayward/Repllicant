using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    // structures to place
    [SerializeField] private List<Structure> structures;
    // where structures are placed in scene
    [SerializeField] private Tilemap walls;
    [SerializeField] private Tilemap ground;
    [SerializeField] private Transform props;
    // base tileset
    [SerializeField] private BaseLevelTiles baseLevelTiles;
    // level dimensions
    [SerializeField] int width = 80;
    [SerializeField] int height = 50;

    // ai
    [SerializeField] private GameObject nav_Mesh;

    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel();
        StartCoroutine(DelayBake());
    }

    // Delay navmesh rebuild till after level gen
    private IEnumerator DelayBake()
    {
        yield return new WaitForSeconds(0.5f);
        //nav_Mesh.GetComponent<NavMeshSurface>().BuildNavMesh();
    }


    // Generate a complete level with structures
    private void GenerateLevel()
    {
        (List<Rect>, List<Rect>) partitionedLevel = PartitionLevel();
        List<Rect> partitionedAreas = partitionedLevel.Item1;
        List<Rect> borders = partitionedLevel.Item2;
        CreateBaseGroundTiles();
        SpawnStructures(partitionedAreas);
    }


    // Partition the level into different rooms by randomly drawing lines through the level. Return a list of
    // rectangles representing the rooms and borders between rooms.
    private (List<Rect>, List<Rect>) PartitionLevel()
    {
        List<Rect> partitions = new List<Rect>();
        List<Rect> borders = new List<Rect>();
        partitions.Add(new Rect(0, height - 1, width, height));
        int numPartitions = Random.Range(1, 5);

        for (int i = 0; i < numPartitions; i++)
        {
            // Choose an area to partition, making sure that it is not too small
            Rect rectToPartition = Rect.zero;
            int indexOfPartition = -1;
            while (rectToPartition.width <= 2 || rectToPartition.height <= 2)
            {
                indexOfPartition = Random.Range(0, partitions.Count);
                rectToPartition = partitions[indexOfPartition];
            }
            partitions.RemoveAt(indexOfPartition);
            if (i % 2 == 0)
            {
                // Cut vertically through the given area to partition
                int partitionPoint = Random.Range(1, (int)rectToPartition.width - 1);
                Rect leftRect = new Rect(rectToPartition.x, rectToPartition.y, partitionPoint, rectToPartition.height);
                Rect borderRect = new Rect(rectToPartition.x + partitionPoint, rectToPartition.y, 1, rectToPartition.height);
                Rect rightRect = new Rect(rectToPartition.x + partitionPoint + 1, rectToPartition.y,
                    rectToPartition.width - (partitionPoint + 1), rectToPartition.height);
                partitions.Add(leftRect);
                partitions.Add(rightRect);
                borders.Add(borderRect);
            }
            else
            {
                // Cut horizontally through the given area to partition
                int partitionPoint = Random.Range(1, (int)rectToPartition.height - 1);
                Rect topRect = new Rect(rectToPartition.x, rectToPartition.y, rectToPartition.width, partitionPoint);
                Rect borderRect = new Rect(rectToPartition.x, rectToPartition.y - partitionPoint, rectToPartition.width,
                    1);
                Rect bottomRect = new Rect(rectToPartition.x, rectToPartition.y - partitionPoint - 1,
                    rectToPartition.width, rectToPartition.height - partitionPoint - 1);
                partitions.Add(topRect);
                partitions.Add(bottomRect);
                borders.Add(borderRect);
            }
        }
        partitions = AddBufferToAreas(partitions);
        return (partitions, borders);
    }


    // Add a buffer area around an area for a list of areas
    List<Rect> AddBufferToAreas(List<Rect> areas)
    {
        List<Rect> bufferedAreas = new List<Rect>();
        foreach (Rect area in areas)
        {
            bufferedAreas.Add(new Rect(area.x + 1, area.y - 1, area.width - 2, area.height - 2));
        }
        return bufferedAreas;
    }


    // Fills the ground in and around the level with random ground tiles
    private void CreateBaseGroundTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile groundTile = baseLevelTiles.groundTiles[Random.Range(0, baseLevelTiles.groundTiles.Count)];
                ground.SetTile(new Vector3Int(x + (int)transform.position.x, y + (int)transform.position.z, 0), groundTile);
            }
        }
    }


    // Recursively fills a partitioned level with random structures
    private void SpawnStructures(List<Rect> partitionedLevel)
    {
        foreach (Rect partitionedArea in partitionedLevel)
        {
            Structure structureToGen = ChooseStructureToPlace(partitionedArea);
            if (structureToGen == null)
            {
                continue;
            }
            SpawnStructures(SpawnStructure(structureToGen, partitionedArea));
        }
    }


    // Spawns a given structure in the level in a given area. Returns a list of areas where there is still room to
    // place more structures
    private List<Rect> SpawnStructure(Structure structureToGen, Rect partitionedArea)
    {
        // Choose offsets to randomize the placement of the structure in the area
        int widthOffset = Random.Range(0, (int)(partitionedArea.width - structureToGen.width));
        int heightOffset = Random.Range(-(int)(partitionedArea.height - structureToGen.height), 0);

        Vector3 structurePosition = new Vector3(partitionedArea.x + widthOffset + transform.position.x, partitionedArea.y + heightOffset + transform.position.z, 0);
        GameObject structure = Instantiate(structureToGen.structure, structurePosition, Quaternion.identity);

        if (structure != null)
        {
            Tilemap structWallsTilemap = structure.transform.Find("Walls").GetComponent<Tilemap>();
            Tilemap structGroundTilemap = structure.transform.Find("Ground").GetComponent<Tilemap>();

            if (structWallsTilemap != null && structGroundTilemap != null)
            {
                // Copy the ground and wall tiles from the structure to corresponding tilemaps of the level generator
                for (int i = 0; i < structureToGen.width; i++)
                {
                    for (int j = 0; j < structureToGen.height; j++)
                    {
                        TileBase wallTile = structWallsTilemap.GetTile(new Vector3Int(i, j, 0));
                        TileBase groundTile = structGroundTilemap.GetTile(new Vector3Int(i, j, 0));

                        if (wallTile != null)
                        {
                            Vector3Int wallTilePosition = new Vector3Int(
                                (int)(partitionedArea.x + i + widthOffset),
                                (int)(partitionedArea.y + j + heightOffset),
                                0
                            );
                            walls.SetTile(wallTilePosition, wallTile);
                        }

                        if (groundTile != null)
                        {
                            Vector3Int groundTilePosition = new Vector3Int(
                                (int)(partitionedArea.x + i + widthOffset + transform.position.x),
                                (int)(partitionedArea.y + -j + heightOffset + transform.position.z),
                                0
                            );
                            ground.SetTile(groundTilePosition, groundTile);
                        }
                    }
                }
            }

            // Re-parent the props from the structure to the level generator
            Transform structProps = structure.transform.Find("Props");
            if (structProps != null)
            {
                foreach (Transform childProp in structProps)
                {
                    // Choose offsets to randomize the placement of the prop in the area
                    int xOffset = Random.Range(0, (int)(structureToGen.width-1));
                    int zOffset = Random.Range(-(int)(structureToGen.height-1), 0); 

                    // set the world position of the prop
                    Vector3 propWorldPosition = new Vector3(
                        childProp.position.x + xOffset,
                        0,
                        childProp.position.y + zOffset
                    );
                    childProp.position = propWorldPosition;

                    // randomize the rotation of the prop
                    childProp.rotation = Quaternion.Euler(childProp.localEulerAngles.x, Random.Range(0, 360), childProp.localEulerAngles.z);
                    childProp.SetParent(props);
                }
            }
            Destroy(structure);
        }
        return GetRemainingAreas(partitionedArea, structureToGen, widthOffset, heightOffset);
    }


    // Get the remaining areas in a partitioned room after placing a structure inside of it with a given width and
    // height offset
    List<Rect> GetRemainingAreas(Rect initialArea, Structure generatedStructure, int widthOffset, int heightOffset)
    {
        // Get the areas to the right and left of where the structure was placed
        if (initialArea.width - generatedStructure.width > initialArea.height - generatedStructure.height)
        {
            return new List<Rect>
            {
                new(initialArea.x, initialArea.y, Mathf.Max(0,widthOffset - 1), initialArea.height),
                new(initialArea.x + widthOffset + generatedStructure.width + 1, initialArea.y,
                    Mathf.Max(0,initialArea.width - widthOffset - generatedStructure.width - 1), initialArea.height)
            };
        }
        // Get the areas above and below where the structure was placed
        else
        {
            return new List<Rect>
            {
                new(initialArea.x, initialArea.y, initialArea.width, Mathf.Max(0,Mathf.Abs(heightOffset) - 1)),
                new(initialArea.x, initialArea.y + heightOffset - generatedStructure.height - 1,
                    initialArea.width, Mathf.Max(0,initialArea.height + heightOffset - generatedStructure.height - 1))
            };
        }
    }


    // Choose a random structure to place that will fit within the given area
    Structure ChooseStructureToPlace(Rect area)
    {
        List<Structure> validStructs = new List<Structure>();
        for (int i = 0; i < structures.Count; i++)
        {
            if ((int)area.width >= structures[i].width && (int)area.height >= structures[i].height)
            {
                validStructs.Add(structures[i]);
            }
        }
        if (validStructs.Count > 0)
        {
            return validStructs[Random.Range(0, validStructs.Count)];
        }
        return null;
    }


    // Clear all existing tiles, props, and enemies from the level
    private void ClearLevel()
    {
        walls.ClearAllTiles();
        ground.ClearAllTiles();
        List<Transform> propsInScene = new List<Transform>();
        foreach (Transform prop in props)
        {
            propsInScene.Add(prop);
        }
        foreach (Transform prop in propsInScene)
        {
            Destroy(prop.gameObject);
        }
    }

}
