using UnityEngine;

public class MoveRange : MonoBehaviour
{
    [SerializeField]
    private UnitData unitData;

    public int CurrentMoveRange { get; private set; }

    void Awake()
    {
        CurrentMoveRange = unitData.Speed;
    }

    public void DecrementMoveRange(int num) => CurrentMoveRange -= num;

    public void ResetMoveRange() => CurrentMoveRange = unitData.Speed;
}
