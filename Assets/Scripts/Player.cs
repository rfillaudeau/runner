using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    public event Action onCollidedWithObstacle;
    public event Action onSlide;

    public bool isGrounded { get { return _isGrounded; } }
    public Vector3 moveDirection { get { return _moveDirection; } }

    [SerializeField] private float _forwardSpeed = 1f;
    [SerializeField] private float _sideSpeed = 1f;
    [SerializeField] private float _slideForce = 10f;
    [SerializeField] private float _slideLength = 1f;
    [SerializeField] private string _obstacleTag;

    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    private PlayerInput _input;

    private float _distanceToGround;

    private Vector3 _moveDirection = Vector3.zero;

    private bool _slide = false;
    private bool _isGrounded = false;
    private bool _isSliding = false;
    private bool _canMove = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        _input = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        _distanceToGround = transform.localScale.y * _collider.height / 2f + 0.1f;
    }

    private void OnEnable()
    {
        _input.onTap += OnTap;
        GameManager.onStartGame += EnableMovement;
        LevelEnd.onLevelComplete += DisableMovement;
    }

    private void OnDisable()
    {
        _input.onTap -= OnTap;
        GameManager.onStartGame -= EnableMovement;
        LevelEnd.onLevelComplete -= DisableMovement;
    }

    private void FixedUpdate()
    {
        HandleGroundDetection();
        HandleMovement();
        HandleSlide();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(_obstacleTag))
        {
            onCollidedWithObstacle?.Invoke();
            DisableMovement();
        }
    }

    private void EnableMovement()
    {
        _canMove = true;
    }

    private void DisableMovement()
    {
        _rigidbody.velocity = Vector3.zero;

        _canMove = false;
    }

    private void HandleSlide()
    {
        if (!_canMove || !_isGrounded || _isSliding)
        {
            _slide = false;

            return;
        }

        if (_slide)
        {
            StartCoroutine(Slide());
        }
    }

    private void HandleMovement()
    {
        if (!_canMove || !_isGrounded)
        {
            _moveDirection = Vector3.zero;

            return;
        }

        _moveDirection = transform.forward * Time.fixedDeltaTime * _forwardSpeed;
        _moveDirection.x = _input.swipeDirection.x * Time.fixedDeltaTime * _sideSpeed;

        _rigidbody.MovePosition(transform.position + _moveDirection);
    }

    private void OnTap()
    {
        _slide = true;
    }

    private void HandleGroundDetection()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _distanceToGround);
    }

    private IEnumerator Slide()
    {
        float colliderHeight = _collider.height;
        Vector3 colliderCenter = _collider.center;

        colliderCenter.y = -colliderHeight / 4f;

        _collider.center = colliderCenter;
        _collider.height = colliderHeight / 2f;

        _rigidbody.AddForce(transform.forward * _slideForce, ForceMode.Impulse);

        _isSliding = true;

        onSlide?.Invoke();

        yield return new WaitForSeconds(_slideLength);

        _collider.center = Vector3.zero;
        _collider.height = colliderHeight;

        _isSliding = false;

        _slide = false;
    }
}
