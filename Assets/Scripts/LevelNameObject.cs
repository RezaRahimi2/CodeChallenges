using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelNameObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro m_textMesh;

    public void Initialize(string text)
    {
        m_textMesh.text = text;
    }
}
