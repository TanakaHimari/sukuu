using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StoryData", menuName = "Story/StoryData")]
public class StoryData : ScriptableObject
{
    public List<Story> stories = new List<Story>();
}


[System.Serializable]
public class Story
{
    // ▼ UI 表示に必要な基本情報
    public Sprite Background;        // 背景
    public Sprite CharacterImage;    // 立ち絵
    public string CharacterName;     // キャラ名

    // ▼ メイン会話（最初に表示される）
    [TextArea] public string StoryText;  
    // ▼ 選択肢
    public string Choice1;
    public string Choice2;

    // ▼ 選択肢専用会話
    [TextArea] public string DialogueForChoice1;
    [TextArea] public string DialogueForChoice2;

    // ▼ 選択肢ごとの分岐先
    public int NextIndexForChoice1;
    public int NextIndexForChoice2;

    // ▼ 共通会話に戻る Story のインデックス（必要なら使用）
    public int CommonIndex;

    // ▼ 好感度タグ
    public enum CharacterID { None, CharacterA, CharacterB }
    public enum AffectionTag { None, Increase, Decrease, Keep }

    [System.Serializable]
    public class AffectionEffect
    {
        public CharacterID targetCharacter;
        public AffectionTag tag;
    }

    public AffectionEffect EffectForChoice1;
    public AffectionEffect EffectForChoice2;

    // ▼ 使用済みフラグ
    public bool isUsed = false;
}