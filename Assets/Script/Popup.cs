using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField]
    [Header("ポップアップ画面")]
    private GameObject Panel;

    public void ShowOptions()
    {
        Panel.SetActive(true);
    }

    public void HideOptions()
    {
        Panel.SetActive(false);
    }

}
