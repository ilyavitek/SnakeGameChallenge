using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	#region Editor Variables

	[SerializeField] GameObject m_gameOverPanel;

	[SerializeField] Text m_gameOverText;

	[SerializeField] SnakeController m_snake;

	[Range (5, 15)]
	[SerializeField] int m_fieldSizeX;
	[Range (5, 15)]
	[SerializeField] int m_fieldSizeY;

	[SerializeField] Transform m_platform;

	#endregion


	#region Public Properties

	public static GameController Instance {
		get { return m_instance; }
	}


	public int FieldSizeX {
		get { return m_fieldSizeX; }
	}

	public int FieldSizeY {
		get { return m_fieldSizeY; }
	}


	public SnakeController Snake {
		get { return m_snake; }
	}

	#endregion


	#region Private Variables

	static GameController m_instance;

	#endregion


	#region Behaviour Overrides

	void Awake () {
		if (m_instance != null) {
			Debug.LogError ("Dublication of Singleton");
			Destroy (this.gameObject);
		} else {
			m_instance = this;
		}		
	}

	void Start () {
		m_platform.localScale = new Vector3 (m_fieldSizeX, m_fieldSizeY, 1f);
		m_gameOverPanel.SetActive (false);
	}

	void OnEnable () {
		SnakeController.Crash += FinishGame;
		SnakeController.Win += WinGame;
	}

	void OnDisable () {
		SnakeController.Crash -= FinishGame;
		SnakeController.Win -= WinGame;
	}

	#endregion


	#region Public Methods

	public void FinishGame () {
		Time.timeScale = 0f;
		m_gameOverPanel.SetActive (true);
	}

	public void WinGame () {
		m_gameOverText.text = StaticManager.WIN_TEXT;
		FinishGame ();
	}

	public void Pause (bool pause) {
		Time.timeScale = pause ? 0f : 1f;
	}

	public void Restart () {
		Time.timeScale = 1f;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	#endregion


	#region Private Methods

	#endregion
}
