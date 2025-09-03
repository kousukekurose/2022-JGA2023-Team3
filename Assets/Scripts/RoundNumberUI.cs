using UnityEngine;
using UnityEngine.UI;

// ���E���h����\������UI���Ǘ�����N���X
public class RoundNumberUI : MonoBehaviour
{
    // ���E���h����\������e�L�X�g
    [SerializeField]
    Image roundNum = null;

    // ���E���h����\������摜�̔w�i
    [SerializeField]
    Image roundImage = null;

    // �����摜
    [SerializeField]
    Sprite[] numImage = null;

    // ���E���h�摜�J���[
    [SerializeField]
    Sprite[] roundColor = null;

    // ���E���h�����擾����ϐ�
    int roundCount = 0;

    private void Awake()
    {
        // ���E���h���擾
        roundCount = GameManager.Instance.RoundCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        // ���E���h�ɑΉ������摜��\��
        roundNum.sprite = numImage[roundCount];
        roundImage.sprite = roundColor[roundCount];
    }
}
