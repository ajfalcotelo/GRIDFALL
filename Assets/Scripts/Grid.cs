using TMPro;
using UnityEngine;

public class Grid
{
    private readonly int width;
    private readonly int height;
    private readonly float cellSize;
    private readonly Vector3 originPosition;
    private readonly PathNode[,] gridArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new PathNode[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridArray[x, y] = new PathNode(x, y);
            }
        }

        bool showDebug = false;
        if (showDebug)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GameObject gameObject = new("World_Text", typeof(TextMeshPro));
                    Transform transform = gameObject.transform;
                    transform.SetParent(null, false);
                    transform.localPosition = XYToWorldPos(x, y) + new Vector2(cellSize, cellSize) * 0.5f;
                    TextMeshPro textMesh = gameObject.GetComponent<TextMeshPro>();
                    textMesh.alignment = TextAlignmentOptions.Center;
                    textMesh.text = gridArray[x, y].ToString();
                    textMesh.fontSize = 3;
                    textMesh.color = Color.white;

                    Debug.DrawLine(XYToWorldPos(x, y), XYToWorldPos(x + 1, y), Color.white, 100f);
                    Debug.DrawLine(XYToWorldPos(x, y), XYToWorldPos(x, y + 1), Color.white, 100f);
                }
            }
            Debug.DrawLine(XYToWorldPos(0, height), XYToWorldPos(width, height), Color.white, 100f);
            Debug.DrawLine(XYToWorldPos(width, 0), XYToWorldPos(width, height), Color.white, 100f);
        }
    }

    public PathNode GetNode(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        } else
        {
            return default;
        }
    }

    public int GetGridWidth()
    {
        return width;
    }

    public int GetGridHeight()
    {
        return height;
    }

    public void WorldToXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    private Vector2 XYToWorldPos(int x, int y)
    {
        return new Vector2(x, y) * cellSize + (Vector2) originPosition;
    }

}
