using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    public GameObject player;

    public void Spawn()
    {
        player.transform.position = this.transform.position;
    }
}
