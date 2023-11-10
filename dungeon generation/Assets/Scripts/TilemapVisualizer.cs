using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap,wallTilemap;
    [SerializeField]
    private TileBase floorTile,WallTile,wallSideRight,wallSiderLeft,wallButtom,wallFull
        ,wallInnerCornerDownLeft,wallInnerCornerDownRight,
        wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft,wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft;//对角线
    
    //绘制地板
    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions,floorTilemap,floorTile);

    }
    //绘制墙
    internal void PaintSingleBaseWall(Vector2Int position,string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if(WallByteTypes.wallTop.Contains(typeAsInt)) { tile = WallTile; }
        else if (WallByteTypes.wallBottm.Contains(typeAsInt)) { tile = wallButtom; }
        else if (WallByteTypes.wallSideLeft.Contains(typeAsInt)) { tile = wallSiderLeft; }
        else if (WallByteTypes.wallSideRight.Contains(typeAsInt)) { tile = wallSideRight; }
        else if (WallByteTypes.wallFull.Contains(typeAsInt)) { tile = wallFull; }
        if (tile!=null)
        PaintSigleTile(wallTilemap, tile, position);
    }

    //遍历每个tile
    private void PaintTiles(IEnumerable<Vector2Int> Positions, Tilemap Tilemap, TileBase Tile)
    {
        foreach (var position in Positions)
        {
            PaintSigleTile(Tilemap,Tile,position);
        }
    }

    //画某个cell
    private void PaintSigleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition,tile);
    }
    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

    internal void PaintSingleConrnerWall(Vector2Int position, string binaryType)
    {
        int typeASInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        if(WallByteTypes.wallInnerCornerDownLeft.Contains(typeASInt))
        {
            tile=wallInnerCornerDownLeft;
        }
        else if (WallByteTypes.wallInnerCornerDownRight.Contains(typeASInt))
        {
            tile = wallInnerCornerDownRight;
        }
        else if (WallByteTypes.wallDiagonalCornerUpRight.Contains(typeASInt))
        {
            tile = wallDiagonalCornerUpRight;
        }
        else if (WallByteTypes.wallDiagonalCornerUpLeft.Contains(typeASInt))
        {
            tile = wallDiagonalCornerUpLeft;
        }
        else if (WallByteTypes.wallDiagonalCornerDownLeft.Contains(typeASInt))
        {
            tile = wallDiagonalCornerDownLeft;
        }
        else if (WallByteTypes.wallDiagonalCornerDownRight.Contains(typeASInt))
        {
            tile = wallDiagonalCornerDownRight;
        }
       

        if (tile != null) { PaintSigleTile(wallTilemap,tile,position); }    
        
    }
}
