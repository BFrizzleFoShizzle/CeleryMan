using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomGeneration
{

    //*****************************************************************************
    //      Row Generation
    //*****************************************************************************

    // Pure Random Obstacle Placement
    public static int[] GenerateRowObstacles_Random(ObstacleBank obstacleBank) {
        List<int> obstacles = new List<int>();
        for (int i = 0; i < Constants.ROW_WIDTH; i++) {
            if (RollPercentageChance(30f)) obstacles.Add(Random.Range(0, obstacleBank.GetMaxObstacleId() + 1));
            else obstacles.Add(-1);
        }
        return obstacles.ToArray();
    }

    // Pure Random Obstacle Placement with a challenge level input
    public static int[] GenerateRowObstacles_RandomWithChallenge(ObstacleBank obstacleBank, int challengeLevel) {
        List<int> obstacles = new List<int>();
        for (int i = 0; i < Constants.ROW_WIDTH; i++) {
            if (RollPercentageChance(10f+ challengeLevel)) obstacles.Add(Random.Range(0, obstacleBank.GetMaxObstacleId() + 1));
            else obstacles.Add(-1);
        }
        return obstacles.ToArray();
    }

    // Pure Random Obstacle Placement
    public static int[] GenerateRowObstacles_Dynamic1(ObstacleBank obstacleBank, int challengeLevel, int[] prevRow) {
        bool[] paths = new bool[Constants.ROW_WIDTH];
        for (int i = 0; i < Constants.ROW_WIDTH; i++) {
            if (prevRow[i] < 0) paths[i] = true; // if path has no obstacle it is a path => true
        }
        return GenerateRowObstacles_Dynamic1(obstacleBank, challengeLevel, paths);
    }

    private static int[] GenerateRowObstacles_Dynamic1(ObstacleBank obstacleBank, int challengeLevel, bool[] paths) {
        bool[] guaranteedPaths = new bool[Constants.ROW_WIDTH];
        for (int i = 0; i < Constants.ROW_WIDTH; i++) {
            if(IsPath(paths, i)) {
                int startI = i;
                while (IsPath(paths, i + 1)) i++;                
                guaranteedPaths[Random.Range(startI, i + 1)] = true;                
            }
        }

        List<int> obstacles = new List<int>();
        for (int i = 0; i < Constants.ROW_WIDTH; i++) {
            if(IsPath(guaranteedPaths, i)) {
                obstacles.Add(-1);
            } else {
                if (IsPath(paths, i)) {
                    if (RollPercentageChance(20f + challengeLevel)) obstacles.Add(Random.Range(0, obstacleBank.GetMaxObstacleId() + 1));
                    else obstacles.Add(-1);
                } else {
                    if (RollPercentageChance(10f + challengeLevel)) obstacles.Add(Random.Range(0, obstacleBank.GetMaxObstacleId() + 1));
                    else obstacles.Add(-1);
                }
            }

        }
        return obstacles.ToArray();
    }

    private static bool IsPath(bool[] paths, int index) {
        if (index < 0 || index >= paths.Length) return false;
        else return paths[index];
    }


    //*****************************************************************************
    //     Dice
    //*****************************************************************************
    public static bool RollPercentageChance(float pctChance) {
        return (Random.Range(0f, 100)<= pctChance);
    }
}
