using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class StoryManager : MonoBehaviour
{
    [SerializeField] private StoryData[] storyDatas;

    [Header("UI")]
    [SerializeField] private GameObject background;
    private GameObject currentBackgroundParent;

    [SerializeField] private Image characterImage;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private Image characterNameImage;
    [Header("Choices")]
    [SerializeField] private Button choiceButton1;
    [SerializeField] private Button choiceButton2;
    [SerializeField] private TextMeshProUGUI choiceText1;
    [SerializeField] private TextMeshProUGUI choiceText2;

    private ColorBlock defaultColor1;
    private ColorBlock defaultColor2;

    [Header("Input")]
    public InputAction nextAction;

    // 現在の StoryData と Story のインデックス
    public int storyIndex { get; private set; }
    public int textIndex { get; private set; }

    private Story currentStory;

    void Start()
    {
        defaultColor1 = choiceButton1.colors;
        defaultColor2 = choiceButton2.colors;

        LoadStory(storyIndex, textIndex);
    }

    void Update()
    {
        // 選択肢が無いときだけクリックで進む
        if (!choiceButton1.gameObject.activeSelf && !choiceButton2.gameObject.activeSelf)
        {
            if (nextAction.WasPerformedThisFrame())
            {
                GoToNextByClick();
            }
        }
    }

    private void GoToNextByClick()
    {
        int nextIndex = currentStory.NextIndexForChoice1; // Choice1 を通常進行として扱う

        // -1 のときは進まない
        if (nextIndex != -1)
        {
            LoadStory(storyIndex, nextIndex);
        }
    }


    void ResetChoiceButtonColors()
    {
        choiceButton1.colors = defaultColor1;
        choiceButton2.colors = defaultColor2;
    }

    // ▼ Story を読み込む
    public void LoadStory(int sIndex, int tIndex)
    {
        ResetChoiceButtonColors();

        storyIndex = sIndex;
        textIndex = tIndex;

        currentStory = storyDatas[storyIndex].stories[textIndex];

        // ▼ 背景セットの切り替え
        if (currentStory.BackgroundParent != null)
        {
            currentBackgroundParent = currentStory.BackgroundParent;
            currentBackgroundParent.SetActive(true);
        }

        // ▼ 立ち絵・テキスト・キャラ名
        characterImage.sprite = currentStory.CharacterImage;
        storyText.text = currentStory.StoryText;
        characterNameImage.sprite = currentStory.CharacterNameImage;

        // ▼ 選択肢の表示
        if (!string.IsNullOrEmpty(currentStory.Choice1) || !string.IsNullOrEmpty(currentStory.Choice2))
        {
            choiceButton1.gameObject.SetActive(true);
            choiceButton2.gameObject.SetActive(true);

            choiceText1.text = currentStory.Choice1;
            choiceText2.text = currentStory.Choice2;

            choiceButton1.onClick.RemoveAllListeners();
            choiceButton2.onClick.RemoveAllListeners();

            choiceButton1.onClick.AddListener(() => OnChoiceSelected(1));
            choiceButton2.onClick.AddListener(() => OnChoiceSelected(2));
        }
        else
        {
            choiceButton1.gameObject.SetActive(false);
            choiceButton2.gameObject.SetActive(false);
        }
    }

    // ▼ 選択肢が押された時
    private void OnChoiceSelected(int choice)
    {
        currentStory.isUsed = true;

        // 選択肢専用会話を表示
        if (choice == 1)
        {
            storyText.text = currentStory.DialogueForChoice1;
        }
        else
        {
            storyText.text = currentStory.DialogueForChoice2;
        }

        // 選択肢専用会話を読ませるために一旦ボタンを消す
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);

        // 次の Story に進む処理を遅延実行
        StartCoroutine(GoToNext(choice));
    }

    private IEnumerator GoToNext(int choice)
    {
        yield return new WaitForSeconds(0.6f);

        // ▼ シーン遷移が指定されている場合はそちらへ
        if (choice == 1 && !string.IsNullOrEmpty(currentStory.SceneForChoice1))
        {
            SceneManager.LoadScene(currentStory.SceneForChoice1);
            yield break;
        }
        else if (choice == 2 && !string.IsNullOrEmpty(currentStory.SceneForChoice2))
        {
            SceneManager.LoadScene(currentStory.SceneForChoice2);
            yield break;
        }

        // ▼ 通常のストーリー分岐
        int nextIndex = -1;

        if (choice == 1)
            nextIndex = currentStory.NextIndexForChoice1;
        else
            nextIndex = currentStory.NextIndexForChoice2;

        if (nextIndex != -1)
            LoadStory(storyIndex, nextIndex);
    }

    private void OnEnable()
    {
        nextAction.Enable();
    }

    private void OnDisable()
    {
        nextAction.Disable();
    }
}