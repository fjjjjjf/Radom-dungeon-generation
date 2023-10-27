using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungeonGenerator : DungeonRandomGenerator
{
    [SerializeField]
    private int corridorLength = 14,corridorCount =5;
    [SerializeField]
    private float roomPercent =0.6f;// [Range(0.1f,1)]
    [SerializeField]
    public SimpleRandomWalkSO roomGenerationParameters;
    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPositions=new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        CreateCorridors(floorPositions, potentialRoomPositions);

        HashSet<Vector2Int> roomPositions = createRooms(potentialRoomPositions);


        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);//找到死路


        CreateRoomsAtDeadEnd(deadEnds, roomPositions);
        floorPositions.UnionWith(roomPositions);


        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloor)
    {
        foreach (var position in deadEnds)
        {
            if (roomFloor.Contains(position) == false)
            {
                //死路   
                var room = RunRandomWalk(randomWalkSO, position);
                roomFloor.UnionWith(room);
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds =new List<Vector2Int>();
        foreach(var position in floorPositions)
        {
            int neighboursCount = 0;  //找到同一方向的下一个房间
            foreach(var direction in Direction2D.cardinalDirectionsList) { 
                   
                if(floorPositions.Contains(position+direction)) {  neighboursCount++; }
               

            }
            //只有之前的路
            if(neighboursCount ==1 ) {
                deadEnds.Add(position);
            }


        }
        return deadEnds;
    }

    private HashSet<Vector2Int> createRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions =new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count*roomPercent) ;

        List<Vector2Int> roomToCreate =potentialRoomPositions.OrderBy(x=>Guid.NewGuid()).Take(roomToCreateCount).ToList();

        foreach(var roomPosition in roomToCreate)
        {
            var roomFloor =RunRandomWalk(randomWalkSO,roomPosition);
            roomPositions.UnionWith(roomFloor);
        }
        return roomPositions;
    }

    private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> protentialRoomPositions)
    {
        var currentPosition = startPosition;
         protentialRoomPositions.Add(currentPosition);
        
        
        for(int i = 0;i<corridorCount;i++)
        {
            var corridor = ProceduralGenerationAlogorithms.RandomWalkCorridor(currentPosition,corridorLength);
            currentPosition = corridor[corridor.Count-1];
            protentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);

        }
    }
}
