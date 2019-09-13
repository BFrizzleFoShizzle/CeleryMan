using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomGeneration
{

    //*****************************************************************************
    //      Row Generation
    //*****************************************************************************
    public static List<int> GenerateowObstacles() {
        return GenerateRandomRowObstacles();
    }

    // Pure Random Obstacle Placement
    public static List<int> GenerateRandomRowObstacles() {
        // FILLER CODE!!!
        List<int> obstacles = new List<int>();
        for (int i = 0; i < Constants.ROW_WIDTH; i++) {
            obstacles.Add(Random.Range(-6, 3));
        }
        return obstacles;
    }
}
