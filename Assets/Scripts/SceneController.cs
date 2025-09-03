using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//�V�[�����Ǘ�����N���X
public class SceneController : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    float animationTime = 1;

    private void Start()
    {
        Time.timeScale = 1;
        animator = GetComponent<Animator>();
    }

    //�V�[���ړ�
    public void LoadNextStage(string sceneName)
    {
        StartCoroutine(OnLoadNextScene(sceneName));
    }

    IEnumerator OnLoadNextScene(string sceneName)
    {
        yield return new WaitForSeconds(animationTime);
        SceneManager.LoadScene(sceneName);
    }
    //�I��
    public void ExitGame()
    {
        Application.Quit();
    }
}
