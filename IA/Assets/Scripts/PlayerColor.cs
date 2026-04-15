using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour
{
    [SerializeField] Material material;


    public void ChangeColor()
    {
        material.color = Color.red;
    }
    public void ChangeToWhite()
    {
        material.color = Color.white;
    }
}
