using UnityEngine;
using UnityEngine.UI;

public class EnemyWarningScript : MonoBehaviour, ISetImageActive
{
	[SerializeField]
	Image warningImage;

	[SerializeField, Range(0.0f, 50.0f)]
	[Tooltip("警告が出るプレイヤーと敵の最低距離")]
	float warningDist;

	[SerializeField, Range(0.0f, 1.0f)]
	[Tooltip("警告色の濃度")]
	float warningColorValue;

	[SerializeField, Range(0.0f, 10.0f)]
	[Tooltip("点滅速度")]
	float blinkingSpeed;

	private float blinkingValue;

	public void InitWarning()
	{
		warningImage.color = Color.clear;
		SetImageActive(false);
	}

	public void SetImageActive(bool isEnable)
	{
		warningImage.gameObject.SetActive(isEnable);
	}

	public void WarningUpdate(Vector3 pPos, Vector3 ePos)
	{
		float dist = pPos.z - ePos.z;
		if(warningImage.gameObject.activeSelf == false && dist < warningDist)
		{
			SoundManager.Instance.PlaySE("Siren");
			SetImageActive(true);
		}

		if(warningImage.gameObject.activeSelf == true && dist > warningDist)
		{
			SetImageActive(false);
		}

		if(warningImage.gameObject.activeSelf == true)
		{
			WarningEvent();
		}
	}

	void WarningEvent()
	{
		blinkingValue += blinkingSpeed * Time.deltaTime - Mathf.Floor(blinkingValue);

		warningImage.color = new Color(warningColorValue, 0.0f, 0.0f, blinkingValue * warningColorValue);
	}
}
