using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public const int ROW_WIDTH = 10;
    public const float TILE_SIZE = 1f;
    public const int NUMBER_ROWS_AHEAD = 20;
    public const int NUMBER_ROWS_BEHIND = 20;
    public const int EMPTY_TILE_INDEX = -1;
    public const int PAYDAY_ROW_INTERVAL_FIRST = 20;
    public const int PAYDAY_ROW_INTERVAL_BASE = 20;
    public const int PAYDAY_ROW_INTERVAL_DELTA = 3;
    public const int MONEY_START = 1000;
    public const int MONEY_PAYDAY = 500;
	public const int MONEY_STUNT = 400;
	public const int MONEY__LOW_THRESHOLD = 500;

    public const float SMASH_MIN_DIST = 3.0f;
    public const float SMASH_MAX_DIST = 8.0f;
    public const float SMASH_TILE_CHARGE = 0.3f;
}
