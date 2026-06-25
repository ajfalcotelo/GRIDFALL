using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathfindLayer : BaseLayer<PathNode>
{
    public PathfindLayer(Tilemap ground)
        : base(ground) { }

    public void BuildLayer(Tilemap obstacle)
    {
        var bounds = ground.cellBounds;
        var width = bounds.size.x;
        var height = bounds.size.y;
        cells = new PathNode[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var pos = new Vector2Int(x, y);
                SetNode(pos, new PathNode(pos));
            }
        }

        foreach (var node in cells)
        {
            node.ListNeighbors();
        }

        foreach (var pos in obstacle.cellBounds.allPositionsWithin)
        {
            if (obstacle.HasTile(pos))
            {
                GetNode(pos).IsWalkable = false;
            }
        }
    }
}
