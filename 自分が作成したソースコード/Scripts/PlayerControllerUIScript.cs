using UnityEngine.UI;
using UnityEngine;

public class PlayerControllerUIScript : MonoBehaviour, ISetImageActive
{
	[SerializeField]
	Image	controllerBaseImage;
	[SerializeField]
	Image	controllerFrontImage;
	[SerializeField]
	Image	playerArrowImage;

	[SerializeField,Range(0f, 500f)]
	[Tooltip("プレイヤーと矢印の距離")]
	float playerArrowDist;

	private const float MAX_DIST = 150.0f;

	public void PlayerControllerUpdate(Vector3 pos, Vector3 forward, float deg, bool isCoolDownFlg)
	{
		if(Input.GetMouseButtonDown(0)) //左クリックしたら
		{
			controllerBaseImage.transform.position = Input.mousePosition;
			controllerFrontImage.transform.position = Input.mousePosition;
		}
		PlayerControllerUIUpdate(isCoolDownFlg);
		PlayerArrowUIUpdate(pos, forward, deg);
	}
	void PlayerControllerUIUpdate(bool isCoolDownFlg)
	{
		if(Input.GetMouseButton(0)) //左クリックされている間
		{
			SetImageActive(!isCoolDownFlg);
			Vector2 mPos = Input.mousePosition;
			Vector2 bPos = controllerBaseImage.transform.position;

			Vector2 vec = mPos - bPos;
			controllerFrontImage.transform.position = bPos + vec;
			if(Vector2.Distance(bPos, mPos) > MAX_DIST)
			{
				vec = vec.normalized;
				controllerFrontImage.transform.position = bPos + vec * MAX_DIST;
			}
		}
		if(Input.GetMouseButtonUp(0)) //左クリックが離されたら
		{
			SetImageActive(!isCoolDownFlg);
		}
	}

	void PlayerArrowUIUpdate(Vector3 pos, Vector3 forward, float deg)
	{
		if(Input.GetMouseButton(0)) //左クリックされている間
		{
			Transform arrow = playerArrowImage.transform;

			Vector2 pPos = Camera.main.WorldToScreenPoint(pos);
			Vector2 pForward = new Vector2(forward.x, forward.z);

			Vector3 arrowAngle = arrow.localEulerAngles;

			arrow.position = pPos + pForward * playerArrowDist;
			arrow.localEulerAngles = new Vector3(arrowAngle.x, arrowAngle.y, -deg);
		}
	}

	public void SetImageActive(bool isEnable)
	{
		controllerBaseImage.gameObject.SetActive(isEnable);
		controllerFrontImage.gameObject.SetActive(isEnable);
		playerArrowImage.gameObject.SetActive(isEnable);
	}
}