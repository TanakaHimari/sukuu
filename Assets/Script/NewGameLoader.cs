using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameLoader : MonoBehaviour
{
    [SerializeField]
    [Header("ロードするシーン名を入力")]
    private string sceneName = "Scene";

    public static bool isNewGame = false;

    // ニューゲームボタンにアタッチする
    public void OnNewGame()
    {
        isNewGame = true;
        SceneManager.LoadScene(sceneName);
    }

}