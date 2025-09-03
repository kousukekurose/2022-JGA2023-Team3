using UnityEngine;
using UnityEngine.UI;

// だましレシピUIを管理するクラス
public class PredictRecipeUI : MonoBehaviour
{
    // プレイヤーを指定
    [SerializeField]
    Player player = null;

    // レシピ画像指定
    [SerializeField]
    Sprite[] recipeImage = null;

    // 事前に参照するコンポーネント
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントを取得
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // だましレシピに値があれば
        if (player.FakeRecipeId != -1)
        {
            // レシピ表示
            image.sprite = recipeImage[player.FakeRecipeId];
        }
        else
        {
            image.sprite = null;
        }
    }
}
