using UnityEngine;

// �^�C�g����ʂ��Ǘ�����N���X
public class TitleController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.CurrentSceneState = GameManager.SceneState.Title;
        GameManager.Instance.AllReset();
        GameManager.Instance.JoinDisabled();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
