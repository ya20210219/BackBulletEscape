using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyScript : MonoBehaviour
{
	[SerializeField]
	[Tooltip("エネミーのアニメーション")]
	private Animator enemyAnimation = null;

	[SerializeField]
	[Tooltip("敵の移動速度")]
	float enemySpeed;

	[SerializeField]
	[Tooltip("怯み時間")]
	float flirtingTime;

	[SerializeField]
	[Tooltip("立ち直り加速度")]
	float accelerationPower;

	[SerializeField]
	[Tooltip("立ち直り加速する時間")]
	float accelerationTime;

	private bool isClearDirecting;

	[SerializeField] EnemyWeakPointScript enemyWeakPointScript;

	private UnityEvent gameOverEvent = null;

	private UnityEvent effectEvent = null;

	private UnityEvent bulletEvent = null;

	private bool isFlirting;
	Camera cam;
	Rect windowRect = new Rect(0, 0, 1, 1); // 画面内か判定するためのRect
	Rigidbody rigidBody;

	void Start()
	{
		rigidBody =  GetComponent<Rigidbody>();
		enemyWeakPointScript.AddListenerToHitEvent(HitWeekPoint);
		cam = Camera.main;
	}
	public void EnemyUpdate ()
	{
		if(isClearDirecting == false)
		{
			CheckCamera();
			if(isFlirting == false && CheckCamera() == false)
			{
				NormalOperate();
			}
			if(isFlirting == false && CheckCamera() == true)
			{
				AccelerationOperate();
			}
		}
	}

	void NormalOperate()
	{
		rigidBody.velocity = transform.forward * enemySpeed;
	}

	void AccelerationOperate()
	{
		rigidBody.velocity = transform.forward * (enemySpeed + accelerationPower);
	}

	public void AddListenerToBulletEvent(UnityAction action)
	{
		bulletEvent = new UnityEvent();
		bulletEvent.AddListener(action);
	}

	public void AddListenerToGameOverEvent(UnityAction action)
	{
		gameOverEvent = new UnityEvent();
		gameOverEvent.AddListener(action);
	}
	public void RemoveListenerFromGameOverEvent(UnityAction action)
	{
		if (gameOverEvent != null)
		{
			gameOverEvent.RemoveListener(action);
		}
	}

	public void AddListenerToEnemyEffectEvent(UnityAction action)
	{
		Debug.Log("エネミーイベント生成");
		effectEvent = new UnityEvent();
		effectEvent.AddListener(action);
	}
	public void RemoveListenerFromEnemyEffectEvent(UnityAction action)
	{
		if (effectEvent != null)
		{
			effectEvent.RemoveListener(action);
		}
	}
	IEnumerator FlirtingOperate()
	{
		rigidBody.velocity *= 0;
		//指定秒停止
		yield return new WaitForSeconds(flirtingTime);
		isFlirting = false;
		enemyAnimation.SetBool("Down",false);
	}

	bool CheckCamera()
	{
		var viewportPos = cam.WorldToViewportPoint(transform.position);

		return !windowRect.Contains(viewportPos);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Bullet"))
		{
			Debug.Log("痛い");
			bulletEvent.Invoke();
			Destroy(other.gameObject);
		}

		if(other.CompareTag("Player"))
		{
			Debug.Log("敵と当たった");
			if(gameOverEvent != null)
			{
				Debug.Log("EnemyEventActive");
				gameOverEvent.Invoke();
			}
			if(effectEvent != null)
			{
				Debug.Log("EnemyEffectActive");
				effectEvent.Invoke();
			}
		}
	}

	private void HitWeekPoint()
	{
		SoundManager.Instance.PlaySE("EnemyDamage");
		enemyAnimation.SetBool("Down",true);
		Debug.Log("HitEvent");
		isFlirting = true;
		StartCoroutine(FlirtingOperate());
	}

	public void EnableClearDirecting()
	{
		rigidBody.velocity *= 0;
		isClearDirecting = true;
	}

	public Vector3 GetWeekPos()
	{
		return enemyWeakPointScript.transform.position;
	}
}