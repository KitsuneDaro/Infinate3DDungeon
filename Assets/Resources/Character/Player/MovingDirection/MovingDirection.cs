using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingDirection : MonoBehaviour
{
    public Vector3 direction = Vector3.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        direction = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow)) {
            direction.x += 1.0f;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            direction.x -= 1.0f;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            direction.z += 1.0f;
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            direction.z -= 1.0f;
        }

        direction = direction.normalized;
    }
}
