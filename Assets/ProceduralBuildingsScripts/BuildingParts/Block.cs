using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{

    bool isStaircase;
    public bool IsStaircase { get => isStaircase; }

    bool isStaircaseBelow;
    public bool IsStaircaseBelow { get => isStaircaseBelow; }

    bool isDoor;
    public bool IsDoor { get => isDoor; }

    WallType[] walls;
    public WallType[] Walls { get => walls; set => walls = value; }    

    int blockRotation;
    public int BlockRotation {get => blockRotation; }

    BlockType blockType;
    public BlockType GetBlockType{ get => blockType; }


    public Block(int blockRotation, BlockType blockType, bool isStaircase, bool isStaircaseBelow, bool isDoor) {
        this.blockRotation = blockRotation;
        this.blockType = blockType;
        this.isStaircase = isStaircase;
        this.isStaircaseBelow = isStaircaseBelow;
        this.isDoor = isDoor;
        MakeWalls();
    }

    private void MakeWalls() {
        if(blockType == BlockType.NoBlock || blockType == BlockType.ZeroWalls) {
            walls = new WallType[] {};
        }
        else if(blockType == BlockType.OneWall) {
            walls = new WallType[1];
            for(int i=0; i<1; i++) {
                walls[i] = GenerateRandomWallType();
            }
        }
        else if(blockType == BlockType.Corner || blockType == BlockType.Corridor) {
            walls = new WallType[2];
            for(int i=0; i<2; i++) {
                walls[i] = GenerateRandomWallType();
            }
        }
        else if(blockType == BlockType.TripleWall) {
            walls = new WallType[3];
            for(int i=0; i<3; i++) {
                walls[i] = GenerateRandomWallType();
            }
        }       
        else if(blockType == BlockType.QuadripleWall) {
            walls = new WallType[4];
            for(int i=0; i<4; i++) {
                walls[i] = GenerateRandomWallType();
            }
        }

    }

    public WallType GenerateRandomWallType() {
        System.Array wallTypes = System.Enum.GetValues(typeof(WallType));
        WallType randomWT = (WallType)wallTypes.GetValue(UnityEngine.Random.Range(0,wallTypes.Length));
        return randomWT;
    }

    public override string ToString() {
        return blockType.ToString() + " " + blockRotation;
    }

}

