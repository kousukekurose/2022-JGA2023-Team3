using UnityEngine;

// �񓚉�ʂ̃{�^�����Ǘ�����N���X
public class AnswerButtonCtrl : MonoBehaviour
{
    // �{�^���I�u�W�F�N�g���w��
    [SerializeField]
    ButtonObject[] buttonObjects;

    // �񓚃{�^�����ׂĂ��Ď�
    public void MonitorAnswerButton()
    {
        // �{�^���̐��J��Ԃ�
        for (int i = 0; i < buttonObjects.Length; i++)
        {
            // ���ׂđI���ς݂ɂ���
            buttonObjects[i].SetIsFixed();
        }
    }
}
