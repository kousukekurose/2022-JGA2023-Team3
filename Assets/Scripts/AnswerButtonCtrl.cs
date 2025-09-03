using UnityEngine;

// 回答画面のボタンを管理するクラス
public class AnswerButtonCtrl : MonoBehaviour
{
    // ボタンオブジェクトを指定
    [SerializeField]
    ButtonObject[] buttonObjects;

    // 回答ボタンすべてを監視
    public void MonitorAnswerButton()
    {
        // ボタンの数繰り返し
        for (int i = 0; i < buttonObjects.Length; i++)
        {
            // すべて選択済みにする
            buttonObjects[i].SetIsFixed();
        }
    }
}
