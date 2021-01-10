using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BuildingGenerator
{
    static List<List<int>> maxIsland;
    static int maxIslandSize;

    public static Building Generate(LoadImages loadImages, float x, float z, int cluster) {
        
        Vector3 loc = new Vector3(x, 0f, z);

        int sizea = UnityEngine.Random.Range(6,11);
        int sizeb = UnityEngine.Random.Range(6,11);
        Vector2Int size = new Vector2Int(sizea, sizeb);

        int[] imageAmounts = loadImages.ImageAmounts;
        int imageAmount = imageAmounts[cluster];

        int imageSpace = 0;

        int floor = 0;
        List<Floor> floors = new List<Floor>();

        List<int> previousRandomBlock = new List<int>();

        while(imageSpace < imageAmount) {

            int[,] randomFloorLayout = GenerateRandomFloorLayout(size);
            randomFloorLayout = RemoveRedundantIslands(randomFloorLayout, size);           

            List<int> currentRandomBlock = maxIsland[UnityEngine.Random.Range(0,maxIslandSize)];


            if(floor == 0) {

                randomFloorLayout[currentRandomBlock[1], currentRandomBlock[0]] = 2;
                previousRandomBlock = currentRandomBlock;

            }
            else {

                while(randomFloorLayout[previousRandomBlock[1], previousRandomBlock[0]] == 0) {
                    
                    randomFloorLayout = GenerateRandomFloorLayout(size);
                    randomFloorLayout = RemoveRedundantIslands(randomFloorLayout, size); 
                    currentRandomBlock = maxIsland[UnityEngine.Random.Range(0,maxIslandSize)];    
                    
                }

                while(currentRandomBlock[1] == previousRandomBlock[1] && currentRandomBlock[0] == currentRandomBlock[0]){
                    currentRandomBlock = maxIsland[UnityEngine.Random.Range(0,maxIslandSize)];
                }

                randomFloorLayout[currentRandomBlock[1], currentRandomBlock[0]] = 2;
                randomFloorLayout[previousRandomBlock[1], previousRandomBlock[0]] = 3;

                previousRandomBlock = currentRandomBlock;

            }

            Floor currentFloor = new Floor(randomFloorLayout, size, floor);

            imageSpace += currentFloor.GetNumOfImages();

            floors.Add(currentFloor);
            //Debug.Log("floor " + floor);
            floor++;
        }

        return new Building(loc, size, floors);
    }


    public static int[,] GenerateRandomFloorLayout(Vector2Int size) {

        int[,] floorLayout = new int[size.y, size.x];

        for(int y = 0; y<size.y; y++) {
            for(int x = 0; x < size.x; x++) {
                floorLayout[y, x] = UnityEngine.Random.Range(0,2);
            }
        }

        return floorLayout;
    }

    public static int[,] RemoveRedundantIslands(int[,] floorLayout, Vector2Int size) {

        int[,] visited = new int[size.y,size.x];

        List<List<List<int>>> islands = new List<List<List<int>>>();
        
        for(int y = 0; y < size.y; y++) {
            for(int x = 0; x < size.x; x++) {
                if(visited[y, x] == 0 && floorLayout[y,x] > 0) {
                
                    List<List<int>> island = new List<List<int>>();
                    DFS dfs = new DFS(island, floorLayout, visited, size);
                    dfs.Search(x, y);
                    island = dfs.Island;
                    visited = dfs.Visited;
                    islands.Add(island);
                
                }

            }
        }

        maxIsland = new List<List<int>>();
        maxIslandSize = -1;

        int islandCount = 0;
        string outStr = "";

        
        foreach (List<List<int>> island in islands) {
            outStr += "Island " + islandCount  + ": \n";

            int currentIslandSize = 0;

            foreach (List<int> block in island) {
                outStr += "\t(" + block[0] + ", " + block[1] + " )\n";
                currentIslandSize++;
            }

            if(currentIslandSize > maxIslandSize) {
                maxIslandSize = currentIslandSize;
                maxIsland = island;
            }

            islandCount ++;
        }

        //Debug.Log(outStr);

        floorLayout = new int[size.y, size.x];
        foreach (List<int> block in maxIsland) {
    
            floorLayout[block[1], block[0]] = 1;
            
        }

        return floorLayout;
    }

}
