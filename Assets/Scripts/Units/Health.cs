using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField]
    private Image healthBar;

    [SerializeField]
    private UnitData unitData;

    public int CurrentHealth { get; private set; }

    void Start()
    {
        CurrentHealth = unitData.MaxHealth;
        UpdateHealthUI();
    }

    public void TakeHeal(int amount)
    {
        CurrentHealth += amount;
        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        float normalized = (float)CurrentHealth / unitData.MaxHealth;
        healthBar.fillAmount = normalized;
    }
}
