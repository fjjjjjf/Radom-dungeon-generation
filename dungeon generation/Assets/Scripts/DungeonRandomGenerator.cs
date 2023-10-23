using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonRandomGenerator : AbstractDungeonGenerator
{
    //[SerializeField]
    //protected Vector2Int startPosition = Vector2Int.zero;
    [SerializeField]
    private SimpleRandomWalkSO randomWalkSO;



    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(randomWalkSO);
       
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPositions);
       
        WallGenerator.CreateWalls(floorPositions,tilemapVisualizer);

    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters)
    {
        var currentPosition = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < parameters.iterations; i++)
        {
            var path = ProceduralGenerationAlogorithms.SimpleRandomWalk(currentPosition, parameters.walkLength);
            floorPositions.UnionWith(path);
            if (parameters.startRandomlyEachIteration)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }

        }
        return floorPositions;
    }

 
}



