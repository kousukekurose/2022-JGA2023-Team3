using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

// ゲーム全体を管理するクラス（シングルトンインスタンス）
public class GameManager : MonoBehaviour
{
    // プレイヤー数を定数に指定
    private const int PLAYER_COUNT = 2;

    // 表示する最大予想レシピ数を指定
    private const int SHOW_PREDICT_RECIPE = 3;

    // レシピ表示数
    private const int RECIPE_COUNT = 4;

    // プレイするラウンド数
    private const int ROUND_NUM = 3;

    // 参加するプレイヤーのプレハブを指定
    [SerializeField]
    GameObject[] player = null;

    //レシピテーブルのCSVソースファイルを指定します。
    [SerializeField]
    private TextAsset sourceFile = null;

    // 事前に参照するコンポーネント
    PlayerInputManager inputManager;
    AudioSource audioSource;

    // プレイヤー参加時の音を指定
    [SerializeField]
    AudioClip onPlayerJoin;

    // プレイヤーの初期位置指定
    [SerializeField]
    Vector3[] firstPos;

    // プレイヤーの初期向き指定
    [SerializeField]
    Vector3[] firstRot;

    // ラウンドを表示するテキスト
    [SerializeField]
    GameObject roundText;

    // レシピ テーブルを取得します。
    public List<RecipeData> RecipeTable { get; private set; } = new();

    // 選択料理ID
    public int[] SelectCookingIds { set; get; } = new int[PLAYER_COUNT];

    // 回答料理
    public int[] AnswerCookingIds { set; get; } = new int[PLAYER_COUNT];

    // 選択食材
    public List<Food>[] Ingredients { set; get; } = new List<Food>[PLAYER_COUNT];

    //プレイヤーの出現数
    public List<Player> Players { get; private set; } = new(PLAYER_COUNT);

    // ランダムで取得したレシピ
    public List<int> RandomRecipe { set; get; } = new(RECIPE_COUNT);

    // ラウンド数
    public int RoundCount { get; set; } = 0;

    // ラウンドスコアを保存
    public struct RoundResult
    {
        // ラウンドスコア
        public int[] RoundScore { get; set; }
        // 選択した料理
        public int[] SelectCookingId { get; set; }
        // 取得した食材
        public List<int>[] SelectIngredients { get; set; }
    }
    public RoundResult[] roundResults = new RoundResult[PLAYER_COUNT];

    PlayerInputManager playerInputManager;

    // State管理
    public enum SceneState
    {
        Title,
        Explanation,
        Lobby,
        Stage,
        Answer,
        Result,
        FinalResult,
        Setting,
    }
    public SceneState CurrentSceneState { get; set; } = SceneState.Title;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
        private set => instance = value;
    }
    private static GameManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            Instance = this;
            // シーンを跨いでも破棄されない
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        //リストに読み込んだ情報を反映
        RecipeTable = LoadCookFromCSV();
        playerInputManager = GetComponent<PlayerInputManager>();
    }
    private void Start()
    {
        // コンポーネントを取得
        audioSource = GetComponent<AudioSource>();
        inputManager = GetComponent<PlayerInputManager>();
        inputManager.playerPrefab.GetComponent<PlayerInputManager>();
    }

    // すべてリセット
    public void AllReset()
    {
        // ラウンド事の結果リセット
        for (int i = 0; i < roundResults.Length; i++)
        {
            roundResults[i].RoundScore = new int[ROUND_NUM];
            roundResults[i].SelectCookingId = new int[ROUND_NUM];
            roundResults[i].SelectIngredients = new List<int>[ROUND_NUM];
        }

        // 合計スコア、プレイヤーリセット
        for (int i = 0; i < PLAYER_COUNT; i++)
        {
            PlayerPrefs.SetInt($"Score{i}", 0);
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
        Players = new(PLAYER_COUNT);

        // ラウンドカウントリセット
        RoundCount = 0;
        RoundReset();
    }

    // 1ラウンドに必要な情報をリセット
    public void RoundReset()
    {
        for (int i = 0; i < PLAYER_COUNT; i++)
        {
            SelectCookingIds[i] = -1;
            Ingredients[i] = new List<Food>();
            if (Players.Count != 0)
            {
                Players[i].SelectNum = 0;
                Players[i].FakeRecipeId = -1;
            }
        }
        AnswerCookingIds = new int[PLAYER_COUNT];
        RandomRecipe = new(RECIPE_COUNT);
    }

    // プレイヤーの参加を可能にする
    public void JoinEnabled()
    {
        playerInputManager.EnableJoining();
    }

    // プレイヤーの参加を不可能にする
    public void JoinDisabled()
    {
        playerInputManager.DisableJoining();
    }

    // ラウンドリザルト保存
    public void SetResultScore(int playerIndex, int score)
    {
        roundResults[playerIndex].RoundScore[RoundCount] = score;
        roundResults[playerIndex].SelectCookingId[RoundCount] = SelectCookingIds[playerIndex];
        roundResults[playerIndex].SelectIngredients[RoundCount] = GetIngredientsList(playerIndex);
    }

    // プレイヤーが参加した時の処理
    public void OnPlayerJoin(PlayerInput input)
    {
        input.transform.SetParent(Instance.transform, false);
        audioSource.PlayOneShot(onPlayerJoin);
        Players.Add(input.GetComponent<Player>());
        Ingredients[input.playerIndex] = new List<Food>();
        if (input.playerIndex == 0)
        {
            inputManager.playerPrefab = player[1];
            //プレイヤーが生成される場所を指定
            input.transform.position = firstPos[0];
            input.transform.rotation = Quaternion.Euler(firstRot[0]);
        }
        else if (input.playerIndex == 1)
        {
            //プレイヤーが生成される場所を指定
            input.transform.position = firstPos[1];
            input.transform.rotation = Quaternion.Euler(firstRot[1]);
            JoinDisabled();
        }
    }

    // すべてのプレイヤーのアクションマップを切り替えます。
    public void SetActionMap(string mapNameOrId)
    {
        foreach (var player in Players)
        {
            player.GetComponent<PlayerInput>().defaultActionMap = mapNameOrId;
            player.GetComponent<PlayerInput>().SwitchCurrentActionMap(mapNameOrId);
        }
    }

    //Cook構造体のcsvファイルを読み込む
    public List<RecipeData> LoadCookFromCSV()
    {
        // 行単位に分割
        var lines = sourceFile.text.Split('\n');
        var recipeTable = new List<RecipeData>(lines.Length);

        //行単位でループ処理
        for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
        {
            var line = lines[lineIndex];

            //先頭はスキップ
            if (lineIndex == 0)
            {
                continue;
            }
            //カラム単位に分割
            var columus = line.Split(',');
            RecipeData cook = new();
            cook.cookingId = int.Parse(columus[1]);
            cook.ingredientId = int.Parse(columus[2]);

            recipeTable.Add(cook);
        }
        return recipeTable;
    }

    // 選択料理レシピを返却
    public IEnumerable<int> GetIngredientIds(int playerIndex)
    {
        // 選択した料理のレシピをソートして取得
        var ingredientIds = RecipeTable
            .Where(recipe => recipe.cookingId == SelectCookingIds[playerIndex]).OrderBy(recipe => recipe.ingredientId).Select(recipe => recipe.ingredientId);
        return ingredientIds;
    }

    // 食材を返却
    public List<int> GetIngredientsList(int playerIndex)
    {
        return Ingredients[playerIndex].OrderBy(ingredient => ingredient.FoodId).Select(ingredient => ingredient.FoodId).ToList();
    }

    // 品質スコアを返却
    public List<int> GetQualityList(int playerIndex)
    {
        return Ingredients[playerIndex].Select(ingredient => ingredient.QualityId).ToList();
    }

    // 予想レシピを返却
    public List<int> GetPredictRecipe(int playerIndex)
    {
        if (Ingredients[playerIndex] != null && Ingredients[playerIndex].Count != 0)
        {
            // 一時保存用リスト
            List<int> tmpIngredientsList;
            List<int[]> matchRecipe = new List<int[]>();

            // 重複削除
            tmpIngredientsList = Ingredients[playerIndex].Select(ingredient => ingredient.FoodId).Distinct().ToList();
            tmpIngredientsList.Sort();

            // 料理数ループ
            for (int i = 0; i < RecipeTable.Select(recipe => recipe.cookingId).Distinct().Count(); i++)
            {
                int count = 0;
                foreach (var ingredient in tmpIngredientsList)
                {
                    // 取得食材が[i]番目のレシピにふくまれているか調べる
                    if (RecipeTable.Where(recipe => recipe.cookingId == i)
                        .Select(recipe => recipe.ingredientId).Contains(ingredient))
                    {
                        count++;
                    }
                }
                // レシピ番号と一致数を保存
                int[] tmp = { i, count };
                matchRecipe.Add(tmp);
            }
            // 一致数が多いものから3つ取得
            return matchRecipe
                .Where(recipe => recipe[1] != 0).OrderByDescending(recipe => recipe[1]).Select(recipe => recipe[0]).Take(SHOW_PREDICT_RECIPE).ToList();
        }
        else
        {
            return null;
        }
    }

    // 回答結果判定
    public bool IsAnswerResult(int answerPlayerIndex)
    {
        int selectPlayerIndex = 0;
        switch (answerPlayerIndex)
        {
            case 0:
                selectPlayerIndex = 1;
                break;
            case 1:
                selectPlayerIndex = 0;
                break;
            default:
                break;
        }

        // 料理回答結果
        if (SelectCookingIds[selectPlayerIndex] == AnswerCookingIds[answerPlayerIndex])
        {
            // 正解
            return true;
        }
        else
        {
            // 不正解
            return false;
        }
    }
}
