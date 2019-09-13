using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBank : MonoBehaviour
{

    public GameObject[] ObstaclePrefabs;

    public GameObject GetObstaclePrefab(int obstacleIndex) {
        if (obstacleIndex < 0 || obstacleIndex > ObstaclePrefabs.Length) {
            Debug.LogError("ObstacleBank invalid obstacleIndex=" + obstacleIndex);
            return null;
        }
        return ObstaclePrefabs[obstacleIndex];
    }
}
