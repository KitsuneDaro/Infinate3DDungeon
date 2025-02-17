using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    private static GameObject MeshResource = (GameObject)Resources.Load("Block/Block");
    private static int ColorAlphaStageN = 5;
    private static Material[] MaterialList = new Material[ColorAlphaStageN];
    private static bool MaterialListInitFlag = false;

    private GameObject mesh;
    
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

        MeshRenderer meshRender = mesh.transform.Find("default").GetComponent<MeshRenderer>();
        
        InitaterialList(meshRender.material);
    }

    public void InitaterialList(Material material) {
        if (!MaterialListInitFlag) {
            for (int materialListIndex = 0; materialListIndex < MaterialList.Length; materialListIndex++) {
                float alpha = (1.0f / (ColorAlphaStageN - 1)) * materialListIndex;
                Color color = new Color(1.0f, 1.0f, 1.0f, alpha);

                MaterialList[materialListIndex] = new Material(material);
                BlendModeUtils.SetBlendMode(MaterialList[materialListIndex], BlendModeUtils.Mode.Fade);
                MaterialList[materialListIndex].color = color;
            }

            MaterialListInitFlag = true;
        }
    }

    public void SetColor(float alpha)
    {
        MeshRenderer meshRender = mesh.transform.Find("default").GetComponent<MeshRenderer>();
        
        for (int materialListIndex = 0; materialListIndex < MaterialList.Length; materialListIndex++) {
            float materialUpperAlpha = (1.0f / ColorAlphaStageN) * (materialListIndex + 1);

            if (alpha <= materialUpperAlpha) {
                meshRender.material = MaterialList[materialListIndex];
                break;
            }
        }
    }

    public void DeleteMesh()
    {
        GameObject.Destroy(mesh);
    }
}
