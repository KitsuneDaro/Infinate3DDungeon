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
        
    }

    void DrawDelta(Vector3 playerPosition)
    {
        Vector3 newPlayerIntPosition = Vector3Utils.Floor(playerPosition);
        Vector3 deltaPlayerIntPosition = newPlayerIntPosition - playerIntPosition;
    }
    
    void DrawFirst(Vector3 playerPosition)
    {
        BoundsInt bounds = new BoundsInt(Vector3Int.one * -radius, Vector3Int.one * displaySize);

        AddBlocks(playerPosition, bounds);
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
                    
                    float alpha = 1.0f;

                    if (relativeBlockPosition.magnitude > radius) {
                        alpha = 0.0f;
                    } else {
                        if (relativeBlockPosition.magnitude > radius - 1) {
                            alpha = radius - relativeBlockPosition.magnitude;
                        }
                    }

                    if (alpha < 1.0f) {
                        block.setColor(new Color(1.0f, 1.0f, 1.0f, alpha));
                    }
                }
            }

            Debug.Log(drawingBounds.max.x);
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
        holdingBlockArray[(int)Mathf.Floor(repeatedBlockPosition.x), (int)Mathf.Floor(repeatedBlockPosition.y), (int)Mathf.Floor(repeatedBlockPosition.z)] = block;
    }
}
