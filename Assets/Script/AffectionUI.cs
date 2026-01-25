using UnityEngine;
using TMPro;
using static Story;

public class AffectionUI : MonoBehaviour
{
    [SerializeField] private TMP_Text affectionText;

    void Update()
    {
        if (StoryManager.Instance == null)
            return;


        // 今しゃべってるキャラを取得
        var speaker = StoryManager.Instance.CurrentSpeaker;

        // 誰もしゃべってないときは非表示
        if (speaker == CharacterID.None)
        {
            affectionText.text = "";
            return;
        }

        // 好感度取得
        int value = AffectionManager.Instance.GetAffection(speaker);

        // 表示
        affectionText.text = $"{speaker} : {value}";
    }
}
