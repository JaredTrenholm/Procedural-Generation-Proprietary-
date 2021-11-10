using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject chunkPrefab;
    public PlayerStart playerStart;
    public float width;
    public float length;
    public string seed;
    public bool randomSeed;

    private List<GameObject> chunks = new List<GameObject>();

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
        DeleteChunks();
        ActivateSeed(GetSeed());
        GenerateChunks();
        PlacePlayer();
    }

    private void DeleteChunks()
    {
        foreach(GameObject chunk in chunks)
        {
            Destroy(chunk);
        }
        chunks.Clear();
    }
    private void GenerateChunks()
    {
        for(int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject chunk = Instantiate(chunkPrefab);
                chunk.transform.position = new Vector3(this.transform.position.x + x*5, this.transform.position.y, this.transform.position.z + z*5);
                chunk.GetComponent<Chunk>().GenerateChunk(Random.Range(0,101));
                chunks.Add(chunk);
            }
        }
    }

    private void PlacePlayer()
    {
        bool playerPlaced = false;
        foreach(GameObject chunk in chunks)
        {
            if (Random.Range(0, 11) > 9)
            {
                chunk.GetComponent<Chunk>().PlaceObject(playerStart.gameObject);
                playerPlaced = true;
                break;
            }
        }
        if(playerPlaced != true)
        {
            playerStart.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
        }
        playerStart.Spawn();
    }
    private int GetSeed()
    {
        if (randomSeed)
            seed = "" + Random.Range(int.MinValue, int.MaxValue);
        return seed.GetHashCode();
    }
    private void ActivateSeed(int seedCode)
    {
        Random.InitState(seedCode);
    }
}