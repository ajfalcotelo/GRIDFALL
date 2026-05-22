using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class test : MonoBehaviour
{
    
    [SerializeField] private Tilemap tilemap;
    private Pathfinding pathfinding;
    private PlayerInputActions inputActions;


    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Player.Click.Enable();
        inputActions.Player.Click.performed += OnClick;
    }

    void OnDisable()
    {
        inputActions.Player.Click.performed -= OnClick;
        inputActions.Player.Click.Disable();
    }

    void Start()
    {
        BoundsInt cellbounds = tilemap.cellBounds;
        pathfinding = new (cellbounds.size.x, cellbounds.size.y, cellbounds.min);
    }

    void OnClick(InputAction.CallbackContext callbackContext)
    {
        var mousePosition = Mouse.current.position.ReadValue();
        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        pathfinding.GetGrid().WorldToXY(mouseWorldPosition, out int x, out int y);
        List<PathNode> path = pathfinding.FindPath(Vector2Int.zero, new Vector2Int(x, y));
        if (path != null)
        {
            // for (int i = 0; i < path.Count - 1; i++)
            // {
            //     Debug.Log(path);
            //     Debug.DrawLine(new Vector3(path[i].X, path[i].Y) + Vector3.one * 0.5f, new Vector3(path[i+1].X, path[i+1].Y) + Vector3.one * 0.5f, Color.blue, 5f);
            // }

            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector3 start = new Vector3(path[i].X, path[i].Y, 0) + tilemap.cellBounds.min + Vector3.one * 0.5f;
                Vector3 end = new Vector3(path[i + 1].X, path[i + 1].Y, 0) + tilemap.cellBounds.min + Vector3.one * 0.5f;

                Debug.DrawLine(start, end, Color.green, 3f);
            }
        }
    }
}
