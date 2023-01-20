using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CatStateMachine : MonoBehaviour
{
    private Rigidbody2D _rb2D;
    private Vector2 _direction;
    [SerializeField] public float _moveSpeed;

    private bool _isPlayer;


    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlayer)
        {

            _direction.x = Input.GetAxis("Horizontal");
            _direction.y = Input.GetAxis("Vertical");

            _rb2D.velocity = _direction * _moveSpeed * Time.deltaTime;
        }

    }
}
