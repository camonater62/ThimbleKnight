using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxChunkManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    // TODO: bounded regions
    private float _lowerBound = float.MinValue;
    private float _upperBound = float.MaxValue;

    private List<GameObject> _children = new();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++) {
            _children.Add(transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
