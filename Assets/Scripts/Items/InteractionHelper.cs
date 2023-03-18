using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHelper : MonoBehaviour
{
    public static InteractionHelper instance;
    public GameObject dotPrefab;
    public GameObject interactionPrefab;
    [HideInInspector] public GameObject cam;

    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }
}
