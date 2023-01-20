using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlimeControls : MonoBehaviour
{

    private Transform _playerTransform;
    /*private Vector2 _distanceToPlayer;
    private Vector2 _followDistance;*/

    private Rigidbody2D _rb2D;
    private float _distance;
    [SerializeField] public float _followDistance;
    [SerializeField] public float _moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = GameObject.Find("Player").transform;
        _rb2D= GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _distance = Vector2.Distance(_playerTransform.position, transform.position);
        //Debug.Log($" d = {_distance}");

        
        if (_distance < _followDistance)
        {
            _rb2D.velocity = new Vector2(_playerTransform.position.x - transform.position.x, _playerTransform.position.y - transform.position.y);
        }
        else if (_distance > 4) 
        {
            _rb2D.velocity = Vector2.zero;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            KnockBack();
        }
    }


    private void KnockBack()
    {

    }

}
