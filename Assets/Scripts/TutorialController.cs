using System.Collections;
using UnityEngine;
using TMPro;

// チュートリアル画面を管理するクラス
public class TutorialController : MonoBehaviour
{
    //カメラの位置調整
    [SerializeField]
    private CameraController cameraCtrl = null;
    //プレイヤーが参加したときに表示
    [SerializeField]
    TextMeshProUGUI text;

    enum SceneState
    {
        // シーン開始直後
        Usually,
        // 一人目参加
        OnePerson,
        // 二人目参加
        TwoPerson,
    }
    SceneState currentState = SceneState.Usually;

    // Start is called before the first frame update
    void Start()
    {
        text.enabled = false;
        GameManager.Instance.JoinEnabled();
        currentState = SceneState.Usually;
        Debug.Log(currentState);
        cameraCtrl.TargetGroupSetting();
        //GameManager.Instance.CurrentSceneState = GameManager.SceneState.Tutorial;
    }

    // Update is called once per frame
    void Update()
    {
        // このシーンの進捗管理
        switch (currentState)
        {
            case SceneState.Usually:
                if (GameManager.Instance.Players.Count == 1)
                {
                    currentState = SceneState.OnePerson;
                    // 一人目参加
                    text.enabled = true;
                    text.text = "プレイヤー１が参加しました";
                    StartCoroutine("ShowText");
                    cameraCtrl.AddMember(GameManager.Instance.Players[0].transform);
                    Debug.Log(currentState);
                }
                break;
            case SceneState.OnePerson:
                if (GameManager.Instance.Players.Count == 2)
                {
                    currentState = SceneState.TwoPerson;
                    // 二人目参加
                    text.enabled = true;
                    text.text = "プレイヤー２が参加しました";
                    StartCoroutine("ShowText");
                    cameraCtrl.AddMember(GameManager.Instance.Players[1].transform);
                    Debug.Log(currentState);
                }
                break;
            case SceneState.TwoPerson:
                break;
            default:
                break;
        }
    }

    //ボタンが選択されたらテキストを表示させる
    IEnumerator ShowText()
    {
        yield return new WaitForSeconds(1.0f);
        text.text = "";
    }
}
