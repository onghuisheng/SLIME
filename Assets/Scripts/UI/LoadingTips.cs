using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingTips : MonoBehaviour
{

    [SerializeField]
    List<string> m_TipList;

    private void OnEnable()
    {
        GetComponent<Text>().text = m_TipList[Random.Range(0, m_TipList.Count)];
    }

}
