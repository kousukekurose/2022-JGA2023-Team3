using UnityEngine;
using UnityEngine.UI;

public class PhotoChange : MonoBehaviour
{
    //�����摜��\��
    [SerializeField]
    Sprite[] imageText;
    [SerializeField] 
    Image image;
    [SerializeField]
    Image image1;

    enum SceneState
    {
        //1����
        Image1,
        //2���� 
        Image2,
        //3����
        Image3,
        //4����
        Image4,
        //5����
        Image5,
    }
    SceneState currentState = SceneState.Image1;

    void Start()
    {
        //�ŏ��̉摜���Z�b�g����
        image1.sprite = imageText[0];
        image.enabled = false;
    }

    //�{�^���������ꂽ��摜��؂�ւ���
    public void OnClik()
    {
        // ���̃V�[���̐i���Ǘ�
        switch (currentState)
        {
            case SceneState.Image1:
                image.enabled = true;
                image1.enabled = false;
                image.sprite = imageText[1];
                currentState = SceneState.Image2;
                break;
            case SceneState.Image2:
                image.sprite = imageText[2];
                currentState = SceneState.Image3;
                break;
            case SceneState.Image3:
                image.sprite = imageText[3];
                currentState = SceneState.Image4;
                break;
            case SceneState.Image4:
                image.sprite = imageText[4];
                currentState = SceneState.Image5;
                break;
            case SceneState.Image5:
                image1.enabled = true;
                image.enabled = false;
                image1.sprite = imageText[0];
                currentState = SceneState.Image1;
                break;
            default:
                break;
        }
    }
}

