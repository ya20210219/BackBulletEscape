using System.Collections;
using UnityEngine;

public class SmallClearScript : ClearBaseScript, IEventDirection
{
	override protected void Start()
	{
		base.Start();
	}

	void Update()
	{

	}

	public IEnumerator ClearDirecting()
	{
		isDirecting = true;
		if (clearEvent != null)
		{
			clearEvent.Invoke();
		}

		yield return new WaitForSeconds(3);	//仮で3秒。データが入り次第数値を変える
		isDirecting = false;
		isDirectingEnd = true;

		Debug.Log("ClearFlagActive");

		Debug.Log("EventActive");
		if (clearEvent != null)
		{
			clearEvent.Invoke();
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			if(isDirecting == false && isDirectingEnd == false)
			{
				Debug.Log("ゴールと当たった");
				StartCoroutine(ClearDirecting());
			}
		}
	}
}
