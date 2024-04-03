using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GetTile : MonoBehaviour
{

    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private GameObject _cursor;

    [SerializeField] private Text _text;
    
    // Update is called once per frame
    void Update()
    {
        RaycastHit2D physHit2D = Physics2D.Raycast(_cursor.transform.position, _cursor.transform.TransformDirection(0, 0, 10));
        
    }
}
