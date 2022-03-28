using UnityEngine;
using UnityEngine.Events;

public class CoinScript : MonoBehaviour,IAddListener
{
	private UnityEvent coinEffectEvent = null;

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			SoundManager.Instance.PlaySE("GetCoin");
			GetPos();
			coinEffectEvent.Invoke();
			Destroy(this.gameObject);
		}
	}

	public void AddListenerToEffectEvent(UnityAction action)
	{
		Debug.Log("コインイベント生成");
		coinEffectEvent = new UnityEvent();
		Debug.Log(coinEffectEvent);
		coinEffectEvent.AddListener(action);
	}
	public Vector3 GetPos()
	{
		return this.gameObject.transform.position;
	}
}
