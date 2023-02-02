using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControls : MonoBehaviour
{
    private Rigidbody2D _rb2D;
    [SerializeField] public float _bulletSpeed;
    private GameObject _player;
    private Vector2 _direction;


    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        _rb2D= GetComponent<Rigidbody2D>();
        _player = GameObject.Find("Player");

        _direction = new Vector2 (_player.transform.position.x - transform.position.x, _player.transform.position.y- transform.position.y).normalized;

        _rb2D.velocity = _direction*_bulletSpeed * Time.deltaTime;

    }

    // Update is called once per frame
    void Update()
    {

    }

    
}
