using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestruction : MonoBehaviour
{
    public float timeToPass = 0.125f;
    private float timePassed = 100f;
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (timePassed >= timeToPass)
            {
                FireRaycast();
                timePassed = 0f;
            } else
            {
                timePassed += Time.deltaTime;
            }
        } else if (Input.GetKey(KeyCode.Mouse1) != true)
        {
            timePassed = timeToPass;
        }
    }

    private void FireRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.TransformDirection(Vector3.forward), out hit))
        {
            Debug.Log(hit.collider.gameObject.name);
            BlockDestruction block = hit.collider.gameObject.GetComponent<BlockDestruction>();
            if (block != null)
            {
                if(Vector3.Distance(block.gameObject.transform.position, this.gameObject.transform.position) <= 10f)
                block.DestroyBlock();
            }
        }
    }
}
