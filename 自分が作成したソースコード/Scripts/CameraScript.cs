using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CameraScript : MonoBehaviour
{
	private Vector3 LimitPos;
	private UnityEvent gameStartEvent = null;
	private UnityEvent countStartEvent = null;

	[SerializeField]
	[Tooltip("カメラのスクロール速度")]
	float scrollSpeed;

	void Start()
	{

	}

	void Update()
	{
		if(transform.position.z > LimitPos.z)
		{
			var pos = transform.position;
			transform.position = new Vector3(pos.x,pos.y,LimitPos.z);
		}
	}

	public void Follow(Vector3 pos, Vector3 offset)
	{
		var camPos = this.transform.position;
		if(transform.position.z > LimitPos.z)
		{
			transform.position = new Vector3(camPos.x,camPos.y,LimitPos.z);
		}
		else
		{
			transform.position = new Vector3(camPos.x, camPos.y, pos.z + offset.z);
		}

	}

	public void SetLimitPos(Vector3 pos)
	{
		LimitPos = pos;
	}

	public void SetCameraStartPosition(Vector3 pos)
	{
		var camPos = this.transform.position;
		pos.y = camPos.y;
		transform.position = pos;
	}

	public IEnumerator ScrollStart(Vector3 pos)
	{
		var camPos = this.transform.position;
		while(camPos.z > pos.z)
		{
			camPos.z -= scrollSpeed * Time.deltaTime;
			transform.position = camPos;
			yield return null;
		}

		if(gameStartEvent != null)
		{
			gameStartEvent.Invoke();
		}
	}

	public void AddListenerToGameStartEvent(UnityAction action)
	{
		gameStartEvent = new UnityEvent();
		gameStartEvent.AddListener(action);
	}
	public void RemoveListenerFromGameStartEvent(UnityAction action)
	{
		if (gameStartEvent != null)
		{
			gameStartEvent.RemoveListener(action);
		}
	}

	public void AddListenerToCountDownStartEvent(UnityAction action)
	{
		countStartEvent = new UnityEvent();
		countStartEvent.AddListener(action);
	}
	public void RemoveListenerFromCountDownStartEvent(UnityAction action)
	{
		if (countStartEvent != null)
		{
			countStartEvent.RemoveListener(action);
		}
	}
}
