using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ChickenStateMode
{
    LOCOMOTION,
    PECK,
    WALK
}

public class ChickStateMachine : MonoBehaviour
{
    private Rigidbody2D _rb2D;
    private Vector2 _direction;
    private Animator _animator;

    private ChickenStateMode _currentState;

    [SerializeField] public float _moveSpeed;

    public bool _isWalking = true;
    public bool _isPecking = false;

    public float _walkTime;
    private float _walkCounter;
    public float _waitTime;
    private float _waitCounter;

    private int _walkDirection;

    //un vieille tentative
    #region Vectors
    private Vector2 _goLeft;
    private Vector2 _goRight;
    private Vector2 _goUp;
    private Vector2 _goDown;

    private List<Vector2> _possibleDir = new List<Vector2>();
    #endregion

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();

        _rb2D = GetComponent<Rigidbody2D>();

        _walkTime = 1f;
        _waitTime = 3f;
    }



    // Start is called before the first frame update
    void Start()
    {
        _walkCounter = _walkTime;
        _waitCounter = _waitTime;
    }

    private void Update()
    {
        OnStateUpdate();
    }

    private void FixedUpdate()
    {
        if (_isWalking)
        {
            _walkCounter -= Time.deltaTime;

            switch (_walkDirection)
            {
                case 0:
                    _rb2D.velocity = new Vector2(0, _moveSpeed * Time.fixedDeltaTime);
                    break;
                case 1:
                    _rb2D.velocity = new Vector2(_moveSpeed * Time.fixedDeltaTime, 0);
                    break;
                case 2:
                    _rb2D.velocity = new Vector2(0, -(_moveSpeed * Time.fixedDeltaTime));
                    break;
                case 3:
                    _rb2D.velocity = new Vector2(-(_moveSpeed * Time.fixedDeltaTime), 0);
                    break;
                default:
                    _rb2D.velocity = Vector2.zero;
                    break;

            }

            if (_walkCounter < 0)
            {
                
                _isWalking = false;
                _waitCounter = _waitTime;
                _isPecking = true;
            }

        }
        else
        {
            _waitCounter -= Time.fixedDeltaTime;

            _rb2D.velocity = Vector2.zero;


            if (_waitCounter < 0)
            {
                ChooseDirection();
            }
        }
    }

    public void ChooseDirection()
    {
        _walkDirection = Random.Range(0, 4);
        _isWalking = true;
        _walkCounter = _walkTime;
        _isPecking = false;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Hit"))
        {
            Destroy(gameObject);
        }
    }

    void TransitionToState(ChickenStateMode toState)
    {
        OnStateExit();

        _currentState = toState;

        OnStateEnter();
    }

    void OnStateEnter()
    {
        switch (_currentState)
        {
            case ChickenStateMode.LOCOMOTION:
                break;
            case ChickenStateMode.PECK:
                _animator.SetBool("isPecking", true);
                break;
            case ChickenStateMode.WALK:
                _animator.SetBool("isWalking", true);
                break;
        }
    }

    void OnStateUpdate()
    {
        switch (_currentState)
        {
            case ChickenStateMode.LOCOMOTION:
                switch (_walkDirection)
                {
                    case 0:
                        _animator.SetFloat("DirectionX", 0);
                        _animator.SetFloat("DirectionY", 1);
                        break;
                    case 1:
                        _animator.SetFloat("DirectionX", 1);
                        _animator.SetFloat("DirectionY", 0);
                        break;
                    case 2:
                        _animator.SetFloat("DirectionX", 0);
                        _animator.SetFloat("DirectionY", -1);
                        break;
                    case 3:
                        _animator.SetFloat("DirectionX", -1);
                        _animator.SetFloat("DirectionY", 0);
                        break;
                }

                if (!_isWalking)
                {
                    TransitionToState(ChickenStateMode.PECK);
                }
                break;

            case ChickenStateMode.PECK:
                switch (_walkDirection)
                {
                    case 0:
                        _animator.SetFloat("DirectionX", 0);
                        _animator.SetFloat("DirectionY", 1);
                        break;
                    case 1:
                        _animator.SetFloat("DirectionX", 1);
                        _animator.SetFloat("DirectionY", 0);
                        break;
                    case 2:
                        _animator.SetFloat("DirectionX", 0);
                        _animator.SetFloat("DirectionY", -1);
                        break;
                    case 3:
                        _animator.SetFloat("DirectionX", -1);
                        _animator.SetFloat("DirectionY", 0);
                        break;
                }

                if (_isWalking)
                {
                    TransitionToState(ChickenStateMode.WALK);
                }

                break;

            case ChickenStateMode.WALK:
                switch (_walkDirection)
                {
                    case 0:
                        _animator.SetFloat("DirectionX", 0);
                        _animator.SetFloat("DirectionY", 1);
                        break;
                    case 1:
                        _animator.SetFloat("DirectionX", 1);
                        _animator.SetFloat("DirectionY", 0);
                        break;
                    case 2:
                        _animator.SetFloat("DirectionX", 0);
                        _animator.SetFloat("DirectionY", -1);
                        break;
                    case 3:
                        _animator.SetFloat("DirectionX", -1);
                        _animator.SetFloat("DirectionY", 0);
                        break;
                }

                if (!_isWalking&&_isPecking)
                {
                    TransitionToState(ChickenStateMode.PECK);
                }
                break;

            default:
                break;
        }
    }

    void OnStateExit()
    {
        switch (_currentState)
        {
            case ChickenStateMode.LOCOMOTION:
                break;
            case ChickenStateMode.PECK:
                _animator.SetBool("isPecking", false);
                break;
            case ChickenStateMode.WALK:
                _animator.SetBool("isWalking", false);
                break;
        }
    }



    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Spell"))
        {
            Destroy(gameObject);
        }
    }



}
