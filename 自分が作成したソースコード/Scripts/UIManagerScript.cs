using UnityEngine;
using UnityEngine.Events;

public interface ISetImageActive
{
	void SetImageActive(bool isEnable);
}

public class UIManagerScript : MonoBehaviour
{
	[SerializeField]
	CountDownScript countDownScript;

	[SerializeField]
	MiniMapManagerScript miniMapScript;

	[SerializeField]
	PlayerControllerUIScript playerControllerUIScript;

	[SerializeField]
	InGameUIScript inGameUIScript;

	[SerializeField]
	EnemyWarningScript enemyWarningScript;

	private int nowSceneIndexNum;

	public void InitUI(UnityAction action, Vector3 enemyPos, Vector3 clearPos, SceneController.SceneType type)
	{
		countDownScript.AddListenerToGameCountDownEvent(action);
		countDownScript.SetImageActive(true);
		miniMapScript.SetMiniMapStatus(enemyPos, clearPos);
		miniMapScript.SetImageActive(true);
		playerControllerUIScript.SetImageActive(false);
		inGameUIScript.SetStageNumImage(type);
		enemyWarningScript.InitWarning();
	}

	public void UpdateUI(Vector3 pPos, Vector3 ePos, Vector3 forward, float deg, bool isCoolDownFlg)
	{
		miniMapScript.UpdateMiniMap(pPos, ePos);
		playerControllerUIScript.PlayerControllerUpdate(pPos, forward, deg, isCoolDownFlg);
		enemyWarningScript.WarningUpdate(pPos, ePos);
	}

	public void StartCountDown()
	{
		countDownScript.CountDownInit();
	}

	public void SetAllImageActive(bool isEnable)
	{
		countDownScript.SetImageActive(isEnable);
		miniMapScript.SetImageActive(isEnable);
		playerControllerUIScript.SetImageActive(isEnable);
		inGameUIScript.SetImageActive(isEnable);
	}

	public void SetStartImageActive(bool isEnable)
	{
		countDownScript.SetImageActive(isEnable);
		miniMapScript.SetImageActive(isEnable);
		playerControllerUIScript.SetImageActive(isEnable);
	}

	public void AddCountDownEvent(UnityAction addEvent)
	{
		countDownScript.AddListenerToGameCountDownEvent(addEvent);
	}

	public void SetWarning()
	{
		enemyWarningScript.SetImageActive(true);
	}
}
