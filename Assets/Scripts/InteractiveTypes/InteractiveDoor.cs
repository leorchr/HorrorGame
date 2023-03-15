using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveDoor : Interactive
{
    public override void OnInteraction()
    {
        Debug.Log("Tu as ouvert la porte");
        transform.position += new Vector3(0, 5, 0);
    }
}
