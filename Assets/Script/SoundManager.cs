using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;


public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// 読み取りのみで書き換えは不可能
    /// </summary>
    public static SoundManager Instance { get; private set; }

    [SerializeField]
    [Header("Audio Mixer")]
    private AudioMixer audioMixer;

    [SerializeField]
    [Header("BGM用AudioSource")]
    private AudioSource bgmSource;

    [SerializeField]
    [Header("SE用AudioSource")]
    private AudioSource seSource;


    /// <summary>
    /// サウンド流すセット
    /// </summary>
    /// [System.Serializable]inspectorに表示させるためのもの
    [System.Serializable]

    public class SoundData
    {
        [Header("サウンド名")]
        public string name;
        [Header("サウンド")]
        public AudioClip clip;
        [Header("音量")]
        [Range(0, 1)]
        public float volume = 1f;
    }
    [SerializeField]
    [Header("SE 一覧")]
    private List<SoundData> seList = new();

    //名前から検索する
    private Dictionary<string, SoundData> seDict;

    private void Awake()
    {

        // すでに存在しているなら自分を破棄
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 自分を唯一のインスタンスとして登録
        Instance = this;

        // シーンをまたいでも破壊されないようにする
        DontDestroyOnLoad(gameObject);

        // SE の辞書を作成
        seDict = new Dictionary<string, SoundData>();
        foreach (var s in seList)
        {
            if (!seDict.ContainsKey(s.name))
                seDict.Add(s.name, s);
        }


    }
    /// <summary>
    /// BGM再生の関数
    /// </summary>
    /// <param name="clip">再生する BGM の AudioClip</param>
    /// <param name="volume">音量</param>
    public void PlayBGM(AudioClip clip, float volume = 1f)
    {

        //再生する曲をセット
        bgmSource.clip = clip;
        //音量をセット
        bgmSource.volume = volume;
        //再生開始
        bgmSource.Play();
    }
    /// <summary>
    /// BGMを停止する
    /// </summary>
    [Tooltip("BGMを止めるときの関数")]
    public void StopBGM()
    {
        bgmSource.Stop();
    }
    /// <summary>
    /// SE再生の関数
    /// </summary>
    /// <param name="name">サウンド名</param>
    public void PlaySE(string name)
    {
        if (seDict.TryGetValue(name, out var data))
        {
            if (seSource.isPlaying && seSource.clip != null && seSource.clip == data.clip)
                return;

            seSource.clip = data.clip;
            seSource.volume = data.volume;
            seSource.Play();
        }

    }

    /// <summary>
    /// BGM調整
    /// </summary>
    /// <param name="volume">音量</param>
  
       public void SetBGMVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        // 完全ミュート
        if (volume <= 0.0001f)
        {
            bgmSource.mute = true;
            audioMixer.SetFloat("BGMVolume", -80f);
            return;
        }

        // ミュート解除
        bgmSource.mute = false;

        float safeVolume = Mathf.Max(volume, 0.0001f);
        float dB = Mathf.Log10(safeVolume) * 20f;
        dB = Mathf.Clamp(dB, -80f, 0f);

        audioMixer.SetFloat("BGMVolume", dB);

    }

    /// <summary>
    /// SE調整
    /// </summary>
    /// <param name="volume">音量</param>
    public void SetSEVolume(float volume)
    {
        volume = Mathf.Clamp01(volume); // ★ 追加：0〜1に固定


        // 同じく SEVolume を調整
        audioMixer.SetFloat("SEVolume", Mathf.Log10(volume) * 20);
    }







}
