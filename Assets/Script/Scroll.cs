//https://note.com/what_is_picky/n/nf9b5dca6e5b6

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using System.Collections.Generic;


[System.Serializable]
public class BackgroundLayer
{
    [Tooltip("スクロール対象の画像")]
    [SerializeField]
    public Image image;

    [Tooltip("x軸のスクロール速度")]
    [Range(0.1f, 10f)]
    public float scrollSpeedX = 1f;

}

public class Scroll : MonoBehaviour
{
    //UVオフセットの最大値
    private const float k_maxLength = 1f;
    //テクスチャのプロパティ名
    private const string k_propName = "_MainTex";

    [Header("背景")]
    [SerializeField]
    //背景レイヤーのリスト
    private List<BackgroundLayer> _layers;
    //複製したマテリアルを保持
    private List<Material> _copiedMaterials = new();

    void Start()
    {
        //各レイヤーに対してマテリアルを複製し設定＆保持
        foreach (var layer in _layers)
        {
            Assert.IsNotNull(layer.image, "BackgroundLayerのImageがnullです");

            var originalMaterial = layer.image.material;
            Assert.IsNotNull(originalMaterial, "Imageのマテリアルがnullです");

            var copiedMaterial = new Material(originalMaterial);
            layer.image.material = copiedMaterial;
            _copiedMaterials.Add(copiedMaterial);
        }
    }

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            //一時停止中なら処理をスキップ
            return;
        }

        //各レイヤーに対してスクロール処理を適用
        for (int i = 0; i < _layers.Count; i++)
        {

            float speed = _layers[i].scrollSpeedX;
            //時間に応じて計算
            float x = Mathf.Repeat(Time.time * speed, k_maxLength);
            Vector2 offset = new(x, 0f);

            _copiedMaterials[i].SetTextureOffset(k_propName, offset);


        }
    }

    private void OnDestroy()
    {
        //複製したマテリアルを破棄
        foreach (var mat in _copiedMaterials)
        {
            Destroy(mat);
        }
        _copiedMaterials.Clear();


    }

}
