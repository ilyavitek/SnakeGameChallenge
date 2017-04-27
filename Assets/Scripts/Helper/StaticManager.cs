using UnityEngine;

public class StaticManager {

	#region Texts

	public const string SCORE_TEXT = "Score: ";
	public const string HIGH_SCORE_TEXT = "HighScore: ";
	public const string WIN_TEXT = "YOU WIN!";
	public const string SECONDS_TEXT = "s";
	public const string TIME_FORMAT = "N0";

	#endregion

	#region Directions

	public static Vector3[] DirectionVariants = new Vector3[4] {
		Vector3.up,
		Vector3.down,
		Vector3.left,
		Vector3.right
	};

	#endregion

	#region TAGs

	public const string FRUIT_TAG = "Fruit";
	public const string PLATFORM_TAG = "Platform";
	public const string BODY_TAG = "Body";

	#endregion

	#region PlayerPrefs

	public const string SOLID_WALLS_SETTING = "SolidWalls";

	public const string SFX_MUTE_SETTING = "SFX";
	public const string MUSIC_MUTE_SETTING = "Music";

	public const string HIGH_SCORE_PLAYER_PREF = "HS";

	public const string SNAKE_TIME_FOR_STEP_PLAYER_PREF = "TimeForStep";

	#endregion

	#region Numbers

	public const int MIN_DISTANCE_FROM_WALLS_TO_START = 2;

	public const float DEFAULT_SNAKE_TIME_FOR_STEP = 0.4f;

	public const int START_SCORE = 0;

	public const int SCORE_INCREMENT = 1;

	#endregion

}
