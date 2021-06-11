using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalableObject : MonoBehaviour
{
    public float sensitivity = 0.01f;

    public void ScaleObject(float amount)
    {
        transform.localScale += new Vector3(amount * sensitivity, amount * sensitivity, amount * sensitivity);
    }
}
