using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

// 最終結果画面を管理するクラス
public class FinalResultController : MonoBehaviour
{
    // 合計スコア
    int[] totalScore = { 0, 0 };

    // 勝敗イメージ
    [SerializeField]
    Image[] resultImage;

    // 勝敗テキスト
    [SerializeField]
    TextMeshProUGUI[] resultText;

    // ラウンドスコアを表示するUI
    [SerializeField]
    ChildArrayScore[] roundScoreText;
    [Serializable]
    public class ChildArrayScore
    {
        public TextMeshProUGUI[] roundScore;
    }

    // 最終スコア
    [SerializeField]
    TextMeshProUGUI[] totalScoreText;

    // 料理の画像
    [SerializeField]
    Sprite[] dishImage;

    // 料理の画像を表示するUI
    [SerializeField]
    Image[] dishImageUI;

    // 食材の画像
    [SerializeField]
    Sprite[] ingredientsImatge;

    // 食材の画像を表示するUI
    [SerializeField]
    ChildArrayIngredients[] ingredientsImatgesUI;
    [Serializable]
    public class ChildArrayIngredients
    {
        public Image[] ingredientsImatges;
    }

    // 表示するラウンド
    int[] RoundNum { set; get; } = { 0, 0 };

    // プレイヤーの初期位置指定
    [SerializeField]
    Vector3[] firstPos;

    // SwitchAudioControllerを指定
    [SerializeField]
    SwitchAudioController switchAudioController;

    // 事前に参照するコンポーネント
    AudioSource audioSource;

    // BGMを指定
    [SerializeField]
    AudioClip audioClipBGM = null;

    // ジングルを指定
    [SerializeField]
    AudioClip audioClipJingle = null;

    // スコアを表示するまでの待機時間
    [SerializeField]
    float time = 3.0f;

    // 時間をカウント
    float count = 0;

    // 勝利エフェクト
    [SerializeField]
    GameObject effect = null;

    // エフェクト生成有無
    bool isEffect = true;

    // プレイヤー事のエフェクト生成位置
    [SerializeField]
    Transform[] effectPos;

    //エフェクト生成位置
    Transform generatePos;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントを取得
        audioSource = GetComponent<AudioSource>();
        // ジングル再生
        audioSource.clip = audioClipJingle;
        audioSource.Play();

        // State更新
        GameManager.Instance.CurrentSceneState = GameManager.SceneState.FinalResult;
        // ActionMap更新
        GameManager.Instance.SetActionMap("Extensively");

        for (int i = 0; i < GameManager.Instance.Players.Count; i++)
        {
            // 初期位置指定
            GameManager.Instance.Players[i].transform.position = firstPos[i];
        }
    }

    public void Update()
    {
        count += Time.deltaTime;
        // サウンドを変更
        switchAudioController.SwitchAudio(audioClipJingle, audioClipBGM, audioSource);
        if (count > time)
        {
            // 勝敗表示
            totalScore[0] = PlayerPrefs.GetInt("Score0");
            totalScore[1] = PlayerPrefs.GetInt("Score1");

            // 勝敗テキスト表示
            if (totalScore[0] > totalScore[1])
            {
                resultText[0].text = "勝ち";
                resultImage[0].color = Color.red;
                resultText[1].text = "負け";
                resultImage[1].color = Color.blue;
                generatePos = effectPos[0];
            }
            else if (totalScore[1] > totalScore[0])
            {
                resultText[0].text = "負け";
                resultImage[0].color = Color.blue;
                resultText[1].text = "勝ち";
                resultImage[1].color = Color.red;
                generatePos = effectPos[1];
            }
            else
            {
                resultText[0].text = "引き分け";
                resultText[1].text = "引き分け";
                resultImage[0].color = Color.yellow;
                resultImage[1].color = Color.yellow;
            }

            // 勝利エフェクト生成
            if (isEffect)
            {
                if (generatePos != null)
                {
                    Instantiate(effect, generatePos.position, Quaternion.identity);
                }
                isEffect = false;
            }

            for (int i = 0; i < GameManager.Instance.Players.Count; i++)
            {
                // スコア表示
                // ラウンドスコア表示
                for (int j = 0; j < GameManager.Instance.roundResults[i].RoundScore.Length; j++)
                {
                    ShowScore(roundScoreText[i].roundScore[j], GameManager.Instance.roundResults[i].RoundScore[j]);
                }
                // 合計スコア表示
                totalScore[i] = PlayerPrefs.GetInt($"Score{i}", 0);
                ShowScore(totalScoreText[i], totalScore[i]);
            }
            for (int i = 0; i < GameManager.Instance.Players.Count; i++)
            {
                // UI表示
                ShowResultUI(i, GameManager.Instance.roundResults[i].SelectCookingId[RoundNum[i]], GameManager.Instance.roundResults[i].SelectIngredients[RoundNum[i]]);
            }
        }
    }

    // スコア表示
    private void ShowScore(TextMeshProUGUI scoreText, int score)
    {
        scoreText.text = string.Format("{0}", score);
    }

    // UI表示
    private void ShowResultUI(int playerIndex, int cookingId, List<int> ingredients)
    {
        // レシピ表示
        dishImageUI[playerIndex].sprite = dishImage[cookingId];
        // 食材表示
        for (int i = 0; i < ingredientsImatgesUI[playerIndex].ingredientsImatges.Length; i++)
        {
            // 食材を持っていない場合は表示しない
            if (ingredients.Count > i)
            {
                ingredientsImatgesUI[playerIndex].ingredientsImatges[i].sprite = ingredientsImatge[ingredients[i]];
            }
            else
            {
                ingredientsImatgesUI[playerIndex].ingredientsImatges[i].sprite = null;
            }
        }
    }

    // RoundNumを加算
    public void AddRoundNum(int playerIndex)
    {
        if (RoundNum[playerIndex] < 2)
        {
            RoundNum[playerIndex]++;
        }
    }

    // RoundNumを減算
    public void SubRoundNum(int playerIndex)
    {
        if (RoundNum[playerIndex] > 0)
        {
            RoundNum[playerIndex]--;
        }
    }
}
