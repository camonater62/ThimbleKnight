using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _Layers;
    [SerializeField] private GameObject _Player;
    [SerializeField] private float _ParallaxFactor = 5.0f;
    [SerializeField] private float _YMultiplier = 1.0f;
    [SerializeField] private bool _ReverseOrder = false;
    [SerializeField] private int _CenterElement = 0;

    private List<Vector3> _OriginalPositions = new();

    void Start()
    {
        for (int i = 0; i < _Layers.Count; i++) {
            _OriginalPositions.Add(_Layers[i].transform.position);
        }
    }

    void Update()
    {
        Vector3 player_pos = _Player.transform.position;
        
        for (int i = 0; i < _Layers.Count; i++) {

            


            float zpos = i - _CenterElement;
            if (_ReverseOrder) {
                zpos = _CenterElement - i;
            }
            if (zpos < 0) {
                zpos = 1 / Mathf.Abs(zpos - 1);
            }
            float factor = zpos * _ParallaxFactor;
            
            float xcomp = player_pos.x * factor;
            float ycomp = player_pos.y * factor * _YMultiplier;

            

            Vector3 position = _OriginalPositions[i] - new Vector3(xcomp, ycomp, 0);

            float width = _Layers[i].GetComponent<SpriteRenderer>().bounds.size.x;
            Vector3 widthVector = new(width, 0, 0);
            float minx = _OriginalPositions[i].x - width;
            float maxx = _OriginalPositions[i].x + width;

            while (position.x < minx) {
                position += widthVector;
            }
            while (position.x > maxx) {
                position -= widthVector;
            }

            _Layers[i].transform.position = position;
        }
    }
}
