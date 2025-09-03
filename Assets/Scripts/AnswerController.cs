using System;
using UnityEngine;

// 回答画面を管理するクラス
public class AnswerController : MonoBehaviour
{
    // プレイヤーの初期位置指定
    [SerializeField]
    Vector3[] firstPos;

    // レシピ表示用のマテリアル（通常時）を指定します。
    [SerializeField]
    private Material[] recipeMaterials = null;
    // レシピ表示用のマテリアル（選択時）を指定します。
    [SerializeField]
    private Material[] pressedRecipeMaterials = null;
    // レシピ表示用のマテリアル（選択後）を指定します。
    [SerializeField]
    private Material[] selectedRecipeMaterials = null;

    // 表示するボタンオブジェクトを指定
    [SerializeField]
    ChildArray[] buttonObjects;
    [Serializable]
    public class ChildArray
    {
        public ButtonObject[] buttons;
    }

    // Start is called before the first frame update
    void Start()
    {
        // State更新
        GameManager.Instance.CurrentSceneState = GameManager.SceneState.Answer;
        // ActionMap更新
        GameManager.Instance.SetActionMap("Extensively");

        // 初期位置指定
        for (int i = 0; i < GameManager.Instance.Players.Count; i++)
        {
            GameManager.Instance.Players[i].transform.position = firstPos[i];
        }

        // 初期値設定
        for (int i = 0; i < buttonObjects.Length; i++)
        {
            for (int j = 0; j < buttonObjects[i].buttons.Length; j++)
            {
                // ランダム生成したレシピ番号取得
                int tmpCookingId = GameManager.Instance.RandomRecipe[j];
                // ボタンの各パラメータの初期値設定
                buttonObjects[i].buttons[j].Initialize
                    (recipeMaterials[tmpCookingId], pressedRecipeMaterials[tmpCookingId], selectedRecipeMaterials[tmpCookingId], tmpCookingId);
            }
        }
    }
}
