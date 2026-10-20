
using UnityEngine;
using System.Collections;

public class BuildSoundTimer : MonoBehaviour
{
    public AudioSource buildSource;

    private Coroutine stopCoroutine;

    public void PlayBuildSound()
    {
        if (buildSource == null)
            return;

        if (stopCoroutine != null)
        {
            StopCoroutine(stopCoroutine);
            stopCoroutine = null;
        }

        buildSource.Stop();
        buildSource.loop = true;
        buildSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        buildSource.Play();

        stopCoroutine = StartCoroutine(StopAfterSixSeconds());
    }

    private IEnumerator StopAfterSixSeconds()
    {
        yield return new WaitForSeconds(6f);

        if (buildSource != null)
        {
            buildSource.Stop();
            buildSource.loop = false;
        }

        stopCoroutine = null;
    }

    private void OnDisable()
    {
        if (stopCoroutine != null)
        {
            StopCoroutine(stopCoroutine);
            stopCoroutine = null;
        }

        if (buildSource != null)
        {
            buildSource.Stop();
            buildSource.loop = false;
        }
    }
}

