using System;
using System.Collections;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public static event Action onCollected;

    [SerializeField] private Animator _animator;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Collected());
    }

    private IEnumerator Collected()
    {
        onCollected?.Invoke();

        _animator.SetTrigger("Collected");

        yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        Destroy(gameObject);
    }
}
