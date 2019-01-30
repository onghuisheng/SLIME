using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeManager : MonoBehaviour
{
    public List<GameObject> m_SlimeInScene;
    public List<GameObject> m_SlimeInWave;
    public List<GameObject> m_GolemSlimeInWave;
    //public int m_limit;

    public static SlimeManager instance = null;

    public List<int> m_SlimeWaves;
    public List<int> m_GolemSlimeWaves;

    //[HideInInspector]
    public int m_CurrentWave;

    //[HideInInspector]
    public bool m_FinishSpawnWave;

    public GameObject m_DayAndNight;

    public bool m_BreakTime;

    public enum GameType
    {
        Normal,
        Infinite,
    }

    public static GameType m_GameType;


    // Use this for initialization
    void Awake()
    {
        m_SlimeInScene = new List<GameObject>();
        m_SlimeInWave = new List<GameObject>();
        m_GolemSlimeInWave = new List<GameObject>();

        if (instance == null)
            instance = GetComponent<SlimeManager>();

        m_CurrentWave = 0;
        m_FinishSpawnWave = false;
    }
    
    public void Remove()
    {
        foreach (GameObject go in m_SlimeInScene)
        {
            if (go.GetComponent<SlimeBase>().toDespawn)
            {
                m_SlimeInScene.Remove(go);
                break;
            }
        }

        if (m_SlimeInScene.Count == 0 && m_FinishSpawnWave == true && m_SlimeInWave.Count == m_SlimeWaves[m_CurrentWave]/* && m_GolemSlimeInWave.Count == m_GolemSlimeWaves[m_CurrentWave]*/)
        {
            m_SlimeInWave.Clear();
            m_GolemSlimeInWave.Clear();
            m_FinishSpawnWave = false;
            m_BreakTime = true;

            // ADD BREAK START SOUND
            AudioManager.Instance.Play2D("shortsmallhorn", AudioManager.AudioType.Additive, new AudioSourceData2D() { pitchOverride = 0.6f }, 1);

            StartCoroutine(NextWave(30.0f));
        }
    }

    IEnumerator NextWave(float m_Time)
    {
        if (m_CurrentWave == 2 && m_GameType == GameType.Normal)
        {
            yield break;
        }

        yield return new WaitForSeconds(m_Time);

        m_CurrentWave++;

        if (m_CurrentWave > 2 && m_GameType == GameType.Infinite)
        {
            m_CurrentWave = 0;
        }

        m_BreakTime = false;

        // ADD BREAK END SOUND
        AudioManager.Instance.Play2D("warhorn", AudioManager.AudioType.Additive, new AudioSourceData2D() { pitchOverride = 1 });
        CommanderSpeaker.Instance.PlaySpeaker("npc_incoming", AudioManager.AudioType.Additive, 1.5f);
    }
}
