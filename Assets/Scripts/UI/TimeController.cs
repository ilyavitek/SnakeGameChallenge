using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour {

	#region Editor Variables

	#endregion


	#region Public Properties

	#endregion


	#region Private Variables

	Text m_timeText;

	float m_elapsedTime;

	#endregion


	#region Behaviour Overrides

	void Start () {
		m_timeText = GetComponent<Text> ();
		m_elapsedTime = 0f;
	}

	void Update () {
		m_elapsedTime += Time.deltaTime;
		m_timeText.text = m_elapsedTime.ToString (StaticManager.TIME_FORMAT) + StaticManager.SECONDS_TEXT;
	}

	#endregion


	#region Public Methods

	#endregion


	#region Private Methods

	#endregion
}
