using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class PlayerScript : MonoBehaviour
{
	[SerializeField]
	[Tooltip("プレイヤーのアニメーション")]
	private Animator playerAnimation = null;
	[SerializeField]
	[Tooltip("発射されるときの速度")]
	float shotSpeed;

	[SerializeField]
	[Tooltip("減速させる割合")]
	float slowdownSpeed;

	[SerializeField]
	[Tooltip("弾を生成する際の弾とプレイヤーの距離")]
	Vector3 bulletOffset;

	[SerializeField,Range(0f, 1f)]
	[Tooltip("プレイヤーの振り向き割合")]
	float lookValue = 0.1f;

	[SerializeField]
	[Tooltip("ゴール演出中の速度")]
	float DirectingSpeed;
	private float playerSpeed;
	Rigidbody rb;
	public GameObject bullet;
	private Vector2 startMouse;
	private bool	isDirecting;

	private bool	isStop;
	[SerializeField,Range(0f, 1f)]
	[Tooltip("プレイヤーからのマズルフラッシュの距離")]
	float muzzleFlashDist;
	[SerializeField]
	[Tooltip("クールダウン時間")]
	float coolDownCount;

	[SerializeField,Range(0f, 10f)]
	[Tooltip("スタン時間")]
	float stanCount;

	private Vector3 directingCurvePos;
	[SerializeField]
	float curveRate = 0.0035f;
	private UnityEvent playerEffectEvent = null;

	private UnityEvent muzzleFlashEffectEvent = null;

	private Coroutine actionCoroutine = null;

	void Start()
	{
		rb =  GetComponent<Rigidbody>();
		actionCoroutine = null;
	}
	public void PlayerUpdate ()
	{
		if(Time.timeScale == 0)
		{
			return;
		}
		//プレイヤー操作に関する関数
		if(isDirecting == false)
		{
			CheckRotateBasePos();
			if(isStop == false)
			{
				Operate();
			}
		}
		if(isDirecting == true)
		{
			DirectingOperate();
		}
		//減速
		rb.velocity *= slowdownSpeed;
	}

	void Operate()
	{
		if(Input.GetMouseButton(0)) //左クリックされている間
		{
			Vector3 angle = transform.localEulerAngles;
			Vector2 mPos = Input.mousePosition;
			Vector2 nowMouseToPlayer = mPos - startMouse;
			nowMouseToPlayer *= -1;
			nowMouseToPlayer = nowMouseToPlayer.normalized;

			float targetAngle = Vector2.SignedAngle(nowMouseToPlayer, Vector2.down);
			float rotateAngle = Mathf.LerpAngle(angle.y, targetAngle, lookValue);
			transform.localEulerAngles = new Vector3(angle.x, rotateAngle, angle.z);
		}
		if(Input.GetMouseButtonUp(0)) //左クリックが離されたら
		{
			playerAnimation.SetBool("Shot",true);
			playerSpeed = shotSpeed;
			//速度(発射)
			rb.velocity = transform.forward * playerSpeed;

			SoundManager.Instance.PlaySE("BulletShot");
			muzzleFlashEffectEvent.Invoke();
			CreateBullet();
			CheckStopOperate(coolDownCount);
		}
	}

	void DirectingOperate()
	{
		Transform myTransform = this.transform;
		var myVec = myTransform.forward;
		var myAngle = myTransform.localEulerAngles;

		float yAngle = Mathf.LerpAngle(myAngle.y, 0.0f, curveRate);
		myAngle = new Vector3(myAngle.x, yAngle, myAngle.z);

		transform.localEulerAngles = myAngle;
		rb.velocity = myVec * DirectingSpeed;
	}

	void CreateBullet()
	{
		var playerBase = this.transform;

		Vector3 pos = playerBase.position;

		//位置補正
		Vector3 offsetGun = bulletOffset;

		Quaternion dir = playerBase.rotation;

		Quaternion Invert = Quaternion.AngleAxis(180, new Vector3(1,0,0));
		Quaternion rot = dir * Invert;

		pos = dir * offsetGun + pos;
		//弾生成
		Instantiate (bullet, pos, rot);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("GameClearObj"))
		{
			isDirecting = true;
			transform.LookAt(directingCurvePos);
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if(TagUtility.GetParentTagName(other.gameObject) == "Block")
		{
			playerEffectEvent.Invoke();
		}

		if(other.gameObject.CompareTag("Block/KnockBack/Explosion"))
		{
			CheckStopOperate(stanCount);
		}
	}

	public void SetDirectingCurvePos(Vector3 pos)
	{
		directingCurvePos = pos;
	}

	IEnumerator StopOperate(float stopFrame)
	{
		isStop = true;
		yield return new WaitForSeconds(stopFrame);	
		isStop = false;
		playerAnimation.SetBool("Shot",false);
		actionCoroutine = null;
	}

	public void AddListenerToPlayerEffectEvent(UnityAction action)
	{
		Debug.Log("プレイヤーイベントの作成");
		playerEffectEvent = new UnityEvent();
		Debug.Log(playerEffectEvent);
		playerEffectEvent.AddListener(action);
	}

	public void AddListenerToMuzzleFlashEffectEvent(UnityAction action)
	{
		Debug.Log("プレイヤーイベントの作成");
		muzzleFlashEffectEvent = new UnityEvent();
		Debug.Log(muzzleFlashEffectEvent);
		muzzleFlashEffectEvent.AddListener(action);
	}

	private void CheckRotateBasePos()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Vector2 mPos = Input.mousePosition;
			startMouse = mPos;
		}
	}

	public bool GetStop()
	{
		return isStop;
	}

	public float GetLocalAngleY()
	{
		return transform.localEulerAngles.y;
	}

	public Vector3 GetMuzzleFlashPos()
	{
		Transform t = this.gameObject.transform;
		Vector3 pos = t.position;
		Vector3 forward = t.forward;
		return pos + (-forward) * muzzleFlashDist;
	}

	void CheckStopOperate(float time)
	{
		if (actionCoroutine != null)
		{
			StopCoroutine(actionCoroutine);
			actionCoroutine = null;
			Debug.Log("コルーチン破棄");
		}
		actionCoroutine = StartCoroutine(StopOperate(time));
	}
}