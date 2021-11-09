using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Material topLayerMaterial;
    public Material middleLayerMaterial;
    public Material bottomLayerMaterial;
    public int width = 5;
    public int maxDepth = 2;
    public void PlaceBlocks()
    {
        CreateTopLayer();
        CreateBottomLayer();
    }
    private void CreateTopLayer()
    {
        for (float x = 0; x < width; x++)
        {
            for (float z = 0; z < width; z++)
            {
                GameObject cubeCreated = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cubeCreated.transform.position = new Vector3(this.transform.position.x+x, this.transform.position.y + GetNewHeight(this.transform.position.x + x, this.transform.position.z + z), this.transform.position.z+z);
                cubeCreated.GetComponent<MeshRenderer>().material = topLayerMaterial;
                cubeCreated.transform.parent = this.gameObject.transform;
                CreateUnderneath(cubeCreated);
            }
        }
    }
    private void CreateUnderneath(GameObject cube)
    {
        float depth = 1;
        while (cube.transform.position.y - depth > this.transform.position.y - maxDepth)
        {
            GameObject cubeCreated = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cubeCreated.transform.position = new Vector3(cube.transform.position.x, cube.transform.position.y - depth, cube.transform.position.z);
            cubeCreated.GetComponent<MeshRenderer>().material = middleLayerMaterial;
            depth += 1;
            cubeCreated.transform.parent = this.gameObject.transform;
        }
    }
    private void CreateBottomLayer()
    {
        for (float x = 0; x < width; x++)
        {
            for (float z = 0; z < width; z++)
            {
                GameObject cubeCreated = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cubeCreated.transform.position = new Vector3(this.transform.position.x +x, this.transform.position.y - maxDepth, this.transform.position.z + z);
                cubeCreated.GetComponent<MeshRenderer>().material = bottomLayerMaterial;
                cubeCreated.transform.parent = this.gameObject.transform;
            }
        }
    }

    private float GetNewHeight(float x, float y)
    {
        float perlin = Mathf.PerlinNoise(x/width, y/width);
        if (Random.Range(0, 101) >= 90)
        {
            perlin = Mathf.Round(perlin) + Random.Range(-1, 2);
        } else
        {
            perlin = Mathf.Round(perlin);
        }
        return perlin;
    }
}
