using UnityEngine;
using System.Collections;



public class BGMchange : MonoBehaviour
{
    [SerializeField]
    [Header("再生するBGM")]
    private AudioClip bgmClip;

    [SerializeField]
    [Header("音量")]
    [Range(0f, 1f)]
    private float volume = 1f;

    /// <summary>
    /// Inspector で指定した BGM を再生する
    /// </summary>
    /// 
    private IEnumerator Start()
    {
        // 1フレーム待つ（SoundManager が生成されるのを待つ）
        yield return null;

        PlayBGM();
    }

    public void PlayBGM()
    {
        SoundManager.Instance.PlayBGM(bgmClip, volume);
    }



}