using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBank : MonoBehaviour {

    public GameObject[] ObstaclePrefabs; // Don't reference this from other scripts. It's public so that objects can be added from Unity GUI

    public GameObject GetObstaclePrefab(int obstacleIndex) {
        if (obstacleIndex < 0 || obstacleIndex >= ObstaclePrefabs.Length) {
            Debug.LogError("ObstacleBank invalid obstacleIndex=" + obstacleIndex);
            return null;
        }
        return ObstaclePrefabs[obstacleIndex];
    }

    public int GetMaxObstacleId() {
        return ObstaclePrefabs.Length - 1;
    }
}