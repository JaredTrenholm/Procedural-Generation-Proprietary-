using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Material topLayerMaterial;
    public Material middleLayerMaterial;
    public float width;
    public float length;
    public float maxDepth;
    public float freq;
    public float amp;
    public string seed;
    public bool randomSeed;
    private List<GameObject> topLayerCubes = new List<GameObject>();
    private List<GameObject> middleLayerCubes = new List<GameObject>();

    private List<Vector3> vertices = new List<Vector3>();
    private Mesh mesh;
    public MeshFilter filter;
    void Start()
    {
        mesh = new Mesh();
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
        DestroyPreviousLevel();
        ActivateSeed(GetSeed());
        PlaceBlocks();
    }
    private void DestroyPreviousLevel()
    {
        foreach (GameObject cube in topLayerCubes)
        {
            GameObject.Destroy(cube);
        }
        foreach (GameObject cube in middleLayerCubes)
        {
            GameObject.Destroy(cube);
        }
        topLayerCubes.Clear();
        middleLayerCubes.Clear();
    }
    private void PlaceBlocks()
    {
        CreateTopLayer();
        CreateMiddleLayer();
    }
    private void CreateTopLayer()
    {
        for (float x = 0; x < width; x++)
        {
            for (float z = 0; z < length; z++)
            {
                vertices.Add(new Vector3(x, PerlinNoise(x / 2, z / 2) + PerlinNoise(z / 2, x / 2), z));
            }
        }

        Vector3[] verticesArray = new Vector3[vertices.Count];
        int verticeCount = 0;
        foreach (Vector3 vertice in vertices)
        {
            verticesArray[verticeCount] = vertice;
        }
        mesh.vertices = verticesArray;
        mesh.SetVertices(vertices);
        filter.mesh = mesh;
    }
    private void CreateMiddleLayer()
    {
        foreach(GameObject topCube in topLayerCubes)
        {
            for (float depth = 1; depth < maxDepth; depth++)
            {
                GameObject cubeCreated = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cubeCreated.transform.position = new Vector3(topCube.transform.position.x, topCube.transform.position.y - depth, topCube.transform.position.z);
                cubeCreated.GetComponent<MeshRenderer>().material = middleLayerMaterial;
                middleLayerCubes.Add(cubeCreated);
            }
        }
    }
    private float PerlinNoise(float x, float y)
    {
        return ((Mathf.PerlinNoise(x/Random.Range(1,11), y / Random.Range(1, 11)) *freq)*amp);
    }
    private int GetSeed()
    {
        if(randomSeed)
            seed = "" + Random.Range(int.MinValue, int.MaxValue);
        return seed.GetHashCode();
    }
    private void ActivateSeed(int seedCode)
    {
        Random.InitState(seedCode);
    }
}
