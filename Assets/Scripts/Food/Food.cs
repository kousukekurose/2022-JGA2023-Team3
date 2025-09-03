using UnityEngine;

// 食材を管理するクラス
public class Food : MonoBehaviour
{
    public int FoodId { get => foodId; private set => foodId = value; }
    // 食材ID
    [SerializeField]
    int foodId;

    // 品質（ランダム設定）
    public int QualityId { get; private set; }
    // ランダム値の範囲指定
    [SerializeField]
    int minQuality = 1;
    [SerializeField]
    int maxQuality = 4;

    // 消滅エフェクトを指定
    [SerializeField]
    GameObject destoryEffect = null;

    // 生成エフェクトを指定
    [SerializeField]
    GameObject gengerateEffect = null;

    // 品質エフェクトを指定
    [SerializeField]
    GameObject[] qualityEffect;

    // 食材のデータ本体を取得します。
    public ItemData ItemData
    {
        get => itemData;
        private set => itemData = value;
    }
    [SerializeField]
    private ItemData itemData;

    private void Start()
    {
        Instantiate(gengerateEffect, transform.position, Quaternion.identity);
        // 品質設定
        QualityId = Random.Range(minQuality, maxQuality);

        //エラーを吐いていたためコメントアウト(一応動作に影響なし)
        //GetComponent<Image>().sprite = itemData.icon;
    }

    // 食品IDと品質IDを返す
    public int[] GetFoodId()
    {
        int[] item = { foodId, QualityId };
        return item;
    }

    // 拾われた時呼ばれる
    public void FoodDestroy()
    {
        // 消滅エフェクトを発生させて消去する
        Instantiate(destoryEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // 品質を表示
    public void ShowQuality()
    {
        Instantiate(qualityEffect[QualityId], transform.position, Quaternion.identity);
    }
}