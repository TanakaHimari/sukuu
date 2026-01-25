using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenechange : MonoBehaviour
{

    [SerializeField]
    [Header("ロードするシーン名を入力")]
    private string sceneName = "Scene";

    /// <summary>
    /// inspector側で指定したシーン名をロードする
    /// </summary>
    public void ChangeScene()
    {
        Debug.Log("シーン移動");
        SceneManager.LoadScene(sceneName);
    }

}