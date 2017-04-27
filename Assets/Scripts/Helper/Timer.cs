using UnityEngine;
using System.Collections;

public class Timer {

	#region Other Variables

	float m_time;
	float m_currentTime;
	bool m_isStop = false;
	bool m_isDebug;

	#endregion

	public float CurrentTime {
		get { return m_currentTime; }
	}

	#region Constructor

	public Timer (float time, bool isDebug = false) {
		m_time = time;
		m_currentTime = time;
		m_isDebug = isDebug;
	}

	public Timer (bool isDebug = false) {
		m_isStop = true;
		m_isDebug = isDebug;
	}

	public bool IsActive () {
		return !m_isStop;
	}

	#endregion

	#region Public Methods

	public bool Launch () {
		if (!m_isStop) {
			if (m_isDebug) {
				DebugCurrentTime ();
			}
			if (m_currentTime > 0) {
				m_currentTime -= Time.deltaTime;
			}
			if (m_currentTime <= 0) {
				m_currentTime = m_time;
				return true;
			}
		}

		return false;
	}

	public void Play () {
		m_currentTime = m_time;
		m_isStop = false;
	}

	public void SetTimer (float newTime, bool isStart = true) {
		m_time = newTime;
		m_currentTime = m_time;
		m_isStop = !isStart;
	}

	public void Stop () {
		m_isStop = true;
	}

	public void Restart () {
		m_currentTime = m_time;
		m_isStop = false;
	}

	public static float GetRandomTime (float minTime, float maxTime) {
		return Random.Range (minTime, maxTime);
	}

	#endregion

	#region Private Methods

	void DebugCurrentTime () {
		Debug.Log ("current time: " + m_currentTime);
	}

	#endregion
}