using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {

	#region Editor Variables

	[SerializeField] AudioClip m_eatFruit;
	[SerializeField] AudioClip m_gameOver;
	[SerializeField] AudioClip m_winGame;

	[SerializeField] AudioSource m_sfx;
	[SerializeField] AudioSource m_music;

	[SerializeField] Toggle m_musicToggle;
	[SerializeField] Toggle m_sfxToggle;

	#endregion


	#region Public Properties

	#endregion


	#region Private Variables

	#endregion


	#region Behaviour Overrides

	void Start () {
		m_sfxToggle.isOn = PlayerPrefs.GetInt (StaticManager.SFX_MUTE_SETTING) == 1 ? true : false;
		m_musicToggle.isOn = PlayerPrefs.GetInt (StaticManager.MUSIC_MUTE_SETTING) == 1 ? true : false;
	}

	void OnEnable () {
		SnakeController.EatFruit += PlayEatFruitSound;
		SnakeController.Crash += GameOver;
		SnakeController.Win += WinGame;
	}

	void OnDisable () {
		SnakeController.EatFruit -= PlayEatFruitSound;
		SnakeController.Crash -= GameOver;
		SnakeController.Win -= WinGame;
	}

	#endregion


	#region Public Methods

	public void SaveSFXSettings () {
		PlayerPrefs.SetInt (StaticManager.SFX_MUTE_SETTING, m_sfxToggle.isOn ? 1 : 0);
	}

	public void SaveMusicSettings () {
		PlayerPrefs.SetInt (StaticManager.MUSIC_MUTE_SETTING, m_musicToggle.isOn ? 1 : 0);
	}

	#endregion


	#region Private Methods

	void PlayEatFruitSound () {
		m_sfx.clip = m_eatFruit;
		m_sfx.Play ();
	}

	void FinishGame (AudioClip clipToPlay) {
		m_music.mute = true;
		m_sfx.clip = clipToPlay;
		m_sfx.Play ();
	}

	void GameOver () {
		FinishGame (m_gameOver);
	}

	void WinGame () {
		FinishGame (m_winGame);
	}

	#endregion
}
