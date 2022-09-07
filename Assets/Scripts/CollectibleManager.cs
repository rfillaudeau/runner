using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    [SerializeField] private float _secondsBetweenChecks = 1f;
    [SerializeField] private float _enableCollectibleDistance = 1f;

    private List<Collectible> _collectibles;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        LevelGenerator.onLevelGenerated += StartCheckingCollectibles;
    }

    private void OnDisable()
    {
        LevelGenerator.onLevelGenerated -= StartCheckingCollectibles;
    }

    private void StartCheckingCollectibles()
    {
        _collectibles = new List<Collectible>();
        _collectibles.AddRange(FindObjectsOfType<Collectible>());

        StartCoroutine(ManageCollectiblesVisibility());
    }

    private IEnumerator ManageCollectiblesVisibility()
    {
        while (gameObject.activeInHierarchy)
        {
            foreach (Collectible collectible in _collectibles)
            {
                if (collectible == null)
                {
                    continue;
                }

                if (Vector3.Distance(_camera.transform.position, collectible.transform.position) <= _enableCollectibleDistance)
                {
                    collectible.gameObject.SetActive(true);
                }
                else
                {
                    collectible.gameObject.SetActive(false);
                }
            }

            yield return new WaitForSeconds(_secondsBetweenChecks);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(Camera.main.transform.position, _enableCollectibleDistance);
    }
}
