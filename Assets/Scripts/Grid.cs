using System.Collections.Generic;
using UnityEngine;

public class NodeGrid
{
    public Dictionary<Vector2Int, PathNode> BuildGrid(int width, int height)
    {
        var nodes = new Dictionary<Vector2Int, PathNode>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                nodes.Add(new Vector2Int(x, y), new PathNode(new Vector2Int(x, y)));
            }
        }

        return nodes;
    }
}
