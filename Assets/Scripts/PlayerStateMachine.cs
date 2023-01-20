using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

enum PlayerStateMode
{
    LOCOMOTION,
    ROLL,
    SPRINT
}

public class PlayerStateMachine : MonoBehaviour
{
    #region Exposed
    [Header("Timer")]
    [SerializeField] float _rollDuration = 0.5f;
    #endregion

    private Rigidbody2D _rb2D;
    private Vector3 _direction;
    private float _rollCount;
    public float _rollSpeedMultiplier;

    [Header("Move parameters")]
    private float _currentSpeed;
    [SerializeField] public float _moveSpeed;
    [SerializeField] public float _walkSpeed;
    [SerializeField] public float _runSpeed;
    [SerializeField] public float _rollSpeed;


    [Header ("Roll animation")]
    [SerializeField] private AnimationCurve _rollCurve;

    #region Private & Protected

    private PlayerStateMode _currentState;
    private Animator _animator;
    private float _endRollTime;


    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();

        _rb2D= GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        TransitionToState(PlayerStateMode.LOCOMOTION);

        

    }
    private void Update()
    {
        OnStateUpdate();



        SetInput();

    }

    private void FixedUpdate()
    {
        _rb2D.velocity = _direction.normalized * _currentSpeed * Time.fixedDeltaTime;

    }
    #endregion

    #region Methods

    void OnStateEnter()
    {
        switch (_currentState)
        {
            case PlayerStateMode.LOCOMOTION:
                break;
            case PlayerStateMode.ROLL:

                _animator.SetBool("isRolling", true);
                _endRollTime= Time.timeSinceLevelLoad + _rollDuration;

                break;
            case PlayerStateMode.SPRINT:

                _animator.SetBool("isRunning", true);
                //_animator.SetBool("isRolling", false);

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
            case PlayerStateMode.LOCOMOTION:
                _currentSpeed = _walkSpeed;

                _animator.SetFloat("DirectionX", _direction.x);
                _animator.SetFloat("DirectionY", _direction.y);

                if (Input.GetButtonDown("Jump"))
                {
                    TransitionToState(PlayerStateMode.ROLL);
                }
                break;

            case PlayerStateMode.ROLL:

                _rollCount += Time.deltaTime;
                _rollSpeed = _rollCurve.Evaluate(_rollCount/_rollDuration) * _rollSpeedMultiplier;

                _currentSpeed = _rollSpeed;

                if (Time.timeSinceLevelLoad > _endRollTime)
                {
                    if (Input.GetButton("Fire3"))
                    {
                        TransitionToState(PlayerStateMode.SPRINT);
                    }
                    else
                    {
                        TransitionToState(PlayerStateMode.LOCOMOTION);
                    }

                }
                break;

            case PlayerStateMode.SPRINT:
                _currentSpeed = _runSpeed;

                _animator.SetFloat("DirectionX", _direction.x);
                _animator.SetFloat("DirectionY", _direction.y);

                
                if (Input.GetButtonUp("Fire3"))
                {
                    TransitionToState(PlayerStateMode.LOCOMOTION);
                }
                
                


                break;
            default:
                break;
        }
    }

    //en exit, ne changer que les bools
    void OnStateExit()
    {
        switch (_currentState)
        {
            case PlayerStateMode.LOCOMOTION:
                break;
            case PlayerStateMode.ROLL:

                _animator.SetBool("isRolling", false);
                _rollCount = 0; 

                break;
            case PlayerStateMode.SPRINT:

                _animator.SetBool("isRunning", false);


                break;
        }
    }



    void TransitionToState(PlayerStateMode toState)
    {
        OnStateExit();

        _currentState = toState;

        OnStateEnter();
    }

    #endregion


    void SetInput()
    {
        _direction.x = Input.GetAxis("Horizontal");
        _direction.y = Input.GetAxis("Vertical");
    }

}

