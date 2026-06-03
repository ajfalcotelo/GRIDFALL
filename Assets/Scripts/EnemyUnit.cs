using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    void Start()
    {
        transform.position = GridManager.Instance.GetGroundTilemap.GetCellCenterWorld(
            Vector3Int.RoundToInt(transform.position)
        );
    }
}
