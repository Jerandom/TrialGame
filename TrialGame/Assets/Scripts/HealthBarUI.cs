using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField]
    [ProgressBar(0, 100, ColorGetter = "GetHealthBarColour")]
    private float DynamicHealthBarAmount = 50f;

    [SerializeField]
    private float MaxHealth = 100f;

    [SerializeField]
    private Image HealthBarImage;

    private void Awake()
    {
        HealthBarImage = GameObject.Find("PlayerHPBar").GetComponent<Image>();

        HealthBarImage.fillAmount = getDynamicHealthBarAmountNomalized();
        HealthBarImage.color = GetHealthBarColour(DynamicHealthBarAmount);
    }

    private void Update()
    {
        HealthBarImage.fillAmount = getDynamicHealthBarAmountNomalized();
        HealthBarImage.color = GetHealthBarColour(DynamicHealthBarAmount);

        if (Input.GetKeyDown(KeyCode.F))
        {
            DynamicHealthBarAmount -= 10f;
        }
    }

    private Color GetHealthBarColour(float value)
    {
        return Color.Lerp(Color.red, Color.green, Mathf.Pow(value / 100f, 2));
    }

    public float getDynamicHealthBarAmountNomalized()
    {
        return Mathf.Lerp(HealthBarImage.fillAmount, DynamicHealthBarAmount / MaxHealth, 3f * Time.deltaTime);
    }
}
