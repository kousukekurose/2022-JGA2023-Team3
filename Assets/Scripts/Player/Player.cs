using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

// プレイヤーを管理するクラス
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

    //プレイヤーの状態を表す
    enum MotionState
    {
        Locomotion,
        PickUp,
        Connoisseur,
    }
    MotionState currentState = MotionState.Locomotion;

    // キャラクターアニメーションを管理しているAnimatorを指定
    [SerializeField]
    private Animator animator = null;
    // AnimatorのパラメーターID
    static readonly int speedId = Animator.StringToHash("Speed");
    static readonly int connoisseurStartId = Animator.StringToHash("ConnoisseurStart");
    static readonly int connoisseurEndId = Animator.StringToHash("ConnoisseurEnd");
    static readonly int pickUpOrRetuenId = Animator.StringToHash("PickUpOrReturn");

    // 移動Action(PlayerInput側から呼ばれる)
    public void OnMove(InputAction.CallbackContext context)
    {
        //PickUpOrReturn||Connoisseurアニメーション中だったら値を返す
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
            //wailSE再生
            audioSource.Play();
        }

        // 入力値を保持しておく
        inputMove = context.ReadValue<Vector2>();
        currentState = MotionState.Locomotion;
        animator.SetFloat(speedId, 1.0f);
        // ボタン押し終わり
        if (context.canceled)
        {
            //walkSE停止
            audioSource.Stop();
            animator.SetFloat(speedId, 0.0f);
        }
    }
    //目利きアクション
    public void OnConnoisseur(InputAction.CallbackContext context)
    {
        // ボタン押し始め
        if (context.started)
        {
            isHold = true;
            holdCount = 0f;
            animator.SetTrigger(connoisseurStartId);
            audioSource.PlayOneShot(connoisseurSatrt);
            currentState = MotionState.Connoisseur;
            connoisseurGauge.SetActive(true);
        }
        // ボタン押し終わり
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

    #region Itemを取得する
    //Itemを取得する
    public void OnItemPickUp(InputAction.CallbackContext context)
    {
        //ボタンが押されたときのみ
        if (context.started)
        {
            var nearItem = itemSearch.GetNearItem();
            if (nearItem == null) return;
            Food item = nearItem.GetComponent<Food>();
            animator.SetTrigger(pickUpOrRetuenId);
            currentState = MotionState.PickUp;
            //取得したItemのfoodIdをFoodsに格納
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

    #region Itemを破棄する
    //ボタンが押されたら取得していたアイテムを出す
    public void OnItemReturn(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // 捨てられるエリアにいれば
            if (inAreas[playerInput.playerIndex] == true)
            {
                var ingredient = GameManager.Instance.Ingredients[playerInput.playerIndex];
                var selectNum = SelectNum;
                // 食材を持っていれば
                if (ingredient.Count != 0)
                {
                    // 食材破棄
                    ingredient.RemoveAt(selectNum);
                    animator.SetTrigger(pickUpOrRetuenId);
                    audioSource.PlayOneShot(retuneSE);
                    StageController.Instance.GenerateEffect(playerInput.playerIndex, SelectNum);
                    // 最後列の食材を捨てたら
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
    //Pauseアクションが押された時に呼び出される
    public void OnPause(InputAction.CallbackContext context)
    {
        //ボタンが押されたら
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

        // 操作入力と鉛直方向速度から、現在速度を計算
        var moveVelocity = new Vector3(
            inputMove.x * speed,
            verticalVelocity,
            inputMove.y * speed
        );
        // 現在フレームの移動量を移動速度から計算
        var moveDelta = moveVelocity * Time.deltaTime;

        // CharacterControllerに移動量を指定し、オブジェクトを動かす
        characterController.Move(moveDelta);

        if (inputMove != Vector2.zero)
        {
            // 移動入力がある場合は、振り向き動作も行う
            // 操作入力からy軸周りの目標角度[deg]を計算
            var targetAngleY = -Mathf.Atan2(inputMove.y, inputMove.x)
                * Mathf.Rad2Deg + 90;

            // イージングしながら次の回転角度[deg]を計算
            var angleY = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngleY,
                ref turnVelocity,
                0.1f
            );

            // オブジェクトの回転を更新
            transform.rotation = Quaternion.Euler(0, angleY, 0);
            //オブジェクトのYを固定
            characterController.enabled = false;
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
            characterController.enabled = true;
        }

        // ボタンが押されている間カウント
        if (isHold)
        {
            holdCount += Time.deltaTime;
            slider.value += Time.deltaTime;
        }

        // 時間経過で目利きアクション
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
        // タグ判定
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
                Debug.Log("決定した");
                other.GetComponent<ButtonObject>().ButtonChoice();
                isChoice = false;
            }
        }
    }

    // タグチェック
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

    //ボタンが選択されたらテキストを表示させる
    IEnumerator ShowText()
    {
        yield return new WaitForSeconds(1.0f);
        text.text = "";
    }

    #region ロビーアクション
    public void OnTutorialRecipeA(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            audioSource.PlayOneShot(tutorialSe);
            text.enabled = true;
            if (playerInput.playerIndex == 0)
            {
                text.text = "プレイヤー１がレシピAを選択しました。";
                StartCoroutine("ShowText");
            }
            else if (playerInput.playerIndex == 1)
            {
                text.text = "プレイヤー2がレシピAを選択しました。";
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
                text.text = "プレイヤー１がレシピBを選択しました。";
                StartCoroutine("ShowText");
            }
            else if (playerInput.playerIndex == 1)
            {
                text.text = "プレイヤー2がレシピBを選択しました。";
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
                text.text = "プレイヤー１がレシピXを選択しました。";
                StartCoroutine("ShowText");
            }
            else if (playerInput.playerIndex == 1)
            {
                text.text = "プレイヤー2がレシピXを選択しました。";
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
                text.text = "プレイヤー１がレシピYを選択しました。";
                StartCoroutine("ShowText");
            }
            else if (playerInput.playerIndex == 1)
            {
                text.text = "プレイヤー2がレシピYを選択しました。";
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

    // フォーカスを左に移動
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

    // フォーカスを右に移動
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