using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public int speed = 10;
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
        float deltaTime = Time.deltaTime;
        
        pos += speed * gameInfo.movingDirection.direction * deltaTime;

        transform.position = pos;

        transform.rotation = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, 0.5f), gameInfo.movingDirection.direction);

        Debug.Log(transform.rotation);
    }
}
