using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthSetup : MonoBehaviour
{
    [ContextMenu("Setup Health Bar")]
    public void SetupHealthBar()
    {
        // Si ya tiene barra, no hacer nada
        if (GetComponentInChildren<Slider>() != null) return;

        // Crear Canvas
        GameObject canvas = new GameObject("HealthBarCanvas");
        canvas.transform.SetParent(transform);
        canvas.transform.localPosition = new Vector3(0, 1.5f, 0);

        Canvas canvasComponent = canvas.AddComponent<Canvas>();
        canvasComponent.renderMode = RenderMode.WorldSpace;
        canvasComponent.worldCamera = Camera.main;

        RectTransform rect = canvas.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(100, 20);
        rect.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        // Crear Slider
        GameObject sliderObj = new GameObject("HealthSlider");
        sliderObj.transform.SetParent(canvas.transform);
        sliderObj.transform.localPosition = Vector3.zero;

        Slider slider = sliderObj.AddComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = 100;
        slider.value = 100;

        // Crear Background
        GameObject background = new GameObject("Background");
        background.transform.SetParent(sliderObj.transform);
        background.AddComponent<Image>().color = new Color(0.5f, 0, 0, 0.7f);

        RectTransform bgRect = background.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;

        // Crear Fill Area
        GameObject fillArea = new GameObject("Fill Area");
        fillArea.transform.SetParent(sliderObj.transform);

        RectTransform fillRect = fillArea.AddComponent<RectTransform>();
        fillRect.anchorMin = new Vector2(0, 0.25f);
        fillRect.anchorMax = new Vector2(1, 0.75f);
        fillRect.offsetMin = new Vector2(5, 0);
        fillRect.offsetMax = new Vector2(-5, 0);

        // Crear Fill
        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(fillArea.transform);
        fill.AddComponent<Image>().color = Color.green;

        RectTransform fillRt = fill.GetComponent<RectTransform>();
        fillRt.anchorMin = Vector2.zero;
        fillRt.anchorMax = new Vector2(0.5f, 1f);
        fillRt.sizeDelta = Vector2.zero;

        Debug.Log("✅ Barra de vida creada automáticamente");
    }
}
