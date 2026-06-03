using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/UnitData")]
public class UnitData : ScriptableObject
{
    [SerializeField]
    private int maxHealth;

    [SerializeField]
    private int strength;

    [SerializeField]
    private int defense;

    [SerializeField]
    private int speed;

    public int MaxHealth => maxHealth;
    public int Strength => strength;
    public int Defense => defense;
    public int Speed => speed;
}
