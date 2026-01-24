using System.Collections.Generic;
using UnityEngine;

// ▼ 全ストーリーをまとめる ScriptableObject
[CreateAssetMenu(fileName = "New StoryData", menuName = "Story/StoryData")]
public class StoryData : ScriptableObject
{
    public List<Story> stories = new List<Story>();
}

[System.Serializable]
public class Story
{
    // ① 最初に必ず表示されるメイン会話
    [TextArea] public string MainDialogue;

    // ② プレイヤーの選択肢（2択）
    public string Choice1;
    public string Choice2;

    // ③ 選択肢ごとに専用で表示される会話
    [TextArea] public string DialogueForChoice1;
    [TextArea] public string DialogueForChoice2;

    // ④ 選択肢ごとの「分岐先」Storyインデックス
    //    → ここで選択肢専用の会話やイベントをやる
    public int NextIndexForChoice1;
    public int NextIndexForChoice2;

    // ⑤ 共通会話に戻るための Story インデックス
    //    → Choice1/2 の分岐が終わったあと、ここに合流する
    public int CommonIndex;

    // ▼ 好感度関連（キャラ2人用）
    public enum CharacterID
    {
        None,
        CharacterA,
        CharacterB
    }

    public enum AffectionTag
    {
        None,
        Increase,
        Decrease,
        Keep
    }

    [System.Serializable]
    public class AffectionEffect
    {
        public CharacterID targetCharacter;
        public AffectionTag tag;
    }

    public AffectionEffect EffectForChoice1;
    public AffectionEffect EffectForChoice2;
}