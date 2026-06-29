using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeDebug : MonoBehaviour
{
    private enum DebugMode
    {
        Custom,
        Unit,
        Position,
    }

    public static NodeDebug Instance { get; private set; }

    [SerializeField]
    private DebugMode debugMode;

    [SerializeField]
    private bool showDebug;

    [SerializeField]
    private Tilemap groundTilemap;

    private Dictionary<Vector2Int, TextMeshPro> text = new();
    private Dictionary<Vector2Int, string> customText = new();
    private Transform debugContainer;

    void Awake()
    {
        Instance = this;

        debugContainer = new GameObject("Debug_Container").transform;
        debugContainer.SetParent(transform);
    }

    void Start()
    {
        for (int x = 0; x < groundTilemap.cellBounds.size.x; x++)
        for (int y = 0; y < groundTilemap.cellBounds.size.y; y++)
        {
            var cell = new Vector2Int(x, y);
            GameObject gameObject = new("World_Text", typeof(TextMeshPro));
            Transform transform = gameObject.transform;
            transform.SetParent(debugContainer, false);
            transform.position = GridManager.Instance.NodeToWorld(cell) + Vector2.one * 0.5f;
            TextMeshPro textMesh = gameObject.GetComponent<TextMeshPro>();
            textMesh.alignment = TextAlignmentOptions.Center;
            textMesh.fontSize = 3;
            textMesh.color = Color.white;
            text.Add(cell, textMesh);
        }
    }

    void Update()
    {
        if (showDebug)
        {
            for (int x = 0; x < groundTilemap.cellBounds.size.x; x++)
            for (int y = 0; y < groundTilemap.cellBounds.size.y; y++)
            {
                var cell = new Vector2Int(x, y);
                if (text.TryGetValue(cell, out TextMeshPro value))
                {
                    string debugText = debugMode switch
                    {
                        DebugMode.Custom => customText.TryGetValue(cell, out string val) ? val : "",
                        DebugMode.Unit => $"{cell}",
                        DebugMode.Position =>
                            $"{GridManager.Instance.OccupancyLayer.GetNode(cell)}",
                        _ => "",
                    };

                    value.text = debugText;
                }
            }
        }
    }

    // call on scripts for custom texts
    public void UpdateText(Dictionary<Vector2Int, string> text)
    {
        debugMode = DebugMode.Custom;
        customText = text;
    }
}
