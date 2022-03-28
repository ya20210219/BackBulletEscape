using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleStarScript : MonoBehaviour
{
	[SerializeField]
	RawImage tile;

	float x, y;

	[SerializeField,Range(-0.05f, 0.05f)]	
	float speedX, speedY;

	void Start()
	{
		StartCoroutine(StartStarDirecting());
	}
	
	IEnumerator StartStarDirecting()
	{
		while(true)
		{
			x += speedX * Time.deltaTime;
			y += speedY * Time.deltaTime;
			tile.uvRect = new Rect(x, y, tile.uvRect.width, tile.uvRect.height);
			yield return null;
		}
	}
}
