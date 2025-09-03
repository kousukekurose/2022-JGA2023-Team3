using System;
using System.Collections;
using UnityEngine;

// ステージを管理するクラス
public class StageController : MonoBehaviour
{
    //ポーズUIを指定
    [SerializeField]
    private PauseUI pauseUI = null;

    //カウントダウンUIを指定
    [SerializeField]
    CountDownUI countDownUI = null;

    [SerializeField]
    private CameraController cameraCtrl = null;

    // プレイヤーの初期位置指定
    [SerializeField]
    Vector3[] firstPos;

    // プレイヤーの初期向き指定
    [SerializeField]
    Vector3[] firstRot;

    Animator animator;

    // エフェクト指定
    [SerializeField]
    GameObject effect;

    // エフェクトの生成位置指定
    [SerializeField]
    ChildArray[] GeneratePos;
    [Serializable]
    public class ChildArray
    {
        public Transform[] generatePos;
    }

    // このクラスのシングルトン インスタンスを取得
    public static StageController Instance { get; private set; }
    // ポーズ状態の場合は true、プレイ状態の場合は false
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
        //Time.timeScale から独立して Animator を更新
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        pauseUI.Hide();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.CurrentSceneState = GameManager.SceneState.Stage;
        GameManager.Instance.SetActionMap("Player");
        cameraCtrl.TargetGroupSetting();
        // 初期位置指定
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
    // プレイ状態とポーズ状態を切り替えます。
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

    //ステージ開始演出
    IEnumerator Defeat()
    {
        //5秒間停止
        yield return new WaitForSecondsRealtime(5.0f);
        gameState = SceneState.Play;
    }

    // エフェクト生成
    public void GenerateEffect(int platerIndex, int selectNum)
    {
        Instantiate(effect, GeneratePos[platerIndex].generatePos[selectNum].position, Quaternion.identity);
    }
}
