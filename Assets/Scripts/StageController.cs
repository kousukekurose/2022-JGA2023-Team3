using System;
using System.Collections;
using UnityEngine;

// �X�e�[�W���Ǘ�����N���X
public class StageController : MonoBehaviour
{
    //�|�[�YUI���w��
    [SerializeField]
    private PauseUI pauseUI = null;

    //�J�E���g�_�E��UI���w��
    [SerializeField]
    CountDownUI countDownUI = null;

    [SerializeField]
    private CameraController cameraCtrl = null;

    // �v���C���[�̏����ʒu�w��
    [SerializeField]
    Vector3[] firstPos;

    // �v���C���[�̏��������w��
    [SerializeField]
    Vector3[] firstRot;

    Animator animator;

    // �G�t�F�N�g�w��
    [SerializeField]
    GameObject effect;

    // �G�t�F�N�g�̐����ʒu�w��
    [SerializeField]
    ChildArray[] GeneratePos;
    [Serializable]
    public class ChildArray
    {
        public Transform[] generatePos;
    }

    // ���̃N���X�̃V���O���g�� �C���X�^���X���擾
    public static StageController Instance { get; private set; }
    // �|�[�Y��Ԃ̏ꍇ�� true�A�v���C��Ԃ̏ꍇ�� false
    public bool IsPaused { get; private set; } = false;
    enum SceneState
    {
        Start,
        Play,
        End,
    }
    SceneState gameState = SceneState.Start;

    private void Awake()
    {
        Instance = this;
        gameState = SceneState.Start;
        animator = GetComponent<Animator>();
        //Time.timeScale ����Ɨ����� Animator ���X�V
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        pauseUI.Hide();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.CurrentSceneState = GameManager.SceneState.Stage;
        GameManager.Instance.SetActionMap("Player");
        cameraCtrl.TargetGroupSetting();
        // �����ʒu�w��
        for (int i = 0; i < GameManager.Instance.Players.Count; i++)
        {
            GameManager.Instance.Players[i].transform.position = firstPos[i];
            GameManager.Instance.Players[i].transform.rotation = Quaternion.Euler(firstRot[i]);
        }
    }
    private void Update()
    {
        switch (gameState)
        {
            case SceneState.Start:
                UpdateForStart();
                break;
            case SceneState.Play:
                countDownUI.TimeCount();
                break;
            case SceneState.End:
                break;
            default:
                break;
        }
    }
    public void Pause()
    {
        IsPaused = true;
        pauseUI.Show();
    }
    public void OnResume()
    {
        IsPaused = false;
        pauseUI.Hide();
    }
    // �v���C��Ԃƃ|�[�Y��Ԃ�؂�ւ��܂��B
    public void TogglePause()
    {
        if (IsPaused)
        {
            OnResume();
        }
        else
        {
            Pause();
        }
    }
    void UpdateForStart()
    {
        if (gameState == SceneState.Start)
        {
            StartCoroutine(Defeat());
        }
    }

    //�X�e�[�W�J�n���o
    IEnumerator Defeat()
    {
        //5�b�Ԓ�~
        yield return new WaitForSecondsRealtime(5.0f);
        gameState = SceneState.Play;
    }

    // �G�t�F�N�g����
    public void GenerateEffect(int platerIndex, int selectNum)
    {
        Instantiate(effect, GeneratePos[platerIndex].generatePos[selectNum].position, Quaternion.identity);
    }
}
