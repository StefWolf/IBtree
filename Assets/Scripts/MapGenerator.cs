using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public int SmoothNumber;
    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)]
    public int randomFillPercent;
    int[,] map;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    public struct Coord
    {
        public int tileX;
        public int tileY;

        public Coord(int x, int y)
        {
            tileX = x;
            tileY = y;
        }
    }

    void ProcessMap()
    {
        List<List<Coord>> wallRegions = GetRegions(1);

        int MinSizeForWallRegion = 50;
        foreach (List<Coord> wallRegion in wallRegions)
        {
            if (wallRegion.Count < MinSizeForWallRegion)
            {
                foreach (Coord tile in wallRegion)
                {
                    map[tile.tileX, tile.tileY] = 0;
                }
            }
        }
    }

    List<Coord> GetRegionTiles(int startX, int startY)
    {
        List<Coord> tiles = new List<Coord>();
        int[,] mapFlags = new int[width, height];

        int tileType = map[startX, startY];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));

        mapFlags[startX, startY] = 1;
        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            tiles.Add(tile);
            for (int ii = tile.tileX - 1; ii <= tile.tileX + 1; ii++)
            {
                for (int jj = tile.tileY - 1; jj <= tile.tileY + 1; jj++)
                {
                    if (IsInMapRange(ii, jj) && (jj == tile.tileY || ii == tile.tileX))
                    {
                        if (mapFlags[ii, jj] == 0 && map[ii, jj] == tileType)
                        {
                            mapFlags[ii, jj] = 1;
                            queue.Enqueue(new Coord(ii, jj));
                        }
                    }
                }
            }
        }
        return tiles;
    }

    List<List<Coord>> GetRegions(int tileType)
    {
        List<List<Coord>> regions = new List<List<Coord>>();
        int[,] mapFlags = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapFlags[x, y] == 0 && map[x, y] == tileType)
                {
                    List<Coord> newRegion = GetRegionTiles(x, y);
                    regions.Add(newRegion);

                    foreach (Coord coord in newRegion)
                    {
                        mapFlags[coord.tileX, coord.tileY] = 1;
                    }
                }
            }
        }
        return regions;
    }
   

    bool IsInMapRange(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        for (int ii = 0; ii < SmoothNumber; ii++)
            SmoothMap();

        ProcessMap();

        int borderSize = 5;
        int[,] borderMap = new int[width + borderSize * 2, height + borderSize * 2];

        for (int ii = 0; ii < borderMap.GetLength(0); ii++)
            for (int jj = 0; jj < borderMap.GetLength(1); jj++)
                if (ii >= borderSize && ii < width + borderSize && jj >= borderSize && jj < height + borderSize)
                    borderMap[ii, jj] = map[ii - borderSize, jj - borderSize];
                else
                    borderMap[ii, jj] = 1;

        GetComponent<MeshGenerator>().GenerateMesh(borderMap, 1);
    }

    void RandomFillMap()
    {
        if (useRandomSeed)
            seed = Time.time.ToString();

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap()
    {
        for (int ii = 0; ii < width; ii++)
            for (int jj = 0; jj < height; jj++)
            {
                int neighboutWallTiles = GetSurroundWallCount(ii, jj);

                if (neighboutWallTiles > 4)
                    map[ii, jj] = 1;
                else if (neighboutWallTiles < 4)
                    map[ii, jj] = 0;
            }
    }

    int GetSurroundWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (IsInMapRange(neighbourX, neighbourY))
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

}
