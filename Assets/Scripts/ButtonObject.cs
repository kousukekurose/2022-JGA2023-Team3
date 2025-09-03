using UnityEngine;
using TMPro;
using UnityEngine.Events;

// ボタンオブジェクトを管理するクラス
public class ButtonObject : MonoBehaviour
{
    // 判定するレイヤーを指定
    [SerializeField]
    private LayerMask playerLayer = default;

    // 待機中の色を指定
    [SerializeField]
    Material normalMaterial = null;

    // 選択中の色を指定
    [SerializeField]
    Material pressedMaterial = null;

    // 選択後の色を指定
    [SerializeField]
    Material selectedMaterial = null;

    // 料理ID
    int cookingId;

    // 決定時に編集するテキストを指定
    [SerializeField]
    TextMeshPro text = null;

    // 決定音指定
    [SerializeField]
    AudioClip choiceSound = null;

    // 事前に参照するコンポーネント
    MeshRenderer meshRenderer;
    AudioSource audioSource;

    // ボタン決定時のイベントを指定
    [SerializeField]
    UnityEvent choiceEvent = null;

    // 選択テキスト
    [SerializeField]
    string choiceText = null;

    // 決定されたか判定
    public bool IsFixed { private set; get; } = false;

    private void Awake()
    {
        // コンポーネントを取得
        meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // 通常時のマテリアルを設定
        meshRenderer.material = normalMaterial;
    }

    // このボタンの各パラメーターを一度に設定します。
    public void Initialize(Material normalMaterial, Material pressedMaterial, Material selectedMaterial, int cookingId)
    {
        this.normalMaterial = normalMaterial;
        this.pressedMaterial = pressedMaterial;
        this.selectedMaterial = selectedMaterial;
        this.cookingId = cookingId;
        meshRenderer.material = normalMaterial;
    }

    // 接触中の処理
    public void OnTriggerStay(Collider other)
    {
        int layerMask = 1 << other.gameObject.layer;
        if ((layerMask & playerLayer) != 0)
        {
            if (!IsFixed)
            {
                // ボタンカラー変更
                meshRenderer.material = pressedMaterial;
            }
        }
    }

    // 接触時の処理
    void OnTriggerEnter(Collider other)
    {
        int layerMask = 1 << other.gameObject.layer;
        if ((layerMask & playerLayer) != 0)
        {
            if (!IsFixed)
            {
                // ボタンカラー変更
                meshRenderer.material = pressedMaterial;
            }
        }
    }


    // 接触がなくなったときの処理
    public void OnTriggerExit(Collider other)
    {
        int layerMask = 1 << other.gameObject.layer;
        if ((layerMask & playerLayer) != 0)
        {
            if (!IsFixed)
            {
                // ボタンカラー変更
                meshRenderer.material = normalMaterial;
            }
        }
    }

    // ボタン選択時の処理
    public void ButtonChoice()
    {
        if (!IsFixed)
        {
            // ボタンカラー変更
            meshRenderer.material = selectedMaterial;
            audioSource.PlayOneShot(choiceSound);
            if (text != null)
            {
                // テキスト変更
                text.text = choiceText;
            }
            choiceEvent.Invoke();
        }
    }

    // フラグ更新
    public void SetIsFixed()
    {
        IsFixed = true;
    }

    // 回答を設定
    public void AnswerCookingId(int playerIndex)
    {
        GameManager.Instance.AnswerCookingIds[playerIndex] = cookingId;
    }
}
