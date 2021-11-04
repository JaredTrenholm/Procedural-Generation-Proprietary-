using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public bool useRandomSeed;
    public string seed;
    public float width;
    public float length;

    public float freq;
    public float amp;

    private List<GameObject> cubes = new List<GameObject>();
    void Start()
    {
        StartGeneration();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartGeneration();
        }
    }
    private void StartGeneration()
    {
        foreach(GameObject cube in cubes)
        {
            GameObject.Destroy(cube);
        }
        ActivateSeed(CreateSeed());
        PlaceBlocks();
    }
    private void PlaceBlocks()
    {
        for(float x = 0; x < width; x++)
        {
            for (float z = 0; z < length; z++)
            {
                GameObject cubeCreated = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cubeCreated.transform.position = new Vector3(x, PerlinNoise(x * Time.realtimeSinceStartup, z * Time.realtimeSinceStartup), z);
                cubes.Add(cubeCreated);
                cubeCreated.transform.localScale = new Vector3(1.125f, 1.125f, 1.125f);
            }
        }
    }
    private int CreateSeed()
    {
        if (useRandomSeed)
        {
            seed = "" + Random.Range(int.MinValue, int.MaxValue);
        }
        return seed.GetHashCode();
    }
    private void ActivateSeed(int seedCode)
    {
        Random.InitState(seedCode);
    }
    private float PerlinNoise(float x, float y)
    {
        return (Mathf.PerlinNoise(Random.Range(0,100) + x, Random.Range(0,100) + y)*freq)-amp;
    }
}
