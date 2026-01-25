using static Story;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AffectionManager : MonoBehaviour
{
    public static AffectionManager Instance;

    private Dictionary<CharacterID, int> affectionDict = new();

    private void Awake()
    {
        Instance = this;

        affectionDict[CharacterID.CharacterA] = 50;   // ← A の初期値
        affectionDict[CharacterID.CharacterB] = 99;  // ← B の初期値
    }


    // 好感度変化
    public void ApplyAffection(CharacterID id, AffectionTag tag)
    {
        if (!affectionDict.ContainsKey(id)) return;

        switch (tag)
        {
            case AffectionTag.Increase:
                affectionDict[id]++;
                break;
            case AffectionTag.Decrease:
                affectionDict[id]--;
                break;
            case AffectionTag.Keep:
                break;
        }
    }

    // 個別取得
    public int GetAffection(CharacterID id)
    {
        return affectionDict.TryGetValue(id, out int value) ? value : 0;
    }

    // ★ 全キャラの好感度をまとめて取得
    public Dictionary<CharacterID, int> GetAllAffection()
    {
        return new Dictionary<CharacterID, int>(affectionDict);
    }
}