using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomGeneration
{

    //*****************************************************************************
    //      Row Generation
    //*****************************************************************************

    public static int[] GenerateRowObstacles_Empty() {
        List<int> obstacles = new List<int>();
        for (int i = 0; i < Constants.ROW_WIDTH; i++) obstacles.Add(Constants.EMPTY_TILE_INDEX);
        return obstacles.ToArray();
    }

    // Pure Random Obstacle Placement
    public static int[] GenerateRowObstacles_Random(ObstacleBank obstacleBank) {
        List<int> obstacles = new List<int>();
        for (int i = 0; i < Constants.ROW_WIDTH; i++) {
            if (RollPercentageChance(30f)) obstacles.Add(Random.Range(0, obstacleBank.GetMaxObstacleId() + 1));
            else obstacles.Add(Constants.EMPTY_TILE_INDEX);
        }
        return obstacles.ToArray();
    }

    // Pure Random Obstacle Placement with a challenge level input
    public static int[] GenerateRowObstacles_RandomWithChallenge(ObstacleBank obstacleBank, int challengeLevel) {
        List<int> obstacles = new List<int>();
        for (int i = 0; i < Constants.ROW_WIDTH; i++) {
            if (RollPercentageChance(10f+ challengeLevel)) obstacles.Add(Random.Range(0, obstacleBank.GetMaxObstacleId() + 1));
            else obstacles.Add(Constants.EMPTY_TILE_INDEX);
        }
        return obstacles.ToArray();
    }

    // Improved Obstacle Placement 1
    private const float BASE_OBSTACLE_CHANCE = 30f;
    private const float NO_PATH_OBSTACLE_CHANCE_MULTIPLIER = 0.5f;
    private const float CHALLENGE_LEVEL_MULTIPLIER = 1f;
    private const int CHALLENGE_LEVEL_MAX = 100;

    public static int[] GenerateRowObstacles_Improved1(ObstacleBank obstacleBank, int challengeLevel, int[] prevRow) {
        bool[] paths = new bool[Constants.ROW_WIDTH];
        for (int i = 0; i < Constants.ROW_WIDTH; i++) {
            if (prevRow[i] < 0) paths[i] = true; // if path has no obstacle it is a path => true
        }
        return GenerateRowObstacles_Improved1(obstacleBank, challengeLevel, paths);
    }

    private static int[] GenerateRowObstacles_Improved1(ObstacleBank obstacleBank, int challengeLevel, bool[] paths) {
        //Debug.Log("GenerateRowObstacles_Improved1 challengLevel=" + challengeLevel);
        if (challengeLevel > CHALLENGE_LEVEL_MAX) challengeLevel = CHALLENGE_LEVEL_MAX;
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
                obstacles.Add(Constants.EMPTY_TILE_INDEX);
            } else {
                float obstacleChance = CHALLENGE_LEVEL_MULTIPLIER * challengeLevel + BASE_OBSTACLE_CHANCE;
                //Debug.Log("obstacleChance=" + obstacleChance);
                if (IsPath(paths, i)) {
                    if (RollPercentageChance(obstacleChance)) obstacles.Add(Random.Range(0, obstacleBank.GetMaxObstacleId() + 1));
                    else obstacles.Add(Constants.EMPTY_TILE_INDEX);
                } else {
                    if (RollPercentageChance(NO_PATH_OBSTACLE_CHANCE_MULTIPLIER*obstacleChance)) obstacles.Add(Random.Range(0, obstacleBank.GetMaxObstacleId() + 1));
                    else obstacles.Add(Constants.EMPTY_TILE_INDEX);
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
