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
    [SerializeField] private Tilemap ground;
    [SerializeField] private Tilemap accents;
    [SerializeField] private Tilemap walls;
    [SerializeField] private Transform props;
    [SerializeField] private Transform enemies;
    // base tileset
    [SerializeField] private BaseLevelTiles baseLevelTiles;
    // level dimensions
    [SerializeField] public int width = 80;
    [SerializeField] public int height = 50;

    // Enemy Spawning
    [SerializeField] bool enemiesSpawnable = false;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] int enemyMin = 0;
    [SerializeField] int enemyMax = 0;


    // Generate a complete level with structures
    public void GenerateLevel()
    {
        Rect level = new Rect(transform.position.x, transform.position.z, width, height);
        (List<Rect>, List<Rect>) partitionedLevel = PartitionLevel();
        List<Rect> partitionedAreas = partitionedLevel.Item1;
        List<Rect> borders = partitionedLevel.Item2;
        // spawns the structure (with props) and enemies
        SpawnStructures(partitionedAreas);
        SpawnEnemiesInArea(level);
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


    // NOT USED rn
    // Fills the ground with random ground tiles from the base ground tile scriptable object
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
            Tilemap structGroundTilemap = structure.transform.Find("Ground").GetComponent<Tilemap>();
            Tilemap structAccentsTilemap = structure.transform.Find("Ground/Accents").GetComponent<Tilemap>();
            Tilemap structWallsTilemap = structure.transform.Find("Walls").GetComponent<Tilemap>();

            if (structGroundTilemap != null && structWallsTilemap != null && structAccentsTilemap != null)
            {
                // Calculate the size of the area to check
                int checkAreaWidth = structureToGen.width * 3;
                int checkAreaHeight = structureToGen.height * 3;

                // Calculate the offset to center the structure in the check area
                int xOffset = (checkAreaWidth - structureToGen.width) / 2;
                int yOffset = (checkAreaHeight - structureToGen.height) / 2;

                // Copy the ground and wall tiles from the structure to corresponding tilemaps of the level generator
                for (int i = 0; i < checkAreaWidth; i++)
                {
                    for (int j = 0; j < checkAreaHeight; j++)
                    {
                        // Calculate the position of the tile in the check area relative to the structure's position
                        Vector3Int structureTilePos = new Vector3Int(i - xOffset, j - yOffset, 0);

                        // Get the ground and wall tiles from the structure's tilemaps
                        TileBase groundTile = structGroundTilemap.GetTile(structureTilePos);
                        TileBase accentsTile = structAccentsTilemap.GetTile(structureTilePos);
                        TileBase wallTile = structWallsTilemap.GetTile(structureTilePos);

                        // Calculate the position of the Ground tile in the level generator's tilemap
                        Vector3Int groundTilePosition = new Vector3Int(
                            Mathf.RoundToInt(structurePosition.x + structureTilePos.x),
                            Mathf.RoundToInt(structurePosition.y + structureTilePos.y),
                            0
                        );

                        // Place the ground tile if it exists
                        if (groundTile != null)
                        {
                            ground.SetTile(groundTilePosition, groundTile);
                        }

                        // Calculate the position of the Accents tile in the level generator's tilemap
                        Vector3Int accentsTilePosition = new Vector3Int(
                            Mathf.RoundToInt(structurePosition.x + structureTilePos.x),
                            Mathf.RoundToInt(structurePosition.y + structureTilePos.y),
                            0
                        );

                        // Place the accents tile if it exists
                        if (accentsTile != null)
                        {
                            accents.SetTile(accentsTilePosition, accentsTile);
                        }

                        // Calculate the position of the wall tile in the level generator's tilemap
                        Vector3Int wallTilePosition = new Vector3Int(
                            Mathf.RoundToInt(structurePosition.x + structureTilePos.x),
                            Mathf.RoundToInt(structurePosition.y + structureTilePos.y),
                            0
                        );

                        // Place the wall tile if it exists
                        if (wallTile != null)
                        {
                            walls.SetTile(wallTilePosition, wallTile);
                        }
                    }
                }
            }
            else { Debug.Log("structGroundTilemap != null && structWallsTilemap != null && structAccentsTilemap != null check failed");}
        

        // Re-parent the props from the structure to the level generator
        Transform structProps = structure.transform.Find("Props");
            if (structProps != null)
            {
                foreach (Transform childProp in structProps)
                {
                    // Choose offsets to randomize the placement of the prop in the area
                    int xOffset = Random.Range(0, (int)(structureToGen.width - 1));
                    int zOffset = Random.Range(-(int)(structureToGen.height - 1), 0);

                    // set the world position of the prop
                    Vector3 propWorldPosition = new Vector3(
                        childProp.position.x + xOffset,
                        0,
                        childProp.position.y + zOffset
                    );
                    // check for overlaps
                    if (IsPositionValid(propWorldPosition, 3f)) {
                    childProp.position = propWorldPosition;

                    // randomize the rotation of the prop
                    childProp.rotation = Quaternion.Euler(childProp.localEulerAngles.x, Random.Range(0, 360), childProp.localEulerAngles.z);
                    childProp.SetParent(props);
                    childProp.gameObject.layer = props.gameObject.layer;
                    }
                }
            }

            // PARTIALLY FUNCTIONING FOR PONDS 
            Transform structFixedProps = structure.transform.Find("FixedProps");
            if (structFixedProps != null)
            {
                foreach (Transform childProp in structFixedProps)
                {
                    // Apply manual offsets to allign with center of object
                    Vector3 worldPosition = new Vector3(childProp.position.x - 1.0f, 0, childProp.position.y + 3.0f);
                    
                    if(IsPositionValid(worldPosition, 5f))
                    { 
                        childProp.position = worldPosition;
                        childProp.SetParent(props);
                        childProp.gameObject.layer = props.gameObject.layer;
                    }

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


    private void SpawnEnemiesInArea(Rect area)
    {
        // prevent error
        if (enemyPrefabs == null) { return; }
        // randomize the number of enemies that can spawn, based on given min and max values
        int maxEnemies = Random.Range(enemyMin, enemyMax + 1);
        int numEnemies = Mathf.Clamp(maxEnemies, enemyMin, enemyMax);

        // check the placement area before placing the enemy
        for (int i = 0; i < numEnemies; i++)
        {
            Vector3 enemyPosition = GetRandomPositionInArea(area);
            // Check if the enemy position is valid (not within props)
            if (IsPositionValid(enemyPosition, 3f))
            {
                // Instantiate enemy
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
                GameObject enemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
                enemy.transform.SetParent(enemies);
                enemy.layer = enemies.gameObject.layer;
            }
        }
    }


    // Get a random position within a given area
    private Vector3 GetRandomPositionInArea(Rect area)
    {
        float x = Random.Range(area.xMin, area.xMax);
        float y = Random.Range(area.yMin, area.yMax);
        return new Vector3(x, 0, y);
    }


    // Check if a position is valid for placing
    private bool IsPositionValid(Vector3 position, float overlapRadius)
    {
        int propsLayer = LayerMask.NameToLayer("Props");
        int mobsLayer = LayerMask.NameToLayer("Mobs");
        int groundLayer = LayerMask.NameToLayer("Ground");
        

        // check for colliders around position
        Collider[] colliders = Physics.OverlapSphere(position, overlapRadius);

        foreach (Collider collider in colliders)
        {
            // Check for invalid pos
            if ((collider.gameObject.layer == propsLayer) | ((collider.gameObject.layer == mobsLayer)))
            {
                return false;
            }
            // Ignore ground
            if (collider.gameObject.layer == groundLayer)
            {
                continue;
            }
        }
        return true;
    }
}
