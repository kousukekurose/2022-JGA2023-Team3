using UnityEngine;
using TMPro;

// �Q�[���v���C���Ԃ��Ǘ�����N���X
public class CountDownUI : MonoBehaviour
{
    // �R���|�[�l���g�w��
    [SerializeField]
    SceneController sceneController = null;

    [SerializeField]
    GameObject predictRecipe;

    // �Q�[���v���C����
    [SerializeField]
    public float time = 90;

    [SerializeField]
    TextMeshProUGUI text = null;

    [SerializeField]
    AudioClip timeUpSE = null;

    AudioSource audioSource;
    Animator animator;
    // Animator�̃p�����[�^�[ID
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
        // �e�L�X�g�\��
        text.text = string.Format("{0:0}:{1:00}", (int)time / 60, (int)time % 60);
        time -= Time.deltaTime;

        // 0�ɂȂ�����I��
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
