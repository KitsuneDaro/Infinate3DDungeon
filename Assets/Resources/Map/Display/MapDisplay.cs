using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public GameInfo gameInfo;

    private const int radius = 10;
    private const int displaySize = 2 * radius + 1;
    private Vector3 playerIntPosition;
    private Block[,,] holdingBlockArray = new Block[displaySize, displaySize, displaySize];
    
    // Start is called before the first frame update
    void Start()
    {
        DrawFirst(gameInfo.mainCamera.centerPosition);
    }

    // Update is called once per frame
    void Update()
    {
        DrawDelta(gameInfo.mainCamera.centerPosition);
        UpdateBlocks(gameInfo.mainCamera.centerPosition, gameInfo.mainCamera.relativePosition);
    }

    void DrawDelta(Vector3 playerPosition)
    {
        Vector3 newPlayerIntPosition = Vector3Utils.Floor(playerPosition);
        Vector3 playerFractionalPosition = Vector3Utils.GetFractionalPart(playerPosition);
        
        Vector3 deltaPlayerIntPosition = newPlayerIntPosition - playerIntPosition;
        Vector3 nowDeltaPlayerIntPosition = Vector3.zero;

        for (int xyz = 0; xyz < 3; xyz++) {
            int deltaPlayerIntPositionXYZ = (int)deltaPlayerIntPosition[xyz];
            
            if (deltaPlayerIntPositionXYZ == 0) {
                continue;
            }

            Vector3Int deleteingBoundsIntSize = Vector3Int.one * displaySize;
            deleteingBoundsIntSize[xyz] = deltaPlayerIntPositionXYZ;

            Vector3Int deleteingBoundsIntPosition = Vector3Int.one * -radius;
            if (deltaPlayerIntPositionXYZ < 0){
                deleteingBoundsIntPosition[xyz] *= -1;
                deleteingBoundsIntPosition[xyz] += 1;
            }

            BoundsInt deletingBounds = new BoundsInt(deleteingBoundsIntPosition, deleteingBoundsIntSize);
            Debug.Log(deletingBounds.min);
            Debug.Log(deletingBounds.max);

            DeleteBlocks(playerIntPosition + nowDeltaPlayerIntPosition, deletingBounds);

            nowDeltaPlayerIntPosition[xyz] = deltaPlayerIntPositionXYZ;


            Vector3Int addingBoundsIntSize = Vector3Int.one * displaySize;
            addingBoundsIntSize[xyz] = -deltaPlayerIntPositionXYZ;

            Vector3Int addingBoundsIntPosition = Vector3Int.one * -radius;
            if (deltaPlayerIntPositionXYZ > 0){
                addingBoundsIntPosition[xyz] *= -1;
                addingBoundsIntPosition[xyz] += 1;
            }

            BoundsInt addingBounds = new BoundsInt(addingBoundsIntPosition, addingBoundsIntSize);
            Debug.Log(addingBounds.min);
            Debug.Log(addingBounds.max);
            
            AddBlocks(playerIntPosition + nowDeltaPlayerIntPosition, addingBounds);

        }
        
        playerIntPosition = newPlayerIntPosition;
    }
    
    void DrawFirst(Vector3 playerPosition)
    {
        BoundsInt bounds = new BoundsInt(Vector3Int.one * -radius, Vector3Int.one * displaySize);

        AddBlocks(playerPosition, bounds);
    }

    void UpdateBlocks(Vector3 playerPosition, Vector3 mainCameraPosition)
    {
        Vector3 newPlayerIntPosition = Vector3Utils.Floor(playerPosition);
        Vector3 playerFractionalPosition = Vector3Utils.GetFractionalPart(playerPosition);

        for (int x = -radius; x <= radius; x++) {
            for (int y = -radius; y <= radius; y++) {
                for (int z = -radius; z <= radius; z++) {
                    Vector3 relativeBlockIntPosition = new Vector3(x, y, z);
                    Vector3 relativeBlockPosition = relativeBlockIntPosition - playerFractionalPosition;
                    Vector3 blockPosition = relativeBlockIntPosition + newPlayerIntPosition;
                    Vector3 repeatedBlockPosition = RepeatBlockPosition(blockPosition);
                    Block block = GetHoldingBlock(repeatedBlockPosition);
                    
                    float alpha = Mathf.Min(
                        getAlphaBySphere(relativeBlockPosition, radius),
                        Mathf.Max(
                            getAlphaByPlane(relativeBlockPosition + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), 0.0f),
                            getAlphaByPlane(relativeBlockPosition, mainCameraPosition, 2.0f)
                        )
                    );

                    if (block != null) {
                        block.SetColor(alpha);
                    }
                }
            }
        }
    }

    float getAlphaBySphere(Vector3 relativeBlockPosition, int radius) {
        float alpha = 1.0f;

        if (relativeBlockPosition.magnitude > radius + 0.5f) {
            alpha = 0.0f;
        } else {
            if (relativeBlockPosition.magnitude > radius - 0.5f) {
                alpha = -relativeBlockPosition.magnitude + radius + 0.5f;
            }
        }
        
        return alpha;
    }

    float getAlphaByPlane(Vector3 relativeBlockPosition, Vector3 normal, float margin) {
        float distance = Vector3.Dot(relativeBlockPosition.normalized, normal);
        float alpha = 0.0f;

        if (distance < -margin) {
            alpha = 1.0f;
        } else if (distance >= margin) {
            alpha = 0.0f;
        } else {
            alpha = (margin - distance) / (2 * margin);
        }

        return alpha;
    }

    void AddBlocks(Vector3 playerPosition, BoundsInt drawingBounds)
    {
        Vector3 newPlayerIntPosition = Vector3Utils.Floor(playerPosition);
        
        for (int x = drawingBounds.min.x; x < drawingBounds.max.x; x++) {
            for (int y = drawingBounds.min.y; y < drawingBounds.max.y; y++) {
                for (int z = drawingBounds.min.z; z < drawingBounds.max.z; z++) {
                    Vector3 relativeBlockIntPosition = new Vector3(x, y, z);
                    Vector3 blockPosition = relativeBlockIntPosition + newPlayerIntPosition;
                    Vector3 repeatedBlockPosition = RepeatBlockPosition(blockPosition);
                    
                    Block block = new Block(
                        blockPosition, 
                        Quaternion.identity,
                        transform
                    );
                    SetHoldingBlock(repeatedBlockPosition, block);
                }
            }
        }
    }

    void DeleteBlocks(Vector3 playerPosition, BoundsInt drawingBounds) {
        Vector3 newPlayerIntPosition = Vector3Utils.Floor(playerPosition);
        
        for (int x = drawingBounds.min.x; x < drawingBounds.max.x; x++) {
            for (int y = drawingBounds.min.y; y < drawingBounds.max.y; y++) {
                for (int z = drawingBounds.min.z; z < drawingBounds.max.z; z++) {
                    Vector3 relativeBlockIntPosition = new Vector3(x, y, z);
                    Vector3 blockPosition = relativeBlockIntPosition + newPlayerIntPosition;
                    Vector3 repeatedBlockPosition = RepeatBlockPosition(blockPosition);
                    
                    DeleteHoldingBlock(repeatedBlockPosition);
                }
            }
        }
    }

    Vector3 RepeatBlockPosition(Vector3 blockPosition) {
        return Vector3Utils.Repeat(blockPosition, Vector3.one * displaySize);
    }

    Block GetHoldingBlock(Vector3 repeatedBlockPosition) {
        return holdingBlockArray[(int)Mathf.Floor(repeatedBlockPosition.x), (int)Mathf.Floor(repeatedBlockPosition.y), (int)Mathf.Floor(repeatedBlockPosition.z)];
    }

    void SetHoldingBlock(Vector3 repeatedBlockPosition, Block block) {
        holdingBlockArray[(int)Mathf.Floor(repeatedBlockPosition.x), (int)Mathf.Floor(repeatedBlockPosition.y), (int)Mathf.Floor(repeatedBlockPosition.z)] = block;
    }

    void DeleteHoldingBlock(Vector3 repeatedBlockPosition) {
        Block holdingBlock = holdingBlockArray[(int)Mathf.Floor(repeatedBlockPosition.x), (int)Mathf.Floor(repeatedBlockPosition.y), (int)Mathf.Floor(repeatedBlockPosition.z)];
        if (holdingBlock != null) {
            holdingBlock.DeleteMesh();
            SetHoldingBlock(repeatedBlockPosition, null);
        }
    }
}
