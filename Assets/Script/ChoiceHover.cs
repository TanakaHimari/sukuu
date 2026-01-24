using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class ChoiceHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("ホバー時の文字色")]
    public Color hoverTextColor = Color.yellow;

    [Header("ホバー時のUI色")]
    public Color hoverUIColor = Color.yellow;

    [Header("ホバー時に表示するUI")]
    public RectTransform hoverUI;

    [Header("UI の表示オフセット")]
    public Vector2 offset = new Vector2(-200f, 0f);

    private TextMeshProUGUI textMesh;
    private Color normalTextColor;

    private Image hoverUIImage;
    private Color normalUIColor;

    private RectTransform buttonRect;

    void Start()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        normalTextColor = textMesh.color;

        buttonRect = GetComponent<RectTransform>();

        if (hoverUI != null)
        {
            hoverUIImage = hoverUI.GetComponent<Image>();
            if (hoverUIImage != null)
                normalUIColor = hoverUIImage.color;

            hoverUI.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        textMesh.color = hoverTextColor;

        if (hoverUI != null)
        {
            hoverUI.gameObject.SetActive(true);

            // UI の位置をボタンの右側に移動
            hoverUI.anchoredPosition = buttonRect.anchoredPosition + offset;

            // UI の色も変更
            if (hoverUIImage != null)
                hoverUIImage.color = hoverUIColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textMesh.color = normalTextColor;

        if (hoverUI != null)
        {
            hoverUI.gameObject.SetActive(false);

            // UI の色を元に戻す
            if (hoverUIImage != null)
                hoverUIImage.color = normalUIColor;
        }
    }
}