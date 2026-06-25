using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeDebug : MonoBehaviour
{
    private Dictionary<Vector2Int, TextMeshPro> test;
    private Tilemap groundTilemap;

    void Start()
    {
        groundTilemap = GridManager.Instance.GetGroundTilemap;
        test = new();
        for (int x = 0; x < groundTilemap.cellBounds.size.x; x++)
        {
            for (int y = 0; y < groundTilemap.cellBounds.size.y; y++)
            {
                var cell = new Vector2Int(x, y);
                GameObject gameObject = new("World_Text", typeof(TextMeshPro));
                Transform transform = gameObject.transform;
                transform.SetParent(null, false);
                transform.localPosition =
                    GridManager.Instance.NodeToWorld(cell) + Vector2.one * 0.5f;
                TextMeshPro textMesh = gameObject.GetComponent<TextMeshPro>();
                textMesh.alignment = TextAlignmentOptions.Center;
                textMesh.fontSize = 3;
                textMesh.color = Color.white;
                test.Add(cell, textMesh);
            }
        }
    }

    void Update()
    {
        for (int x = 0; x < groundTilemap.cellBounds.size.x; x++)
        for (int y = 0; y < groundTilemap.cellBounds.size.y; y++)
        {
            var cell = new Vector2Int(x, y);
            var node = GridManager.Instance.OccupancyLayer.GetNode(cell);
            if (test.TryGetValue(cell, out TextMeshPro value))
            {
                value.text = $"{node}";
            }
        }
    }
}
