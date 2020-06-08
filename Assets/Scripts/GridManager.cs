using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    //rows and column input
    public int rows = 12;
    public int cols = 6;
    //arbitrary size in ratio of redmi note 5 pro
    public int width;
    public int height;
    //reference prefab for dot sprite
    public GameObject refDot;
    //height and width of each cell in grid
    private float rowHeight;
    private float colWidth;
    // grid : 2 dimensional array to store positions as vector2
    private Vector2[,] grid;
    void Start()
    {
        SizeCorrection();
        rowHeight = height / rows;
        colWidth = width / cols;
        InitGrid();
        ComputeGrid();
        DrawGrid();
    }


    //function for checking odd or even
    bool IsOdd(int x)
    {
        if(x%2 != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //function to initialise grid with zero vectors.
    void InitGrid()
    {
        grid = new Vector2[rows, cols];
        for(int i = 0; i < rows; i++)
        {
            for(int j = 0; j < cols; j++)
            {
                grid[i,j] = Vector2.zero;
            }
        }
    }
    //calculating each cell of grid with vector2 positions
    void ComputeGrid()
    {
        //modulus of limit is the maximum value in x or y of each row and columnn
        if (IsOdd(rows))
        {
            int limit = height / 2;
            float change = 0;
            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    grid[i,j] = new Vector2(grid[i,j].x, limit - change);
                }
                change += rowHeight;
            }                           
        }
        else
        {
            float limit = (height - rowHeight) / 2;
            float change = 0; 
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    grid[i,j] = new Vector2(grid[i,j].x, change - limit);
                }
                change += rowHeight;
            }
        }
        if (IsOdd(cols))
        {
            int limit = width / 2;
            float change = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    grid[i,j] = new Vector2(limit - change, grid[i,j].y);
                }
                change += colWidth;
            }
        }
        else
        {
            float limit = (width - colWidth) / 2;
            float change = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    grid[i,j] = new Vector2(change - limit, grid[i,j].y);
                    change += colWidth;
                }
                change = 0;
            }
        }
    }
    //to correct the width and height
    void SizeCorrection()
    {
        if(width % 2 != 0)
        {
            width = width + 1;
        }
        if(height % 2 != 0)
        {
            height = height + 1;
        }
    }
    //draw sprites at each node to visualise the grid positions
    void DrawGrid()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                GameObject dot = (GameObject)Instantiate(refDot, transform);
                dot.transform.position = new Vector3(grid[i,j].x, grid[i,j].y, 0f);
            }
        }
    }
    //change input indexes to that of cell acting as center 
    public void GetOrigin(ref int i,ref int j)
    {
        float x;
        float y;
        if (IsOdd(rows))
        {
            x = (rows - 1) / 2;
        }
        else
        {
            x = (rows / 2) - 1;
        }
        if (IsOdd(cols))
        {
            y = (cols - 1) / 2;
        }
        else
        {
            y = (cols / 2) - 1;
        }
        i = (int)x;
        j = (int)y;
    }
    //return value of array. that is positions stored in given indexes
    public Vector3 GetGridPosition(int i, int j)
    {
        return new Vector3(grid[i, j].x, grid[i, j].y, 0f);
    }
    //returns number of rows and columns
    public void GetParameters(ref int row_count, ref int col_count)
    {
        row_count = rows;
        col_count = cols;
    }
}
