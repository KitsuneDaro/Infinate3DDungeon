using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    private GameObject mesh;
    private static GameObject MeshResource = (GameObject)Resources.Load("Block/Block");
    
    // Start is called before the first frame update
    public Block(Vector3 position, Quaternion quaternion, Transform parentTransform)
    {
        mesh = (GameObject)GameObject.Instantiate(
            MeshResource, 
            position, 
            quaternion,
            parentTransform
        );

        mesh.transform.localScale = Vector3.one * 1.25f;
    }

    public void setColor(Color color)
    {
        MeshRenderer meshRender = mesh.transform.Find("default").GetComponent<MeshRenderer>();
        meshRender.material = new Material(meshRender.material);
        BlendModeUtils.SetBlendMode(meshRender.material, BlendModeUtils.Mode.Fade);
        meshRender.material.color = color;
    }
}
