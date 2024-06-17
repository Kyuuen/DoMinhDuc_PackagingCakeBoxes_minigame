using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private int width;
    private int height;
    private float cellSize;
    private int[,] gridArray;
    private GameObject gridFrame;

    private Vector2 cakePos;
    private Vector2 boxPos;
    private Vector2[] candiesPos;
    
    // Need input object's pos
    public Grid(int width, int height, float cellSize, GameObject gridFrame)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new int[width, height];
        this.gridFrame = gridFrame;

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                CreateOverlayGrid(null, GetWorldPostition(x,y));
            }
        }

        
    }

    private Vector3 GetWorldPostition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    public GameObject CreateOverlayGrid(Transform parent, Vector3 localPos)
    {
        GameObject gameObject = (GameObject)Instantiate(gridFrame, null);
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPos;
        return gameObject;
    }
}
