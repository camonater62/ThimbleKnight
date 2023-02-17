using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_Layers;
    [SerializeField] private GameObject m_Player;
    [SerializeField] private float m_ParallaxFactor = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = m_Player.transform.position;
        
        for (int i = 0; i < m_Layers.Count; i++) {
            Vector3 position = -offset * m_ParallaxFactor * i;
            m_Layers[i].transform.position = position;
        }
    }
}
