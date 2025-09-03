using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//ロビーシーンを管理するクラス

public class LobbyController : MonoBehaviour
{
    //説明画像を表示
    [SerializeField]
    Sprite imageText1;
    [SerializeField]
    Sprite imageText2;
    [SerializeField]
    Sprite imageText3;
    [SerializeField]
    Image image;
    [SerializeField]
    Image image1;

    [SerializeField]
    float animationTime = 3;

    [SerializeField]
    string sceneName;

    // プレイヤーの初期位置指定
    [SerializeField]
    Vector3[] firstPos;

    // プレイヤーの初期向き指定
    [SerializeField]
    Vector3[] firstRot;

    [SerializeField]
    AudioSource audioSource;
    // このクラスのインスタンスを取得します。
    public static LobbyController Instance { get; private set; }

    // RecipeA～RecipeYまでのUIボタンを指定します。
    [SerializeField]
    private RecipeButton[] recipeButtons = new RecipeButton[4];
    // レシピ表示用のマテリアル（通常時）を指定します。
    [SerializeField]
    private Material[] recipeMaterials = null;
    // レシピ表示用のマテリアル（選択時）を指定します。
    [SerializeField]
    private Material[] pressedRecipeMaterials = null;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        image.sprite = imageText1;
        image1.enabled = false;
        GameManager.Instance.CurrentSceneState = GameManager.SceneState.Lobby;
        GameManager.Instance.SetActionMap("Lobby");
        GameManager.Instance.RoundReset();
        if (GameManager.Instance.Players.Count == 0)
        {
            GameManager.Instance.JoinEnabled();
        }

        // 初期位置指定
        for (int i = 0; i < GameManager.Instance.Players.Count; i++)
        {
            GameManager.Instance.Players[i].transform.position = firstPos[i];
            GameManager.Instance.Players[i].transform.rotation = Quaternion.Euler(firstRot[i]);
        }

        audioSource = GetComponent<AudioSource>();

        // レシピを初期化
        for (int index = 0; index < recipeButtons.Length;)
        {
            // ランダムでレシピID取得
            var cookingIndex = Random.Range(0, recipeMaterials.Length);

            // 重複チェック
            if (GameManager.Instance.RandomRecipe.Contains(cookingIndex))
            {
                // NG:やり直し
                continue;
            }
            else
            {
                // OK:ID登録
                GameManager.Instance.RandomRecipe.Add(cookingIndex);
            }
            recipeButtons[index].Initialize(
                cookingIndex, recipeMaterials[cookingIndex], pressedRecipeMaterials[cookingIndex]);
            index++;
        }
    }


    private void Update()
    {
        LoadNextScene();
    }

    public void LoadNextScene()
    {
        if (GameManager.Instance.Players.Count == 2)
        {
            image.sprite = imageText2;
            image1.enabled = true;
            if (GameManager.Instance.SelectCookingIds[0] != -1 && GameManager.Instance.SelectCookingIds[1] != -1)
            {
                StartCoroutine(OnLoadNextScene());
            }

        }
    }

    IEnumerator OnLoadNextScene()
    {
        yield return new WaitForSeconds(animationTime);
        SceneManager.LoadScene(sceneName);
    }
    // プレイヤーIDとロビー画面内のレシピボタンIDを指定して、対応するcookingIdを登録します。
    public void AddCookingId(int playerIndex, int recipeButtonIndex)
    {
        //レシピボタンIDを取得
        var cookingId = recipeButtons[recipeButtonIndex].CookingId;
        //プレイヤー選択した料理設定
        GameManager.Instance.SelectCookingIds[playerIndex] = cookingId;
    }
}

