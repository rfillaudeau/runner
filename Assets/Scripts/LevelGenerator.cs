using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private int _levelSize = 5;
    [SerializeField] private GameObject _levelStart;
    [SerializeField] private GameObject _levelEnd;
    [SerializeField] private GameObject[] _levelElements;
    [SerializeField] private float _levelElementSize = 30f;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        Vector3 mapPosition = Vector3.zero;
        Instantiate(_levelStart, mapPosition, Quaternion.identity);

        mapPosition.z += _levelElementSize;

        for (int i = 0; i < _levelSize; i++)
        {
            int index = Random.Range(0, _levelElements.Length);

            Instantiate(_levelElements[index], mapPosition, Quaternion.identity);

            mapPosition.z += _levelElementSize;
        }

        Instantiate(_levelEnd, mapPosition, Quaternion.identity);
    }
}
