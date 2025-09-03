using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;

// 結果画面を管理するクラス
public class ResultController : MonoBehaviour
{
    // 料理成功点数
    [SerializeField]
    int resultPoint = 500;

    // 品質点数
    [SerializeField]
    int[] qualityScore;

    // 1つ一致するごとに貰える点数
    [SerializeField]
    int point = 80;

    // 1つ不一致するごとに減る点数
    [SerializeField]
    int misPoint = 50;

    // 回答正解点数
    [SerializeField]
    int correctAnswerPoint = 300;

    // 回答不正解点数
    [SerializeField]
    int uncorrectAnswerPoint = 150;

    // 一致数
    int matchCount = 0;

    // 合計スコア
    int[] totalScore = new int[] { 0, 0 };

    // ラウンドスコア
    int roundScore;

    // 表示スコア
    int showScore;

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
    ChildArray[] ingredientsImatgesUI;
    [Serializable]
    public class ChildArray
    {
        public Image[] ingredientsImatges;
    }

    // 回答結果イメージ
    [SerializeField]
    Image[] resultImage;

    // 回答結果テキスト
    [SerializeField]
    TextMeshProUGUI[] resultText;
    [SerializeField]
    TextMeshProUGUI[] answerText;

    // 回答結果スコア
    [SerializeField]
    TextMeshProUGUI[] answerScore;

    // 料理結果
    [SerializeField]
    TextMeshProUGUI[] correctText;

    // あっている食材数
    [SerializeField]
    TextMeshProUGUI[] IngredientsNo;

    // 食材スコア
    [SerializeField]
    TextMeshProUGUI[] IngredientsScore;

    // 間違っている食材数
    [SerializeField]
    TextMeshProUGUI[] misMatchNo;

    // 間違っている食材スコア
    [SerializeField]
    TextMeshProUGUI[] misMatchScore;

    // 品質スコア
    [SerializeField]
    TextMeshProUGUI[] tmpQualityScore;

    // 合計スコア
    [SerializeField]
    TextMeshProUGUI[] totalScoreText;

    // プレイヤーの初期位置指定
    [SerializeField]
    Vector3[] firstPos;

    // SceneNavigationを指定
    [SerializeField]
    SceneNavigation sceneNavigation;

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

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントを取得
        audioSource = GetComponent<AudioSource>();
        // サウンド再生
        audioSource.clip = audioClipJingle;
        audioSource.Play();

        // State更新
        GameManager.Instance.CurrentSceneState = GameManager.SceneState.Result;
        // ActionMap更新
        GameManager.Instance.SetActionMap("Extensively");

        for (int i = 0; i < GameManager.Instance.Players.Count; i++)
        {
            roundScore = 0;
            // 初期位置指定
            GameManager.Instance.Players[i].transform.position = firstPos[i];

            // 選択した料理を表示
            dishImageUI[i].sprite = dishImage[GameManager.Instance.SelectCookingIds[i]];

            totalScore[i] = PlayerPrefs.GetInt($"Score{i}", 0);
            // 回答結果判定
            // 結果テキスト表示
            if (GameManager.Instance.IsAnswerResult(i))
            {
                showScore = correctAnswerPoint;
                resultText[i].text = "あたり";
                answerText[i].text = "あたり";
                resultImage[i].color = Color.red;
            }
            else
            {
                showScore = uncorrectAnswerPoint;
                resultText[i].text = "間違い";
                answerText[i].text = "間違い";
                resultImage[i].color = Color.blue;
            }
            // 回答結果スコア表示
            ShowScore(answerScore[i], showScore);
            roundScore += showScore;

            // 選択料理レシピ取得
            var ingredientIds = GameManager.Instance.GetIngredientIds(i);

            // 取得食材ソート
            List<int> ingredientList = GameManager.Instance.GetIngredientsList(i);


            // 取得した食材を表示
            for (int j = 0; j < ingredientList.Count; j++)
            {
                ingredientsImatgesUI[i].ingredientsImatges[j].sprite = ingredientsImatge[ingredientList[j]];
            }

            // レシピ通りの食材を持っているか
            if (IsMatch(ingredientList, ingredientIds))
            {
                // レシピ通り
                showScore = resultPoint;
                IngredientsNo[i].text = " ";
            }
            else
            {
                // 一致した食材の数分
                showScore = matchCount * point;
                ShowScore(IngredientsNo[i], matchCount);
            }
            // 食材スコア表示
            ShowScore(IngredientsScore[i], showScore);
            roundScore += showScore;

            // 不一致数取得
            var mismatchCount = IsMisMatch(ingredientList, ingredientIds);
            ShowScore(misMatchNo[i], mismatchCount);
            showScore = mismatchCount * misPoint;
            // 不一致スコア表示
            ShowScore(misMatchScore[i], showScore);
            roundScore += showScore;

            // 品質スコア取得
            List<int> qualityList = GameManager.Instance.GetQualityList(i);
            showScore = QualityResult(qualityList);
            // 品質スコア表示
            ShowScore(tmpQualityScore[i], showScore);
            roundScore += showScore;

            // 合計スコア表示
            ShowScore(totalScoreText[i], roundScore);
            totalScore[i] += roundScore;
            PlayerPrefs.SetInt($"Score{i}", totalScore[i]);

            // 遷移先指定
            if (GameManager.Instance.RoundCount >= 2)
            {
                sceneNavigation.SceneName = "FinalResult";
            }

            // ラウンドリザルト保存
            GameManager.Instance.SetResultScore(i, roundScore);
        }
        GameManager.Instance.RoundCount++;
    }

    private void Update()
    {
        // サウンド更新
        switchAudioController.SwitchAudio(audioClipJingle, audioClipBGM, audioSource);
    }

    // レシピと取得食材判定
    private bool IsMatch(List<int> cooking, IEnumerable<int> ingredientIds)
    {
        bool isMatch = true;
        matchCount = 0;
        // レシピの素材を持っているか
        foreach (var ingredientId in ingredientIds)
        {
            int count = 0;
            // 必要な食材の数を食材ごとにカウント
            count = cooking.Where(cook => cook == ingredientId).Count();
            matchCount += count;
            if (count == 0)
            {
                isMatch = false;
            }
        }
        if (isMatch)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // 不要な食材を判定 
    private int IsMisMatch(List<int> cooking, IEnumerable<int> ingredientIds)
    {
        int mismatchCount = 0;
        for (int i = 0; i < cooking.Count; i++)
        {
            // 不必要な食材だったらカウント
            if (!(ingredientIds.Any(ingredientId => ingredientId == cooking[i])))
            {
                mismatchCount++;
            }
        }
        return mismatchCount;
    }

    // 品質スコア
    private int QualityResult(List<int> qualityList)
    {
        int score = 0;
        foreach (var quality in qualityList)
        {
            score += qualityScore[quality];
        }
        return score;
    }

    // スコア表示
    private void ShowScore(TextMeshProUGUI scoreText, int score)
    {
        scoreText.text = string.Format("{0}", score);
    }
}
