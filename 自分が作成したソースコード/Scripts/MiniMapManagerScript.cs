using UnityEngine;
using UnityEngine.UI;

public class MiniMapManagerScript : MonoBehaviour, ISetImageActive
{
	[SerializeField]
	Image   minimapPlayer;
	[SerializeField]
	Image   minimapGoal;
	[SerializeField]
	Image   minimapEnemy;
	[SerializeField]
	Image   minimapPlayerPassed;
	[SerializeField]
	Image   minimapEnemyPassed;
	[SerializeField]
	Image   minimapBackGround;

	Vector3 minimapStartPos;
	Vector3 inGameEnemyStartPos;

	float enemyToGoalDist;
	float minimapEnemyToGoalDist;

	public void SetMiniMapStatus(Vector3 ePos, Vector3 gPos)
	{
		Vector3 enemyImagePos = minimapEnemy.rectTransform.position;
		Vector3 goalImagePos = minimapGoal.rectTransform.position;

		inGameEnemyStartPos = ePos;
		minimapStartPos = enemyImagePos;
		minimapEnemyToGoalDist = goalImagePos.y - enemyImagePos.y;
		enemyToGoalDist = gPos.z - ePos.z;
	}

	public void UpdateMiniMap(Vector3 pPos, Vector3 ePos)
	{
		Vector3 enemyImagePos = minimapEnemy.rectTransform.position;
		Vector3 playerImagePos = minimapPlayer.rectTransform.position;

		float nowInGameEnemyToStartDist =  ePos.z - inGameEnemyStartPos.z;
		float nowInGameEnemyToGoalScale = nowInGameEnemyToStartDist / enemyToGoalDist;
		float nowInGamePlayerToStartDist = pPos.z - inGameEnemyStartPos.z;
		float nowInGamePlayerToGoalScale = nowInGamePlayerToStartDist / enemyToGoalDist;

		enemyImagePos.y = minimapStartPos.y + minimapEnemyToGoalDist * nowInGameEnemyToGoalScale;
		playerImagePos.y = minimapStartPos.y + minimapEnemyToGoalDist * nowInGamePlayerToGoalScale;

		minimapEnemy.rectTransform.position = enemyImagePos;
		minimapPlayer.rectTransform.position = playerImagePos;

		minimapPlayerPassed.fillAmount = nowInGamePlayerToGoalScale;
		minimapEnemyPassed.fillAmount = nowInGameEnemyToGoalScale;
	}

	public void SetImageActive(bool isEnable)
	{
		minimapPlayer.gameObject.SetActive(isEnable);
		minimapGoal.gameObject.SetActive(isEnable);
		minimapEnemy.gameObject.SetActive(isEnable);
		minimapPlayerPassed.gameObject.SetActive(isEnable);
		minimapEnemyPassed.gameObject.SetActive(isEnable);
		minimapBackGround.gameObject.SetActive(isEnable);
	}
}