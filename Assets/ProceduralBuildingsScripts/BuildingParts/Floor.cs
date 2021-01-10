using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor
{
     
    public Vector2Int size;
    public Vector2Int Size{  get => size; }  

    int level;
    public int Level{  get => level; } 

    int[,] floorLayout;
    public int[,] FloorLayout { get => floorLayout; } 

    Block[,] blocks;
    public Block[,] Blocks { get => blocks; }


    public Floor(int[,] floorLayout, Vector2Int size, int level) {
        this.level = level;
        this.floorLayout = floorLayout;
        this.size = size;
        CalculateBlocks();
    }

    public void CalculateBlocks() {

        bool doorExists = (this.level == 0) ? false : true;
        

        this.blocks = new Block[size.y, size.x];

        for(int y= 0; y<size.y; y++) {
       
            for(int x = 0; x<size.x; x++) {



                BlockType currentBlockType = BlockType.NoBlock; 
                int rotation = -1;

                if(floorLayout[y, x] > 0) {

                    int top, left, right, bot;

                    // upper neighbor
                    if(y-1 >= 0) {
                        top = floorLayout[y-1,x]>0 ? 1 : 0;
                    }
                    else {
                        top = 0;
                    }

                    // left neighbor
                    if(x-1 >= 0) {
                        left = floorLayout[y,x-1] > 0 ? 1 : 0;
                    }
                    else {
                        left = 0;
                    }

                    // right neighbor
                    if(x+1 < size.x) {
                        right = floorLayout[y,x+1] > 0 ? 1 : 0;
                    }
                    else {
                        right = 0;
                    }

                    // bottom neighbor
                    if(y+1 < size.y) {
                        bot = floorLayout[y+1,x] > 0 ? 1 : 0;
                    }
                    else {
                        bot = 0;
                    }

                    int numOfNeighbors = top + left + right + bot;

                    if(numOfNeighbors == 4) {
                        currentBlockType = BlockType.ZeroWalls;
                        rotation = 0;
                    }
                    else if(numOfNeighbors == 3) {
                        currentBlockType = BlockType.OneWall;

                        if(bot == 0) {
                            rotation = 0;
                        }
                        else if(left == 0) {
                            rotation = 1;
                        }
                        else if(top == 0) {
                            rotation = 2;
                        }
                        else if(right == 0) {
                            rotation = 3;
                        }
                    }
                    else if(numOfNeighbors == 2) {
                        

                        if(bot == 0 && left == 0) {
                            currentBlockType = BlockType.Corner;
                            rotation = 0;
                        }
                        else if(left == 0 && top == 0) {
                            currentBlockType = BlockType.Corner;
                            rotation = 1;
                        }
                        else if(top == 0 && right == 0) {
                            currentBlockType = BlockType.Corner;
                            rotation = 2;
                        }
                        else if(right == 0 && bot == 0) {
                            currentBlockType = BlockType.Corner;
                            rotation = 3;
                        }
                        else if(top == 0 && bot == 0) {
                            currentBlockType = BlockType.Corridor;
                            rotation = 0;                           
                        }
                        else if(left == 0 && right == 0) {
                            currentBlockType = BlockType.Corridor;
                            rotation = 1;                           
                        }                        

                    }
                    else if(numOfNeighbors == 1) {
                        currentBlockType = BlockType.TripleWall;

                        if(bot == 0 & left == 0 && right == 0) {
                            rotation = 0;
                        }
                        else if(bot == 0 & left == 0 && top == 0) {
                            rotation = 1;
                        }
                        else if(right == 0 & left == 0 && top == 0) {
                            rotation = 2;
                        }
                        else if(right == 0 & bot == 0 && top == 0) {
                            rotation = 3;
                        }

                    }
                    else if(numOfNeighbors == 0) {
                        currentBlockType = BlockType.QuadripleWall;
                        rotation = 0;                           
                    }




                }
              
                this.blocks[y,x] = new Block(rotation, currentBlockType, floorLayout[y,x] == 2, floorLayout[y,x] == 3, !doorExists);                   
                
                if(!doorExists && floorLayout[y, x] > 0){
                    doorExists = true;
                }
            
            }
        
        }            

    }

    public override string ToString() {
        string floor = "Floor(" + level + "):\n";

        floor += "\t\tFloor Layout:\n";
        for(int y= 0; y<size.y; y++) {
            floor += "\t\t\t";
            for(int x = 0; x<size.x; x++) {
                floor += floorLayout[y, x] + ", ";
            }
            floor += "\n";
        }

        for(int y= 0; y<size.y; y++) {
            floor += "\t\t\t";
            for(int x = 0; x<size.x; x++) {
                floor += blocks[y, x].ToString() + ", ";
            }
            floor += "\n";
        }       
        return floor;

    }

    public int GetNumOfImages() {
        int numOfImages = 0;
    
        for(int y= 0; y<size.y; y++) {

            for(int x = 0; x<size.x; x++) {
                Block currentBlock = blocks[y, x];

                foreach(WallType wall in currentBlock.Walls) {
                    if(wall == WallType.Image) {
                        numOfImages++;
                    }
                }
                
            }
           
        } 

        return numOfImages;      
    }
}
