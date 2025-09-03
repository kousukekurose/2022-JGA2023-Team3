using UnityEngine;
using UnityEngine.UI;

// ラウンド数を表示するUIを管理するクラス
public class RoundNumberUI : MonoBehaviour
{
    // ラウンド数を表示するテキスト
    [SerializeField]
    Image roundNum = null;

    // ラウンド数を表示する画像の背景
    [SerializeField]
    Image roundImage = null;

    // 数字画像
    [SerializeField]
    Sprite[] numImage = null;

    // ラウンド画像カラー
    [SerializeField]
    Sprite[] roundColor = null;

    // ラウンド数を取得する変数
    int roundCount = 0;

    private void Awake()
    {
        // ラウンド数取得
        roundCount = GameManager.Instance.RoundCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        // ラウンドに対応した画像を表示
        roundNum.sprite = numImage[roundCount];
        roundImage.sprite = roundColor[roundCount];
    }
}
