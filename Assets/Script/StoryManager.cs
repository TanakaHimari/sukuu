using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using static Story;
using System.Collections.Generic;


public class StoryManager : MonoBehaviour
{

    private bool isWaitingForClickAfterChoice = false;


    public static int lastChoiceStoryIndex = -1;
    public static int lastChoiceTextIndex = -1;
    public GameObject endingUI;
    public bool isEndingScene = false;


    [Header("背景親オブジェクト")]
    private GameObject currentBackground;
    public Transform backgroundParentTransform;

    [Header("親友の背景")]public GameObject friendBackgroundPrefab;
    [Header("ヒロインの背景")] public GameObject heroineBackgroundPrefab;









    public static StoryManager Instance;

    [SerializeField] private StoryData[] storyDatas;

    [Header("UI")]

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

    public CharacterID CurrentSpeaker { get; private set; }

    // キャラごとに横UIの画像を設定するためのデータ構造
    [System.Serializable]
    public class CharacterUIData
    {
        public CharacterID character; // 対象キャラ
        public Sprite uiSprite;       // そのキャラ用のUI画像
    }

    // Inspector で設定するリスト
    public CharacterUIData[] characterUIList;

    // 実行時に高速アクセスするための辞書
    private Dictionary<CharacterID, Sprite> characterUIDict;

    void Awake()
    {
        Instance = this;

        characterUIDict = new Dictionary<CharacterID, Sprite>();
        foreach (var data in characterUIList)
        {
            characterUIDict[data.character] = data.uiSprite;
        }
    }



    // 選択肢横のUI画像（Imageコンポーネント）
    public Image choiceSideUIImage;

    // 話者を設定し、UI画像も切り替える
    public void SetSpeaker(CharacterID id)
    {
        CurrentSpeaker = id;

        // ▼ 話者に応じて横UI画像を切り替える
        // characterUIDict に登録されていれば、その Sprite を取得
        if (characterUIDict.TryGetValue(id, out Sprite sprite))
        {
            // UI画像を切り替えて表示
            choiceSideUIImage.sprite = sprite;
            choiceSideUIImage.gameObject.SetActive(true);
        }
        else
        {
            // 話者が None や未登録キャラの場合 → UIを非表示にする
            choiceSideUIImage.gameObject.SetActive(false);
        }

        // ▼ ここから下は既存の話者設定処理（名前表示など）
        // 例：
        // nameText.text = GetCharacterName(id);
    }

    private void SetBackground(Story story)
    {
        if (currentBackground != null)
            Destroy(currentBackground);

        GameObject prefabToUse = null;

        switch (story.backgroundType)
        {
            case BackgroundType.Friend:
                prefabToUse = friendBackgroundPrefab;
                break;
            case BackgroundType.Heroine:
                prefabToUse = heroineBackgroundPrefab;
                break;
        }

        if (prefabToUse != null)
        {
            currentBackground = Instantiate(prefabToUse, backgroundParentTransform);
        }
    }





    private Story currentStory;

    public void ResetAllStoryData()
    {
        //リトライ用インデックスを追加
        lastChoiceStoryIndex = -1;
        lastChoiceTextIndex = -1;

        
    }


    void Start()
    {
        endingUI.SetActive(false);

        if (NewGameLoader.isNewGame)
        {
            ResetAllStoryData();
            AffectionManager.Instance.ResetAll();
            NewGameLoader.isNewGame = false;

            LoadStory(0, 0);
            return;
        }

        // ▼ リトライの場合は index を使う
        if (!isEndingScene &&
            lastChoiceStoryIndex >= 0 &&
            lastChoiceTextIndex >= 0)
        {
            LoadStory(lastChoiceStoryIndex, lastChoiceTextIndex);
            return;
        }

        // 通常開始
        LoadStory(0, 0);

    }



    void Update()
    {
        // 選択肢が無いときだけクリックで進む
        if (!choiceButton1.gameObject.activeSelf && !choiceButton2.gameObject.activeSelf)
        {
            // nextAction（キーボード）でも進む
            if (nextAction.WasPerformedThisFrame())
            {
                GoToNextByClick();
            }

            // ★ マウス左クリックでも進む
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                GoToNextByClick();
            }
        }

        if (isWaitingForClickAfterChoice)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame ||
                nextAction.WasPerformedThisFrame())
            {
                isWaitingForClickAfterChoice = false;
            }
        }


    }



    private void GoToNextByClick()
    {
        // ▼ 好感度分岐チェック
        foreach (var branch in currentStory.affectionBranches)
        {
            int current = AffectionManager.Instance.GetAffection(branch.targetCharacter);
            if (current >= branch.threshold)
            {
                LoadStory(storyIndex, branch.nextIndex);
                return;
            }
        }

        // ▼ 通常の共通分岐
        if (currentStory.CommonIndex != -1)
        {
            LoadStory(storyIndex, currentStory.CommonIndex);
        }

    }




    void ResetChoiceButtonColors()
    {
        choiceButton1.colors = defaultColor1;
        choiceButton2.colors = defaultColor2;
    }

    // ▼ Story を読み込む
    public void LoadStory(int sIndex, int tIndex)
    {Debug.Log($"LOAD: sIndex={sIndex}, tIndex={tIndex}");

 
        ResetChoiceButtonColors();

        storyIndex = sIndex;
        textIndex = tIndex;

        currentStory = storyDatas[storyIndex].stories[textIndex];
        Debug.Log($"LoadStory: speaker = {currentStory.speaker}");

        SetBackground(currentStory);
        SetSpeaker(currentStory.speaker);

        // ▼ 立ち絵・テキスト・キャラ名
        // ▼ 立ち絵
        if (currentStory.CharacterImage != null)
        {
            characterImage.gameObject.SetActive(true);
            characterImage.sprite = currentStory.CharacterImage;
        }
        else
        {
            // ★ 画像が無いときは非表示（モノローグ用）
            characterImage.gameObject.SetActive(false);
        }

        storyText.text = currentStory.StoryText;
        characterNameImage.sprite = currentStory.CharacterNameImage;

        // ▼ 選択肢の表示
        if (!string.IsNullOrEmpty(currentStory.Choice1) || !string.IsNullOrEmpty(currentStory.Choice2))
        {
            lastChoiceStoryIndex = storyIndex;
            lastChoiceTextIndex = textIndex;

            // 選択肢あり
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
            // 選択肢なし → 横UIも消す
            choiceButton1.gameObject.SetActive(false);
            choiceButton2.gameObject.SetActive(false);

            // ★ 横UIを強制的に非表示
            choiceSideUIImage.gameObject.SetActive(false);
        }


       

        // ▼ ストーリーデータの最後に到達したか判定

        bool noChoices =
    string.IsNullOrEmpty(currentStory.Choice1) &&
    string.IsNullOrEmpty(currentStory.Choice2);

        bool noNext =
            currentStory.NextIndexForChoice1 == -1 &&
            currentStory.NextIndexForChoice2 == -1 &&
            currentStory.CommonIndex == -1 &&
            string.IsNullOrEmpty(currentStory.SceneForChoice1) &&
            string.IsNullOrEmpty(currentStory.SceneForChoice2);

        bool isEnding = noChoices && noNext;

        if (isEnding)
        {
            endingUI.SetActive(true);
        }

        // BGM が設定されている場合だけ切り替える
        if (currentStory.bgmClip != null)
        {
            SoundManager.Instance.PlayBGM(currentStory.bgmClip);
        }

        bool isMonologueEnd =
    !isEndingScene &&   // ★ これが重要！
    string.IsNullOrEmpty(currentStory.Choice1) &&
    string.IsNullOrEmpty(currentStory.Choice2) &&
    currentStory.NextIndexForChoice1 == -1 &&
    currentStory.NextIndexForChoice2 == -1 &&
    currentStory.CommonIndex == -1 &&
    string.IsNullOrEmpty(currentStory.SceneForChoice1) &&
    string.IsNullOrEmpty(currentStory.SceneForChoice2);

        if (isMonologueEnd)
        {
            SceneManager.LoadScene("InGame");
            return;
        }

    }

    // ▼ 選択肢が押された時
   
        private void OnChoiceSelected(int choice)
    {
        StartCoroutine(ShowChoiceDialogue(choice));
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);


        // ▼ 好感度変化の反映
        var effect = (choice == 1) ? currentStory.EffectForChoice1 : currentStory.EffectForChoice2;

        if (effect != null)
        {
            AffectionManager.Instance.ApplyAffection(effect.targetCharacter, effect.tag);
        }



        if (choice == 1)
        {
            storyText.text = currentStory.DialogueForChoice1;

            if (currentStory.CharacterNameImageForChoice1 != null)
                characterNameImage.sprite = currentStory.CharacterNameImageForChoice1;
        }
        else
        {
            storyText.text = currentStory.DialogueForChoice2;

            if (currentStory.CharacterNameImageForChoice2 != null)
                characterNameImage.sprite = currentStory.CharacterNameImageForChoice2;
        }

        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);

        // ★ GoToNext をここで呼ばない！
        // StartCoroutine(GoToNext(choice)); ← 削除

        // ★ ShowChoiceDialogue の中で GoToNext を呼ぶ
        StartCoroutine(ShowChoiceDialogue(choice));


        DebugAffection();
    }

    private IEnumerator ShowChoiceDialogue(int choice)
    {
        // まず選択肢ボタンを消す
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);

        // 選択肢専用の会話を表示
        if (choice == 1)
        {
            storyText.text = currentStory.DialogueForChoice1;
            characterNameImage.sprite = currentStory.CharacterNameImageForChoice1;
        }
        else
        {
            storyText.text = currentStory.DialogueForChoice2;
            characterNameImage.sprite = currentStory.CharacterNameImageForChoice2;
        }

        // クリック待ち開始
        isWaitingForClickAfterChoice = true;

        // クリックされるまで待つ
        while (isWaitingForClickAfterChoice)
            yield return null;

        // クリックされたら次へ
        StartCoroutine(GoToNext(choice));

    }


    // ▼ 好感度を確認する（AとB両方）
    private void DebugAffection()
    {
        var all = AffectionManager.Instance.GetAllAffection();

        int affectionA = all[CharacterID.CharacterA];
        int affectionB = all[CharacterID.CharacterB];

        Debug.Log($"Aの好感度: {affectionA}");
        Debug.Log($"Bの好感度: {affectionB}");
    }



    private IEnumerator GoToNext(int choice)
    {
        // ★ 選択肢を押した瞬間のクリックを無効化
        yield return null;



        //yield return new WaitForSeconds(2f);

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

        isWaitingForClickAfterChoice = true;

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