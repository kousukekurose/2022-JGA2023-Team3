using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnoisseurUI : MonoBehaviour
{
    [SerializeField]
    GameObject predictRecipeUI = null;

    // •\Ž¦•b”
    [SerializeField]
    float showTime = 0.0f;

    // ”ñ•\Ž¦•b”
    [SerializeField]
    float hideTime = 0.0f;

    float count = 0.0f;

    bool isShowUI = false;

    void Update()
    {
        //@ƒJƒƒ‰‚Æ“¯‚¶Œü‚«‚ÉÝ’è
        transform.rotation = Camera.main.transform.rotation;

        count += Time.deltaTime;

        var state = GameManager.Instance.CurrentSceneState;
        switch (state)
        {
            case GameManager.SceneState.Lobby:
                predictRecipeUI.SetActive(true);
                break;
            case GameManager.SceneState.Stage:
                if (count >= hideTime && !isShowUI)
                {
                    isShowUI = true;
                    count = 0;
                    StartCoroutine(OnShowUI());
                }
                break;
            default:
                predictRecipeUI.SetActive(false);
                break;
        }
    }

    IEnumerator OnShowUI()
    {
        predictRecipeUI.SetActive(true);
        yield return new WaitForSeconds(showTime);
        predictRecipeUI.SetActive(false);
        isShowUI = false;
    }
}
