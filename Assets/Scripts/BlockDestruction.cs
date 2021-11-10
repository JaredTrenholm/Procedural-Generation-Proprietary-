using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDestruction : MonoBehaviour
{
    public Type type;
    public enum Type
    {
        Break,
        NonBreak
    }

    public void DestroyBlock()
    {
        if(type == Type.Break)
        {
            Destroy(this.gameObject);
        }
    }
}
