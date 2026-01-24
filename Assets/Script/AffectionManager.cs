using System.Collections.Generic;
using UnityEngine;

public class AffectionManager : MonoBehaviour
{
    // キャラごとの好感度を管理する辞書
    // CharacterA と CharacterB の2人分を初期化
    private Dictionary<Story.CharacterID, int> affection = new Dictionary<Story.CharacterID, int>()
    {
        { Story.CharacterID.CharacterA, 0 },
        { Story.CharacterID.CharacterB, 0 }
    };

    // ▼ Story から渡された効果を適用する
    public void ApplyEffect(Story.AffectionEffect effect)
    {
        // null や対象なしの場合は何もしない
        if (effect == null || effect.targetCharacter == Story.CharacterID.None)
            return;

        // タグに応じて好感度を変化させる
        switch (effect.tag)
        {
            case Story.AffectionTag.Increase:
                affection[effect.targetCharacter] += 1;
                break;

            case Story.AffectionTag.Decrease:
                affection[effect.targetCharacter] -= 1;
                break;

            case Story.AffectionTag.Keep:
                // 値は変えない（演出用）
                break;
        }
    }

    // ▼ 現在の好感度を取得する
    public int GetAffection(Story.CharacterID id)
    {
        return affection[id];
    }
}