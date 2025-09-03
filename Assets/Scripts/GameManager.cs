using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

// �Q�[���S�̂��Ǘ�����N���X�i�V���O���g���C���X�^���X�j
public class GameManager : MonoBehaviour
{
    // �v���C���[����萔�Ɏw��
    private const int PLAYER_COUNT = 2;

    // �\������ő�\�z���V�s�����w��
    private const int SHOW_PREDICT_RECIPE = 3;

    // ���V�s�\����
    private const int RECIPE_COUNT = 4;

    // �v���C���郉�E���h��
    private const int ROUND_NUM = 3;

    // �Q������v���C���[�̃v���n�u���w��
    [SerializeField]
    GameObject[] player = null;

    //���V�s�e�[�u����CSV�\�[�X�t�@�C�����w�肵�܂��B
    [SerializeField]
    private TextAsset sourceFile = null;

    // ���O�ɎQ�Ƃ���R���|�[�l���g
    PlayerInputManager inputManager;
    AudioSource audioSource;

    // �v���C���[�Q�����̉����w��
    [SerializeField]
    AudioClip onPlayerJoin;

    // �v���C���[�̏����ʒu�w��
    [SerializeField]
    Vector3[] firstPos;

    // �v���C���[�̏��������w��
    [SerializeField]
    Vector3[] firstRot;

    // ���E���h��\������e�L�X�g
    [SerializeField]
    GameObject roundText;

    // ���V�s �e�[�u�����擾���܂��B
    public List<RecipeData> RecipeTable { get; private set; } = new();

    // �I�𗿗�ID
    public int[] SelectCookingIds { set; get; } = new int[PLAYER_COUNT];

    // �񓚗���
    public int[] AnswerCookingIds { set; get; } = new int[PLAYER_COUNT];

    // �I��H��
    public List<Food>[] Ingredients { set; get; } = new List<Food>[PLAYER_COUNT];

    //�v���C���[�̏o����
    public List<Player> Players { get; private set; } = new(PLAYER_COUNT);

    // �����_���Ŏ擾�������V�s
    public List<int> RandomRecipe { set; get; } = new(RECIPE_COUNT);

    // ���E���h��
    public int RoundCount { get; set; } = 0;

    // ���E���h�X�R�A��ۑ�
    public struct RoundResult
    {
        // ���E���h�X�R�A
        public int[] RoundScore { get; set; }
        // �I����������
        public int[] SelectCookingId { get; set; }
        // �擾�����H��
        public List<int>[] SelectIngredients { get; set; }
    }
    public RoundResult[] roundResults = new RoundResult[PLAYER_COUNT];

    PlayerInputManager playerInputManager;

    // State�Ǘ�
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
            // �V�[�����ׂ��ł��j������Ȃ�
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        //���X�g�ɓǂݍ��񂾏��𔽉f
        RecipeTable = LoadCookFromCSV();
        playerInputManager = GetComponent<PlayerInputManager>();
    }
    private void Start()
    {
        // �R���|�[�l���g���擾
        audioSource = GetComponent<AudioSource>();
        inputManager = GetComponent<PlayerInputManager>();
        inputManager.playerPrefab.GetComponent<PlayerInputManager>();
    }

    // ���ׂă��Z�b�g
    public void AllReset()
    {
        // ���E���h���̌��ʃ��Z�b�g
        for (int i = 0; i < roundResults.Length; i++)
        {
            roundResults[i].RoundScore = new int[ROUND_NUM];
            roundResults[i].SelectCookingId = new int[ROUND_NUM];
            roundResults[i].SelectIngredients = new List<int>[ROUND_NUM];
        }

        // ���v�X�R�A�A�v���C���[���Z�b�g
        for (int i = 0; i < PLAYER_COUNT; i++)
        {
            PlayerPrefs.SetInt($"Score{i}", 0);
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
        Players = new(PLAYER_COUNT);

        // ���E���h�J�E���g���Z�b�g
        RoundCount = 0;
        RoundReset();
    }

    // 1���E���h�ɕK�v�ȏ������Z�b�g
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

    // �v���C���[�̎Q�����\�ɂ���
    public void JoinEnabled()
    {
        playerInputManager.EnableJoining();
    }

    // �v���C���[�̎Q����s�\�ɂ���
    public void JoinDisabled()
    {
        playerInputManager.DisableJoining();
    }

    // ���E���h���U���g�ۑ�
    public void SetResultScore(int playerIndex, int score)
    {
        roundResults[playerIndex].RoundScore[RoundCount] = score;
        roundResults[playerIndex].SelectCookingId[RoundCount] = SelectCookingIds[playerIndex];
        roundResults[playerIndex].SelectIngredients[RoundCount] = GetIngredientsList(playerIndex);
    }

    // �v���C���[���Q���������̏���
    public void OnPlayerJoin(PlayerInput input)
    {
        input.transform.SetParent(Instance.transform, false);
        audioSource.PlayOneShot(onPlayerJoin);
        Players.Add(input.GetComponent<Player>());
        Ingredients[input.playerIndex] = new List<Food>();
        if (input.playerIndex == 0)
        {
            inputManager.playerPrefab = player[1];
            //�v���C���[�����������ꏊ���w��
            input.transform.position = firstPos[0];
            input.transform.rotation = Quaternion.Euler(firstRot[0]);
        }
        else if (input.playerIndex == 1)
        {
            //�v���C���[�����������ꏊ���w��
            input.transform.position = firstPos[1];
            input.transform.rotation = Quaternion.Euler(firstRot[1]);
            JoinDisabled();
        }
    }

    // ���ׂẴv���C���[�̃A�N�V�����}�b�v��؂�ւ��܂��B
    public void SetActionMap(string mapNameOrId)
    {
        foreach (var player in Players)
        {
            player.GetComponent<PlayerInput>().defaultActionMap = mapNameOrId;
            player.GetComponent<PlayerInput>().SwitchCurrentActionMap(mapNameOrId);
        }
    }

    //Cook�\���̂�csv�t�@�C����ǂݍ���
    public List<RecipeData> LoadCookFromCSV()
    {
        // �s�P�ʂɕ���
        var lines = sourceFile.text.Split('\n');
        var recipeTable = new List<RecipeData>(lines.Length);

        //�s�P�ʂŃ��[�v����
        for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
        {
            var line = lines[lineIndex];

            //�擪�̓X�L�b�v
            if (lineIndex == 0)
            {
                continue;
            }
            //�J�����P�ʂɕ���
            var columus = line.Split(',');
            RecipeData cook = new();
            cook.cookingId = int.Parse(columus[1]);
            cook.ingredientId = int.Parse(columus[2]);

            recipeTable.Add(cook);
        }
        return recipeTable;
    }

    // �I�𗿗����V�s��ԋp
    public IEnumerable<int> GetIngredientIds(int playerIndex)
    {
        // �I�����������̃��V�s���\�[�g���Ď擾
        var ingredientIds = RecipeTable
            .Where(recipe => recipe.cookingId == SelectCookingIds[playerIndex]).OrderBy(recipe => recipe.ingredientId).Select(recipe => recipe.ingredientId);
        return ingredientIds;
    }

    // �H�ނ�ԋp
    public List<int> GetIngredientsList(int playerIndex)
    {
        return Ingredients[playerIndex].OrderBy(ingredient => ingredient.FoodId).Select(ingredient => ingredient.FoodId).ToList();
    }

    // �i���X�R�A��ԋp
    public List<int> GetQualityList(int playerIndex)
    {
        return Ingredients[playerIndex].Select(ingredient => ingredient.QualityId).ToList();
    }

    // �\�z���V�s��ԋp
    public List<int> GetPredictRecipe(int playerIndex)
    {
        if (Ingredients[playerIndex] != null && Ingredients[playerIndex].Count != 0)
        {
            // �ꎞ�ۑ��p���X�g
            List<int> tmpIngredientsList;
            List<int[]> matchRecipe = new List<int[]>();

            // �d���폜
            tmpIngredientsList = Ingredients[playerIndex].Select(ingredient => ingredient.FoodId).Distinct().ToList();
            tmpIngredientsList.Sort();

            // ���������[�v
            for (int i = 0; i < RecipeTable.Select(recipe => recipe.cookingId).Distinct().Count(); i++)
            {
                int count = 0;
                foreach (var ingredient in tmpIngredientsList)
                {
                    // �擾�H�ނ�[i]�Ԗڂ̃��V�s�ɂӂ��܂�Ă��邩���ׂ�
                    if (RecipeTable.Where(recipe => recipe.cookingId == i)
                        .Select(recipe => recipe.ingredientId).Contains(ingredient))
                    {
                        count++;
                    }
                }
                // ���V�s�ԍ��ƈ�v����ۑ�
                int[] tmp = { i, count };
                matchRecipe.Add(tmp);
            }
            // ��v�����������̂���3�擾
            return matchRecipe
                .Where(recipe => recipe[1] != 0).OrderByDescending(recipe => recipe[1]).Select(recipe => recipe[0]).Take(SHOW_PREDICT_RECIPE).ToList();
        }
        else
        {
            return null;
        }
    }

    // �񓚌��ʔ���
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

        // �����񓚌���
        if (SelectCookingIds[selectPlayerIndex] == AnswerCookingIds[answerPlayerIndex])
        {
            // ����
            return true;
        }
        else
        {
            // �s����
            return false;
        }
    }
}
