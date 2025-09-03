using System.Collections;
using UnityEngine;
using TMPro;

// �`���[�g���A����ʂ��Ǘ�����N���X
public class TutorialController : MonoBehaviour
{
    //�J�����̈ʒu����
    [SerializeField]
    private CameraController cameraCtrl = null;
    //�v���C���[���Q�������Ƃ��ɕ\��
    [SerializeField]
    TextMeshProUGUI text;

    enum SceneState
    {
        // �V�[���J�n����
        Usually,
        // ��l�ڎQ��
        OnePerson,
        // ��l�ڎQ��
        TwoPerson,
    }
    SceneState currentState = SceneState.Usually;

    // Start is called before the first frame update
    void Start()
    {
        text.enabled = false;
        GameManager.Instance.JoinEnabled();
        currentState = SceneState.Usually;
        Debug.Log(currentState);
        cameraCtrl.TargetGroupSetting();
        //GameManager.Instance.CurrentSceneState = GameManager.SceneState.Tutorial;
    }

    // Update is called once per frame
    void Update()
    {
        // ���̃V�[���̐i���Ǘ�
        switch (currentState)
        {
            case SceneState.Usually:
                if (GameManager.Instance.Players.Count == 1)
                {
                    currentState = SceneState.OnePerson;
                    // ��l�ڎQ��
                    text.enabled = true;
                    text.text = "�v���C���[�P���Q�����܂���";
                    StartCoroutine("ShowText");
                    cameraCtrl.AddMember(GameManager.Instance.Players[0].transform);
                    Debug.Log(currentState);
                }
                break;
            case SceneState.OnePerson:
                if (GameManager.Instance.Players.Count == 2)
                {
                    currentState = SceneState.TwoPerson;
                    // ��l�ڎQ��
                    text.enabled = true;
                    text.text = "�v���C���[�Q���Q�����܂���";
                    StartCoroutine("ShowText");
                    cameraCtrl.AddMember(GameManager.Instance.Players[1].transform);
                    Debug.Log(currentState);
                }
                break;
            case SceneState.TwoPerson:
                break;
            default:
                break;
        }
    }

    //�{�^�����I�����ꂽ��e�L�X�g��\��������
    IEnumerator ShowText()
    {
        yield return new WaitForSeconds(1.0f);
        text.text = "";
    }
}
