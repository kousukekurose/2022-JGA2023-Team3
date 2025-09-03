using UnityEngine;

//���V�s�̃}�e���A�����Ǘ�����N���X

public class RecipeButton : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerLayer = default;

    //���̃��V�sID���擾���܂��B
    public int CookingId { get => cookingId; private set { cookingId = value; } }
    [SerializeField]
    private int cookingId = -1;
    // ���̃{�^���̒ʏ펞�}�e���A�����w�肵�܂��B
    [SerializeField]
    private Material normalMaterial;
    // ���̃{�^���̑I�����}�e���A�����w�肵�܂��B
    [SerializeField]
    private Material pressedMaterial;

    //AudioSource�^�̕ϐ�a��錾 �g�p����AudioSource�R���|�[�l���g���A�^�b�`�K�v
    AudioSource audioSource;
    new MeshRenderer renderer;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        renderer = GetComponent<MeshRenderer>();
    }

    // ���̃{�^���̊e�p�����[�^�[����x�ɐݒ肵�܂��B
    public void Initialize(int cookingId, Material normalMaterial, Material pressedMaterial)
    {
        this.CookingId = cookingId;
        this.normalMaterial = normalMaterial;
        this.pressedMaterial = pressedMaterial;
        renderer.material = normalMaterial;
    }

    void OnTriggerEnter(Collider other)
    {
       var layerMask = 1 << other.gameObject.layer;
        if ((layerMask & playerLayer) != 0)
        {
            audioSource.Play();
            GetComponent<MeshRenderer>().material = pressedMaterial;
        }
    }

    void OnTriggerExit(Collider other)
    {
        var layerMask = 1 << other.gameObject.layer;
        if ((layerMask & playerLayer) != 0)
        {
            GetComponent<MeshRenderer>().material = normalMaterial;
        }
    }
}
