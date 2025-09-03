using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// レシピ予想UIを管理するクラス
public class PredictRecipe : MonoBehaviour
{
    // 予想レシピ
    [SerializeField]
    ChildArray[] predictRecipeImages;
    [Serializable]
    public class ChildArray
    {
        public Image[] recipeImages;
    }

    // レシピ
    [SerializeField]
    Sprite[] recipe;

    List<int> predictRecipe;

    // Update is called once per frame
    void Update()
    {
        // プレイヤー数ループ
        for (int i = 0; i < GameManager.Instance.Players.Count; i++)
        {
            // 予想レシピ取得
            predictRecipe = GameManager.Instance.GetPredictRecipe(i);
            var recipeImages = predictRecipeImages[i].recipeImages;
            // 予想レシピが空でない場合
            if (predictRecipe != null)
            {
                // 予想レシピ表示枠ループ
                for (int j = 0; j < recipeImages.Length; j++)
                {
                    // 予想レシピがあれば表示
                    if (j < predictRecipe.Count)
                    {
                        // プレイヤー[i]の予想レシピUIを更新
                        recipeImages[j].sprite = recipe[predictRecipe[j]];
                        // 表示アニメーションを再生
                        recipeImages[j].GetComponent<PredictRecipeController>().ShowAnimation();
                    }
                    // 無ければ非表示
                    else
                    {
                        // 非表示アニメーションを再生
                        recipeImages[j].GetComponent<PredictRecipeController>().HideAnimation();
                    }
                }
            }
            // 予想レシピが空の場合
            else
            {
                // 予想レシピ数ループ
                for (int j = 0; j < recipeImages.Length; j++)
                {
                    // 非表示アニメーションを再生
                    recipeImages[j].GetComponent<PredictRecipeController>().HideAnimation();
                }
            }
        }
    }
}
