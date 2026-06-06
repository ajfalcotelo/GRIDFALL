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

    [SerializeField]
    private int range;

    public int MaxHealth => maxHealth;
    public int Strength => strength;
    public int Defense => defense;
    public int Speed => speed;
    public int Range => range;
}
