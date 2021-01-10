using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFS
{

    Vector2Int size;

    int[,] visited;
    public int[,] Visited{ get => visited; }

    int[,] floorLayout;

    List<List<int>> island;
    public List<List<int>> Island { get => island; }


    int[] xNeighbors = {0,  0, 1, -1};
    int[] yNeighbors = {1, -1,  0,  0};

    public DFS(List<List<int>> island, int[,] floorLayout, int[,] visited, Vector2Int size) {
        this.island = island;
        this.floorLayout = floorLayout;
        this.visited = visited;
        this.size = size;
    }  

    public void Search(int x, int y) {
        this.visited[y, x] = 1;

        List<int> currentCoordinates = new List<int>();
        currentCoordinates.Add(x);
        currentCoordinates.Add(y);
        this.island.Add(currentCoordinates);       

        for(int i = 0; i<4; i++) {

            int xToCheck = x + xNeighbors[i];
            int yToCheck = y + yNeighbors[i];

            if(IsSafe(xToCheck, yToCheck)) {
                Search(xToCheck, yToCheck);
            }
        }


    }

    private bool IsSafe(int x, int y) {
        return ((x >= 0) && (y >= 0) && (x < this.size.x) && (y < this.size.y) && 
            (this.visited[y, x] == 0) && this.floorLayout[y, x] > 0);
    }

}
