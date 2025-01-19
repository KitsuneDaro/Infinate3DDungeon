using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public int speed = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        float deltaTime = Time.deltaTime;
        
        base.Update();

        if (Input.GetKey(KeyCode.LeftArrow)) {
            pos.x -= speed * deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            pos.x += speed * deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            pos.z -= speed * deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            pos.z += speed * deltaTime;
        }

        transform.position = pos;
    }
}
