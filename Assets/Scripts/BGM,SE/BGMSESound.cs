using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSESound : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;
    
    [SerializeField]
    AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Music()
    {
        audioSource.PlayOneShot(audioClip);
    }

}
