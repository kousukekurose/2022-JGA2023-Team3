using UnityEngine;

// ボタンSEを管理するクラス
public class ButtonSoundEffect : MonoBehaviour
{
    // ボタン選択音
    [SerializeField]
    AudioClip selectSound = null;

    // ボタン決定音
    [SerializeField]
    AudioClip choiceSound = null;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        // コンポーネントを取得
        audioSource = GetComponent<AudioSource>();
    }

    // ボタン選択音再生
    public void OnPlaySelectSound()
    {
        // 選択音を1回再生
        audioSource.PlayOneShot(selectSound);
    }

    // ボタン決定音再生
    public void OnPlayChoiceSound()
    {
        // 決定音を1回再生
        audioSource.PlayOneShot(choiceSound);
    }
}
