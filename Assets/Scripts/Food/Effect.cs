using System.Collections;
using UnityEngine;

// エフェクトを管理するクラス
public class Effect : MonoBehaviour
{
    // エフェクト再生時の音
    [SerializeField]
    AudioClip audioClip = null;

    // エフェクト削除までの時間を指定
    [SerializeField]
    float destroyCount = 1.5f;

    // 事前に参照するコンポーネント
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントを取得
        audioSource = GetComponent<AudioSource>();
        // エフェクト音再生
        audioSource.PlayOneShot(audioClip);
        // エフェクトを破棄
        EffectDestroy();
    }

    // エフェクトを削除
    private void EffectDestroy()
    {
        // コルーチン開始
        StartCoroutine(OnEffectDestroy());
    }
    IEnumerator OnEffectDestroy()
    {
        // エフェクトが終了するまで待つ
        yield return new WaitForSeconds(destroyCount);
        // エフェクト破棄
        Destroy(gameObject);
    }
}
