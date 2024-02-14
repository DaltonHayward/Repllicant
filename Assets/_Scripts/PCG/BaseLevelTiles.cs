using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BaseLevelTiles", order = 2)]
public class BaseLevelTiles : ScriptableObject
{
    public List<Tile> groundTiles;

    public Tile lowerTopWall;
    public Tile upperTopWall;
    public Tile bottomWall;
    public Tile leftWall;
    public Tile rightWall;

    public Tile bottomRightWallCorner;
    public Tile bottomLeftWallCorner;
    public Tile rightBottomWallCorner;
    public Tile leftBottomWallCorner;
    public Tile rightTopWallCorner;
    public Tile leftTopWallCorner;
    public Tile lowerTopRightCorner;
    public Tile upperTopRightCorner;
    public Tile lowerTopLeftCorner;
    public Tile upperTopLeftCorner;
}
