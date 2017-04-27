using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

	#region Editor Variables

	[SerializeField] Text m_highScoreText;

	#endregion


	#region Public Properties

	#endregion


	#region Private Variables

	Text m_scoreText;

	int m_score;

	int m_highScore;

	#endregion


	#region Behaviour Overrides

	void Start () {
		m_score = StaticManager.START_SCORE;
		m_highScore = PlayerPrefs.GetInt (StaticManager.HIGH_SCORE_PLAYER_PREF);
		m_scoreText = GetComponent<Text> ();
		UpdateScoreText ();
		UpdateHighScoreText ();
	}

	void OnEnable () {
		SnakeController.EatFruit += AddScore;
	}

	void OnDisable () {
		SnakeController.EatFruit -= AddScore;
	}

	#endregion


	#region Public Methods


	#endregion


	#region Private Methods

	void AddScore () {
		m_score += StaticManager.SCORE_INCREMENT;
		UpdateScoreText ();
		CheckHighScore ();
	}

	void UpdateScoreText () {
		m_scoreText.text = StaticManager.SCORE_TEXT + m_score;
	}

	void UpdateHighScoreText () {
		m_highScoreText.text = StaticManager.HIGH_SCORE_TEXT + m_highScore;
	}

	void CheckHighScore () {
		if (m_score > m_highScore) {
			m_highScore = m_score;
			PlayerPrefs.SetInt (StaticManager.HIGH_SCORE_PLAYER_PREF, m_highScore);
			UpdateHighScoreText ();
		}
	}

	#endregion
}
