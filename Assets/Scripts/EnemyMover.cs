using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 5.0f)] private float moveSpeed = 1.0f;
    
    private Enemy enemy;
    private GridManager gridManager;
    private PathFinder pathFinder;

    private List<Node> path = new List<Node>();

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }

    private void OnEnable()
    {
        RecalculatePath(true);
        ReturnToStart();
    }

    private void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = Vector2Int.zero;

        if (resetPath)
        {
            coordinates = pathFinder.StartCoordinates;
        }
        else
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }

        StopAllCoroutines();
        path.Clear();
        path = pathFinder.GetNewPath(coordinates);
        StartCoroutine(FollowPath());
    }

    private void ReturnToStart()
    {
        transform.position = gridManager.GetPositionFromCoordinates(pathFinder.StartCoordinates);
    }

    private IEnumerator FollowPath()
    {
        for (int nodeId = 1; nodeId < path.Count; ++nodeId)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[nodeId].coordinates);
            float travelPercent = 0.0f;

            transform.LookAt(endPosition);

            while (travelPercent < 1.0f)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);

                travelPercent += Time.deltaTime * moveSpeed;
                yield return new WaitForEndOfFrame();
            }
        }

        FinishPath();
    }

    private void FinishPath()
    {
        enemy.PenalizeGold();
        gameObject.SetActive(false);
    }
}
