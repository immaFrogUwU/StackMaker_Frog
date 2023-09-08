using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int row;
    private int col;
    public int Row
    {
        get
        {
            return row;
        }
        set
        {
            row = value;
        }
    }
    public int Col
    {
        get
        {
            return col;
        }
        set
        {
            col = value;
        }
    }
    public  Grid (int row, int col)
    {
        this.row = row;
        this.col = col;
        
    }
}
