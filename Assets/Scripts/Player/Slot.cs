using UnityEngine;
using UnityEngine.UI;

// �X���b�g���Ǘ�����N���X
public class Slot : MonoBehaviour
{
    public Image icon;
    ItemData itemData;

    // �A�j���[�V�����̎��s�m�F�p
    bool isFirst = true;

    // ���O�ɎQ�Ƃ���R���|�[�l���g�v
    Animator animator;

    // Animator�̃p�����[�^�[ID
    static readonly int pickUpId = Animator.StringToHash("PickUp");

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    //�A�C�e�����ǉ����ꂽ�Ƃ��ɂ��̉摜��\��
    public void AddItem(ItemData newitemData)
    {
        itemData = newitemData;
        //�擾�����A�C�e����icon��ǉ�����
        icon.sprite = newitemData.icon;
        if (isFirst)
        {
            animator.SetTrigger(pickUpId);
            isFirst = false;
        }
    }
    //�A�C�e�����폜����Ƃ��ɂ��̉摜���폜
    public void ClearSlot()
    {
        itemData = null;
        icon.sprite = null;
        if (!isFirst)
        {
            isFirst = true;
        }
    }
}
