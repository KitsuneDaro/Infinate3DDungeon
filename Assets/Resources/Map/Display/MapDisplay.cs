using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Player player;
    public const int radius = 10;
    public const int displaySize = 2 * radius + 1;
    private Vector3 playerIntPosition;
    private Block[,,] holdingBlockArray = new Block[displaySize, displaySize, displaySize];
    
    // Start is called before the first frame update
    void Start()
    {
        DrawFirst(new Vector3(0, 0.25f, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        DrawDelta(player.transform.position);
        UpdateBlocks(player.transform.position);
    }

    void DrawDelta(Vector3 playerPosition)
    {
        Vector3 newPlayerIntPosition = Vector3Utils.Floor(playerPosition);
        Vector3 playerFractionalPosition = Vector3Utils.GetFractionalPart(playerPosition);
        
        Vector3 deltaPlayerIntPosition = newPlayerIntPosition - playerIntPosition;
        Vector3 nowDeltaPlayerIntPosition = Vector3.zero;

        for (int xyz = 0; xyz < 3; xyz++) {
            int deltaPlayerIntPositionXYZ = (int)deltaPlayerIntPosition[xyz];
            nowDeltaPlayerIntPosition[xyz] = deltaPlayerIntPositionXYZ;

            if (deltaPlayerIntPositionXYZ == 0) {
                continue;
            }

            Vector3Int deleteingBoundsIntSize = Vector3Int.one * (displaySize - 1);
            deleteingBoundsIntSize[xyz] = -deltaPlayerIntPositionXYZ;

            Vector3Int deleteingBoundsIntPosition = Vector3Int.one * -radius;
            if (deltaPlayerIntPositionXYZ < 0){
                deleteingBoundsIntPosition[xyz] *= -1;
            }

            BoundsInt deletingBounds = new BoundsInt(deleteingBoundsIntPosition, deleteingBoundsIntSize);
            Debug.Log(deletingBounds.min);
            Debug.Log(deletingBounds.max);
            Debug.Log(deleteingBoundsIntSize);
            
            DeleteBlocks(playerIntPosition + nowDeltaPlayerIntPosition + playerFractionalPosition, deletingBounds);
            AddBlocks(playerIntPosition + nowDeltaPlayerIntPosition + playerFractionalPosition, deletingBounds);
        }
        
        playerIntPosition = newPlayerIntPosition;
    }
    
    void DrawFirst(Vector3 playerPosition)
    {
        BoundsInt bounds = new BoundsInt(Vector3Int.one * -radius, Vector3Int.one * (displaySize - 1));

        AddBlocks(playerPosition, bounds);
    }

    void UpdateBlocks(Vector3 playerPosition)
    {
        Vector3 newPlayerIntPosition = Vector3Utils.Floor(playerPosition);
        Vector3 playerFractionalPosition = Vector3Utils.GetFractionalPart(playerPosition);

        for (int x = -radius; x <= radius; x++) {
            for (int y = -radius; y <= radius; y++) {
                for (int z = -radius; z <= radius; z++) {
                    Vector3 relativeBlockIntPosition = new Vector3(x, y, z);
                    Vector3 relativeBlockPosition = relativeBlockIntPosition + playerFractionalPosition;
                    Vector3 blockPosition = relativeBlockIntPosition + newPlayerIntPosition;
                    Vector3 repeatedBlockPosition = RepeatBlockPosition(blockPosition);
                    Block block = GetHoldingBlock(repeatedBlockPosition);
                    
                    float alpha = 1.0f;

                    if (relativeBlockPosition.magnitude > radius + 0.5f) {
                        alpha = 0.0f;
                    } else {
                        if (relativeBlockPosition.magnitude > radius - 0.5f) {
                            alpha = radius + 0.5f - relativeBlockPosition.magnitude;
                        }
                    }

                    if (alpha < 1.0f) {
                        block.SetColor(new Color(1.0f, 0.0f, 1.0f, alpha));
                    }
                }
            }
        }
    }

    void AddBlocks(Vector3 playerPosition, BoundsInt drawingBounds)
    {
        Vector3 newPlayerIntPosition = Vector3Utils.Floor(playerPosition);
        Vector3 playerFractionalPosition = Vector3Utils.GetFractionalPart(playerPosition);

        for (int x = drawingBounds.min.x; x <= drawingBounds.max.x; x++) {
            for (int y = drawingBounds.min.y; y <= drawingBounds.max.y; y++) {
                for (int z = drawingBounds.min.z; z <= drawingBounds.max.z; z++) {
                    Vector3 relativeBlockIntPosition = new Vector3(x, y, z);
                    Vector3 relativeBlockPosition = relativeBlockIntPosition + playerFractionalPosition;
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
        Vector3 playerFractionalPosition = Vector3Utils.GetFractionalPart(playerPosition);

        for (int x = drawingBounds.min.x; x <= drawingBounds.max.x; x++) {
            for (int y = drawingBounds.min.y; y <= drawingBounds.max.y; y++) {
                for (int z = drawingBounds.min.z; z <= drawingBounds.max.z; z++) {
                    Vector3 relativeBlockIntPosition = new Vector3(x, y, z);
                    Vector3 blockPosition = relativeBlockIntPosition + newPlayerIntPosition;
                    Vector3 repeatedBlockPosition = RepeatBlockPosition(blockPosition);
                    
                    SetHoldingBlock(repeatedBlockPosition, null);
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
        if (block == null) {
            holdingBlockArray[(int)Mathf.Floor(repeatedBlockPosition.x), (int)Mathf.Floor(repeatedBlockPosition.y), (int)Mathf.Floor(repeatedBlockPosition.z)].DeleteMesh();
        }

        holdingBlockArray[(int)Mathf.Floor(repeatedBlockPosition.x), (int)Mathf.Floor(repeatedBlockPosition.y), (int)Mathf.Floor(repeatedBlockPosition.z)] = block;
    }
}
