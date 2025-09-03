using UnityEngine;

// �T�E���h�ύX���Ǘ�����N���X
public class SwitchAudioController : MonoBehaviour
{
    // �N���b�v��ύX
    public void SwitchAudio(AudioClip beforeClip, AudioClip afterClip, AudioSource audioSource)
    {
        // �W���O�����I��������BGM�𗬂�
        if (audioSource.clip == beforeClip && !audioSource.isPlaying)
        {
            audioSource.clip = afterClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
