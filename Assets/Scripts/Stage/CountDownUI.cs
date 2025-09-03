using UnityEngine;
using TMPro;

// ゲームプレイ時間を管理するクラス
public class CountDownUI : MonoBehaviour
{
    // コンポーネント指定
    [SerializeField]
    SceneController sceneController = null;

    [SerializeField]
    GameObject predictRecipe;

    // ゲームプレイ時間
    [SerializeField]
    public float time = 90;

    [SerializeField]
    TextMeshProUGUI text = null;

    [SerializeField]
    AudioClip timeUpSE = null;

    AudioSource audioSource;
    Animator animator;
    // AnimatorのパラメーターID
    static readonly int endCountId = Animator.StringToHash("EndCount");

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void TimeCount()
    {
        // テキスト表示
        text.text = string.Format("{0:0}:{1:00}", (int)time / 60, (int)time % 60);
        time -= Time.deltaTime;

        // 0になったら終了
        if (time < 0)
        {
            time = 0;
            sceneController.LoadNextStage("Answer");
        }
        if (time < 6)
        {
            predictRecipe.SetActive(false);
            animator.SetTrigger(endCountId);
        }
    }
    public void TimeUpSE()
    {
        audioSource.PlayOneShot(timeUpSE);
    }
}
