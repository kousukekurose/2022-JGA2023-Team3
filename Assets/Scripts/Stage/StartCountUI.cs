using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class StartCountUI : MonoBehaviour
{

    [SerializeField]
    AudioClip startCount;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(StartCount());
    }

    //ステージ開始演出
    IEnumerator StartCount()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        audioSource.PlayOneShot(startCount);
    }
}
