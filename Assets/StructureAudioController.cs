using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class StructureAudioController : MonoBehaviour
{
    public AudioClip[] clips;
    public float minDelay;
    public float maxDelay;

    private AudioSource _source;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        StartCoroutine(PlaySound());
    }

    private IEnumerator PlaySound()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            _source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
        }
    }
}
