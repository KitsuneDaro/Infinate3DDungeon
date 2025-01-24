using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameInfo gameInfo;
    public Vector3 relativePosition;
    public float movingDirectionWidth = 2.0f;
    public float speed = 2.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        relativePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        
        Vector3 targetPosition = gameInfo.player.transform.position + gameInfo.movingDirection.direction * movingDirectionWidth;
        transform.position += speed * (targetPosition - (transform.position - relativePosition)) * deltaTime;
    }
}
