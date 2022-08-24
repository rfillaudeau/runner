using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static event Action<Vector2> onSwipe;
    public static event Action onTap;

    [SerializeField] private float _touchThreshold = 1f;

    private Vector2 _touchPreviousPosition;
    private Vector2 _touchCurrentPosition;

    private float _touchTravelDistance = 0f;

    private bool _registerInputs = false;

    private void OnEnable()
    {
        GameManager.onStartGame += RegisterInputs;
    }

    private void OnDisable()
    {
        GameManager.onStartGame -= RegisterInputs;
    }

    private void Update()
    {
        HandleTouch();
    }

    private void HandleTouch()
    {
        if (!_registerInputs)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _touchPreviousPosition = Input.mousePosition;
            _touchCurrentPosition = Input.mousePosition;
            _touchTravelDistance = 0f;
        }

        if (Input.GetMouseButton(0))
        {
            _touchCurrentPosition = Input.mousePosition;
            _touchTravelDistance += Vector2.Distance(_touchPreviousPosition, _touchCurrentPosition);

            HandleSwipe();

            _touchPreviousPosition = _touchCurrentPosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            HandleTapAction();
        }
    }

    private void HandleSwipe()
    {
        if (Vector3.Distance(_touchPreviousPosition, _touchCurrentPosition) >= _touchThreshold)
        {
            float xMovement = _touchCurrentPosition.x - _touchPreviousPosition.x;

            if (xMovement > 0f)
            {
                onSwipe?.Invoke(Vector2.right);
            }
            else
            {
                onSwipe?.Invoke(Vector2.left);
            }
        }
    }

    private void HandleTapAction()
    {
        if (_touchTravelDistance >= _touchThreshold)
        {
            return;
        }

        onTap?.Invoke();
    }

    private void RegisterInputs()
    {
        _registerInputs = true;
    }
}
