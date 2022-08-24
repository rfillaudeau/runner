using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimationHandler : MonoBehaviour
{
    [SerializeField] private Player _player;

    [SerializeField] private float _movementSpeed = 1f;

    private Animator _animator;

    private Vector3 _movement;

    private void OnEnable()
    {
        _player.onSlide += Slide;
        _player.onCollidedWithObstacle += Die;
        LevelEnd.onLevelComplete += Victory;
    }

    private void OnDisable()
    {
        _player.onSlide -= Slide;
        _player.onCollidedWithObstacle -= Die;
        LevelEnd.onLevelComplete -= Victory;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 normalizedMovement = _player.moveDirection.normalized;

        _movement = Vector3.Lerp(_movement, normalizedMovement, Time.deltaTime * _movementSpeed);

        _animator.SetFloat("VelocityX", _movement.x);
        _animator.SetFloat("VelocityZ", _movement.z);
    }

    private void Slide()
    {
        _animator.SetTrigger("Slide");
    }

    private void Die()
    {
        _animator.SetTrigger("Die");
    }

    private void Victory()
    {
        _animator.SetTrigger("Victory");
    }
}
