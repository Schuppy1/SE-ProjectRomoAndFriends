using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarScript : MonoBehaviour
{
    public Slider healthBarSlider;          // your slider
    public TextMeshProUGUI healthBarValueText;  // HP text
    public HPMechanics playerHP;            // reference to your HP script

    void Start()
    {
        healthBarSlider.maxValue = playerHP.HP;
        healthBarSlider.value = playerHP.HP;
    }

    void Update()
    {
        healthBarSlider.value = playerHP.HP;

        healthBarValueText.text = playerHP.HP.ToString() + "/" +
                                  healthBarSlider.maxValue.ToString();
    }
}
