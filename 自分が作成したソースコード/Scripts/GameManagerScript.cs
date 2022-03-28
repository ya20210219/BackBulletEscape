using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManagerScript : MonoBehaviour
{
	private GameObject playerObj;   //プレイヤー情報格納用
	PlayerScript playerScript;

	private GameObject camObj;      //カメラ情報格納用
	CameraScript camScript;

	private GameObject clearBaseObj;
	ClearBaseScript clearBaseScript;

	private GameObject smallClearObj;
	SmallClearScript smallClearScript;

	private GameObject enemyObj;
	EnemyScript enemyScript;

	private GameObject effectObj;
	EffectManager effectScript;

	private GameObject[] blockObj;
	List<KnockBackBlock> knockBackBlockScript = new List<KnockBackBlock>();

	private GameObject[] breakBlockObj;
	List<BreakBlock> breakBlockScript = new List<BreakBlock>();

	private GameObject[] explosionBlockObj;
	List<ExplosionBlock> explosionBlockScript = new List<ExplosionBlock>();

	private GameObject[] jellyBlockObj;
	List<JellyBlock> jellyBlockScript = new List<JellyBlock>();

	private GameObject[] coinObj;
	List<CoinScript> coinScript = new List<CoinScript>();

	Canvas countDownCanvas;
	CountDownScript countDownScript;

	[SerializeField]
	UIManagerScript uiManagerScript;

	private Vector3 offset;

	private bool	isGameClear;
	private bool	isGameOver;

	private bool	isClearDirecting;
	private bool    isNowGaming;
	private bool	isCountDown;
	GameTimer gameTimer = new GameTimer();

	void Awake()
	{
		Application.targetFrameRate = 60;
	}

	void Start()
	{
		playerObj = GameObject.FindGameObjectWithTag("Player");
		playerScript = playerObj.GetComponent<PlayerScript>();

		camObj = GameObject.FindGameObjectWithTag("MainCamera");
		camScript = camObj.GetComponent<CameraScript>();

		clearBaseObj = GameObject.FindGameObjectWithTag("GameClearObj");
		clearBaseScript = clearBaseObj.GetComponent<ClearBaseScript>();

		enemyObj = GameObject.FindGameObjectWithTag("Enemy");
		enemyScript = enemyObj.GetComponent<EnemyScript>();

		effectObj = GameObject.FindGameObjectWithTag("Effect");
		effectScript = effectObj.GetComponent<EffectManager>();

		blockObj = GameObject.FindGameObjectsWithTag("Block/KnockBack");
		Debug.Log(blockObj.Length);
		AddScript<KnockBackBlock>(blockObj,knockBackBlockScript);

		explosionBlockObj = GameObject.FindGameObjectsWithTag("Block/KnockBack/Explosion");
		Debug.Log(explosionBlockObj.Length);
		AddScript<ExplosionBlock>(explosionBlockObj,explosionBlockScript);

		breakBlockObj = GameObject.FindGameObjectsWithTag("Block/KnockBack/Break");
		Debug.Log(breakBlockObj.Length);
		AddScript<BreakBlock>(breakBlockObj,breakBlockScript);

		jellyBlockObj = GameObject.FindGameObjectsWithTag("Block/NotKnockBack/Jelly");
		Debug.Log(jellyBlockObj.Length);
		AddScript<JellyBlock>(jellyBlockObj,jellyBlockScript);

		coinObj = GameObject.FindGameObjectsWithTag("Block/NotKnockBack/Coin");
		Debug.Log(coinObj.Length);
		AddScript<CoinScript>(coinObj,coinScript);

		countDownCanvas = GameObject.FindGameObjectWithTag("CountDown").GetComponent<Canvas>();
		countDownScript = countDownCanvas.GetComponent<CountDownScript>();

		Init();

		isNowGaming = false;
	}

	private void Init()
	{
		var clearPos = clearBaseObj.transform.position;
		var playerPos = playerObj.transform.position;
		var enemyPos = enemyObj.transform.position;
		var pos = clearBaseObj.transform.position;

		Debug.Log("ゴール演出イベント追加");
		clearBaseScript.AddListenerToGameClearEvent(ClearDirecting);
		camScript.SetCameraStartPosition(clearPos);
		Debug.Log("ゲームスタートイベント追加");
		camScript.AddListenerToGameStartEvent(StartDirecting);

		Debug.Log("敵イベント追加");
		enemyScript.AddListenerToGameOverEvent(GameOver);

		Debug.Log("エネミーエフェクトイベント追加");
		enemyScript.AddListenerToEnemyEffectEvent(() =>
		{
			PlayEffect(playerPos,EffectManager.EffectType.Enemy, 0.0f);
			enemyScript.RemoveListenerFromEnemyEffectEvent(() =>
			{
				PlayEffect(playerPos,EffectManager.EffectType.Enemy, 0.0f);
			});
		});

		camScript.SetLimitPos(clearPos);
		playerScript.SetDirectingCurvePos(new Vector3(clearPos.x,clearPos.y,clearPos.z + clearBaseScript.GetDirectingCurvePosZ()));
		uiManagerScript.InitUI(GameStartDirecting,enemyPos,clearPos, (SceneController.SceneType)SceneManager.GetActiveScene().buildIndex);

		Debug.Log("バレットエフェクトイベント追加");
		enemyScript.AddListenerToBulletEvent(() => {PlayEffect(enemyScript.GetWeekPos(),EffectManager.EffectType.Bullet, 0.0f);});

		camScript.SetLimitPos(pos);
		playerScript.SetDirectingCurvePos(new Vector3(pos.x,pos.y,pos.z + clearBaseScript.GetDirectingCurvePosZ()));

		Debug.Log("プレイヤーエフェクトイベント追加");
		playerScript.AddListenerToPlayerEffectEvent(() => {PlayEffect(playerObj.transform.position,EffectManager.EffectType.Block, 0.0f);});

		Debug.Log("マズルフラッシュイベント追加");
		playerScript.AddListenerToMuzzleFlashEffectEvent(() => {PlayEffect(playerScript.GetMuzzleFlashPos(),EffectManager.EffectType.MuzzleFlashEffect, playerScript.GetLocalAngleY());});

		AddEffect(playerPos, EffectManager.EffectType.KnockBack, knockBackBlockScript);

		AddEffect(playerPos, EffectManager.EffectType.ExplosionBlock, explosionBlockScript);

		AddEffect(playerPos, EffectManager.EffectType.BreakBlock, breakBlockScript);

		AddEffect(playerPos, EffectManager.EffectType.JellyEffect, jellyBlockScript);

		AddEffect(playerPos, EffectManager.EffectType.CoinEffect, coinScript);

		Debug.Log("GameManagerScriptInit");

		StartCoroutine(GameStartDirectionCoroutine());
	}

	void Update()
	{
		Transform player = playerObj.transform;
		var playerPos = player.position;
		var enemyPos = enemyObj.transform.position;
		var playerForward = player.forward;
		var playerAngleY = player.localEulerAngles.y;

		gameTimer.TimerCount();
		if(isGameClear)
		{
			Debug.Log("クリア!");

		}
		if(isGameOver)
		{
			Debug.Log("ゲームオーバー!");
		}

		if(!isGameOver && isNowGaming)
		{
			playerScript.PlayerUpdate();
			enemyScript.EnemyUpdate();
			if(!isClearDirecting)
			{
				camScript.Follow(playerPos, offset);

				uiManagerScript.UpdateUI(playerPos, enemyPos, playerForward, playerAngleY, playerScript.GetStop());
			}
		}
	}

	private void GameClear()
	{
		Debug.Log("クリア発動");
		clearBaseScript.RemoveListenerFromClearEvent(GameClear);
		isGameClear = true;
		uiManagerScript.SetAllImageActive(false);
		SoundManager.Instance.PlaySE("GameClear");
		SceneManager.LoadScene((int)SceneController.SceneType.GameClearScene,LoadSceneMode.Additive);
		gameTimer.GetRecordTime();
	}

	private void GameOver()
	{
		Debug.Log("ゲームオーバー発動");
		SoundManager.Instance.PlaySE("GameOver");
		SceneManager.LoadScene((int)SceneController.SceneType.GameOverScene,LoadSceneMode.Additive);
		enemyScript.RemoveListenerFromGameOverEvent(GameOver);
		isGameOver = true;
		uiManagerScript.SetAllImageActive(false);
	}

	private void ClearDirecting()
	{
		Debug.Log("クリア演出発動");
		clearBaseScript.RemoveListenerFromClearEvent(ClearDirecting);

		enemyScript.EnableClearDirecting();

		Debug.Log("ゴールイベント追加");
		clearBaseScript.AddListenerToGameClearEvent(GameClear);
		isClearDirecting = true;
	}

	private void PlayEffect(Vector3 pos,EffectManager.EffectType effectType, float angle)
	{
		Debug.Log("エフェクト再生開始");
		effectScript.SetEffectType(effectType);
		effectScript.PlayEffect(pos,angle);
		enemyScript.RemoveListenerFromEnemyEffectEvent(() => {PlayEffect(playerObj.transform.position,EffectManager.EffectType.Enemy, angle);});
	}

	private void StartDirecting()
	{
		Debug.Log("スタート演出発動");
		camScript.RemoveListenerFromCountDownStartEvent(StartDirecting);
		isCountDown = true;
		uiManagerScript.StartCountDown();

		var playerPos = playerObj.transform.position;
		var camPos = camObj.transform.position;

		offset = camPos - playerPos;
	}

	private void GameStartDirecting()
	{
		Debug.Log("ゲームスタート");
		camScript.RemoveListenerFromGameStartEvent(GameStartDirecting);
		isNowGaming = true;
		uiManagerScript.SetStartImageActive(true);
		isCountDown = false;
	}

	private void WarningDirecting()
	{
		Debug.Log("警告");
		uiManagerScript.SetWarning();
	}

	IEnumerator GameStartDirectionCoroutine()
	{
		var playerPos = playerObj.transform.position;

		camScript.StartCoroutine(camScript.ScrollStart(playerPos));

		yield break;
	}

	private void AddScript<T>(GameObject[] gameObject,List<T> list)
	{
		foreach(GameObject block in gameObject)
		{
			Debug.Log(list);
			list.Add(block.GetComponent<T>());
		}
	}

	private void AddEffect <T>(Vector3 pos,EffectManager.EffectType effectType, List<T> list) where T : IAddListener
	{
		foreach(T listener in list)
		{
			Debug.Log("ノックバックエフェクトイベント追加");
			if(effectType == EffectManager.EffectType.KnockBack)
			{
				listener.AddListenerToEffectEvent(() => {PlayEffect(pos,effectType, 0.0f);});
			}
			else
			{
				listener.AddListenerToEffectEvent(() => {PlayEffect(listener.GetPos(),effectType, 0.0f);});
			}
		}
	}
}
