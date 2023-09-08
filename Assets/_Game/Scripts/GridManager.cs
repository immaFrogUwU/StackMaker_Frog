using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Vector3 GridToVector3(int row, int col)
    {
        return new Vector3(col, 0, row);
    }
    public Grid Vector3ToGrid(Vector3 playerPos)
    {
        //float coordX = playerPos.x; 
        int colNumber = GetRowOrColNumber(playerPos.x);
        //float coordZ = playerPos.z;
        int rowNumber = GetRowOrColNumber(playerPos.z);
        return new Grid(colNumber, rowNumber);
    }
    public int GetRowOrColNumber(float x)
    {
        return Mathf.FloorToInt(x + 0.5f);
        //>0.5 && <=1.5 --> 1
    }
}
