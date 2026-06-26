using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MovementPreviewSystem : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private GameObject ghostObject;
    private SpriteRenderer ghostSR;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.widthMultiplier = 0.15f;
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
        lineRenderer.textureMode = LineTextureMode.Stretch;
        lineRenderer.alignment = LineAlignment.View;
    }

    void Start()
    {
        ghostObject = new GameObject("MovementGhostPreview");
        ghostSR = ghostObject.AddComponent<SpriteRenderer>();
        ghostSR.sortingLayerName = "Player";
        ghostObject.SetActive(false);
    }

    public void RenderPreview(IUnitRoot unit, List<PathNode> path)
    {
        if (path == null || path.Count == 0)
        {
            Clear();
            return;
        }

        var previewPath = new List<PathNode>(path);
        previewPath.Insert(0, unit.CurrentNode);

        ghostSR.sprite = unit.SpriteRenderer.sprite;
        ghostSR.color = new Color(1f, 1f, 1f, 0.8f);
        ghostObject.transform.position =
            GridManager.Instance.NodeToWorld(previewPath[^1].Position) + Vector2.one * 0.5f;
        ghostObject.SetActive(true);

        lineRenderer.positionCount = previewPath.Count;

        for (int i = 0; i < previewPath.Count; i++)
        {
            var pos =
                GridManager.Instance.NodeToWorld(previewPath[i].Position) + Vector2.one * 0.5f;

            if (i == 0 && previewPath.Count > 1)
            {
                var dir =
                    GridManager.Instance.NodeToWorld(previewPath[1].Position)
                    - GridManager.Instance.NodeToWorld(previewPath[0].Position);
                pos += dir * 0.5f;
            }

            lineRenderer.SetPosition(i, pos);
        }

        lineRenderer.enabled = true;
    }

    public void Clear()
    {
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
        ghostObject.SetActive(false);
    }
}
