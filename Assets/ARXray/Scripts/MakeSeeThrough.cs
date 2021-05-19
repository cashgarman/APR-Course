using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSeeThrough : MonoBehaviour
{
    void Start()
    {
        GetComponent<Renderer>().material.renderQueue = 3002;
    }
}
