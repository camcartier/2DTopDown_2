using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ChickenStateMode
{
    LOCOMOTION,
    PECK
}

public class ChickStateMachine : MonoBehaviour
{
    private Rigidbody2D _rb2D;
    private Vector2 _direction;
    private Animator _animator;

    private ChickenStateMode _currentState;

    [SerializeField] public float _moveSpeed;

    public bool _isWalking;

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


        ChooseDirection();

    }

    private void Update()
    {
        if (_isWalking)
        {
            _walkCounter -= Time.deltaTime;

            switch (_walkDirection)
            {
                case 0:
                    _rb2D.velocity = new Vector2(0, _moveSpeed * Time.deltaTime);
                    break;
                case 1:
                    _rb2D.velocity = new Vector2(_moveSpeed * Time.deltaTime, 0);
                    break;
                case 2:
                    _rb2D.velocity = new Vector2(0, -(_moveSpeed * Time.deltaTime));
                    break;
                case 3:
                    _rb2D.velocity = new Vector2(-(_moveSpeed * Time.deltaTime),0);
                    break;
                default:
                    _rb2D.velocity = Vector2.zero;
                    break;

            }

            if (_walkCounter < 0)
            {
                _isWalking= false;
                _waitCounter = _waitTime;
            }
                
        }
        else
        {
            _waitCounter -= Time.deltaTime;

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

    private void OnStateEnter()
    {

    }

    private void OnStateUpdate()
    {

    }

    private void OnStateExit()
    {

    }









}
