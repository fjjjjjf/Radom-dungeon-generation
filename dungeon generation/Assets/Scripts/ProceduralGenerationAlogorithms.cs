using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using Random = UnityEngine.Random;
public static class ProceduralGenerationAlogorithms
{

    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(startPosition);
        var previousposition = startPosition;

        for (int i = 0; i < walkLength; i++)
        {
            var newPosition = previousposition + Direction2D.GetRandomCardinalDirection();
            path.Add(newPosition);
            previousposition = newPosition;

        }
        return path;
    }

    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int corridorLength)
    {
        List<Vector2Int> corrider = new List<Vector2Int>();
        var direction = Direction2D.GetRandomCardinalDirection();

        var currentPosition = startPosition;

        corrider.Add(currentPosition);
        for (int i = 0; i < corridorLength; i++)
        {
            currentPosition += direction;
            corrider.Add(currentPosition);
        }
        return corrider;
    }


    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSpilt, int minWidth, int minHeight)
    {
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomList = new List<BoundsInt>();
        roomsQueue.Enqueue(spaceToSpilt);

        //切分房间
        while (roomsQueue.Count > 0)
        {
            var room = roomsQueue.Dequeue();
            if (room.size.y >= minHeight && room.size.x >= minWidth)
            {
                if (Random.value < 0.5f)
                {
                    if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minWidth, minHeight, roomsQueue, room);
                    }
                    else if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, minHeight, roomsQueue, room);
                    }
                    else if (room.size.y >= minHeight && room.size.x >= minWidth)
                    {
                        roomList.Add(room);
                    }
                }
                else
                {
                    if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, minHeight, roomsQueue, room);
                    }
                    else if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minWidth, minHeight, roomsQueue, room);

                    }
                    else if (room.size.y >= minHeight && room.size.x >= minWidth)
                    {
                        roomList.Add(room);
                    }
                }
            }
        }

        return roomList;
    }

    private static void SplitVertically(int minWidth, int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        var xSpilt = Random.Range(1, room.size.x);//沿x轴划分成两个房间
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSpilt, room.size.y, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + xSpilt, room.min.y, room.min.z), new Vector3Int(room.size.x - xSpilt, room.size.y, room.size.z));

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);

    }

    private static void SplitHorizontally(int minWidth, int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        var ySpilt = Random.Range(1, room.size.y);//沿y轴划分成两个房间 ( minHeight,room.size.y-minHeight)
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.min.x, ySpilt, room.min.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySpilt, room.min.z),
            new Vector3Int(room.size.x, room.size.y - ySpilt, room.size.z));

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }
}
public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0,1),//UP
        new Vector2Int(1,0),//RIGHT
        new Vector2Int(0,-1),//DOWN
        new Vector2Int(-1,0),//LEFT
    };
    public static List<Vector2Int> diagonalDirectionsList = new List<Vector2Int>
    {// 判断对角线是否有tile
        new Vector2Int(1,1),//UP-RIGHT
        new Vector2Int(1,-1),//RIGHT-DOWN
        new Vector2Int(-1,-1),//DOWN-LEFT
        new Vector2Int(-1,1),//LEFT-UP
    };
    public static List<Vector2Int> eightDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0,1),//UP
        new Vector2Int(1,1),//UP-RIGHT
        new Vector2Int(1,0),//RIGHT
        new Vector2Int(1,-1),//RIGHT-DOWN
        new Vector2Int(0,-1),//DOWN
        new Vector2Int(-1,-1),//DOWN-LEFT
        new Vector2Int(-1,0),//LEFT
        new Vector2Int(-1,1),//LEFT-UP
    };
    public static Vector2Int GetRandomCardinalDirection()
    {
        return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
    }
}