using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameInfo gameInfo;
    public Vector3 relativePosition;
    public Vector3 targetPosition;
    public Vector3 centerPosition;
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
        
        UpdateTargetPosition();
        UpdateCenterPosition(deltaTime);
        UpdatePosition();
    }

    void UpdateTargetPosition() {
        targetPosition = gameInfo.player.transform.position + gameInfo.movingDirection.direction * movingDirectionWidth;
    }

    void UpdateCenterPosition(float deltaTime) {
        centerPosition += speed * (targetPosition - centerPosition) * deltaTime;
    }

    void UpdatePosition() {
        transform.position = centerPosition + relativePosition;
    }
}
