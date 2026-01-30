using UnityEngine;

public class TitleBGMChanger : MonoBehaviour
{
    public AudioClip titleBGM;

    void Start()
    {
        SoundManager.Instance.PlayBGM(titleBGM);
    }
}
