using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image healthFill;      // drag HealthBarFill here
    public HPMechanics playerHP;  // drag Player here

    private float maxHP;

    void Start()
    {
        maxHP = playerHP.HP;
    }

    void Update()
    {
        float currentHP = Mathf.Clamp(playerHP.HP, 0, maxHP);
        healthFill.fillAmount = currentHP / maxHP;
    }
}
