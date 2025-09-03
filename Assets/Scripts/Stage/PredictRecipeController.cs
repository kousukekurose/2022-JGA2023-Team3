using UnityEngine;

// �\�z���V�s���Ǘ�����N���X
public class PredictRecipeController : MonoBehaviour
{
    // ���O�ɎQ�Ƃ���R���|�[�l���g
    Animator animator;

    // Animator�̃p�����[�^�[ID
    static readonly int showId = Animator.StringToHash("Show");
    static readonly int hideId = Animator.StringToHash("Hide");

    // �A�j���[�V�������s�m�F�p
    bool isFirst = true;

    // Start is called before the first frame update
    void Start()
    {
        // �R���|�[�l���g���擾
        animator = GetComponent<Animator>();
    }

    // �\���A�j���[�V����
    public void ShowAnimation()
    {
        if (isFirst)
        {
            animator.SetTrigger(showId);
            isFirst = false;
        }
    }

    // ��\���A�j���[�V����
     public void HideAnimation()
    {
        if (!isFirst)
        {
            animator.SetTrigger(hideId);
            isFirst = true;
        }
    }  
}
