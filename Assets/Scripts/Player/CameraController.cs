using UnityEngine;
using Cinemachine;

// カメラの設定をするクラス
public class CameraController : MonoBehaviour
{
    // CinemachineTargetGroupのオブジェクトを指定
    [SerializeField]
    CinemachineTargetGroup cinemachineTargetGroup;

    // 初期使用しているカメラを指定
    [SerializeField]
    GameObject firstVirtualCamera = null;

    // ターゲットグループを設定
    public void TargetGroupSetting()
    {
        for (int i = 0; i < GameManager.Instance.Players.Count; i++)
        {
            cinemachineTargetGroup.AddMember(GameManager.Instance.Players[i].transform, 1, 3);
        }
        firstVirtualCamera.SetActive(false);
    }

    // ターゲットグループに追加
    public void AddMember(Transform transform)
    {
        cinemachineTargetGroup.AddMember(transform, 10, 10);
    }
}
