using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class checkear : MonoBehaviour
{
    LineOfSight  Los;
    [SerializeField] GameObject Player;
    [SerializeField] PlayerColor Color;
    private void Awake()
    {
        Los = GetComponent<LineOfSight>();

    }


    public void Cheking()
    {
        if (Los.CheckAngle(transform, Player.transform) && Los.CheckRange(transform, Player.transform) && Los.checkObstacles(transform, Player.transform))
        {
            Color.ChangeColor();
        }
        else
        {
            Color.ChangeToWhite();
        }
    }

    private void Update()
    {
        Cheking();
    }
}
