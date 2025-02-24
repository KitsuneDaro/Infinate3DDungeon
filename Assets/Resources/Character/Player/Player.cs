using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private float runningSpeed = 6.0f;
    public float runningTime = 0.0f;
    public int runningCount = 0;
    public float runningFrameTime = 0.4f;
    public int runningFrameN = 2;
    public float rotatingSpeed = 6.0f;

    private Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);

    public GameInfo gameInfo;

    private MeshManager MeshManager = new MeshManager("Character/Player/");
    private GameObject mesh;
    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        MeshManager.AddMeshResourceByDictionary(
            new Dictionary<string, string>(){
                {"Staying", "Staying/player_staying"},
                {"Running_0", "Running/player_running_0"},
                {"Running_1", "Running/player_running_1"},
            }
        );

        mesh = (GameObject)GameObject.Instantiate(
            MeshManager.GetMeshResource("Staying"), 
            new Vector3(0.0f, 0.0f, 0.0f), 
            Quaternion.identity,
            transform
        );

        transform.localScale = Vector3.one * 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        Vector3 pos = transform.position;
                
        velocity.x -= 0.1f * velocity.x;
        velocity.z -= 0.1f * velocity.z;
        
        velocity.y -= 2.0f * 9.8f * Time.deltaTime;

        if (gameInfo.movingDirection.movingFlag) {
            runningTime += Time.deltaTime;

            velocity.x = runningSpeed * gameInfo.movingDirection.direction.x;
            velocity.z = runningSpeed * gameInfo.movingDirection.direction.z;

            if (pos.y < 0.0f) {
                velocity.y = 4.0f;

                runningCount = (runningCount + 1) % runningFrameN;

                Destroy(mesh);
                mesh = (GameObject)GameObject.Instantiate(
                    MeshManager.GetMeshResource($"Running_{runningCount}"), 
                    pos, 
                    transform.rotation,
                    transform
                );
            }

            transform.rotation = Quaternion.Lerp(
                transform.rotation, 
                Quaternion.LookRotation(gameInfo.movingDirection.direction), 
                rotatingSpeed * Time.deltaTime
            );
        } else {
            if (runningTime > 0.0f) {
                Destroy(mesh);
                mesh = (GameObject)GameObject.Instantiate(
                    MeshManager.GetMeshResource($"Staying"), 
                    pos, 
                    transform.rotation,
                    transform
                );

                runningTime = 0.0f;
            }
        }

        
        if (pos.y < 0.0f) {
            pos.y = 0.0f;

            if (velocity.y < 0.0f) {
                velocity.y = 0.0f;
            }
        }

        pos += velocity * Time.deltaTime;

        transform.position = pos;

        Debug.Log(velocity.y);
    }
}
