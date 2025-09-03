using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//シーンを管理するクラス
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

    //シーン移動
    public void LoadNextStage(string sceneName)
    {
        StartCoroutine(OnLoadNextScene(sceneName));
    }

    IEnumerator OnLoadNextScene(string sceneName)
    {
        yield return new WaitForSeconds(animationTime);
        SceneManager.LoadScene(sceneName);
    }
    //終了
    public void ExitGame()
    {
        Application.Quit();
    }
}
