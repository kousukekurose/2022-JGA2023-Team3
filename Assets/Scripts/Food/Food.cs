using UnityEngine;

// �H�ނ��Ǘ�����N���X
public class Food : MonoBehaviour
{
    public int FoodId { get => foodId; private set => foodId = value; }
    // �H��ID
    [SerializeField]
    int foodId;

    // �i���i�����_���ݒ�j
    public int QualityId { get; private set; }
    // �����_���l�͈͎̔w��
    [SerializeField]
    int minQuality = 1;
    [SerializeField]
    int maxQuality = 4;

    // ���ŃG�t�F�N�g���w��
    [SerializeField]
    GameObject destoryEffect = null;

    // �����G�t�F�N�g���w��
    [SerializeField]
    GameObject gengerateEffect = null;

    // �i���G�t�F�N�g���w��
    [SerializeField]
    GameObject[] qualityEffect;

    // �H�ނ̃f�[�^�{�̂��擾���܂��B
    public ItemData ItemData
    {
        get => itemData;
        private set => itemData = value;
    }
    [SerializeField]
    private ItemData itemData;

    private void Start()
    {
        Instantiate(gengerateEffect, transform.position, Quaternion.identity);
        // �i���ݒ�
        QualityId = Random.Range(minQuality, maxQuality);

        //�G���[��f���Ă������߃R�����g�A�E�g(�ꉞ����ɉe���Ȃ�)
        //GetComponent<Image>().sprite = itemData.icon;
    }

    // �H�iID�ƕi��ID��Ԃ�
    public int[] GetFoodId()
    {
        int[] item = { foodId, QualityId };
        return item;
    }

    // �E��ꂽ���Ă΂��
    public void FoodDestroy()
    {
        // ���ŃG�t�F�N�g�𔭐������ď�������
        Instantiate(destoryEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // �i����\��
    public void ShowQuality()
    {
        Instantiate(qualityEffect[QualityId], transform.position, Quaternion.identity);
    }
}