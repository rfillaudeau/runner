using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action onTap;

    public Vector2 swipeDirection { get; private set; }

    [SerializeField] private float _touchThreshold = 1f;

    private Vector2 _touchPreviousPosition;
    private Vector2 _touchCurrentPosition;

    private float _touchTravelDistance = 0f;

    [SerializeField] private float _waitForSavingTouchPosition = 0.05f;
    private Vector2 _touchPositionToSave;

    private float _elapsedTimeAfterLastSave = 0f;

    private void Update()
    {
        HandleMouse();
    }

    private void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _touchPreviousPosition = Input.mousePosition;
            _touchCurrentPosition = Input.mousePosition;
            _touchTravelDistance = 0f;

            _touchPositionToSave = _touchPreviousPosition;
            _elapsedTimeAfterLastSave = 0f;
        }

        if (Input.GetMouseButton(0))
        {
            _touchCurrentPosition = Input.mousePosition;

            if (_elapsedTimeAfterLastSave >= _waitForSavingTouchPosition)
            {
                _touchPreviousPosition = _touchPositionToSave;
                _touchTravelDistance += Vector2.Distance(_touchPreviousPosition, _touchCurrentPosition);

                _touchPositionToSave = _touchCurrentPosition;
                _elapsedTimeAfterLastSave = 0f;
            }

            _elapsedTimeAfterLastSave += Time.deltaTime;

            HandleSwipe();
        }

        if (Input.GetMouseButtonUp(0))
        {
            HandleTapAction();

            swipeDirection = Vector2.zero;
        }
    }

    private void HandleSwipe()
    {
        if (Vector3.Distance(_touchPreviousPosition, _touchCurrentPosition) >= _touchThreshold)
        {
            float xMovement = _touchCurrentPosition.x - _touchPreviousPosition.x;

            if (xMovement > 0f)
            {
                swipeDirection = Vector2.right;
            }
            else
            {
                swipeDirection = Vector2.left;
            }
        }
        else
        {
            swipeDirection = Vector2.zero;
        }
    }

    private void HandleTapAction()
    {
        if (_touchTravelDistance > _touchThreshold)
        {
            return;
        }

        onTap?.Invoke();
    }
}
