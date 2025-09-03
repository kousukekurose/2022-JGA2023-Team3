using UnityEngine;

// �{�^��SE���Ǘ�����N���X
public class ButtonSoundEffect : MonoBehaviour
{
    // �{�^���I����
    [SerializeField]
    AudioClip selectSound = null;

    // �{�^�����艹
    [SerializeField]
    AudioClip choiceSound = null;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        // �R���|�[�l���g���擾
        audioSource = GetComponent<AudioSource>();
    }

    // �{�^���I�����Đ�
    public void OnPlaySelectSound()
    {
        // �I������1��Đ�
        audioSource.PlayOneShot(selectSound);
    }

    // �{�^�����艹�Đ�
    public void OnPlayChoiceSound()
    {
        // ���艹��1��Đ�
        audioSource.PlayOneShot(choiceSound);
    }
}
