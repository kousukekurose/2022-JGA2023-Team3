using UnityEngine;

// 品質エフェクトを管理するクラス
public class QualityEffect : MonoBehaviour
{
    // 消滅までの時間を指定
    [SerializeField]
    float destoryTime = 5.0f;

    // 消滅までの時間をカウント
    float count = 0;

    // 事前に参照するコンポーネント
    ParticleSystem  qualityParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントを取得
        qualityParticleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // 毎フレーム加算
        count += Time.deltaTime;
        // 指定した時間を経過したら
        if (count > destoryTime)
        {
            // パーティクルを停止
            qualityParticleSystem.Stop();
        }
    }
}
