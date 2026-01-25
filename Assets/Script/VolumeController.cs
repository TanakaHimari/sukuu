using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [Header("BGM スライダー")]
    [SerializeField] private Slider bgmSlider;

    [Header("SE スライダー")]
    [SerializeField] private Slider seSlider;

    private void Start()
    {
        // 初期値を反映（必要なら）
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        seSlider.onValueChanged.AddListener(SetSEVolume);
    }

    private void SetBGMVolume(float value)
    {
        SoundManager.Instance.SetBGMVolume(value);
    }

    private void SetSEVolume(float value)
    {
        SoundManager.Instance.SetSEVolume(value);
    }
}
