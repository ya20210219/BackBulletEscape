using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class TapBlinkingScript : MonoBehaviour
{
	[SerializeField]
	Image tapImage;

	private float time;

	[SerializeField,Range(0.0f, 10.0f)]
	[Tooltip("点滅速度")]
	float speed;

	void Start()
	{
		StartCoroutine(TapBlinking(tapImage.color));
	}

	IEnumerator TapBlinking(Color color)
	{
		while(true)
		{
			time += Time.deltaTime * speed;
			color.a = Mathf.Sin(time) * 0.5f + 0.5f;
			tapImage.color = color;
			yield return null;
		}
	}
}
