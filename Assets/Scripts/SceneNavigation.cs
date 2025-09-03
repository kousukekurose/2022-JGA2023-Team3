using UnityEngine;

//マルチ対応の選択確認をする為のクラス
public class SceneNavigation : MonoBehaviour
{
    //選択ボタン
    bool player1 = false;
    bool player2 = false;
    //ステージ移動させる為の呼び出し
    [SerializeField]
    SceneController sceneController;

    //シーン移動する名前
    public string SceneName
    {
        get => sceneName;
        set
        {
            sceneName = value;
        }
    }
    [SerializeField]
    string sceneName;

    private void Start()
    {
        //初期設定オフ
        player1 = false;
        player2 = false;
    }

    //プレイヤー1のボタン選択が完了
    public void SelectionCompletePlayer1()
    {
        player1 = true;
        ConfirmationOfSelection();
    }
    //プレイヤー２のボタン選択完了
    public void SelectionCompletePlayer2()
    {
        player2 = true;
        ConfirmationOfSelection();
    }
    //二人がボタン選択したのを確認出来たらシーン移動
    public void ConfirmationOfSelection()
    {
        //二人がtrueだったらシーン移動させる
        if (player1 && player2)
        {
            sceneController.LoadNextStage(sceneName);
        }
    }

}
