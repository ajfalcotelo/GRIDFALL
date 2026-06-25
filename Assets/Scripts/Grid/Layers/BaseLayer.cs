using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class BaseLayer<TNode>
{
    protected TNode[,] cells;
    protected Tilemap ground;

    public BaseLayer(Tilemap ground)
    {
        this.ground = ground;
    }

    public TNode GetNode(Vector2Int gridPosition)
    {
        if (!IsWithinCells(gridPosition))
            return default;

        return cells[gridPosition.x, gridPosition.y];
    }

    public TNode GetNode(Vector3 worldposition)
    {
        var pos = WorldToNode(worldposition);

        if (!IsWithinCells(pos))
            return default;

        return cells[pos.x, pos.y];
    }

    public void SetNode(Vector2Int gridPosition, TNode node)
    {
        cells[gridPosition.x, gridPosition.y] = node;
    }

    public void SetNode(Vector3 worldPosition, TNode node)
    {
        var pos = WorldToNode(worldPosition);

        if (!IsWithinCells(pos))
            cells[pos.x, pos.y] = default;

        cells[pos.x, pos.y] = node;
    }

    private Vector2Int WorldToNode(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - ground.cellBounds.min).x);
        int y = Mathf.FloorToInt((worldPosition - ground.cellBounds.min).y);

        return new Vector2Int(x, y);
    }

    private bool IsWithinCells(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < cells.GetLength(0) && pos.y >= 0 && pos.y < cells.GetLength(1);
    }

    public virtual void BuildLayer() { }
}
