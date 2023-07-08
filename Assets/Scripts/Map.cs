using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct CaveEntry{
    public Vector2Int caveEntryPosition;
    public GameObject prefabCaveEntry;
}
public class Map 
{
    int mapWidth, mapHeight;    
    Vector2Int seed;
    public List<CaveEntry> caveEntries = new List<CaveEntry>();
    MapGenerator.Tileset tileset;
    int[,] elevationGrid;
    char[,] objectGrid;
    public Map(Vector2Int  _mapSize, Vector2Int _seed, MapGenerator.Tileset _tileset)
    {
        mapWidth = _mapSize.x;
        mapHeight = _mapSize.y;
        seed = _seed;
        tileset = _tileset;
        elevationGrid = new int[mapWidth, mapHeight];
        objectGrid = new char[mapWidth, mapHeight];

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                objectGrid[x,y] = 'e';
            }
        }
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                elevationGrid[x,y] = 0;
            }
        }
    }
    public int GetWidth()
    {
        return mapWidth;
    }

    public int GetHeight()
    {
        return mapHeight;
    }
    public Vector2Int GetSeed()
    {
        return seed;
    }
    public int GetElevation(int x, int y)
    {
        if(x < mapWidth &&  y < mapHeight)
            return elevationGrid[x, y];
        else
            return -1;
    }
    public void SetElevation(int x, int y, int elevation)
    {
        if(x < mapWidth &&  y < mapHeight)
        {
            elevationGrid[x,y] = elevation;
        }
    }

    public char GetObeject(int x, int y)
    {
        if(x < mapWidth &&  y < mapHeight)
            return objectGrid[x, y];
        else
            return ' ';
    }
    public void SetObject(int x, int y, char symbol)
    {
        if(x < mapWidth &&  y < mapHeight)
            objectGrid[x,y] = symbol; 
    }

    public void BuildTerrain()
    {
        for (int x = 0; x < mapWidth - 1; x++)
        {
            for (int y = 0; y < mapHeight - 1; y++)
            {
                int codigoTile = elevationGrid[x, y] * 1000 + elevationGrid[x + 1, y] * 100 + elevationGrid[x, y + 1] * 10 + elevationGrid[x + 1, y + 1];
                switch (codigoTile)
                {
                case 0000:
                    PlaceTile(tileset.tile_bg0000, x, y, tileset.tilemap);
                    break;

                case 1000: //3 de agua 1 de terra
                    PlaceAnimatedTile(tileset.tile_bg1000, x, y, tileset.tilemap);
                    break;
                case 0100:
                    PlaceAnimatedTile(tileset.tile_bg0100, x, y, tileset.tilemap);
                    break;
                case 0010:
                    PlaceAnimatedTile(tileset.tile_bg0010, x, y, tileset.tilemap);
                    break;
                case 0001:
                    PlaceAnimatedTile(tileset.tile_bg0001, x, y, tileset.tilemap);
                    break;
                case 1100: //2 de agua 2 de terra
                    PlaceAnimatedTile(tileset.tile_bg1100, x, y, tileset.tilemap);
                    break;
                case 1010:
                    PlaceAnimatedTile(tileset.tile_bg1010, x, y, tileset.tilemap);
                    break;
                case 0011:
                    PlaceAnimatedTile(tileset.tile_bg0011, x, y, tileset.tilemap);
                    break;
                case 0101:
                    PlaceAnimatedTile(tileset.tile_bg0101, x, y, tileset.tilemap);
                    break;
                case 1110: //1 de agua 3 de terra
                    PlaceAnimatedTile(tileset.tile_bg1110, x, y, tileset.tilemap);
                    break;
                case 1101:
                    PlaceAnimatedTile(tileset.tile_bg1101, x, y, tileset.tilemap);
                    break;
                case 1011:
                    PlaceAnimatedTile(tileset.tile_bg1011, x, y, tileset.tilemap);
                    break;
                case 0111:
                    PlaceAnimatedTile(tileset.tile_bg0111, x, y, tileset.tilemap);
                    break;
                case 1111:
                    PlaceTile(tileset.tile_bg1111, x, y, tileset.tilemap);
                    break;
                case 2111:
                    PlaceTile(tileset.tile_bg2111, x, y, tileset.tilemap);
                    break;
                case 1211:
                    PlaceTile(tileset.tile_bg1211, x, y, tileset.tilemap);
                    break;
                case 1121:
                    PlaceTile(tileset.tile_bg1121, x, y, tileset.tilemap);
                    break;
                case 1112:
                    PlaceTile(tileset.tile_bg1112, x, y, tileset.tilemap);
                    break;
                case 2211:
                    PlaceTile(tileset.tile_bg2211, x, y, tileset.tilemap);
                    break;
                case 2121:
                    PlaceTile(tileset.tile_bg2121, x, y, tileset.tilemap);
                    break;
                case 1122:
                    PlaceTile(tileset.tile_bg1122, x, y, tileset.tilemap);
                    break;
                case 1212:
                    PlaceTile(tileset.tile_bg1212, x, y, tileset.tilemap);
                    break;
                case 2221:
                    PlaceTile(tileset.tile_bg2221, x, y, tileset.tilemap);
                    break;
                case 2212:
                    PlaceTile(tileset.tile_bg2212, x, y, tileset.tilemap);
                    break;
                case 2122:
                    PlaceTile(tileset.tile_bg2122, x, y, tileset.tilemap);
                    break;
                case 1222:
                    PlaceTile(tileset.tile_bg1222, x, y, tileset.tilemap);
                    break;
                case 2222:
                    PlaceTile(tileset.tile_bg2222, x, y, tileset.tilemap);
                    break;
                case 3222:
                    PlaceTile(tileset.tile_bg2111, x, y, tileset.tilemap);
                    break;
                case 2322:
                    PlaceTile(tileset.tile_bg1211, x, y, tileset.tilemap);
                    break;
                case 2232:
                    PlaceTile(tileset.tile_bg1121, x, y, tileset.tilemap);
                    break;
                case 2223:
                    PlaceTile(tileset.tile_bg1112, x, y, tileset.tilemap);
                    break;
                case 3322:
                    PlaceTile(tileset.tile_bg2211, x, y, tileset.tilemap);
                    break;
                case 3232:
                    PlaceTile(tileset.tile_bg2121, x, y, tileset.tilemap);
                    break;
                case 2233:
                    PlaceTile(tileset.tile_bg1122, x, y, tileset.tilemap);
                    break;
                case 2323:
                    PlaceTile(tileset.tile_bg1212, x, y, tileset.tilemap);
                    break;
                case 3332:
                    PlaceTile(tileset.tile_bg2221, x, y, tileset.tilemap);
                    break;
                case 3323:
                    PlaceTile(tileset.tile_bg2212, x, y, tileset.tilemap);
                    break;
                case 3233:
                    PlaceTile(tileset.tile_bg2122, x, y, tileset.tilemap);
                    break;
                case 2333:
                    PlaceTile(tileset.tile_bg1222, x, y, tileset.tilemap);
                    break;
                case 3333:
                    PlaceTile(tileset.tile_bg2222, x, y, tileset.tilemap);
                    break;
                }
            }
        }
    }

    private void PlaceTile(Tile tile, int x, int y, Tilemap tilemap)
    {
        if (tile != null)
        {
            Vector3Int position = new Vector3Int(x + seed.x, y + seed.y,0);
            tilemap.SetTile(position, tile);
        }
    }
    private void PlaceAnimatedTile(AnimatedTile tile, int x, int y, Tilemap tilemap)
    {
        if (tile != null)
        {
            Vector3Int position = new Vector3Int(x + seed.x, y + seed.y,0);
            tilemap.SetTile(position, tile);
        }
    }
}
