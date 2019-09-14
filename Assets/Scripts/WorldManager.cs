﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public GameObject OfficeSectionPrefab;
    public GameObject OfficePaydaySectionPrefab;
    public ObstacleBank ObstacleBank;
	public ObstacleBank DynamicObstacleBank;

	private class WorldRow {
        private GameObject _officeSectionObject;
        private List<GameObject> _obstacleObjects = new List<GameObject>();
        private int[] _obstacles;
        public bool IsDynamicObjectRow { get; private set; }
        public bool IsPaydayRow { get; private set; }

        public WorldRow(GameObject officeSectionPrefab, ObstacleBank obstacleBank, int z, int[] obstacles, bool isDynamicObjectRow, bool isPaydayRow) {
            IsDynamicObjectRow = isDynamicObjectRow;
            IsPaydayRow = isPaydayRow;
            _obstacles = obstacles;
            _officeSectionObject = Instantiate(officeSectionPrefab, new Vector3(0,0,z), Quaternion.identity);
            float xOffset = Constants.TILE_SIZE * 0.5f * (obstacles.Length - 1);
            for (int i = 0; i < obstacles.Length; i++) {
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

        public int GetObstcleAt(int x) {
            if (x < 0 || x >= _obstacles.Length) return int.MaxValue;
            return _obstacles[x];
        }
    }

    //*****************************************************************************
    //      Startup
    //*****************************************************************************

    void Start()
    {
        // FILLER CODE!!!
        // TODO Thomas
        for (int i = 0; i < Constants.NUMBER_ROWS_AHEAD; i++) {
            CreateRow(i);
        }
    }

    //*****************************************************************************
    //      Section Creation/Destruction
    //*****************************************************************************
    private int _playerCurrentRow = 0;
    private int _nextRowDestroy = 0;
    private List<WorldRow> _worldRows = new List<WorldRow>();
    private int[] _prevRowObstacles;

    public bool PlayerCanEnter(Vector3 playerPos, Vector3 offset, float distance) {
		Ray ray = new Ray(playerPos, offset);
		RaycastHit[] hits = Physics.RaycastAll(ray, distance);

		for(int i=0; i< hits.Length;++i)
		{
			Debug.Log(hits[i].collider.gameObject);
		}

		return hits.Length == 0;
    }

    public bool IsDynamicObjectRow(int z) {
        if (z > _worldRows.Count || z < 0 || _worldRows[z] == null) return false;
        return _worldRows[z].IsDynamicObjectRow;
    }

    public bool IsPaydayRow(int z) {
        if (z > _worldRows.Count || z < 0 || _worldRows[z] == null) return false;
        return _worldRows[z].IsPaydayRow;
    }

    public void PlayerAdvanceToRow(int playerRow) {
        if (playerRow < _playerCurrentRow) Debug.LogError("Player Moved Back! " + _playerCurrentRow + " --> " + playerRow);
        _playerCurrentRow = playerRow;
        while (_worldRows.Count < _playerCurrentRow + Constants.NUMBER_ROWS_AHEAD) {
            CreateRow(_worldRows.Count);
        }

        while (_nextRowDestroy < _playerCurrentRow - Constants.NUMBER_ROWS_BEHIND) {
            DestroyRow(_nextRowDestroy++);
        }
    }

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
		int[] rowObstacles = new int[Constants.ROW_WIDTH];

		bool startRow = false;
		bool paydayRow = false;
		bool dynamicRow = false;
		if (z == 0 || z == 1)
		{
			startRow = true;
		}
		else if (z % Constants.PAYDAY_ROW_INTERVAL == 0)
		{
			paydayRow = true;
		}
		else
		{
			dynamicRow = RandomGeneration.RollPercentageChance(20f);
		}

		ObstacleBank bank = ObstacleBank;

        if (startRow || paydayRow) {
			rowObstacles = RandomGeneration.GenerateRowObstacles_Empty();
		}
		else if (dynamicRow)
		{
			rowObstacles = RandomGeneration.GenerateRowObstacles_Dynamic(DynamicObstacleBank);
			bank = DynamicObstacleBank;
		}
		else
		{
            // rowObstacles = RandomGeneration.GenerateRowObstacles_Random(ObstacleBank);
            rowObstacles = RandomGeneration.GenerateRowObstacles_Improved1(ObstacleBank, _playerCurrentRow / 10, _prevRowObstacles);
        }

        _worldRows.Add(new WorldRow(paydayRow ? OfficePaydaySectionPrefab : OfficeSectionPrefab, bank, z, rowObstacles, dynamicRow, paydayRow));
		_prevRowObstacles = rowObstacles;
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
