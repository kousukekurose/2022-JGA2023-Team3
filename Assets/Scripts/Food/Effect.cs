using System.Collections;
using UnityEngine;

// �G�t�F�N�g���Ǘ�����N���X
public class Effect : MonoBehaviour
{
    // �G�t�F�N�g�Đ����̉�
    [SerializeField]
    AudioClip audioClip = null;

    // �G�t�F�N�g�폜�܂ł̎��Ԃ��w��
    [SerializeField]
    float destroyCount = 1.5f;

    // ���O�ɎQ�Ƃ���R���|�[�l���g
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // �R���|�[�l���g���擾
        audioSource = GetComponent<AudioSource>();
        // �G�t�F�N�g���Đ�
        audioSource.PlayOneShot(audioClip);
        // �G�t�F�N�g��j��
        EffectDestroy();
    }

    // �G�t�F�N�g���폜
    private void EffectDestroy()
    {
        // �R���[�`���J�n
        StartCoroutine(OnEffectDestroy());
    }
    IEnumerator OnEffectDestroy()
    {
        // �G�t�F�N�g���I������܂ő҂�
        yield return new WaitForSeconds(destroyCount);
        // �G�t�F�N�g�j��
        Destroy(gameObject);
    }
}
