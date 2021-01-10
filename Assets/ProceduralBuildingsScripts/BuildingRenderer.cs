using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingRenderer : MonoBehaviour
{

    // FLOOR
    public Transform floorPrefab;
    public Transform floorStairsPrefab;

    //WALLS
    public Transform wallPrefabNorm1;
    public Transform wallPrefabNorm2;

    public Transform wallPrefabNormWin1;
    public Transform wallPrefabNormWin2;

    public Transform wallPrefabNormImg1;
    public Transform wallPrefabNormImg2;

    public Transform wallPrefabSm1;
    public Transform wallPrefabSm2;

    public Transform wallPrefabSmWin1;
    public Transform wallPrefabSmWin2;

    public Transform wallPrefabSmImg1;
    public Transform wallPrefabSmImg2;

    // CEILING

    public Transform ceilingPrefab0;
    public Transform ceilingPrefab1;   
    public Transform ceilingPrefab2Corner;   
    public Transform ceilingPrefab2Corridor;   
    public Transform ceilingPrefab3; 
    public Transform ceilingPrefab4;     


    public Transform doorPrefab;

    Transform buildingFolder;

    public float heightToPlaceWall;
    public float floorPanelSize;
    public float wallPanelHeight;
    public float panelThickness;
    public float panelThicknessHalf;

    public float floorHeight;

    public Material material;

    public LoadImages loadImages;

    int cluster;

    public void Render(Building building, LoadImages loadImages, int cluster) {

        this.cluster = cluster;

        buildingFolder = new GameObject("Building").transform;
        buildingFolder.localPosition = building.Location;
        Debug.Log("its ok");

    


        this.loadImages = loadImages;


        
        // Block b = new Block(0, BlockType.Corridor, true, false, false);

        // WallType[] walls = {WallType.Image, WallType.Image, WallType.Image};

        // b.Walls = walls;

        // BlockType blockType = b.GetBlockType;

        // Transform[] currentWallPrefabs = new Transform[b.Walls.Length];

        // for(int wallNum = 0; wallNum < b.Walls.Length; wallNum++) {
                            
        //     WallType currentWallType  = b.Walls[wallNum];
        //     currentWallPrefabs[wallNum] = getTheRightPrefab(wallNum, currentWallType, blockType);

        // }
        // Transform testBlock = PrepareBlock(b, floorPrefab, currentWallPrefabs, buildingFolder, 0, 0);
        
   
        // iterate trough floors
        
        
       ///* 
        for(int floorNum = 0; floorNum < building.Floors.Count; floorNum++) {
            // create floor gameObject
            Transform currentFloorFolder = new GameObject("Floor " + floorNum).transform;

            // move floor
          
            currentFloorFolder.localPosition = buildingFolder.TransformPoint(0f, floorNum * floorHeight, 0f);
            currentFloorFolder.SetParent(buildingFolder);

            Block[,] currentFloorBlocks = building.Floors[floorNum].Blocks;
            Vector2Int currentFloorSize = building.Floors[floorNum].Size;

            // iterate y floor
            for(int y = 0; y < currentFloorSize.y; y++){
                //iterate x floor
                for(int x = 0; x < currentFloorSize.x; x++) {

                    // if block exists
                    if(currentFloorBlocks[y,x].GetBlockType != BlockType.NoBlock) {
                    
                    
                        //prepare wall prefabs
                        Transform[] currentWallPrefabs = new Transform[currentFloorBlocks[y,x].Walls.Length];

                        bool isDoor = currentFloorBlocks[y,x].IsDoor;
                        BlockType currentBlockType  = currentFloorBlocks[y,x].GetBlockType;

                        for(int wallNum = 0; wallNum < currentFloorBlocks[y,x].Walls.Length; wallNum++) {
                            //Debug.Log(currentFloorBlocks[y,x].Walls[wallNum].ToString() + "\n");
                            WallType currentWallType  = currentFloorBlocks[y,x].Walls[wallNum];
 
                            if(isDoor && wallNum == 0) {
                                currentWallPrefabs[wallNum] = doorPrefab;
                            }
                            else {
                                currentWallPrefabs[wallNum] = getTheRightPrefab(wallNum, currentWallType, currentBlockType);
                            }
                        }

                        bool shouldPlaceCeiling = false; 
                        Transform currentCeilingPrefab = null;

                        if(floorNum +1 == building.Floors.Count) {
                            shouldPlaceCeiling = true;                              
                        }
                        else {
                            BlockType aboveBlockType = building.Floors[floorNum+1].Blocks[y,x].GetBlockType;
                            if(aboveBlockType == BlockType.NoBlock) {
                                shouldPlaceCeiling = true;
                            }
                        }

                        if(shouldPlaceCeiling) {
                            if(currentBlockType == BlockType.ZeroWalls) {
                                currentCeilingPrefab = ceilingPrefab0;
                            }
                            else if(currentBlockType == BlockType.OneWall) {
                                currentCeilingPrefab = ceilingPrefab1;
                            }  
                            if(currentBlockType == BlockType.Corner) {
                                currentCeilingPrefab = ceilingPrefab2Corner;
                            }  
                            if(currentBlockType == BlockType.Corridor) {
                                currentCeilingPrefab = ceilingPrefab2Corridor;
                            }
                            if(currentBlockType == BlockType.TripleWall) {
                                currentCeilingPrefab = ceilingPrefab3;
                            }
                            if(currentBlockType == BlockType.QuadripleWall) {
                                currentCeilingPrefab = ceilingPrefab4;
                            }                                                                                                                                       
                        }

                        //create block
                        Transform currentBlock = PrepareBlock(currentFloorBlocks[y,x], currentCeilingPrefab, currentWallPrefabs,
                            currentFloorFolder, x, y, shouldPlaceCeiling);

                        //rotate block
                        currentBlock = RotateBlock(currentBlock, currentFloorBlocks[y,x].BlockRotation);

                        //move block
                        currentBlock = MoveBlock(currentBlock, currentFloorFolder, x, currentFloorSize.y - y - 1);

                    }
                    
                }

            } 

        }
        
        
    //*/
    }

    private Transform getTheRightPrefab(int wallNum, WallType wallType, BlockType blockType ) {

        Transform[] normWall = {wallPrefabNorm1, wallPrefabNorm2}; 
        Transform[] normWallWin = {wallPrefabNormWin1, wallPrefabNormWin2};    
        Transform[] normWallImg = {wallPrefabNormImg1, wallPrefabNormImg2};  

        Transform[] smWall = {wallPrefabSm1, wallPrefabSm2}; 
        Transform[] smWallWin = {wallPrefabSmWin1, wallPrefabSmWin2};    
        Transform[] smWallImg = {wallPrefabSmImg1, wallPrefabSmImg2};  

        Transform prefabToReturn = null;
        int index = UnityEngine.Random.Range(0,2);

        if(wallNum == 0) {
            if(blockType == BlockType.QuadripleWall) {
                if(wallType == WallType.Empty) {
                    prefabToReturn = smWall[index]; 
                }
                else if(wallType == WallType.Window) {
                    prefabToReturn = smWallWin[index];                                     
                }
                else if(wallType == WallType.Image) {
                    prefabToReturn = smWallImg[index];                                     
                }  
            }
            else {
                if(wallType == WallType.Empty) {
                    prefabToReturn = normWall[index];
                }
                else if(wallType == WallType.Window) {
                    prefabToReturn = normWallWin[index];                                   
                }
                else if(wallType == WallType.Image) {
                    prefabToReturn = normWallImg[index];                                     
                }  
            }

        }
        else if(wallNum == 1) {

            if(blockType == BlockType.Corridor) {

                if(wallType == WallType.Empty) {
                    prefabToReturn = normWall[index];
                }
                else if(wallType == WallType.Window) {
                    prefabToReturn = normWallWin[index];                                   
                }
                else if(wallType == WallType.Image) {
                    prefabToReturn = normWallImg[index];                                     
                }  
            }
            else {
                if(wallType == WallType.Empty) {
                    prefabToReturn = smWall[index]; 
                }
                else if(wallType == WallType.Window) {
                    prefabToReturn = smWallWin[index];                                     
                }
                else if(wallType == WallType.Image) {
                    prefabToReturn = smWallImg[index];                                     
                }  
            }

        }
        else if(wallNum == 3){
            if(wallType == WallType.Empty) {
                prefabToReturn = normWall[index];
            }
            else if(wallType == WallType.Window) {
                prefabToReturn = normWallWin[index];                                   
            }
            else if(wallType == WallType.Image) {
                prefabToReturn = normWallImg[index];                                     
            }           
        }
        else {
            if(wallType == WallType.Empty) {
                prefabToReturn = smWall[index]; 
            }
            else if(wallType == WallType.Window) {
                prefabToReturn = smWallWin[index];                                     
            }
            else if(wallType == WallType.Image) {
                prefabToReturn = smWallImg[index];                                     
            }            
        }

        return prefabToReturn;
    }

    private Transform PrepareBlock(Block block, Transform currentCeilingPrefab, Transform[] currentWallPrefabs, Transform floor, int x, int y, bool shouldPlaceCeiling) {
        // walls are facing in the X direction  !!!!

        Transform blockFolder = new GameObject("Block " + y.ToString() + x.ToString()).transform;
        Transform lightTransform = new GameObject("Light " + y.ToString() + x.ToString()).transform;
        Light lightComp = lightTransform.gameObject.AddComponent<Light>();
        lightTransform.position = new Vector3(0f, floorHeight -1f, 0f);
        lightComp.color = Color.white;
        lightComp.bounceIntensity = 0f;
        lightComp.intensity = 2f;
        lightTransform.SetParent(blockFolder);


        Transform[] blockWalls = new Transform[currentWallPrefabs.Length];

        if(!block.IsStaircaseBelow) {
            Vector3 blockFloorTransformPoint = new Vector3(0f, panelThicknessHalf, 0f);
            Transform blockFloor = PlacePrefab(blockFloorTransformPoint.x, blockFloorTransformPoint.y, blockFloorTransformPoint.z, 
                Quaternion.identity, floorPrefab, blockFolder, true);
        }
        else {

            Vector3 blockFloorTransformPoint = new Vector3(0f, panelThicknessHalf + 0.2f, 0f);
            Transform blockFloor = PlacePrefab(blockFloorTransformPoint.x, blockFloorTransformPoint.y, blockFloorTransformPoint.z, 
                Quaternion.identity, floorStairsPrefab, blockFolder, false);

        }

        if(!block.IsStaircase && shouldPlaceCeiling){

            Vector3 blockCeilingTransformPoint = new Vector3(0f, floorHeight+0.4f, 0f);
            Quaternion ceilingRotation = Quaternion.Euler(0f, -90f, 0f);
            Transform blockCeiling = PlacePrefab(blockCeilingTransformPoint.x, blockCeilingTransformPoint.y, blockCeilingTransformPoint.z, 
                ceilingRotation, currentCeilingPrefab, blockFolder, true);

        } 

        if(block.GetBlockType == BlockType.OneWall) {

            Vector3 blockWallTransformPoint = new Vector3(0f, heightToPlaceWall, -1*(floorPanelSize/2 - panelThicknessHalf));
            Quaternion blockWallRotation = Quaternion.Euler(0f, 90f, 0f);

            blockWalls[0] = PlacePrefab(blockWallTransformPoint.x, blockWallTransformPoint.y, blockWallTransformPoint.z, 
                blockWallRotation, currentWallPrefabs[0], blockFolder, true);
                
        }
        else if (block.GetBlockType == BlockType.Corner) {

            Vector3 blockWall1TransformPoint = new Vector3(-1*(floorPanelSize/2 - panelThicknessHalf), heightToPlaceWall, 0f);
            Vector3 blockWall2TransformPoint = new Vector3(0f, heightToPlaceWall, -1*(floorPanelSize/2 - panelThicknessHalf));
            Quaternion blockWall1Rotation = Quaternion.Euler(0f, 180f, 0f);
            Quaternion blockWall2Rotation = Quaternion.Euler(0f, 90f, 0f);

            blockWalls[0] = PlacePrefab(blockWall1TransformPoint.x, blockWall1TransformPoint.y, blockWall1TransformPoint.z, 
                blockWall1Rotation, currentWallPrefabs[0], blockFolder, true);
            blockWalls[1] = PlacePrefab(blockWall2TransformPoint.x, blockWall2TransformPoint.y, blockWall2TransformPoint.z, 
                blockWall2Rotation, currentWallPrefabs[1], blockFolder, true);

        }
        else if(block.GetBlockType == BlockType.Corridor) {

            Vector3 blockWall1TransformPoint = new Vector3(0f, heightToPlaceWall, 1*(floorPanelSize/2 - panelThicknessHalf));
            Vector3 blockWall2TransformPoint = new Vector3(0f, heightToPlaceWall, -1*(floorPanelSize/2 - panelThicknessHalf)); // dont move
            Quaternion blockWall1Rotation = Quaternion.Euler(0f, -90f, 0f);
            Quaternion blockWall2Rotation = Quaternion.Euler(0f, 90f, 0f);

            blockWalls[0] = PlacePrefab(blockWall1TransformPoint.x, blockWall1TransformPoint.y, blockWall1TransformPoint.z, 
                blockWall1Rotation, currentWallPrefabs[0], blockFolder, true);
            blockWalls[1] = PlacePrefab(blockWall2TransformPoint.x, blockWall2TransformPoint.y, blockWall2TransformPoint.z, 
                blockWall2Rotation, currentWallPrefabs[1], blockFolder, true);

        }
        else if(block.GetBlockType == BlockType.TripleWall) {

            Vector3 blockWall1TransformPoint = new Vector3(-1*(floorPanelSize/2 - panelThicknessHalf), heightToPlaceWall, 0f);
            Vector3 blockWall2TransformPoint = new Vector3(0f, heightToPlaceWall, -1*(floorPanelSize/2 - panelThicknessHalf));
            Vector3 blockWall3TransformPoint = new Vector3(1*(floorPanelSize/2 - panelThicknessHalf), heightToPlaceWall, 0f);
            Quaternion blockWall1Rotation = Quaternion.Euler(0f, 180f, 0f);            
            Quaternion blockWall2Rotation = Quaternion.Euler(0f, 90f, 0f);

            blockWalls[0] = PlacePrefab(blockWall1TransformPoint.x, blockWall1TransformPoint.y, blockWall1TransformPoint.z, 
                blockWall1Rotation, currentWallPrefabs[0], blockFolder, true);
            blockWalls[1] = PlacePrefab(blockWall2TransformPoint.x, blockWall2TransformPoint.y, blockWall2TransformPoint.z, 
                blockWall2Rotation, currentWallPrefabs[1], blockFolder, true);
            blockWalls[2] = PlacePrefab(blockWall3TransformPoint.x, blockWall3TransformPoint.y, blockWall3TransformPoint.z, 
                Quaternion.identity, currentWallPrefabs[2], blockFolder, true);

        }
         else if(block.GetBlockType == BlockType.QuadripleWall) {

            Vector3 blockWall1TransformPoint = new Vector3(-1*(floorPanelSize/2 - panelThicknessHalf), heightToPlaceWall, 0f);
            Vector3 blockWall2TransformPoint = new Vector3(0f, heightToPlaceWall, -1*(floorPanelSize/2 - panelThicknessHalf));
            Vector3 blockWall3TransformPoint = new Vector3(1*(floorPanelSize/2 - panelThicknessHalf), heightToPlaceWall, 0f);
            Vector3 blockWall4TransformPoint = new Vector3(0f, heightToPlaceWall, 1*(floorPanelSize/2 - panelThicknessHalf));
            Quaternion blockWall1Rotation = Quaternion.Euler(0f, 180f, 0f); 
            Quaternion blockWall2Rotation = Quaternion.Euler(0f, 90f, 0f);           
            Quaternion blockWall4Rotation = Quaternion.Euler(0f, -90f, 0f);           

            blockWalls[0]= PlacePrefab(blockWall1TransformPoint.x, blockWall1TransformPoint.y, blockWall1TransformPoint.z, 
                blockWall1Rotation, currentWallPrefabs[0], blockFolder, true);
            blockWalls[1] = PlacePrefab(blockWall2TransformPoint.x, blockWall2TransformPoint.y, blockWall2TransformPoint.z, 
                blockWall2Rotation, currentWallPrefabs[1], blockFolder, true);
            blockWalls[2] = PlacePrefab(blockWall3TransformPoint.x, blockWall3TransformPoint.y, blockWall3TransformPoint.z, 
                Quaternion.identity, currentWallPrefabs[2], blockFolder, true);
            blockWalls[3] = PlacePrefab(blockWall4TransformPoint.x, blockWall4TransformPoint.y, blockWall4TransformPoint.z, 
                blockWall4Rotation, currentWallPrefabs[3], blockFolder, true);                

        }

    
        for(int wallNum = 0; wallNum < block.Walls.Length; wallNum++) {
            if(block.Walls[wallNum] == WallType.Image) {
                if(blockWalls[wallNum].childCount > 0) {

                    //LoadImages load texture
                    Texture2D texture = loadImages.GetNextTexture(cluster);

                    int width = texture.width;
                    int height = texture.height;

                    float scaleX;
                    float scaleZ;

                    if(width >= height) {
                        scaleX = 1.0f;
                        scaleZ = ((float)height)/((float)width);
                    }
                    else {
                        scaleX = ((float)width)/((float)height);
                        scaleZ = 1.0f;
                    }

                    material = new Material(Shader.Find("Diffuse"));
                    material.mainTexture = texture;
                    Transform plane = blockWalls[wallNum].GetChild(0);
                    plane.localRotation = Quaternion.Euler(90f, 0f, 0f);
                    plane.localScale = new Vector3(scaleX, 1.0f, scaleZ);     
                    plane.GetComponent<Renderer>().material = material;
                }

            }
            else if(block.Walls[wallNum] == WallType.Window){
                blockWalls[wallNum].localPosition -= new Vector3(0.0f, 0.2f, 0.0f);                    
            }   
        }


        blockFolder.SetParent(floor);    
        
        return blockFolder;

    }

    private Transform RotateBlock(Transform block, int rotation) {

        block.localRotation = Quaternion.Euler(0f, rotation*90f, 0f);

        return block;
    }

    private Transform MoveBlock(Transform block, Transform floor, int x, int y) {

        Vector3 newBlockTransformPoint = new Vector3(x*floorPanelSize, 0f, y*floorPanelSize);
        block.localPosition = newBlockTransformPoint;
        return block;
    }   


    private Transform PlacePrefab(float x, float y, float z, Quaternion rotation, Transform prefab, Transform parentFolder, bool addCollider) {
        
        Transform prefabInstance = Instantiate(prefab, parentFolder.TransformPoint(x, y, z), rotation);
        if(addCollider) {
            BoxCollider boxCollider = prefabInstance.gameObject.AddComponent<BoxCollider>();           
        }
        prefabInstance.gameObject.layer = 8;
        prefabInstance.SetParent(parentFolder);
        return prefabInstance;
    }
    

}
 