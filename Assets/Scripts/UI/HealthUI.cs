
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("Health Icons")]
    [SerializeField] private Image[] healthIcons;

    [Header("Colors")]
    [SerializeField] private Color fullColor = Color.red;
    [SerializeField] private Color emptyColor = Color.gray;

    public void UpdateHealthUI(int currentHealth)
    {
        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (healthIcons[i] == null)
                continue;

            healthIcons[i].color =
                i < currentHealth ? fullColor : emptyColor;
        }
    }
}
