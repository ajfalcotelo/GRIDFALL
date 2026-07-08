using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeHighlighter : MonoBehaviour
{
    [SerializeField]
    private Tilemap hoverTilemap;

    [SerializeField]
    private Tile hoverTile;

    [SerializeField]
    private Tilemap highlightTilemap;

    [SerializeField]
    private RuleTile highlightRuleTile;

    private Dictionary<Vector2Int, PathNode> nodes;

    public void HighlightNodes(List<PathNode> list)
    {
        nodes = new();
        foreach (var node in list)
        {
            nodes.Add(node.Position, node);
        }

        foreach (var node in nodes.Values)
        {
            Vector3 pos = GridManager.Instance.NodeToWorld(node.Position);
            highlightTilemap.SetTile(Vector3Int.RoundToInt(pos), highlightRuleTile);
        }
    }

    public void ClearSetTiles()
    {
        highlightTilemap.ClearAllTiles();
        hoverTilemap.ClearAllTiles();
    }

    public void HighlightHover(Vector3 mouseWorldPosition)
    {
        Vector3Int current = hoverTilemap.WorldToCell(mouseWorldPosition);
        ClearHover();
        hoverTilemap.SetTile(current, hoverTile);
    }

    public void ClearHover()
    {
        hoverTilemap.ClearAllTiles();
    }
}
