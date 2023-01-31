using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> Layers;
    [SerializeField]
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        const float pFactor = 5;

        Vector3 offset = player.transform.position;
        
        for (int i = 0; i < Layers.Count; i++) {
            Vector3 position = -offset * pFactor * i;
            Layers[i].transform.position = position;
        }
    }
}
