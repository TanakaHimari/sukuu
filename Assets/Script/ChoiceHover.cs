using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

// 選択肢ボタンにホバーしたとき、横のUIを表示したり色を変えるクラス
// ・ホバー時：UI表示、色変更
// ・ホバー解除：UI非表示、色戻す
// ・クリック時：UIを必ず非表示にする（ホバー中でも消える）
public class ChoiceHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("ホバー時の文字色")]
    public Color hoverTextColor = Color.yellow;

    [Header("ホバー時のUI色")]
    public Color hoverUIColor = Color.yellow;

    [Header("ホバー時に表示するUI（枠など）")]
    public RectTransform hoverUI;

    [Header("UI の表示位置オフセット")]
    public Vector2 offset = new Vector2(-200f, 0f);

    private TextMeshProUGUI textMesh;   // ボタン内のテキスト
    private Color normalTextColor;      // 通常時の文字色

    private Image hoverUIImage;         // hoverUI の Image
    private Color normalUIColor;        // 通常時の UI 色

    private RectTransform buttonRect;   // ボタン自身の RectTransform

    void Start()
    {
        // ボタン内のテキスト取得
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        normalTextColor = textMesh.color;

        // ボタンの位置取得
        buttonRect = GetComponent<RectTransform>();

        // hoverUI の初期設定
        if (hoverUI != null)
        {
            hoverUIImage = hoverUI.GetComponent<Image>();
            if (hoverUIImage != null)
                normalUIColor = hoverUIImage.color;

            // 最初は非表示
            hoverUI.gameObject.SetActive(false);
        }
    }

    // マウスが乗ったとき
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 文字色変更
        textMesh.color = hoverTextColor;

        if (hoverUI != null)
        {
            // UI を表示
            hoverUI.gameObject.SetActive(true);

            // ボタン位置 + オフセットに配置
            hoverUI.anchoredPosition = buttonRect.anchoredPosition + offset;

            // UI の色変更
            if (hoverUIImage != null)
                hoverUIImage.color = hoverUIColor;
        }
    }

    // マウスが離れたとき
    public void OnPointerExit(PointerEventData eventData)
    {
        HideHoverUI();
    }

    // クリックした瞬間にも UI を消す
    public void OnPointerClick(PointerEventData eventData)
    {
        HideHoverUI();
    }

    // UI を非表示にし、色も元に戻す共通処理
    private void HideHoverUI()
    {
        // 文字色を元に戻す
        textMesh.color = normalTextColor;

        if (hoverUI != null)
        {
            hoverUI.gameObject.SetActive(false);

            // UI の色も元に戻す
            if (hoverUIImage != null)
                hoverUIImage.color = normalUIColor;
        }
    }
}