using UnityEngine;
using Cinemachine;

// �J�����̐ݒ������N���X
public class CameraController : MonoBehaviour
{
    // CinemachineTargetGroup�̃I�u�W�F�N�g���w��
    [SerializeField]
    CinemachineTargetGroup cinemachineTargetGroup;

    // �����g�p���Ă���J�������w��
    [SerializeField]
    GameObject firstVirtualCamera = null;

    // �^�[�Q�b�g�O���[�v��ݒ�
    public void TargetGroupSetting()
    {
        for (int i = 0; i < GameManager.Instance.Players.Count; i++)
        {
            cinemachineTargetGroup.AddMember(GameManager.Instance.Players[i].transform, 1, 3);
        }
        firstVirtualCamera.SetActive(false);
    }

    // �^�[�Q�b�g�O���[�v�ɒǉ�
    public void AddMember(Transform transform)
    {
        cinemachineTargetGroup.AddMember(transform, 10, 10);
    }
}
