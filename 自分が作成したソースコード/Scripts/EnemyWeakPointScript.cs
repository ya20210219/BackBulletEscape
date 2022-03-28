using UnityEngine;
using UnityEngine.Events;

public class EnemyWeakPointScript : MonoBehaviour
{
	[SerializeField] UnityEvent hitEvent = null;
	void Start()
	{

	}

	void Update ()
	{

	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Bullet"))
		{
			if (hitEvent != null)
			{
				Debug.Log("HitEventActive");
				hitEvent.Invoke();
			}
		}
	}

	public void AddListenerToHitEvent(UnityAction action)
	{
		hitEvent = new UnityEvent();
		hitEvent.AddListener(action);
	}
}