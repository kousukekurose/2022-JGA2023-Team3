using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// ポーズUIを管理するクラス
public class PauseUI : MonoBehaviour
{
    [SerializeField]
    GameObject pauseUI;
    [SerializeField]
    private GameObject firstSelect;
    public void Show()
    {
        gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject (firstSelect);
        pauseUI.SetActive(true);
        Time.timeScale= 0;
    }
    // このUIを非表示に設定します。
    public void Hide()
    {
        gameObject.SetActive(false);
        pauseUI.SetActive(false);
        Time.timeScale= 1;
    }
    public void Title()
    {
        SceneManager.LoadScene("Title");
    }
    public void Exit()
    {
        Application.Quit();
    }
}
