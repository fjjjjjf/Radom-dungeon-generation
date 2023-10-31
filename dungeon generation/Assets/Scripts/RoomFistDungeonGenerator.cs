using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Vector2 = UnityEngine.Vector2;

public class RoomFistDungeonGenerator : DungeonRandomGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int dungeonWidth=20,dungeonHeight=20;
    [SerializeField]
    [Range(0, 10)]
    private int offset = 1;//避免因为地板而连起来
    [SerializeField]
    private bool randomWalkRooms = false;

    protected override void RunProceduralGeneration()
    {
        createRooms();
    }

    private void createRooms()
    {
      var roomsList =ProceduralGenerationAlogorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition,new Vector3Int
          (dungeonWidth,dungeonHeight,0)),minRoomWidth,minRoomHeight);

        HashSet<Vector2Int> floor =new HashSet<Vector2Int>();
        floor = createSimpleRooms(roomsList);

        //获得房间中心坐标，将房间连接起来
        List<Vector2Int> roomCenters =new List<Vector2Int>();
        foreach (var item in roomsList)
        {
            roomCenters.Add((Vector2Int)(Vector3Int.RoundToInt(item.center)));
        }

        HashSet<Vector2Int> corriders = ConnectRooms(roomCenters);
        floor.UnionWith(corriders);

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor,tilemapVisualizer);
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
       HashSet<Vector2Int> corridors =new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[UnityEngine.Random.Range(0,roomCenters.Count)];

        roomCenters.Remove(currentRoomCenter);
        while (roomCenters.Count > 0)
        {
            Vector2Int cloest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(cloest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, cloest);

            currentRoomCenter = cloest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;

    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while(position.y != destination.y) {
            if(position.y < destination.y)
            {
                position += Vector2Int.up;
            }
            else if( position.y > destination.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        while(position.x !=destination.x)
        {
            if (position.x < destination.x)
            {
                position += Vector2Int.right;
            }
            else if (position.x > destination.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance =float.MaxValue;
        //遍历找到最短
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> createSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomsList)
        {
            for(int col=offset;col<room.size.x-offset;col++)
            {
                for(int row =offset;row<room.size.y-offset;row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }
}
