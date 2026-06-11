using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Start()
    {
        transform.position = GridManager.Instance.GetGroundTilemap.GetCellCenterWorld(
            Vector3Int.RoundToInt(transform.position)
        );
    }
}
