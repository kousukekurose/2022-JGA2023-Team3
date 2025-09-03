using UnityEngine;
using TMPro;
using UnityEngine.Events;

// �{�^���I�u�W�F�N�g���Ǘ�����N���X
public class ButtonObject : MonoBehaviour
{
    // ���肷�郌�C���[���w��
    [SerializeField]
    private LayerMask playerLayer = default;

    // �ҋ@���̐F���w��
    [SerializeField]
    Material normalMaterial = null;

    // �I�𒆂̐F���w��
    [SerializeField]
    Material pressedMaterial = null;

    // �I����̐F���w��
    [SerializeField]
    Material selectedMaterial = null;

    // ����ID
    int cookingId;

    // ���莞�ɕҏW����e�L�X�g���w��
    [SerializeField]
    TextMeshPro text = null;

    // ���艹�w��
    [SerializeField]
    AudioClip choiceSound = null;

    // ���O�ɎQ�Ƃ���R���|�[�l���g
    MeshRenderer meshRenderer;
    AudioSource audioSource;

    // �{�^�����莞�̃C�x���g���w��
    [SerializeField]
    UnityEvent choiceEvent = null;

    // �I���e�L�X�g
    [SerializeField]
    string choiceText = null;

    // ���肳�ꂽ������
    public bool IsFixed { private set; get; } = false;

    private void Awake()
    {
        // �R���|�[�l���g���擾
        meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // �ʏ펞�̃}�e���A����ݒ�
        meshRenderer.material = normalMaterial;
    }

    // ���̃{�^���̊e�p�����[�^�[����x�ɐݒ肵�܂��B
    public void Initialize(Material normalMaterial, Material pressedMaterial, Material selectedMaterial, int cookingId)
    {
        this.normalMaterial = normalMaterial;
        this.pressedMaterial = pressedMaterial;
        this.selectedMaterial = selectedMaterial;
        this.cookingId = cookingId;
        meshRenderer.material = normalMaterial;
    }

    // �ڐG���̏���
    public void OnTriggerStay(Collider other)
    {
        int layerMask = 1 << other.gameObject.layer;
        if ((layerMask & playerLayer) != 0)
        {
            if (!IsFixed)
            {
                // �{�^���J���[�ύX
                meshRenderer.material = pressedMaterial;
            }
        }
    }

    // �ڐG���̏���
    void OnTriggerEnter(Collider other)
    {
        int layerMask = 1 << other.gameObject.layer;
        if ((layerMask & playerLayer) != 0)
        {
            if (!IsFixed)
            {
                // �{�^���J���[�ύX
                meshRenderer.material = pressedMaterial;
            }
        }
    }


    // �ڐG���Ȃ��Ȃ����Ƃ��̏���
    public void OnTriggerExit(Collider other)
    {
        int layerMask = 1 << other.gameObject.layer;
        if ((layerMask & playerLayer) != 0)
        {
            if (!IsFixed)
            {
                // �{�^���J���[�ύX
                meshRenderer.material = normalMaterial;
            }
        }
    }

    // �{�^���I�����̏���
    public void ButtonChoice()
    {
        if (!IsFixed)
        {
            // �{�^���J���[�ύX
            meshRenderer.material = selectedMaterial;
            audioSource.PlayOneShot(choiceSound);
            if (text != null)
            {
                // �e�L�X�g�ύX
                text.text = choiceText;
            }
            choiceEvent.Invoke();
        }
    }

    // �t���O�X�V
    public void SetIsFixed()
    {
        IsFixed = true;
    }

    // �񓚂�ݒ�
    public void AnswerCookingId(int playerIndex)
    {
        GameManager.Instance.AnswerCookingIds[playerIndex] = cookingId;
    }
}
