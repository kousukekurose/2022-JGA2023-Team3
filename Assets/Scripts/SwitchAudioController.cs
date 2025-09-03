using UnityEngine;

// サウンド変更を管理するクラス
public class SwitchAudioController : MonoBehaviour
{
    // クリップを変更
    public void SwitchAudio(AudioClip beforeClip, AudioClip afterClip, AudioSource audioSource)
    {
        // ジングルが終了したらBGMを流す
        if (audioSource.clip == beforeClip && !audioSource.isPlaying)
        {
            audioSource.clip = afterClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
