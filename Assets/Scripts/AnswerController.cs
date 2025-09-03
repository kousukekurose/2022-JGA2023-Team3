using System;
using UnityEngine;

// �񓚉�ʂ��Ǘ�����N���X
public class AnswerController : MonoBehaviour
{
    // �v���C���[�̏����ʒu�w��
    [SerializeField]
    Vector3[] firstPos;

    // ���V�s�\���p�̃}�e���A���i�ʏ펞�j���w�肵�܂��B
    [SerializeField]
    private Material[] recipeMaterials = null;
    // ���V�s�\���p�̃}�e���A���i�I�����j���w�肵�܂��B
    [SerializeField]
    private Material[] pressedRecipeMaterials = null;
    // ���V�s�\���p�̃}�e���A���i�I����j���w�肵�܂��B
    [SerializeField]
    private Material[] selectedRecipeMaterials = null;

    // �\������{�^���I�u�W�F�N�g���w��
    [SerializeField]
    ChildArray[] buttonObjects;
    [Serializable]
    public class ChildArray
    {
        public ButtonObject[] buttons;
    }

    // Start is called before the first frame update
    void Start()
    {
        // State�X�V
        GameManager.Instance.CurrentSceneState = GameManager.SceneState.Answer;
        // ActionMap�X�V
        GameManager.Instance.SetActionMap("Extensively");

        // �����ʒu�w��
        for (int i = 0; i < GameManager.Instance.Players.Count; i++)
        {
            GameManager.Instance.Players[i].transform.position = firstPos[i];
        }

        // �����l�ݒ�
        for (int i = 0; i < buttonObjects.Length; i++)
        {
            for (int j = 0; j < buttonObjects[i].buttons.Length; j++)
            {
                // �����_�������������V�s�ԍ��擾
                int tmpCookingId = GameManager.Instance.RandomRecipe[j];
                // �{�^���̊e�p�����[�^�̏����l�ݒ�
                buttonObjects[i].buttons[j].Initialize
                    (recipeMaterials[tmpCookingId], pressedRecipeMaterials[tmpCookingId], selectedRecipeMaterials[tmpCookingId], tmpCookingId);
            }
        }
    }
}
