using UnityEngine;
using UnityEngine.UI;

// ���܂����V�sUI���Ǘ�����N���X
public class PredictRecipeUI : MonoBehaviour
{
    // �v���C���[���w��
    [SerializeField]
    Player player = null;

    // ���V�s�摜�w��
    [SerializeField]
    Sprite[] recipeImage = null;

    // ���O�ɎQ�Ƃ���R���|�[�l���g
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        // �R���|�[�l���g���擾
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // ���܂����V�s�ɒl�������
        if (player.FakeRecipeId != -1)
        {
            // ���V�s�\��
            image.sprite = recipeImage[player.FakeRecipeId];
        }
        else
        {
            image.sprite = null;
        }
    }
}
