using System;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public static event Action onLevelComplete;

    private void OnTriggerEnter(Collider other)
    {
        onLevelComplete?.Invoke();
    }
}
