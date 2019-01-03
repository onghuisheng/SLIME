using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceData2D
{
    // 0-1
    public float volume = 1;

    // 0-Infinity
    public float randomPitchRange = 0.2f;

}

public class AudioSourceData3D
{
    // 0-1
    public float volume = 1;

    // 0-Infinity
    public float randomPitchRange = 0.2f;

    // 0-1
    public float spatialBlend = 0.8f;

    // 0-Infinity
    public float minDistance = 0.5f;

    // 0-Infinity
    public float maxDistance = 5;

    public Vector3 relativeVelocity;
}

[RequireComponent(typeof(AudioSource))]
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    #region Inspector Stuff
    [Serializable]
    private struct AudioKeyPair
    {
        public string clipAlias;
        public AudioClip audioClip;
    }

    [SerializeField]
    AudioKeyPair[] m_AudioClips;

    Dictionary<string, AudioClip> m_AudioClipsDictonary = new Dictionary<string, AudioClip>();
    #endregion

    private struct AudioQueueInfo
    {
        public bool is3D;
        public AudioSource audioSource;
        public AudioClip audioClip;
        public float delayInSeconds;
        public string clipAlias;
    }

    Queue<AudioQueueInfo> m_AudioQueue = new Queue<AudioQueueInfo>();

    AudioSource m_LocalAudioSource;
    AudioSource m_3DAudioSource;

    public enum AudioType
    {
        /// <summary>
        /// Adds the clip into the queue and play it after all previously playing clips has ended
        /// </summary>
        Queue,
        /// <summary>
        /// Plays the clip on top of all existing playing clips
        /// </summary>
        Additive
    }

    private void Start()
    {
        // Add all elements into a dictionary for faster lookup
        foreach (var kp in m_AudioClips)
        {
            m_AudioClipsDictonary.Add(kp.clipAlias, kp.audioClip);
        }

        m_LocalAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AudioManager.Instance.Play3D("ding", transform.position, AudioManager.AudioType.Additive);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            AudioManager.Instance.Play3D("ding", transform.position, AudioManager.AudioType.Queue);
        }
    }

    /// <summary>
    /// Plays a 2D audio clip with the given alias
    /// </summary>
    public void Play2D(string clipAlias, AudioType announceType, float delayInSeconds = 0)
    {
        AudioClip clip = GetAudioClip(clipAlias);

        if (clip == null)
        {
            Debug.LogError("Error trying to retrieve Audio with alias: " + clipAlias);
            return;
        }

        switch (announceType)
        {
            case AudioType.Queue:
                m_AudioQueue.Enqueue(new AudioQueueInfo
                {
                    is3D = false,
                    audioSource = m_LocalAudioSource,
                    audioClip = clip,
                    clipAlias = clipAlias,
                    delayInSeconds = delayInSeconds
                });

                if (!m_LocalAudioSource.isPlaying && (m_3DAudioSource == null || (m_3DAudioSource != null && m_3DAudioSource.isPlaying == false)))
                {
                    PlayNext();
                }
                break;

            case AudioType.Additive:
                m_LocalAudioSource.clip = clip;
                m_LocalAudioSource.PlayDelayed(delayInSeconds);
                break;

            default:
                Debug.LogError("Unhandled Audio type");
                break;
        }
    }

    /// <summary>
    /// Plays a 3D sound at the given position
    /// A GameObject with an AudioSource is spawned at the given position and is removed once the audio finishes
    /// </summary>
    /// <returns>Reference to the AudioSource that was spawned</returns>
    public GameObject Play3D(string clipAlias, Vector3 position, AudioType announceType, float delayInSeconds = 0)
    {
        return Play3D(clipAlias, position, announceType, null, delayInSeconds);
    }

    /// <summary>
    /// Plays a 3D sound at the given position
    /// A GameObject with an AudioSource is spawned at the given position and is removed once the audio finishes
    /// </summary>
    /// <returns>Reference to the AudioSource that was spawned</returns>
    public GameObject Play3D(string clipAlias, Vector3 position, AudioType announceType, AudioSourceData3D audioSourceData, float delayInSeconds = 0)
    {
        AudioClip clip = GetAudioClip(clipAlias);

        if (clip == null)
        {
            Debug.LogError("Error trying to retrieve an Audio's clipAlias");
            return null;
        }

        if (audioSourceData == null)
            audioSourceData = new AudioSourceData3D();

        GameObject obj = new GameObject("[3D_AudioSource]");
        obj.transform.parent = transform;
        obj.transform.position = position;

        const float minMagnitude = 0;
        const float maxMagnitude = 5;

        AudioSource spawnedAudioSource = obj.AddComponent<AudioSource>();
        spawnedAudioSource.clip = GetAudioClip(clipAlias);
        spawnedAudioSource.volume = audioSourceData.volume + (Mathf.Clamp(audioSourceData.relativeVelocity.magnitude, minMagnitude, maxMagnitude) / maxMagnitude); // Higher velocity = louder;
        spawnedAudioSource.spatialBlend = audioSourceData.spatialBlend;
        spawnedAudioSource.minDistance = audioSourceData.minDistance;
        spawnedAudioSource.maxDistance = audioSourceData.maxDistance;
        spawnedAudioSource.pitch = 1 + UnityEngine.Random.Range(-Mathf.Abs(audioSourceData.randomPitchRange), Mathf.Abs(audioSourceData.randomPitchRange));
        spawnedAudioSource.rolloffMode = AudioRolloffMode.Linear;

        switch (announceType)
        {
            case AudioType.Queue:
                m_AudioQueue.Enqueue(new AudioQueueInfo
                {
                    is3D = true,
                    audioSource = spawnedAudioSource,
                    audioClip = clip,
                    clipAlias = clipAlias,
                    delayInSeconds = delayInSeconds
                });

                if (!m_LocalAudioSource.isPlaying && (m_3DAudioSource == null || (m_3DAudioSource != null && m_3DAudioSource.isPlaying == false)))
                {
                    PlayNext();
                }
                break;

            case AudioType.Additive:
                spawnedAudioSource.PlayDelayed(delayInSeconds);
                StartCoroutine(PlayAdditiveRoutine(spawnedAudioSource.gameObject, spawnedAudioSource.clip.length + delayInSeconds));
                break;

            default:
                Debug.LogError("Unhandled Audio type");
                break;
        }

        return obj;
    }

    /// <summary>
    /// Stops the current Audio and plays the next one if any
    /// </summary>
    public void PlayNext()
    {
        if (m_AudioQueue.Count == 0)
            return;
        else
        {
            m_LocalAudioSource.Stop();

            if (m_3DAudioSource != null)
                m_3DAudioSource.Stop();

            var announceInfo = m_AudioQueue.Dequeue();

            if (announceInfo.is3D)
                m_3DAudioSource = announceInfo.audioSource;

            AudioSource source = announceInfo.audioSource;
            source.clip = announceInfo.audioClip;
            source.PlayDelayed(announceInfo.delayInSeconds);
            StartCoroutine(PlayNextRoutine(source.clip.length + announceInfo.delayInSeconds, announceInfo.is3D));
        }
    }

    /// <summary>
    /// Erase all pending Audios in queue
    /// </summary>
    public void ClearQueue()
    {
        while (m_AudioQueue.Count > 0)
        {
            var announceInfo = m_AudioQueue.Dequeue();

            if (announceInfo.is3D)
                Destroy(announceInfo.audioSource.gameObject);
        }
    }

    public AudioClip GetAudioClip(string clipAlias)
    {
        AudioClip clip;
        m_AudioClipsDictonary.TryGetValue(clipAlias, out clip);
        return clip;
    }

    /// <summary>
    /// Stops the current Audio and erase all pending Audios in queue
    /// </summary>
    public void StopAll()
    {
        m_LocalAudioSource.Stop();

        if (m_3DAudioSource != null)
            m_3DAudioSource.Stop();

        ClearQueue();
        StopAllCoroutines();

        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            child.GetComponent<AudioSource>().Stop();
            Destroy(child.gameObject);
        }

        m_3DAudioSource = null;
    }

    private IEnumerator PlayNextRoutine(float delayInSeconds, bool is3D)
    {
        yield return new WaitForSeconds(delayInSeconds);

        if (is3D && m_3DAudioSource != null)
        {
            Destroy(m_3DAudioSource.gameObject);
            m_3DAudioSource = null;
        }

        PlayNext();
    }

    private IEnumerator PlayAdditiveRoutine(GameObject objToDestroy, float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        Destroy(objToDestroy);
    }

}