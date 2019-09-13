using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    private const int ROW_WIDTH = 10;
    public GameObject OfficeSectionPrefab;
    public ObstacleBank ObstacleBank;
    private List<WorldRow> _worldRows = new List<WorldRow>();

    private class WorldRow {
        private GameObject _officeSectionObject;
        private List<GameObject> _obstacleObjects = new List<GameObject>();
        
        public WorldRow(GameObject officeSectionPrefab, ObstacleBank obstacleBank, int z, List<int> obstacles) {
            _officeSectionObject = Instantiate(officeSectionPrefab, new Vector3(0,0,z), Quaternion.identity);
            float xOffset = 0.5f * obstacles.Count-0.5f;
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
    //      Initialization
    //*****************************************************************************
    
    void Start()
    {
        // FILLER CODE!!!
        for (int i = 0; i < 12; i++) {
            CreateNextSection();
        }
    }

    //*****************************************************************************
    //      Section Creation/Destruction
    //*****************************************************************************
    private int _nextSectionDestroy = 0;

    public void ProgressNextSection() {
        CreateNextSection();
        DestroyNextSection();
    }

    public void CreateNextSection() {
        CreateSection(_worldRows.Count);
    }

    private void CreateSection(int z) {    
       _worldRows.Add(new WorldRow(OfficeSectionPrefab, ObstacleBank, z, GenerateRandomSectionObstacles()));
    }

    public void DestroyNextSection() {
        DestroySection(_nextSectionDestroy);
    }

    private void DestroySection(int z) {
        if (z>=0 && z < _worldRows.Count && _worldRows[z] != null) {
            _worldRows[z].ClearObjects();
            _worldRows[z] = null;
        }
    }

    //*****************************************************************************
    //      Section Generation
    //*****************************************************************************
    
    private List<int> GenerateRandomSectionObstacles() {
        // FILLER CODE!!!
        List<int> obstacles = new List<int>();
        for (int i = 0; i < ROW_WIDTH; i++) {
            obstacles.Add(Random.Range(-6, 3));
        }
        return obstacles;
    }
}
