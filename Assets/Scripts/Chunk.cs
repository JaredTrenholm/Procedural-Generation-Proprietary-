using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public GameObject topPrefab;
    public GameObject middlePrefab;
    public GameObject bottomPrefab;
    public GameObject waterPrefab;
    public GameObject treePrefab;
    public GameObject chunkPrefab;

    public int width = 5;
    public int maxDepth = 2;
    public int treeLimit;

    private List<GameObject> topLayer = new List<GameObject>();
    private List<GameObject> detailLayer = new List<GameObject>();
    private Biome biomeType;
    private Vector3 lastTree = Vector3.zero;
    private enum Biome { 
        Plains,
        Pond,
        ForestHigh,
        Swamp
    }
    public void GenerateChunk(int random)
    {
        SetBiomeType(random);
        CreateTopLayer();
        CreateBottomLayer();
        CreateDetails();
    }
    private void SetBiomeType(int random)
    {
        if (random < 50 && random > 25)
        {
            biomeType = Biome.Plains;
        }
        else if (random >= 65)
        {
            biomeType = Biome.Pond;
        }
        else if (random >= 50 && random < 65)
        {
            biomeType = Biome.ForestHigh;
        }
        else if (random % 3 != 0)
        {
            biomeType = Biome.Swamp;
        }
        else
        {
            biomeType = Biome.Plains;
        }
    }
    private void CreateTopLayer()
    {
        GameObject cubeCreated;
        for (float x = 0; x < width; x++)
        {
            for (float z = 0; z < width; z++)
            {
                cubeCreated = GameObject.Instantiate(topPrefab);
                cubeCreated.transform.position = new Vector3(this.transform.position.x + x, this.transform.position.y + GetNewHeight(this.transform.position.x + x, this.transform.position.z + z), this.transform.position.z + z);
                cubeCreated.transform.parent = this.gameObject.transform;
                topLayer.Add(cubeCreated);
                CreateUnderneath(cubeCreated);
            }
        }
    }
    private void CreateUnderneath(GameObject cube)
    {
        float depth = 1;
        while (cube.transform.position.y - depth > this.transform.position.y - maxDepth)
        {
            GameObject cubeCreated = GameObject.Instantiate(middlePrefab);
            cubeCreated.transform.position = new Vector3(cube.transform.position.x, cube.transform.position.y - depth, cube.transform.position.z);
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
                GameObject cubeCreated = GameObject.Instantiate(bottomPrefab);
                cubeCreated.transform.position = new Vector3(this.transform.position.x +x, this.transform.position.y - maxDepth, this.transform.position.z + z);
                cubeCreated.transform.parent = this.gameObject.transform;
            }
        }
    }
    private void CreateDetails()
    {
        switch (biomeType) {
            case Biome.Plains:
                break;
            case Biome.Pond:
                CreateWater();
                break;
            case Biome.ForestHigh:
                CreateTrees();
                break;
            case Biome.Swamp:
                CreateWater();
                CreateTrees();
                break;
        }
    }
    private void CreateWater()
    {
        foreach(GameObject cube in topLayer)
        {
            if(cube.transform.position.y != this.transform.position.y)
            {
                GameObject cubeCreated = GameObject.Instantiate(waterPrefab);
                cubeCreated.transform.position = new Vector3(cube.transform.position.x, this.transform.position.y, cube.transform.position.z);
                cubeCreated.transform.parent = this.gameObject.transform;
                detailLayer.Add(cubeCreated);
            }
        }
    }
    private void CreateTrees()
    {
        int treeCount = 0;
        foreach (GameObject cube in topLayer)
        {
            if (Vector3.Distance(cube.transform.position, lastTree) >= 3)
            {
                if (Random.Range(0, 11) > 9)
                {
                    GameObject cubeCreated = GameObject.Instantiate(treePrefab);
                    cubeCreated.transform.position = new Vector3(cube.transform.position.x, cube.transform.position.y+1, cube.transform.position.z);
                    cubeCreated.transform.parent = this.gameObject.transform;
                    lastTree = cube.transform.position;
                    treeCount++;
                    detailLayer.Add(cubeCreated);
                }
            }

            if(treeCount == treeLimit)
            {
                break;
            }
        }
    }

    private float GetNewHeight(float x, float y)
    {
        float perlin = Mathf.PerlinNoise(x/width, y/width);
        perlin = Mathf.Round(perlin);

        if(biomeType == Biome.Pond || biomeType == Biome.Swamp)
        {
            return -perlin;
        } else
        {
            return perlin;
        }
    }
    public void PlaceObject(GameObject objectToPlace)
    {
        bool placed = false;
        foreach(GameObject cube in topLayer)
        {
            foreach (GameObject detail in detailLayer)
            {
                if (detail.transform.position.x != objectToPlace.transform.position.x && detail.transform.position.z != objectToPlace.transform.position.z)
                {
                    if (Random.Range(0, 11) > 9)
                    {
                        objectToPlace.transform.position = new Vector3(cube.transform.position.x, cube.transform.position.y + 1, cube.transform.position.z);
                        placed = true;
                    }
                }
            }
        }

        if(placed != true)
        {
            objectToPlace.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
        }
    }
}
