using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


enum SlimeStateMode
{
    IDLE,
    WALK
}

public class SlimeControls : MonoBehaviour
{

    private Transform _playerTransform;

    private Animator _animator;
    private SlimeStateMode _currentState;
    private bool _isWalking;

    private Rigidbody2D _rb2D;
    private float _distance;
    [SerializeField] public float _followDistance;
    [SerializeField] public float _moveSpeed;
    //[SerializeField] int _health = 2;

    #region saved pos
    private float _currentPosX;
    private float _currentPosY;
    private float _lastSavedPosX;
    private float _lastSavedPosY;
#endregion

    [SerializeField] float _stunTime;
    private float _stunCounter;
    private bool _isHit;

    

    private void Awake()
    {
        _stunTime = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = GameObject.Find("Player").transform;
        _rb2D= GetComponent<Rigidbody2D>();
        _animator= GetComponentInChildren<Animator>();

        /*
        _lastSavedPosX = _currentPosX;
        _lastSavedPosY = _currentPosY;*/

        _stunCounter = _stunTime;
    }

    // Update is called once per frame
    void Update()
    {
        _distance = Vector2.Distance(_playerTransform.position, transform.position);

        _currentPosX = transform.position.x;
        _currentPosY = transform.position.y;

        //ci dessous le poulet qui fuit
        //Vector2 _direction = new Vector2(transform.position.x - _playerTransform.position.x, transform.position.y - _playerTransform.position.y);

        Vector2 _direction = new Vector2(_playerTransform.position.x - transform.position.x,_playerTransform.position.y - transform.position.y);

        if (_distance < _followDistance)
        {

            _rb2D.velocity = _direction;
            _isWalking= true;
            _animator.SetBool("isWalking", true);

        }
        else if (_distance > 4) 
        {
            _rb2D.velocity = Vector2.zero;
            _isWalking= false;
            _animator.SetBool("isWalking", false);
        }

        _animator.SetFloat("DirectionX", _direction.x);
        _animator.SetFloat("DirectionY", _direction.y);

        if (_isHit)
        {
            _stunCounter -= Time.deltaTime;
            _rb2D.velocity = Vector2.zero;
        }

        if (_stunCounter < 0)
        {
            _isHit= false;
            _stunCounter = _stunTime;
            _rb2D.velocity = _direction;
            
        }
    }



    void OnStateEnter()
    {
        switch (_currentState)
        {
            case SlimeStateMode.IDLE:
                break;
            case SlimeStateMode.WALK:
                _animator.SetBool("isWalking", true);
                break;


        }
    }

    //en update, changer le state machine
    //ne pas le changer dans l'update et dans l'exit
    //sinon stack overflow!
    void OnStateUpdate()
    {
        switch (_currentState)
        {
            case SlimeStateMode.IDLE:
                if (_isWalking)
                {
                    TransitionToState(SlimeStateMode.WALK);
                    
                }

                break;
            case SlimeStateMode.WALK:
                if(!_isWalking)
                {
                    TransitionToState(SlimeStateMode.IDLE);
                }

                break;

        }
    }

    //en exit, ne changer que les bools
    void OnStateExit()
    {
        switch (_currentState)
        {
            case SlimeStateMode.IDLE:
                break;
            case SlimeStateMode.WALK:
                _animator.SetBool("isWalking", false);
                break;
        }
    }



    void TransitionToState(SlimeStateMode toState)
    {
        OnStateExit();

        _currentState = toState;

        OnStateEnter();
    }





    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isHit = true;
        /*
        if (collision.collider.CompareTag("Player"))
        {
            TakeDamage(1);
        }
        if (collision.collider.CompareTag("Spell"))
        {
            TakeDamage(2);
        }*/

    }

    /*
    void TakeDamage(int damage)
    {
        _health -= damage;
    }


    void Death()
    {
        
    }
    */
}
