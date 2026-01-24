using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoiceHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("ホバー時に表示するUI（任意）")]
    public GameObject hoverUI;

    [Header("ホバー時の色")]
    public Color hoverColor = new Color(1f, 0.9f, 0.6f);

    private Button button;
    private Color normalColor;

    void Start()
    {
        button = GetComponent<Button>();
        normalColor = button.colors.normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // UI を表示
        if (hoverUI != null)
            hoverUI.SetActive(true);

        // 色変更
        var cb = button.colors;
        cb.normalColor = hoverColor;
        button.colors = cb;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // UI を非表示
        if (hoverUI != null)
            hoverUI.SetActive(false);

        // 色を元に戻す
        var cb = button.colors;
        cb.normalColor = normalColor;
        button.colors = cb;
    }
}