using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private PlayerUnitRoot player; // temp

    [SerializeField]
    private EnemyUnitRoot enemy; // temp

    private List<IUnitRoot> units;
    private List<IUnitRoot> initiative;
    private IUnitRoot currentUnit;
    private int currentTurnIndex;

    void Start()
    {
        units = new() { player, enemy };
        BattleStart(); // temp
    }

    public void BattleStart()
    {
        initiative = units.OrderByDescending(e => e.Stats.Speed).ToList();
        currentTurnIndex = 0;
        StartUnitTurn();
    }

    public void NextUnitTurn()
    {
        currentTurnIndex++;
        if (currentTurnIndex >= initiative.Count)
        {
            currentTurnIndex = 0;
        }

        StartUnitTurn();
    }

    private void StartUnitTurn()
    {
        currentUnit = initiative[currentTurnIndex];
        currentUnit.Controller.StartTurn();
    }
}
