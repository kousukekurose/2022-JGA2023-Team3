using UnityEngine;
using UnityEngine.UI;

public class PhotoChange : MonoBehaviour
{
    //説明画像を表示
    [SerializeField]
    Sprite[] imageText;
    [SerializeField] 
    Image image;
    [SerializeField]
    Image image1;

    enum SceneState
    {
        //1枚目
        Image1,
        //2枚目 
        Image2,
        //3枚目
        Image3,
        //4枚目
        Image4,
        //5枚目
        Image5,
    }
    SceneState currentState = SceneState.Image1;

    void Start()
    {
        //最初の画像をセットする
        image1.sprite = imageText[0];
        image.enabled = false;
    }

    //ボタンが押されたら画像を切り替える
    public void OnClik()
    {
        // このシーンの進捗管理
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

