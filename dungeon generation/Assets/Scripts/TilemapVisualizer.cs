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
    private TileBase floorTile,WallTile;
    
    //绘制地板
    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions,floorTilemap,floorTile);

    }
    //绘制墙
    internal void PaintSingleBaseWall(Vector2Int position)
    {
        PaintSigleTile(wallTilemap, WallTile, position);
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

   
}
