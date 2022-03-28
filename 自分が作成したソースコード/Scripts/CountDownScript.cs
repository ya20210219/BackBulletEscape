using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class CountDownScript : MonoBehaviour, ISetImageActive
{
	[SerializeField]
	Image   countDownTexture;
	[SerializeField]
	Image   startTexture;

	[SerializeField,Range(0f, 2f)]
	[Tooltip("カウントダウンの速度")]
	float   countTime;

	[SerializeField,Range(0f, 10f)]
	[Tooltip("ゲームスタートの速度")]
	float   gameStartCountTime;

	[SerializeField]
	Sprite[] countDownSprite;

	private const int COUNT_DOWN_NUM = 3;
	private UnityEvent countDownEvent = null;

	public void CountDownInit()
	{
		countDownTexture.transform.localScale = Vector3.zero;
		startTexture.transform.localScale = Vector3.zero;
		StartCoroutine(CountDown());
	}

	IEnumerator CountDown()
	{
		Vector3 countDownSize = countDownTexture.transform.localScale;

		Vector3 startSize = startTexture.transform.localScale;

		for (var i = COUNT_DOWN_NUM; i > 0; i--)
		{
			yield return CountDownScaleUp(i, countDownSize);
		}

		yield return null;
		countDownTexture.transform.localScale = Vector3.zero;
		yield return GameStartScaleUp(startSize);
	}

	IEnumerator CountDownScaleUp(int countNum, Vector3 size)
	{
		countDownTexture.sprite = countDownSprite[countNum];
		countDownTexture.transform.localScale = Vector3.zero;

		while(size.x < 1.0f)
		{
			size = new Vector3(size.x + countTime * Time.deltaTime, size.y + countTime * Time.deltaTime, size.z + countTime * Time.deltaTime);
			countDownTexture.transform.localScale = size;
			yield return null;
		}
	}

	IEnumerator GameStartScaleUp(Vector3 size)
	{
		Debug.Log("ゲームスタートズーム");
		while(size.z < 1.0f)
		{
			size = new Vector3(size.x + (countTime * gameStartCountTime * Time.deltaTime),
								size.y + (countTime * gameStartCountTime * Time.deltaTime),
								size.z + (countTime * gameStartCountTime * Time.deltaTime));
			startTexture.transform.localScale = size;
			yield return null;
		}
		GameStartEvent();
	}

	void GameStartEvent()
	{
		if(countDownEvent != null)
		{
			countDownEvent.Invoke();
		}
		SetImageActive(false);
	}

	public void AddListenerToGameCountDownEvent(UnityAction action)
	{
		countDownEvent = new UnityEvent();
		countDownEvent.AddListener(action);
	}

	public void SetImageActive(bool isEnable)
	{
		countDownTexture.gameObject.SetActive(isEnable);
		startTexture.gameObject.SetActive(isEnable);
	}
}
