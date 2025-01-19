using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Player player;
    public Vector3 relativePosition;
    
    // Start is called before the first frame update
    void Start()
    {
        relativePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + relativePosition;
    }
}
