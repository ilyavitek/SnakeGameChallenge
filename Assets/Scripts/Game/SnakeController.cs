using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeController : MonoBehaviour {

	#region Editor Variables

	[SerializeField] ParticleSystem m_eatFruitEffect;

	[SerializeField] GameObject m_snakeBodyPrefab;

	[SerializeField] List<Transform> m_snakeBody;

	[SerializeField] Toggle m_solidWallsTogle;

	[SerializeField] Slider m_snakeSpeedSlider;

	#endregion


	#region Public Properties

	public List<Transform> SnakeBody {
		get { return m_snakeBody; }
	}

	public bool SolidWalls {
		get { return m_solidWalls; }
		set {
			m_solidWalls = value; 
			PlayerPrefs.SetInt (StaticManager.SOLID_WALLS_SETTING, m_solidWalls ? 1 : 0);
		}
	}

	#endregion


	#region Delegates

	public delegate void SnakeAction ();

	#endregion


	#region Events

	public static event SnakeAction EatFruit;
	public static event SnakeAction Crash;
	public static event SnakeAction Win;

	#endregion


	#region Private Variables

	BoxCollider2D m_boxCollider2DToActivate;

	Vector3 m_direction;
	Vector3 m_nextDirection;

	Timer m_stepTimer;

	bool m_solidWalls;

	int m_winLength;

	float m_timeForStep;

	#endregion


	#region Behaviour Overrides

	void Start () {
		DefineTimeForStep ();
		LoadSolidWallsSetting ();
		SetRandomStartPosAndDirection ();
		StartStepTimer ();
	}

	void Update () {
		DefineNextDirection ();
		CheckToMove ();
	}

	void OnTriggerEnter2D (Collider2D other) {
		bool collisionWithFruit = (other.tag == StaticManager.FRUIT_TAG);
		bool collisionWithBody = (other.tag == StaticManager.BODY_TAG);

		if (collisionWithFruit) {
			CheckWin ();
			EatFruit ();
			ShowEatFruitEffect ();
			IncreaseSnakeSize ();
		} else if (collisionWithBody) {
			Crash ();
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		bool outOfField = other.tag == StaticManager.PLATFORM_TAG;

		if (outOfField) {
			if (m_solidWalls) {
				Crash ();
			} else {
				RevertHeadPosition ();	
			}
		}
	}

	#endregion


	#region Public Methods

	public void ChangeSnakeSpeed (float sliderValue) {
		m_timeForStep = 1f - sliderValue;
		PlayerPrefs.SetFloat (StaticManager.SNAKE_TIME_FOR_STEP_PLAYER_PREF, m_timeForStep);
		StartStepTimer ();
	}

	#endregion


	#region Private Methods

	void DefineTimeForStep () {
		if (PlayerPrefs.HasKey (StaticManager.SNAKE_TIME_FOR_STEP_PLAYER_PREF)) {
			m_timeForStep = PlayerPrefs.GetFloat (StaticManager.SNAKE_TIME_FOR_STEP_PLAYER_PREF);
			m_snakeSpeedSlider.value = 1f - m_timeForStep;
		} else {
			m_timeForStep = StaticManager.DEFAULT_SNAKE_TIME_FOR_STEP;
		}
	}

	void StartStepTimer () {
		m_stepTimer = new Timer (m_timeForStep);
	}

	void SetRandomStartPosAndDirection () {
		m_nextDirection = StaticManager.DirectionVariants [Random.Range (0, StaticManager.DirectionVariants.Length)];

		int sizeX = GameController.Instance.FieldSizeX;
		int sizeY = GameController.Instance.FieldSizeY;

		m_winLength = sizeX * sizeY;

		int randomX = RandomPosThroughSize (sizeX);
		int randomY = RandomPosThroughSize (sizeY);

		m_snakeBody [0].position = new Vector3 (randomX, randomY);
	}

	void DefineRotation (Transform tr) {
		if (m_direction == Vector3.up) {
			tr.rotation = Quaternion.Euler (0, 0, 0);
		} else if (m_direction == Vector3.down) {
			tr.rotation = Quaternion.Euler (0, 0, 180);
		} else if (m_direction == Vector3.right) {
			tr.rotation = Quaternion.Euler (0, 0, -90);
		} else {
			tr.rotation = Quaternion.Euler (0, 0, 90);
		}
	}

	void DefineNextDirection () {
		if (m_direction == Vector3.right || m_direction == Vector3.left) {
			if (Input.GetAxis ("Vertical") > 0f) {
				m_nextDirection = Vector3.up;
			}
			if (Input.GetAxis ("Vertical") < 0f) {
				m_nextDirection = Vector3.down;
			}
	
		} else if ((m_direction == Vector3.up || m_direction == Vector3.down)) {
			if (Input.GetAxis ("Horizontal") > 0f) {
				m_nextDirection = Vector3.right;
			}
			if (Input.GetAxis ("Horizontal") < 0f) {
				m_nextDirection = Vector3.left;
			}
		}
	}

	void CheckToMove () {
		if (m_stepTimer.Launch ()) {
			MoveSnake ();
			m_stepTimer.Restart ();
		}
	}

	void MoveSnake () {
		m_direction = m_nextDirection;

		Vector3 positionForNextBodyPart = m_snakeBody [0].position;
		Vector3 positionOfPrevBodyPart = m_snakeBody [0].position;

		// Move Head
		m_snakeBody [0].position += m_direction;

		// Move Body
		for (int i = 1; i < m_snakeBody.Count; i++) {
			positionForNextBodyPart = m_snakeBody [i].position;
			m_snakeBody [i].position += (positionOfPrevBodyPart - m_snakeBody [i].position);
			positionOfPrevBodyPart = positionForNextBodyPart;
		}

		if (m_boxCollider2DToActivate != null) {
			m_boxCollider2DToActivate.enabled = true;
			m_boxCollider2DToActivate = null;
		}
	}

	void ShowEatFruitEffect () {
		m_eatFruitEffect.transform.position = transform.position;
		m_eatFruitEffect.Play ();
	}

	void RevertHeadPosition () {
		if (m_direction == Vector3.right || m_direction == Vector3.left) {
			float newX = RevertCoordinate (m_snakeBody [0].position.x);
			m_snakeBody [0].position = new Vector3 (newX, m_snakeBody [0].position.y);
		} else {
			float newY = RevertCoordinate (m_snakeBody [0].position.y);
			m_snakeBody [0].position = new Vector3 (m_snakeBody [0].position.x, newY);
		}
	}

	void IncreaseSnakeSize () {
		GameObject newPart = Instantiate (m_snakeBodyPrefab, transform.position - m_direction, Quaternion.identity, transform.parent);
		m_snakeBody.Add (newPart.transform);

		m_boxCollider2DToActivate = newPart.GetComponent<BoxCollider2D> ();
	}

	void LoadSolidWallsSetting () {
		m_solidWalls = PlayerPrefs.GetInt (StaticManager.SOLID_WALLS_SETTING) == 1 ? true : false;
		m_solidWallsTogle.isOn = m_solidWalls;
	}

	void CheckWin () {
		if (m_snakeBody.Count == m_winLength) {
			Win ();
		}
	}

	int RandomPosThroughSize (int size) {
		size -= StaticManager.MIN_DISTANCE_FROM_WALLS_TO_START;
		return Random.Range (-size / 2, size / 2 + 1);
	}

	float RevertCoordinate (float coodrinate) {
		return -1 * coodrinate / Mathf.Abs (coodrinate) * (Mathf.Abs (coodrinate) - 1);
	}

	#endregion
}
