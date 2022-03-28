using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public interface IEventDirection
{
	IEnumerator ClearDirecting();
	void OnTriggerEnter(Collider other);
}

public class ClearBaseScript : MonoBehaviour
{
	protected UnityEvent clearEvent;
	protected bool isDirecting;
	protected bool isDirectingEnd;

	[SerializeField]
	[Tooltip("ゴール演出時に行く中央のZ座標")]
	protected float directingCenterPosZ;

	protected virtual void Start()
	{

	}

	void Awake()
	{
		clearEvent = null;
	}

	void Update()
	{

	}

	public void AddListenerToGameClearEvent(UnityAction action)
	{
		clearEvent = new UnityEvent();
		clearEvent.AddListener(action);
	}
	public void RemoveListenerFromClearEvent(UnityAction action)
	{
		if (clearEvent != null)
		{
			clearEvent.RemoveListener(action);
		}
	}

	public virtual bool GetDirectingEnd()
	{
		return isDirectingEnd;
	}

	public float GetDirectingCurvePosZ()
	{
		return directingCenterPosZ;
	}

	void OnDrawGizmos()
	{
		var pos = transform.position;
		pos.z += directingCenterPosZ;
		Gizmos.color = new Color(1f, 0, 0, 0.5f);
		Gizmos.DrawSphere (pos, 0.5f);
	}
}
