using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looping_Background : MonoBehaviour
{

    private Renderer _cloudMaterial;
    private float _offset;
    [SerializeField] private float _speed;

    // Start is called before the first frame update
    void Start()
    {
        _cloudMaterial = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _offset += Time.deltaTime * _speed;
        Vector2 textureOffset = new Vector2(_offset, 0);
        _cloudMaterial.material.mainTextureOffset = textureOffset;
    }
}
