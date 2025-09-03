using UnityEngine;

// 予想レシピを管理するクラス
public class PredictRecipeController : MonoBehaviour
{
    // 事前に参照するコンポーネント
    Animator animator;

    // AnimatorのパラメーターID
    static readonly int showId = Animator.StringToHash("Show");
    static readonly int hideId = Animator.StringToHash("Hide");

    // アニメーション実行確認用
    bool isFirst = true;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントを取得
        animator = GetComponent<Animator>();
    }

    // 表示アニメーション
    public void ShowAnimation()
    {
        if (isFirst)
        {
            animator.SetTrigger(showId);
            isFirst = false;
        }
    }

    // 非表示アニメーション
     public void HideAnimation()
    {
        if (!isFirst)
        {
            animator.SetTrigger(hideId);
            isFirst = true;
        }
    }  
}
