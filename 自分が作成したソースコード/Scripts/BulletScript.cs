using UnityEngine;

public class BulletScript : MonoBehaviour
{
	[SerializeField]
	[Tooltip("弾の移動速度")]
	float bulletSpeed;

	[SerializeField]
	[Tooltip("球の破壊エフェクト")]
	private ParticleSystem breakBulletEffect;
	Camera cam;
	Rigidbody rigidBody;

	private bool isRendering = true;
	private bool isFirstRendering = false;

	void Start()
	{
 		rigidBody =  GetComponent<Rigidbody>();
		cam = Camera.main;
	}

	void Update()
	{
		Transform t = gameObject.transform;
		Vector3 vec = t.forward;
		Vector3 pos = t.position;

		rigidBody.velocity = vec * bulletSpeed;

		if(isRendering == false && isFirstRendering == true)
		{
			Destroy(this.gameObject);
		}
		isRendering = false;
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			return;
		}
		Destroy(this.gameObject);
	}

	private void OnWillRenderObject()
	{
		if (Camera.current.name != "SceneCamera" && Camera.current.name != "Preview Camera")
		{
			isRendering = true;
			isFirstRendering = true;
		}
	}

	private void OnDestroy()
	{
		var pos = this.transform.position;
		ParticleSystem effect = Instantiate(breakBulletEffect);
		effect.transform.position = pos;
		effect.Play();
		Destroy(effect.gameObject,5.0f);
	}
}
