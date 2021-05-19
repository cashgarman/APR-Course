using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrganList : MonoBehaviour
{
    public Image brainIcon;
    public Image heartIcon;

    public void OnBrainFound()
    {
        brainIcon.color = Color.white;
    }

    public void OnHeartFound()
    {
        heartIcon.color = Color.white;
    }
}
