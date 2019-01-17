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

    public bool loop = false;
}

public class AudioSourceData3D
{
    // 0-1
    public float volume = 1;

    // 0-Infinity
    public float randomPitchRange = 0.2f;

    public bool loop = false;

    // 0-1
    public float spatialBlend = 0.8f;

    // 0-Infinity
    public float minDistance = 0.5f;

    // 0-Infinity
    public float maxDistance = 3;

    public Vector3 relativeVelocity;
}

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
        public Action onComplete;
        public string clipAlias;
    }

    Queue<AudioQueueInfo> m_AudioQueue = new Queue<AudioQueueInfo>();

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

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Instance.Play3D("ding", transform.position, AudioType.Additive, new AudioSourceData3D()
            {
                loop = true
            });
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Instance.Play3D("ding", transform.position, AudioType.Queue);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Instance.Play2D("flashbangboom", AudioType.Additive, new AudioSourceData2D()
            {
                loop = true
            });
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Instance.Play2D("flashbangboom", AudioType.Queue);
        }
    }

    /// <summary>
    /// Plays a 2D audio clip with the given alias
    /// </summary>
    public GameObject Play2D(string clipAlias, AudioType audioType, float delayInSeconds = 0, Action onComplete = null)
    {
        return Play2D(clipAlias, audioType, null, delayInSeconds, onComplete);
    }

    /// <summary>
    /// Plays a 2D audio clip with the given alias
    /// </summary>
    public GameObject Play2D(string clipAlias, AudioType audioType, AudioSourceData2D audioSourceData, float delayInSeconds = 0, Action onComplete = null)
    {
        AudioSourceData3D audioSourceData3D = new AudioSourceData3D();

        if (audioSourceData != null)
        {
            audioSourceData3D = new AudioSourceData3D()
            {
                loop = audioSourceData.loop,
                randomPitchRange = audioSourceData.randomPitchRange,
                volume = audioSourceData.volume
            };
        }

        audioSourceData3D.maxDistance = 999;
        audioSourceData3D.minDistance = 999;
        audioSourceData3D.relativeVelocity = Vector3.zero;
        audioSourceData3D.spatialBlend = 0;

        return Play3D(clipAlias, Camera.main.transform.position, audioType, audioSourceData3D, delayInSeconds, onComplete);
    }

    /// <summary>
    /// Plays a 3D sound at the given position
    /// An GameObject with an AudioSource is spawned at the given position and is removed once the audio finishes
    /// </summary>
    /// <returns>Reference to the AudioSource that was spawned</returns>
    public GameObject Play3D(string clipAlias, Vector3 position, AudioType audioType, float delayInSeconds = 0, Action onComplete = null)
    {
        return Play3D(clipAlias, position, audioType, null, delayInSeconds, onComplete);
    }


    /// <summary>
    /// Plays a 3D sound at the given position
    /// An GameObject with an AudioSource is spawned at the given position and is removed once the audio finishes
    /// </summary>
    /// <returns>Reference to the AudioSource that was spawned</returns>   
    public GameObject Play3D(string clipAlias, Vector3 position, AudioType audioType, AudioSourceData3D audioSourceData, float delayInSeconds = 0, Action onComplete = null)
    {
        AudioClip clip = GetAudioClip(clipAlias);

        if (clip == null)
        {
            Debug.LogError("Error trying to retrieve an Audio's clipAlias");
            return null;
        }

        if (audioSourceData == null)
            audioSourceData = new AudioSourceData3D();

        GameObject obj = new GameObject((audioSourceData.spatialBlend == 0) ? "[2D_AudioSource]" : "[3D_AudioSource]");
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
        spawnedAudioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        spawnedAudioSource.loop = audioSourceData.loop;

        switch (audioType)
        {
            case AudioType.Queue:
                m_AudioQueue.Enqueue(new AudioQueueInfo
                {
                    is3D = (audioSourceData.spatialBlend == 0) ? false : true,
                    audioSource = spawnedAudioSource,
                    audioClip = clip,
                    clipAlias = clipAlias,
                    delayInSeconds = delayInSeconds,
                    onComplete = onComplete
                });

                if ((m_3DAudioSource == null || (m_3DAudioSource != null && m_3DAudioSource.isPlaying == false)))
                {
                    PlayNext();
                }
                break;

            case AudioType.Additive:
                spawnedAudioSource.PlayDelayed(delayInSeconds);

                if (!audioSourceData.loop)
                    StartCoroutine(PlayAdditiveRoutine(spawnedAudioSource.gameObject, spawnedAudioSource.clip.length + delayInSeconds, onComplete));
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
            if (m_3DAudioSource != null)
                m_3DAudioSource.Stop();

            var announceInfo = m_AudioQueue.Dequeue();

            m_3DAudioSource = announceInfo.audioSource;

            AudioSource source = announceInfo.audioSource;
            source.clip = announceInfo.audioClip;
            source.PlayDelayed(announceInfo.delayInSeconds);
            StartCoroutine(PlayNextRoutine(source.clip.length + announceInfo.delayInSeconds, announceInfo.onComplete));
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
    /// Stops the current Audio and erase all pending Audios in the queue
    /// </summary>
    public void StopAll()
    {
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

    private IEnumerator PlayNextRoutine(float delayInSeconds, Action onComplete)
    {
        yield return new WaitForSeconds(delayInSeconds);

        if (onComplete != null)
            onComplete.Invoke();

        if (m_3DAudioSource != null)
        {
            Destroy(m_3DAudioSource.gameObject);
            m_3DAudioSource = null;
        }

        PlayNext();
    }

    private IEnumerator PlayAdditiveRoutine(GameObject objToDestroy, float delayInSeconds, Action onComplete)
    {
        yield return new WaitForSeconds(delayInSeconds);

        if (onComplete != null)
            onComplete.Invoke();

        Destroy(objToDestroy);
    }

}