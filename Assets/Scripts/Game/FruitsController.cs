using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitsController : MonoBehaviour {

	#region Editor Variables

	[SerializeField] GameObject m_fruit;

	#endregion


	#region Public Properties

	#endregion


	#region Private Variables

	List<Vector2> m_avaliableFruitPositions;

	#endregion


	#region Behaviour Overrides

	void Start () {
		NewFruit ();
	}

	void OnEnable () {
		SnakeController.EatFruit += NewFruit;
	}

	void OnDisable () {
		SnakeController.EatFruit -= NewFruit;
	}

	#endregion


	#region Public Methods

	#endregion


	#region Private Methods

	void NewFruit () {
		CreateListOfAvaliableFruitPositions ();

		int randomIndex = Random.Range (0, m_avaliableFruitPositions.Count);
		m_fruit.transform.position = m_avaliableFruitPositions [randomIndex];
	}

	void CreateListOfAvaliableFruitPositions () {
		int sizeX = GameController.Instance.FieldSizeX;
		int sizeY = GameController.Instance.FieldSizeY;

		int minX = -sizeX / 2;
		int minY = -sizeY / 2;

		m_avaliableFruitPositions = new List<Vector2> ();

		for (int i = 0; i < sizeX; i++) {
			for (int j = 0; j < sizeY; j++) {
				Vector2 candidatePos = new Vector2 (minX + i, minY + j);
				if (IsPositionAvaliableForFruit (candidatePos)) {
					m_avaliableFruitPositions.Add (candidatePos);
				}
			}
		}
	}

	bool IsPositionAvaliableForFruit (Vector2 candidatePos) {
		List<Transform> snakeBody = GameController.Instance.Snake.SnakeBody;

		for (int k = 0; k < snakeBody.Count; k++) {
			bool snakeIsHere = (candidatePos == (Vector2)snakeBody [k].position);
			if (snakeIsHere) {
				return false;
			}
		}

		return true;
	}


	#endregion
}
