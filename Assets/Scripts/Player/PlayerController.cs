using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public enum PlayerActionMode
{
    None,
    Move,
    Attack,
}

[RequireComponent(typeof(IUnit))]
public class PlayerController : MonoBehaviour
{
    public event System.Action OnActionFinished = delegate { };

    [SerializeField]
    private Tile hoverTile;

    [SerializeField]
    private Tilemap hoverTilemap;

    [SerializeField]
    private InputController input;

    private Vector3Int prevHoverPosition;

    void Start()
    {
        transform.position = GridManager.Instance.GetGroundTilemap.GetCellCenterWorld(
            Vector3Int.RoundToInt(transform.position)
        );
    }

    void OnEnable()
    {
        input.Click += OnClick;
        input.Hover += OnHover;
    }

    void OnDisable()
    {
        input.Click -= OnClick;
        input.Hover -= OnHover;
    }

    public void SetActionMode(PlayerActionMode action)
    {
        switch (action)
        {
            case PlayerActionMode.Move:
                input.EnablePlayerInputActions();
                break;
            case PlayerActionMode.Attack:
            case PlayerActionMode.None:
                input.DisablePlayerInputActions();
                break;
        }
    }

    private void OnClick(Vector2 mousePosition)
    {
        PathNode target = GridManager.Instance.GetNode(
            Camera.main.ScreenToWorldPoint(mousePosition)
        );
        Debug.Log(target.Position);
        StartCoroutine(new MoveAction().Execute(new ActionContext(GetComponent<IUnit>(), target)));
        OnActionFinished.Invoke();
    }

    private void OnHover(Vector2 mousePosition)
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3Int cellPosition = hoverTilemap.WorldToCell(mouseWorldPosition);

        if (cellPosition != prevHoverPosition)
        {
            hoverTilemap.SetTile(prevHoverPosition, null);
            hoverTilemap.SetTile(cellPosition, hoverTile);
            prevHoverPosition = cellPosition;
        }
    }
}
