using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public GameObject OfficeSectionPrefab;
    public ObstacleBank ObstacleBank;
    private List<WorldRow> _worldRows = new List<WorldRow>();

    private class WorldRow {
        private GameObject _officeSectionObject;
        private List<GameObject> _obstacleObjects = new List<GameObject>();
        
        public WorldRow(GameObject officeSectionPrefab, ObstacleBank obstacleBank, int z, List<int> obstacles) {
            _officeSectionObject = Instantiate(officeSectionPrefab, new Vector3(0,0,z), Quaternion.identity);
            float xOffset = Constants.TILE_SIZE * 0.5f * (obstacles.Count - 1);
            for (int i = 0; i < obstacles.Count; i++) {
                if (obstacles[i] >= 0) {
                    _obstacleObjects.Add(Instantiate(obstacleBank.GetObstaclePrefab(obstacles[i]), new Vector3(i-xOffset, 0, z), Quaternion.identity));
                }
            }
        }

        public void ClearObjects() {
            Destroy(_officeSectionObject);
            foreach(GameObject obstacleObject in _obstacleObjects) {
                Destroy(obstacleObject);
            }
        }

    }
       
    //*****************************************************************************
    //      Startup
    //*****************************************************************************
    
    void Start()
    {
        // FILLER CODE!!!
        for (int i = 0; i < Constants.NUMBER_ROWS_AHEAD; i++) {
            CreateRow(i);
        }
    }

    //*****************************************************************************
    //      Section Creation/Destruction
    //*****************************************************************************
    private int _playerCurrentRow = 0;
    private int _nextRowDestroy = 0;

    public void PlayerAdvanceOneRow() {
        _playerCurrentRow++;
        while(_worldRows.Count < _playerCurrentRow + Constants.NUMBER_ROWS_AHEAD) {
            CreateRow(_worldRows.Count);
        }

        while (_nextRowDestroy < _playerCurrentRow - Constants.NUMBER_ROWS_BEHIND) {
            DestroyRow(_nextRowDestroy++);
        }
    }

    private void CreateRow(int z) {    
       _worldRows.Add(new WorldRow(OfficeSectionPrefab, ObstacleBank, z, RandomGeneration.GenerateowObstacles()));
        Debug.Log("Created Row " + z);
    }

    private void DestroyRow(int z) {
        if (z>=0 && z < _worldRows.Count && _worldRows[z] != null) {
            _worldRows[z].ClearObjects();
            _worldRows[z] = null;
        }
        Debug.Log("Destroyed Row " + z);
    }
}
