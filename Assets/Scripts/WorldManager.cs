using System.Collections;
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
        private GameObject[] _obstacleObjects;
        private int[] _obstacles;
        public bool IsDynamicObjectRow { get; private set; }
        public bool IsPaydayRow { get; private set; }

        public WorldRow(GameObject officeSectionPrefab, ObstacleBank obstacleBank, int z, int[] obstacles, bool isDynamicObjectRow, bool isPaydayRow) {
            IsDynamicObjectRow = isDynamicObjectRow;
            IsPaydayRow = isPaydayRow;
            _obstacles = obstacles;
            _obstacleObjects = new GameObject[_obstacles.Length];
            _officeSectionObject = Instantiate(officeSectionPrefab, new Vector3(0,0,z), Quaternion.identity);
            float xOffset = Constants.TILE_SIZE * 0.5f * (_obstacles.Length - 1);
            for (int i = 0; i < _obstacles.Length; i++) {
                if (_obstacles[i] != Constants.EMPTY_TILE_INDEX) {
                    _obstacleObjects[i] = Instantiate(obstacleBank.GetObstaclePrefab(_obstacles[i]), new Vector3(i-xOffset, 0, z), Quaternion.identity);
                }
            }
        }

        public void ClearObjects() {
            Destroy(_officeSectionObject);
            for (int i = 0; i < _obstacles.Length; i++) {
                if (_obstacles[i] != Constants.EMPTY_TILE_INDEX) Destroy(_obstacleObjects[i]);
            }
        }

        public int GetObstacleAt(int x) {
            if (x < 0 || x >= _obstacles.Length) return int.MaxValue;
            return _obstacles[x];
        }

        public bool DestroyObstacle(int x) {
            if (x < 0 || x >= _obstacles.Length) return false;
            if (_obstacles[x] != Constants.EMPTY_TILE_INDEX) {
                Destroy(_obstacleObjects[x]);
                _obstacles[x] = Constants.EMPTY_TILE_INDEX;
                return true;
            }
            return false;
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
    private bool _prevRowSpecial = false;
    private int _paydayRowInterval = Constants.PAYDAY_ROW_INTERVAL_BASE;
    private int _nextPayDay = Constants.PAYDAY_ROW_INTERVAL_FIRST;
    private bool _paydayReached = false;

    public bool PlayerCanEnter(Vector3 playerPos, Vector3 offset, float distance) {
		Ray ray = new Ray(playerPos, offset);
		RaycastHit[] hits = Physics.RaycastAll(ray, distance);

		for(int i=0; i< hits.Length;++i)
		{
			Debug.Log(hits[i].collider.gameObject);
		}

		return hits.Length == 0;
    }

    public bool DestroyObstacle(int x, int z) {
        if (z > _worldRows.Count || z < 0 || _worldRows[z] == null) return false;
        return _worldRows[z].DestroyObstacle(x);
    }

    public bool IsDynamicObjectRow(int z) {
        if (z > _worldRows.Count || z < 0 || _worldRows[z] == null) return false;
        return _worldRows[z].IsDynamicObjectRow;
    }

    public bool IsPaydayRow(int z) {
        if (z > _worldRows.Count || z < 0 || _worldRows[z] == null) return false;
        return _worldRows[z].IsPaydayRow;
    }

    public bool CheckPaydayReached() {
        if (_paydayReached) {
            _paydayReached = false;
            return true;
        }
        return false;
    }

    public void PlayerAdvanceToRow(int playerRow) {
        if (playerRow < _playerCurrentRow) Debug.LogError("Player Moved Back! " + _playerCurrentRow + " --> " + playerRow);
        _playerCurrentRow = playerRow;
        if (_worldRows[_playerCurrentRow].IsPaydayRow) _paydayReached = true;
        while (_worldRows.Count < _playerCurrentRow + Constants.NUMBER_ROWS_AHEAD) {
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
		else if (z == _nextPayDay)
		{
			paydayRow = true;
            _nextPayDay += _paydayRowInterval;
            _paydayRowInterval += Constants.PAYDAY_ROW_INTERVAL_DELTA;
        }
		else
		{
            if(!_prevRowSpecial) dynamicRow = RandomGeneration.RollPercentageChance(20f);
		}

		ObstacleBank bank = ObstacleBank;

        if (startRow || paydayRow) {
			rowObstacles = RandomGeneration.GenerateRowObstacles_Empty();
            _prevRowSpecial = true;

        }
		else if (dynamicRow)
		{
			rowObstacles = RandomGeneration.GenerateRowObstacles_Dynamic(DynamicObstacleBank);
			bank = DynamicObstacleBank;
            _prevRowSpecial = true;

        }
		else
		{
            // rowObstacles = RandomGeneration.GenerateRowObstacles_Random(ObstacleBank);
            rowObstacles = RandomGeneration.GenerateRowObstacles_Improved1(ObstacleBank, _playerCurrentRow / 10, _prevRowObstacles);
            _prevRowSpecial = false;
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
