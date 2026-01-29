using System.Collections.Generic;
using UnityEngine;
using static Story;

//ヒロインの背景か親友の背景か
public enum BackgroundType
{
    Friend,
    Heroine
}

// ▼ 好感度タグ
public enum CharacterID
{ 
    None,
    CharacterA, 
    CharacterB 
}



[CreateAssetMenu(fileName = "New StoryData", menuName = "Story/StoryData")]
public class StoryData : ScriptableObject
{
    public List<Story> stories = new List<Story>();


 
}

[System.Serializable]
public class AffectionBranch
{
    public CharacterID targetCharacter; // 誰の好感度を見るか
    public int threshold;               // 何以上なら分岐するか
    public int nextIndex;               // その場合の次の Story
}




[System.Serializable]
public class Story
{
    [Header("この会話で流すBGM（任意）")]
    public AudioClip bgmClip;
    // ▼ UI 表示に必要な基本情報
    //背景タイプ
    public BackgroundType backgroundType;
    public Sprite CharacterImage;    // 立ち絵
    public Sprite CharacterNameImage;    // キャラ名
    public CharacterID speaker;// 話者（この StoryText を誰が話すか）



    // ▼ メイン会話（最初に表示される）
    [TextArea] public string StoryText;  

    // ▼ 選択肢
    public string Choice1;
    public string Choice2;

    // ▼ 選択肢専用会話
    [TextArea] public string DialogueForChoice1;
    [TextArea] public string DialogueForChoice2;

    public Sprite CharacterNameImageForChoice1;
    public Sprite CharacterNameImageForChoice2;

    // ▼ 選択肢ごとの分岐先
    public int NextIndexForChoice1;
    public int NextIndexForChoice2;

    // ▼ 共通会話に戻る Story のインデックス（必要なら使用）
    public int CommonIndex;

 
    public enum AffectionTag { None, Increase, Decrease, Keep }

    public List<AffectionBranch> affectionBranches = new List<AffectionBranch>();


    [System.Serializable]
    public class AffectionEffect
    {
        public CharacterID targetCharacter;
        public AffectionTag tag;
    }

    public AffectionEffect EffectForChoice1;
    public AffectionEffect EffectForChoice2;

    public string SceneForChoice1;
    public string SceneForChoice2;


}