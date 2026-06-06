using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Move")]
public class MoveAction : ActionBase
{
    private readonly PathNode TargetNode;

    public MoveAction(PathNode targetNode)
    {
        TargetNode = targetNode;
    }

    public override IEnumerator Execute()
    {
        Vector3 unitPosition = GameManager.Instance.CurrentUnit.GetComponent<Transform>().position;
        Vector2Int unitCellPos = GridManager.Instance.WorldToXY(unitPosition);
        List<PathNode> paths = Pathfinding.FindPath(unitCellPos, TargetNode.Position);

        if (paths != null)
        {
            for (int i = 0; i < paths.Count - 1; i++)
            {
                Vector3 start =
                    new Vector3(paths[i].Position.x, paths[i].Position.y, 0)
                    + GridManager.Instance.GetGroundTilemap.cellBounds.min
                    + Vector3.one * 0.5f;
                Vector3 end =
                    new Vector3(paths[i + 1].Position.x, paths[i + 1].Position.y, 0)
                    + GridManager.Instance.GetGroundTilemap.cellBounds.min
                    + Vector3.one * 0.5f;

                Debug.DrawLine(start, end, Color.green, 3f);
            }
        }

        var unitMoveSpeed = GameManager.Instance.CurrentUnit.GetComponent<Stats>().Speed;
        foreach (PathNode path in paths)
        {
            while (
                Vector2.Distance(
                    unitPosition,
                    GridManager.Instance.XYToWorldPos(path.Position.x, path.Position.y)
                        + Vector2.one * 0.5f
                ) > 0.05f
            )
            {
                unitPosition = Vector2.MoveTowards(
                    unitPosition,
                    GridManager.Instance.XYToWorldPos(path.Position.x, path.Position.y)
                        + Vector2.one * 0.5f,
                    unitMoveSpeed * Time.deltaTime
                );

                yield return null;
            }
        }
    }
}
