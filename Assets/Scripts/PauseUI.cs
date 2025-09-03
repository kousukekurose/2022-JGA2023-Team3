using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// �|�[�YUI���Ǘ�����N���X
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
    // ����UI���\���ɐݒ肵�܂��B
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
