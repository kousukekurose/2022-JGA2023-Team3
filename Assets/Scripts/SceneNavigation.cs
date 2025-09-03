using UnityEngine;

//�}���`�Ή��̑I���m�F������ׂ̃N���X
public class SceneNavigation : MonoBehaviour
{
    //�I���{�^��
    bool player1 = false;
    bool player2 = false;
    //�X�e�[�W�ړ�������ׂ̌Ăяo��
    [SerializeField]
    SceneController sceneController;

    //�V�[���ړ����閼�O
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
        //�����ݒ�I�t
        player1 = false;
        player2 = false;
    }

    //�v���C���[1�̃{�^���I��������
    public void SelectionCompletePlayer1()
    {
        player1 = true;
        ConfirmationOfSelection();
    }
    //�v���C���[�Q�̃{�^���I������
    public void SelectionCompletePlayer2()
    {
        player2 = true;
        ConfirmationOfSelection();
    }
    //��l���{�^���I�������̂��m�F�o������V�[���ړ�
    public void ConfirmationOfSelection()
    {
        //��l��true��������V�[���ړ�������
        if (player1 && player2)
        {
            sceneController.LoadNextStage(sceneName);
        }
    }

}
