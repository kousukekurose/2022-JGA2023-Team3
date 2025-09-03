using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

// �v���C���[���Ǘ�����N���X
public class Player : MonoBehaviour
{
    [SerializeField]
    private LayerMask layer = default;

    [SerializeField]
    private float speed = 3;

    [SerializeField]
    PauseUI pauseUI;

    [SerializeField]
    ItemSearch itemSearch;

    [SerializeField]
    float holdTime = 3.0f;

    [SerializeField]
    int maxGeuge = 3;

    [SerializeField]
    AudioClip walk;

    [SerializeField]
    AudioClip connoisseurSatrt;

    [SerializeField]
    AudioClip connoisseurEnd;

    [SerializeField]
    AudioClip answer;

    [SerializeField]
    AudioClip retuneSE;

    [SerializeField]
    AudioClip tutorialSe;
    [SerializeField]
    TextMeshProUGUI text;

    public int FakeRecipeId { get; set; } = -1;

    public int SelectNum { get; set; } = 0;

    PlayerInput playerInput;
    AudioSource audioSource;
    public GameObject connoisseurGauge;
    public Slider slider;
    private CharacterController characterController;
    private Vector2 inputMove;
    private float verticalVelocity;
    private float turnVelocity;
    bool isChoice = false;
    bool isHold = false;
    float holdCount = 0;
    private bool[] inAreas = new bool[] { false, false };
    [SerializeField]
    string[] buttonTags = { "Button1", "Button2" };

    //�v���C���[�̏�Ԃ�\��
    enum MotionState
    {
        Locomotion,
        PickUp,
        Connoisseur,
    }
    MotionState currentState = MotionState.Locomotion;

    // �L�����N�^�[�A�j���[�V�������Ǘ����Ă���Animator���w��
    [SerializeField]
    private Animator animator = null;
    // Animator�̃p�����[�^�[ID
    static readonly int speedId = Animator.StringToHash("Speed");
    static readonly int connoisseurStartId = Animator.StringToHash("ConnoisseurStart");
    static readonly int connoisseurEndId = Animator.StringToHash("ConnoisseurEnd");
    static readonly int pickUpOrRetuenId = Animator.StringToHash("PickUpOrReturn");

    // �ړ�Action(PlayerInput������Ă΂��)
    public void OnMove(InputAction.CallbackContext context)
    {
        //PickUpOrReturn||Connoisseur�A�j���[�V��������������l��Ԃ�
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("PickUpOrReturn") || 
            animator.GetCurrentAnimatorStateInfo(0).IsTag("Connoisseur"))
        {
            speed = 0;
        }
        else
        {
            speed = 8;
        }

        if (context.started)
        {
            //wailSE�Đ�
            audioSource.Play();
        }

        // ���͒l��ێ����Ă���
        inputMove = context.ReadValue<Vector2>();
        currentState = MotionState.Locomotion;
        animator.SetFloat(speedId, 1.0f);
        // �{�^�������I���
        if (context.canceled)
        {
            //walkSE��~
            audioSource.Stop();
            animator.SetFloat(speedId, 0.0f);
        }
    }
    //�ڗ����A�N�V����
    public void OnConnoisseur(InputAction.CallbackContext context)
    {
        // �{�^�������n��
        if (context.started)
        {
            isHold = true;
            holdCount = 0f;
            animator.SetTrigger(connoisseurStartId);
            audioSource.PlayOneShot(connoisseurSatrt);
            currentState = MotionState.Connoisseur;
            connoisseurGauge.SetActive(true);
        }
        // �{�^�������I���
        else if (context.canceled)
        {
            isHold = false;
            animator.SetTrigger(connoisseurEndId);
            audioSource.PlayOneShot(connoisseurEnd);
            currentState = MotionState.Locomotion;
            connoisseurGauge.SetActive(false);
            slider.value = 0;
        }
    }

    #region Item���擾����
    //Item���擾����
    public void OnItemPickUp(InputAction.CallbackContext context)
    {
        //�{�^���������ꂽ�Ƃ��̂�
        if (context.started)
        {
            var nearItem = itemSearch.GetNearItem();
            if (nearItem == null) return;
            Food item = nearItem.GetComponent<Food>();
            animator.SetTrigger(pickUpOrRetuenId);
            currentState = MotionState.PickUp;
            //�擾����Item��foodId��Foods�Ɋi�[
            if (GameManager.Instance.Ingredients[playerInput.playerIndex].Count < 6)
            {
                Debug.Log($"item : {item}, GameManager.Instance.Ingredients : {GameManager.Instance.Ingredients}");
                GameManager.Instance.Ingredients[playerInput.playerIndex].Add(item);
                itemSearch.ItemList.Remove(item.gameObject);
                item.FoodDestroy();
            }
        }
    }
    #endregion

    #region Item��j������
    //�{�^���������ꂽ��擾���Ă����A�C�e�����o��
    public void OnItemReturn(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // �̂Ă���G���A�ɂ����
            if (inAreas[playerInput.playerIndex] == true)
            {
                var ingredient = GameManager.Instance.Ingredients[playerInput.playerIndex];
                var selectNum = SelectNum;
                // �H�ނ������Ă����
                if (ingredient.Count != 0)
                {
                    // �H�ޔj��
                    ingredient.RemoveAt(selectNum);
                    animator.SetTrigger(pickUpOrRetuenId);
                    audioSource.PlayOneShot(retuneSE);
                    StageController.Instance.GenerateEffect(playerInput.playerIndex, SelectNum);
                    // �Ō��̐H�ނ��̂Ă���
                    if (ingredient.Count == selectNum && selectNum != 0)
                    {
                        selectNum--;
                        SelectNum = selectNum;
                    }
                }
            }
        }
    }
    #endregion
    //Pause�A�N�V�����������ꂽ���ɌĂяo�����
    public void OnPause(InputAction.CallbackContext context)
    {
        //�{�^���������ꂽ��
        if (GameManager.Instance.CurrentSceneState == GameManager.SceneState.Stage)
        {
            StageController.Instance.Pause();
        }
    }
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        audioSource = GetComponent<AudioSource>();
        text.enabled = false;
        connoisseurGauge.SetActive(false);
        slider.value = 0;
    }

    private void Update()
    {
        characterController.enabled = false;
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        characterController.enabled = true;

        // ������͂Ɖ����������x����A���ݑ��x���v�Z
        var moveVelocity = new Vector3(
            inputMove.x * speed,
            verticalVelocity,
            inputMove.y * speed
        );
        // ���݃t���[���̈ړ��ʂ��ړ����x����v�Z
        var moveDelta = moveVelocity * Time.deltaTime;

        // CharacterController�Ɉړ��ʂ��w�肵�A�I�u�W�F�N�g�𓮂���
        characterController.Move(moveDelta);

        if (inputMove != Vector2.zero)
        {
            // �ړ����͂�����ꍇ�́A�U�����������s��
            // ������͂���y������̖ڕW�p�x[deg]���v�Z
            var targetAngleY = -Mathf.Atan2(inputMove.y, inputMove.x)
                * Mathf.Rad2Deg + 90;

            // �C�[�W���O���Ȃ��玟�̉�]�p�x[deg]���v�Z
            var angleY = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngleY,
                ref turnVelocity,
                0.1f
            );

            // �I�u�W�F�N�g�̉�]���X�V
            transform.rotation = Quaternion.Euler(0, angleY, 0);
            //�I�u�W�F�N�g��Y���Œ�
            characterController.enabled = false;
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
            characterController.enabled = true;
        }

        // �{�^����������Ă���ԃJ�E���g
        if (isHold)
        {
            holdCount += Time.deltaTime;
            slider.value += Time.deltaTime;
        }

        // ���Ԍo�߂Ŗڗ����A�N�V����
        if (holdCount >= holdTime)
        {
            holdCount = 0;
            var nearItem = itemSearch.GetNearItem();
            if (nearItem == null) return;
            Food item = nearItem.GetComponent<Food>();
            item.ShowQuality();
            isHold = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (CheckTag(other, "ItemReturn"))
        {
            inAreas[playerInput.playerIndex] = true;

        }

        if (CheckTag(other, "RecipeButton"))
        {
            FakeRecipeId = other.gameObject.GetComponent<RecipeButton>().CookingId;
        }
        if (CheckTag(other, "Return"))
        {
            inAreas[playerInput.playerIndex] = false;

        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.name== "ItemReturn")
        {
            inAreas[playerInput.playerIndex] = false;
            Debug.Log("Exit");
        }

    }


    public void OnTriggerStay(Collider other)
    {
        // �^�O����
        if (CheckTag(other, buttonTags[playerInput.playerIndex]))
        {
            if (isChoice)
            {
                other.GetComponent<ButtonObject>().ButtonChoice();
                isChoice = false;
            }
        }
        var layerMask = 1 << other.gameObject.layer;
        if ((layerMask & layer) != 0)
        {
            if (isChoice)
            {
                Debug.Log("���肵��");
                other.GetComponent<ButtonObject>().ButtonChoice();
                isChoice = false;
            }
        }
    }

    // �^�O�`�F�b�N
    private bool CheckTag(Collider other, string str)
    {
        if (other.CompareTag(str))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //�{�^�����I�����ꂽ��e�L�X�g��\��������
    IEnumerator ShowText()
    {
        yield return new WaitForSeconds(1.0f);
        text.text = "";
    }

    #region ���r�[�A�N�V����
    public void OnTutorialRecipeA(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            audioSource.PlayOneShot(tutorialSe);
            text.enabled = true;
            if (playerInput.playerIndex == 0)
            {
                text.text = "�v���C���[�P�����V�sA��I�����܂����B";
                StartCoroutine("ShowText");
            }
            else if (playerInput.playerIndex == 1)
            {
                text.text = "�v���C���[2�����V�sA��I�����܂����B";
                StartCoroutine("ShowText");
            }
        }

    }

    public void OnTutorialRecipeB(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            audioSource.PlayOneShot(tutorialSe);
            text.enabled = true;
            if (playerInput.playerIndex == 0)
            {
                text.text = "�v���C���[�P�����V�sB��I�����܂����B";
                StartCoroutine("ShowText");
            }
            else if (playerInput.playerIndex == 1)
            {
                text.text = "�v���C���[2�����V�sB��I�����܂����B";
                StartCoroutine("ShowText");
            }
        }

    }

    public void OnTutorialRecipeX(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            audioSource.PlayOneShot(tutorialSe);
            text.enabled = true;
            if (playerInput.playerIndex == 0)
            {
                text.text = "�v���C���[�P�����V�sX��I�����܂����B";
                StartCoroutine("ShowText");
            }
            else if (playerInput.playerIndex == 1)
            {
                text.text = "�v���C���[2�����V�sX��I�����܂����B";
                StartCoroutine("ShowText");
            }
        }
    }

    public void OnTutorialRecipeY(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            audioSource.PlayOneShot(tutorialSe);
            text.enabled = true;
            if (playerInput.playerIndex == 0)
            {
                text.text = "�v���C���[�P�����V�sY��I�����܂����B";
                StartCoroutine("ShowText");
            }
            else if (playerInput.playerIndex == 1)
            {
                text.text = "�v���C���[2�����V�sY��I�����܂����B";
                StartCoroutine("ShowText");
            }
        }
    }

    public void OnRecipeA(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            LobbyController.Instance.AddCookingId(playerInput.playerIndex, 0);
            audioSource.PlayOneShot(answer);
        }

    }

    public void OnRecipeB(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            LobbyController.Instance.AddCookingId(playerInput.playerIndex, 1);
            audioSource.PlayOneShot(answer);
        }

    }

    public void OnRecipeX(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            LobbyController.Instance.AddCookingId(playerInput.playerIndex, 2);
            audioSource.PlayOneShot(answer);
        }
    }

    public void OnRecipeY(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            LobbyController.Instance.AddCookingId(playerInput.playerIndex, 3);
            audioSource.PlayOneShot(answer);
        }
    }
    #endregion

    public void OnChoice(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isChoice = false;
        }
        else if (context.performed)
        {
            isChoice = true;
        }
        else if (context.canceled)
        {
            isChoice = false;
        }
    }

    // �t�H�[�J�X�����Ɉړ�
    public void OnLeftSelect(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (0 < SelectNum)
            {
                SelectNum--;
            }
        }
    }

    // �t�H�[�J�X���E�Ɉړ�
    public void OnRightSelect(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            var ingredient = GameManager.Instance.Ingredients[playerInput.playerIndex];
            if (ingredient.Count - 1 > SelectNum)
            {
                SelectNum++;
            }
        }
    }
}