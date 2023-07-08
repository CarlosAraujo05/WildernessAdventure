using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class MapGenerator : MonoBehaviour
{
    [Serializable]
    public struct Tileset
    {
        public Tilemap tilemap;
        public Tile tile_bg0000;
        public AnimatedTile tile_bg1000;
        public AnimatedTile tile_bg0100;
        public AnimatedTile tile_bg0010;
        public AnimatedTile tile_bg0001;
        public AnimatedTile tile_bg1100;
        public AnimatedTile tile_bg1010;
        public AnimatedTile tile_bg0011;
        public AnimatedTile tile_bg0101;
        public AnimatedTile tile_bg1110;
        public AnimatedTile tile_bg1101;
        public AnimatedTile tile_bg1011;
        public AnimatedTile tile_bg0111;
        public Tile tile_bg1111;
        public Tile tile_bg2111;
        public Tile tile_bg1211;
        public Tile tile_bg1121;
        public Tile tile_bg1112;
        public Tile tile_bg2211;
        public Tile tile_bg2121;
        public Tile tile_bg1122;
        public Tile tile_bg1212;
        public Tile tile_bg2221;
        public Tile tile_bg2212;
        public Tile tile_bg2122;
        public Tile tile_bg1222;
        public Tile tile_bg2222;
    }

    [SerializeField]
    int numberOfCaves = 1;
    [SerializeField]
    Vector2Int mapSize = new Vector2Int(100, 100), mapSeed, caveSize = new Vector2Int(100, 100), caveSeed;

    [SerializeField]
    GameObject prefabTree, prefabWall, prefabRock, prefabCoal, prefabGold, prefabDiamond,
    prefabRockForest, prebfabBigRock,prefabBush1, prefabBush2,prefabCaveEntryForest, prefabCaveEntryCave, prefabGolem, prefabGoblin;

    [SerializeField]
    Tileset forestTileset, caveTileset;
    GameObject caveEntriesGroup, treesGroup, wallsGroup, oresGroup, enemiesGroup;
    void Start()
    {
        List<Map> caves = new List<Map>();
        caveEntriesGroup = CreateGroup("Cave Entries");
        treesGroup = CreateGroup("Trees and Bushes");
        wallsGroup = CreateGroup("Insvisible walls");
        oresGroup = CreateGroup("Rocks and Ores");
        enemiesGroup = CreateGroup("Enemies");
        Map map = GenerateMap(mapSize, mapSeed, forestTileset, numberOfCaves);
        for (int i= 0; i < map.caveEntries.Count;i++){
            Map cave = GenerateCave(caveSize, new Vector2Int(caveSeed.x*i,caveSeed.y), caveTileset);
            SetTeleporters(map.caveEntries[i],cave.caveEntries[0]);
            caves.Add(cave);
        }
        SpawnPlayer(map);
    }

    Map GenerateMap(Vector2Int mapSize, Vector2Int mapSeed, Tileset tileset, int numCaves)
    {
        Map map = new Map(mapSize, mapSeed, tileset);
        Vector2Int noiseSeed = new Vector2Int(GetRandom(map.GetWidth(), map.GetWidth() + 10000), GetRandom(map.GetHeight(), map.GetHeight() + 10000));
        for (int x = 0; x < map.GetWidth(); x++)
        {
            for (int y = 0; y < map.GetHeight(); y++)
            {
                if (x <= 10 || x >= map.GetWidth() - 10 || y <= 3 || y >= map.GetHeight() - 4)
                    map.SetElevation(x, y, 3);
                else if (x <= 16 || x >= map.GetWidth() - 16 || y <= 7 || y >= map.GetHeight() - 8)
                    map.SetElevation(x, y, 2);
                else
                {
                    map.SetElevation(x, y, GetElevationUsingPerlin(x, y, noiseSeed));
                    if (map.GetElevation(x, y) == 0 && (x == 17 || x == map.GetWidth() - 17 || y == 8 || y == map.GetHeight() - 9))
                        map.SetElevation(x, y, 1);
                }
            }
        }
        for(int i =0; i<numCaves; i++){
            CreateCaveEntry(map, GetRandom(8, map.GetHeight()- 8), prefabCaveEntryForest);
        }
        CleanEmptySpaces(map);
        map.BuildTerrain();
        CreateInvisibleWalls(map);
        CreateTrees(map, prefabTree, 0.3f);
        CreateBigRocks(map, prebfabBigRock, 25);
        CreateSmallObjects(map, prefabRockForest,oresGroup, 'r', 60);
        CreateSmallObjects(map, prefabBush1, treesGroup,'b', 35);
        CreateSmallObjects(map, prefabBush2, treesGroup,'b', 35);
        CreateEnemies(map, prefabGoblin, enemiesGroup, 'g', 25);
        return map;
    }
    Map GenerateCave(Vector2Int mapSize, Vector2Int mapSeed, Tileset tileset)
    {
        Map map = new Map(mapSize, mapSeed, tileset);
        for(int x = 0; x < map.GetWidth(); x++){
            for(int y = 0; y < map.GetHeight(); y++){
                map.SetElevation(x,y, 2);
            }
        }
        Vector2Int walkerPosition = new Vector2Int(GetRandom(map.GetWidth()*9/20, map.GetWidth()*11/20 - 2), GetRandom(map.GetWidth()*9/20, map.GetWidth()*11/20 - 2));
        Vector2Int initialWalkerPosition = walkerPosition;
        int floorsToPlace = Mathf.FloorToInt(map.GetWidth() * map.GetHeight()* 0.1f); 
        while(floorsToPlace > 0)
        {
            walkerPosition = MoveWalker(walkerPosition, 1, 1, map.GetWidth() - 2, map.GetHeight() - 2);
            if (map.GetElevation(walkerPosition.x, walkerPosition.y) == 2)
            {
                map.SetElevation(walkerPosition.x, walkerPosition.y, 1);
                floorsToPlace--;
            }
        }
        CreateCaveEntry(map, initialWalkerPosition.y +1, prefabCaveEntryCave);
        CleanEmptySpaces(map);
        CreateInvisibleWalls(map);
        map.BuildTerrain();
        CreateRocksAndOres(map, prefabRock, prefabCoal, prefabGold,prefabDiamond);
        CreateEnemies(map, prefabGolem, enemiesGroup, 'g',5);
        return map;
    }
    void CreateCaveEntry(Map map, int y ,GameObject prefabCaveEntry)
    {
        List<Vector2Int> possiblePositions = GetPossiblePositions(map, y);
        Vector2Int position = ChooseCaveEntryPosition(map, possiblePositions);
        PrepareCaveEntryPosition(map, position);
        SpawnCaveEntry( prefabCaveEntry, position, map);
    }
    void SpawnCaveEntry(GameObject prefab, Vector2Int position, Map map){
        GameObject newCaveEntry = Instantiate(prefab, caveEntriesGroup.transform);
        newCaveEntry.name = string.Format("CaveEntry[{0}, {1}]", position.x, position.y);
        newCaveEntry.transform.localPosition = new Vector3(position.x + map.GetSeed().x, position.y + map.GetSeed().y, 0);
        map.SetObject(position.x,position.y ,'c');
        CaveEntry caveEntry;
        caveEntry.caveEntryPosition = position + map.GetSeed();
        caveEntry.prefabCaveEntry = newCaveEntry;
        map.caveEntries.Add(caveEntry);
    }

    List<Vector2Int> GetPossiblePositions(Map map, int y)
    {
        List<Vector2Int> possiblePositions = new List<Vector2Int>();
        for(int x = 16; x <map.GetWidth()-16; x++)
        {
            if (map.GetElevation(x, y -1) == 1 
            &&map.GetElevation(x-1, y -1) == 1
            &&map.GetElevation(x+1, y -1) == 1 
            && !HasCave(map, x, y))
            {
                possiblePositions.Add(new Vector2Int(x,y));
            }
        }
        if (possiblePositions.Count > 0)
        {
            return possiblePositions; 
        }
        else 
            y = GetRandom(0, map.GetHeight()- 8);
            return GetPossiblePositions(map, y);
    }

    bool HasCave(Map map, int x, int y){
        for(int i = x-5; i < y + 5; i++){
            for(int j = y-5; j < y + 5; j++){
                if (map.GetObeject(i, j) == 'c'){
                    return true;
                }
            }
        }
        return false;
    }
    Vector2Int ChooseCaveEntryPosition(Map map, List<Vector2Int>possiblePositions)
    {
        Vector2Int positionChoosed = possiblePositions[GetRandom(0, possiblePositions.Count)];
        return positionChoosed;
        
    }
    void PrepareCaveEntryPosition(Map map, Vector2Int position)
    {
        map.SetObject(position.x, position.y, 'c');
        map.SetObject(position.x-1, position.y, 'c');
        map.SetObject(position.x+1, position.y, 'c');
        map.SetObject(position.x, position.y+1, 'c');
        map.SetObject(position.x-1, position.y+1, 'c');
        map.SetObject(position.x+1, position.y+1, 'c');
        map.SetElevation(position.x, position.y, 1);
        for( int x = -1; x <= 1; x++){
            for( int y= 1; y <= 2; y++){
                map.SetElevation(position.x+x, position.y +y, 3);
            }
        }
        for(int x = -2; x <= 2; x++){
            map.SetElevation(position.x+x, position.y-1, 1);
        }
        for( int x = -2; x <= 2; x++){
            for( int y= 0; y <= 3; y++){
                if (!(x== 0 && y==0) && map.GetElevation(position.x+x, position.y +y) < 2){
                    map.SetElevation(position.x+x, position.y +y, 2);
                }      
            }
        }
        for( int x = -3; x <= 3; x++){
            for(int y =-1; y <= 4; y++){
                if (map.GetElevation(position.x+x, position.y +y) < 1)
                    map.SetElevation(position.x+x, position.y +y, 1);
            }
        }
        if (map.GetElevation(position.x-3, position.y) ==1 && map.GetElevation(position.x-3, position.y-1) ==2){
            map.SetElevation(position.x-3, position.y, 1);
            map.SetElevation(position.x-3, position.y -1, 1);
        }
        if (map.GetElevation(position.x+3, position.y) ==1 && map.GetElevation(position.x+3, position.y-1) ==2){
            map.SetElevation(position.x+3, position.y, 1);
            map.SetElevation(position.x+3, position.y -1, 1);
        }
        if (map.GetElevation(position.x-3, position.y+4) ==2 && map.GetElevation(position.x-3, position.y+3) ==1){
            map.SetElevation(position.x-3, position.y +4, 1);
            map.SetElevation(position.x-3, position.y +3, 1);
        }
        if (map.GetElevation(position.x+3, position.y +4) ==2 && map.GetElevation(position.x+3, position.y+3) ==1){
            map.SetElevation(position.x+3, position.y +4, 1);
            map.SetElevation(position.x+3, position.y +3, 1);
        }

        if (map.GetElevation(position.x-3, position.y) == 3 && map.GetElevation(position.x-3, position.y-1) ==2){
            map.SetElevation(position.x-3, position.y -1, 2);
        }
        if (map.GetElevation(position.x+3, position.y) ==3 && map.GetElevation(position.x+3, position.y-1) ==2){
            map.SetElevation(position.x+3, position.y -1, 2);
        }
        if (map.GetElevation(position.x-3, position.y+4) ==2 && map.GetElevation(position.x-3, position.y+3) ==3){
            map.SetElevation(position.x-3, position.y +4, 2);
        }
        if (map.GetElevation(position.x+3, position.y +4) ==2 && map.GetElevation(position.x+3, position.y+3) ==3){
            map.SetElevation(position.x+3, position.y +4, 2);
        }
    }


    void CleanEmptySpaces(Map map)
    {
        for (int x = 0; x < map.GetWidth() - 1; x++)
        {
            for (int y = 0; y < map.GetHeight() - 1; y++)
            {
                int codigoTile = map.GetElevation(x, y) * 1000 + map.GetElevation(x + 1, y) * 100 + map.GetElevation(x, y + 1) * 10 + map.GetElevation(x + 1, y + 1);
                if (codigoTile == 0110 || codigoTile == 1001)
                {
                    map.SetElevation(x, y, 1);
                    map.SetElevation(x + 1, y, 1);
                    map.SetElevation(x, y + 1, 1);
                    map.SetElevation(x + 1, y + 1, 1);
                }
                else if (codigoTile == 1221 || codigoTile == 2112)
                {
                    map.SetElevation(x, y, 1);
                    map.SetElevation(x + 1, y, 1);
                    map.SetElevation(x, y + 1, 1);
                    map.SetElevation(x + 1, y + 1, 1);
                }
                else if (codigoTile == 2332 || codigoTile == 3223)
                {
                    map.SetElevation(x, y, 2);
                    map.SetElevation(x + 1, y, 2);
                    map.SetElevation(x, y + 1, 2);
                    map.SetElevation(x + 1, y + 1, 2);
                }
            }
        }
    }

    

    int GetElevationUsingPerlin(int x, int y, Vector2Int noiseSeed)
    {
        float magnification = 8.5f;
        float rawPerlin = Mathf.PerlinNoise(
            (x - noiseSeed.x) / magnification,
            (y - noiseSeed.y) / magnification
        );
        float clampPerlin = Mathf.Clamp01(rawPerlin); 
        float scaledPerlin = clampPerlin * 4;

        if (scaledPerlin == 4)
        {
            scaledPerlin = (4 - 1);
        }
        else if (scaledPerlin > 0.75 && scaledPerlin <= 1)
        {
            scaledPerlin = 1;
        }
        return Mathf.FloorToInt(scaledPerlin);
    }

    Vector2Int MoveWalker(Vector2Int currentPosition, int xMin, int yMin, int xMax, int yMax)
    {
        switch(UnityEngine.Random.Range(0, 4))
        {
            case 0: //Cima
                if (currentPosition.y + 1 < yMax)
                    return currentPosition + new Vector2Int(0, 1);
                else
                    return MoveWalker(currentPosition, xMin, yMin, xMax, yMax);
            case 1: //Direita
                if (currentPosition.x + 1 < xMax)
                    return currentPosition + new Vector2Int(1, 0);
                else
                    return MoveWalker(currentPosition, xMin, yMin, xMax, yMax);
            case 2: //Baixo
                if (currentPosition.y - 1 > yMin)
                    return currentPosition + new Vector2Int(0, -1);
                else
                    return MoveWalker(currentPosition, xMin, yMin, xMax, yMax);
            case 3: //Esquerda
                if (currentPosition.x - 1 > xMin)
                    return currentPosition + new Vector2Int(-1, 0);
                else
                    return MoveWalker(currentPosition, xMin, yMin, xMax, yMax);

        }
        return currentPosition;
    }
    int GetRandom(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }
    void CreateInvisibleWalls(Map map)
    {
        for (int x = 1; x < map.GetWidth() - 1; x++)
        {
            for (int y = 1; y < map.GetHeight() - 1; y++)
            {
                if (y == map.GetHeight() - 1)
                    SpawnObject("Invisable Wall", prefabWall, x, y, wallsGroup, map);
                else if (map.GetElevation(x, y) != 1 && (map.GetElevation(x - 1, y) == 1 || map.GetElevation(x + 1, y) == 1 || map.GetElevation(x, y - 1) == 1 || map.GetElevation(x, y + 1) == 1))
                    SpawnObject("Invisable Wall", prefabWall, x, y, wallsGroup, map);
            }
        }
    }

    void CreateTrees(Map map, GameObject prefabTree, float treeRatio)
    {
        bool hasatree;
        Vector2Int noiseSeed = new Vector2Int(GetRandom(map.GetWidth(), map.GetWidth() + 10000), GetRandom(map.GetHeight(), map.GetHeight() + 10000));
        for (int x = 0; x < map.GetWidth(); x++)
        {
            for (int y = 0; y < map.GetHeight(); y++)
            {
                hasatree = PerlinTree(x, y, treeRatio, noiseSeed);
                if (hasatree && map.GetElevation(x, y) != 0 && map.GetObeject(x, y) == 'e')
                {
                    SpawnObject("Tree", prefabTree, x, y, treesGroup, map);
                    map.SetObject(x, y, 't');
                }
            }
        }
    }
    void CreateBigRocks(Map map, GameObject prefabBigRock, int rocksToPlace){
        while(rocksToPlace > 0){
            Vector2Int position = new Vector2Int(GetRandom(1, map.GetWidth()),GetRandom(1, map.GetHeight()));
            if (map.GetElevation(position.x, position.y) == 1&& map.GetElevation(position.x+1, position.y) == 1 && map.GetObeject(position.x, position.y)== 'e'&& map.GetObeject(position.x + 1, position.y) == 'e'){
                SpawnObject("BigRock", prefabBigRock, position.x, position.y, oresGroup, map);
                map.SetObject(position.x, position.y,'R');
                rocksToPlace--;   
            }
        }
    }
    void CreateSmallObjects(Map map, GameObject prefabObject,GameObject ObjectGroup, char symbol, int ObjectsToPLace){
        while(ObjectsToPLace > 0){
            Vector2Int position = new Vector2Int(GetRandom(1, map.GetWidth()),GetRandom(1, map.GetHeight()));
            if (map.GetElevation(position.x, position.y) > 0 && map.GetObeject(position.x, position.y)== 'e'){
                SpawnObject("BigRock", prefabObject, position.x, position.y, ObjectGroup, map);
                map.SetObject(position.x, position.y, symbol);
                ObjectsToPLace--;   
            }
        }
    }
    void CreateEnemies(Map map, GameObject prefabObject,GameObject ObjectGroup, char symbol, int ObjectsToPLace){
        while(ObjectsToPLace > 0){
            Vector2Int position = new Vector2Int(GetRandom(1, map.GetWidth()),GetRandom(1, map.GetHeight()));
            if (map.GetElevation(position.x, position.y) == 1 && map.GetObeject(position.x, position.y)== 'e'){
                SpawnEnemy("Enemy", prefabObject, position.x, position.y, ObjectGroup, map);
                map.SetObject(position.x, position.y, symbol);
                ObjectsToPLace--;   
            }
        }
    }

    void CreateRocksAndOres(Map map, GameObject prefabRock,GameObject prefabCoal,GameObject prefabGold,GameObject prefabDiamond)
    {
        int oresToPlace = Mathf.FloorToInt(map.GetWidth() * map.GetHeight()* 0.01f);
        while(oresToPlace > 0)
        {
            Vector2Int position = new Vector2Int(GetRandom(1, map.GetWidth()),GetRandom(1, map.GetHeight()));
            if (map.GetElevation(position.x, position.y) == 1 && map.GetObeject(position.x, position.y) == 'e')
            {
                int oreValue = GetRandom(0, 100);
                if (oreValue < 60)
                {
                    SpawnObject("Rock", prefabRock, position.x, position.y, oresGroup , map);
                    map.SetObject(position.x, position.y, 'r');
                }
                else if (oreValue > 60 && oreValue <= 85) 
                {
                    SpawnObject("Coal", prefabCoal, position.x, position.y, oresGroup , map);
                    map.SetObject(position.x, position.y, 'c');
                }
                else if (oreValue > 85 && oreValue <= 95) 
                {
                    SpawnObject("Gold", prefabGold, position.x, position.y, oresGroup , map);
                    map.SetObject(position.x, position.y, 'g');
                }
                else if (oreValue > 95) 
                {
                    SpawnObject("Diamond", prefabDiamond, position.x, position.y, oresGroup , map);
                    map.SetObject(position.x, position.y, 'd');
                }
                oresToPlace--;
            }
        }
    }

    bool PerlinTree(int x, int y, float treeratio, Vector2Int noiseSeed)
    {
        float raw_perlin = Mathf.PerlinNoise(
            (x - noiseSeed.x) / 4f,
            (y - noiseSeed.y) / 4f
        );
        float clamp_perlin = Mathf.Clamp01(raw_perlin);
        if (clamp_perlin <= treeratio)
            return true;
        else
            return false;
    }

    void SpawnObject(string name, GameObject prefab, int x, int y, GameObject group, Map map)
    {
        GameObject newObject = Instantiate(prefab, group.transform);

        newObject.name = string.Format("{0}[{1}, {2}]", name, x, y);
        newObject.transform.localPosition = new Vector3(x + map.GetSeed().x, y + map.GetSeed().y, 0);
    }
    void SpawnEnemy(string name, GameObject prefab, int x, int y, GameObject group, Map map)
    {
        GameObject newObject = Instantiate(prefab, group.transform);

        newObject.name = string.Format("{0}[{1}, {2}]", name, x, y);
        newObject.transform.localPosition = new Vector3(x + map.GetSeed().x, y + map.GetSeed().y -.5f, 0);
        newObject.GetComponent<EnemyController>().movePointPos = new Vector3(x + map.GetSeed().x, y + map.GetSeed().y -.5f, 0);
    }   

    void SpawnPlayer(Map map)
    {
        int playerSeedX = GetRandom(1, map.GetWidth() - 1);
        int playerSeedY = GetRandom(1, map.GetHeight() - 1);
        if (map.GetElevation(playerSeedX, playerSeedY) == 1 && map.GetObeject(playerSeedX, playerSeedY) == 'e')
        {
            Transform playerPosition = GameObject.FindWithTag("Player").GetComponent<Transform>();
            Transform movePoint = GameObject.FindWithTag("Move Point").GetComponent<Transform>();
            playerPosition.position = new Vector3(playerSeedX + map.GetSeed().x, playerSeedY + map.GetSeed().y, 0);
            movePoint.position = playerPosition.position;
        }
        else
            SpawnPlayer(map);
    }

    GameObject CreateGroup(string groupName)
    {
        GameObject newGroup = new GameObject(groupName);
        newGroup.transform.parent = gameObject.transform;
        newGroup.transform.localPosition = new Vector3(0, 0, 0);
        return newGroup;
    }
    void SetTeleporters(CaveEntry caveEntry1, CaveEntry caveEntry2){
        caveEntry1.prefabCaveEntry.GetComponent<Teleporter>().destination = caveEntry2.caveEntryPosition + new Vector2Int(0, -1);
        caveEntry2.prefabCaveEntry.GetComponent<Teleporter>().destination = caveEntry1.caveEntryPosition + new Vector2Int(0, -1);
    }
    
}