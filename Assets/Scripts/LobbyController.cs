using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//���r�[�V�[�����Ǘ�����N���X

public class LobbyController : MonoBehaviour
{
    //�����摜��\��
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

    // �v���C���[�̏����ʒu�w��
    [SerializeField]
    Vector3[] firstPos;

    // �v���C���[�̏��������w��
    [SerializeField]
    Vector3[] firstRot;

    [SerializeField]
    AudioSource audioSource;
    // ���̃N���X�̃C���X�^���X���擾���܂��B
    public static LobbyController Instance { get; private set; }

    // RecipeA�`RecipeY�܂ł�UI�{�^�����w�肵�܂��B
    [SerializeField]
    private RecipeButton[] recipeButtons = new RecipeButton[4];
    // ���V�s�\���p�̃}�e���A���i�ʏ펞�j���w�肵�܂��B
    [SerializeField]
    private Material[] recipeMaterials = null;
    // ���V�s�\���p�̃}�e���A���i�I�����j���w�肵�܂��B
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

        // �����ʒu�w��
        for (int i = 0; i < GameManager.Instance.Players.Count; i++)
        {
            GameManager.Instance.Players[i].transform.position = firstPos[i];
            GameManager.Instance.Players[i].transform.rotation = Quaternion.Euler(firstRot[i]);
        }

        audioSource = GetComponent<AudioSource>();

        // ���V�s��������
        for (int index = 0; index < recipeButtons.Length;)
        {
            // �����_���Ń��V�sID�擾
            var cookingIndex = Random.Range(0, recipeMaterials.Length);

            // �d���`�F�b�N
            if (GameManager.Instance.RandomRecipe.Contains(cookingIndex))
            {
                // NG:��蒼��
                continue;
            }
            else
            {
                // OK:ID�o�^
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
    // �v���C���[ID�ƃ��r�[��ʓ��̃��V�s�{�^��ID���w�肵�āA�Ή�����cookingId��o�^���܂��B
    public void AddCookingId(int playerIndex, int recipeButtonIndex)
    {
        //���V�s�{�^��ID���擾
        var cookingId = recipeButtons[recipeButtonIndex].CookingId;
        //�v���C���[�I�����������ݒ�
        GameManager.Instance.SelectCookingIds[playerIndex] = cookingId;
    }
}

