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
        BoundsInt bounds = new BoundsInt(Vector3Int.one * -radius, Vector3Int.one * radius);

        Draw(playerPosition, bounds);
    }

    void Draw(Vector3 playerPosition, BoundsInt drawingBounds)
    {
        Vector3 newPlayerIntPosition = Vector3Utils.Floor(playerPosition);
        Vector3 playerFractionalPosition = Vector3Utils.GetFractionalPart(playerPosition);

        for (int x = drawingBounds.min.x; x <= drawingBounds.max.x; x++) {
            for (int y = drawingBounds.min.y; y <= drawingBounds.max.y; y++) {
                for (int z = drawingBounds.min.z; z <= drawingBounds.max.z; z++) {
                    Vector3 blockPosition = new Vector3(x, y, z);
                    Block block = new Block(
                        blockPosition, 
                        Quaternion.identity,
                        transform
                    );
                    
                    float alpha = 1.0f;

                    if ((blockPosition + playerFractionalPosition).magnitude > radius) {
                        alpha = 0.0f;
                    } else {
                        if ((blockPosition + playerFractionalPosition).magnitude > radius - 1) {
                            alpha = radius - (blockPosition + playerFractionalPosition).magnitude;
                        }
                    }

                    if (alpha < 1.0f) {
                        block.setColor(new Color(1.0f, 1.0f, 1.0f, alpha));
                    }
                }
            }
        }
    }
}
