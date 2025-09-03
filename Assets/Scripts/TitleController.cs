using UnityEngine;

// タイトル画面を管理するクラス
public class TitleController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.CurrentSceneState = GameManager.SceneState.Title;
        GameManager.Instance.AllReset();
        GameManager.Instance.JoinDisabled();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
