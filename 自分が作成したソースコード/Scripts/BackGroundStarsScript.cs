using UnityEngine;

public class BackGroundStarsScript : MonoBehaviour
{
	[SerializeField]
	Transform camObj;

	Vector3 previousCamPos;

	[SerializeField,Range(0.0f, 10.0f)]
	[Tooltip("減速倍率")]
	float decelerateValue;
	Renderer rend;

	int materialID;

	void Start()
	{
		materialID = Shader.PropertyToID("_MainTex");
		rend = this.GetComponent<Renderer>();
		rend.sharedMaterial.SetTextureOffset (materialID, Vector2.zero);
	}

	void FixedUpdate ()
	{
		Vector3 camPos = camObj.transform.position;
		Vector2 offset = rend.sharedMaterial.GetTextureOffset(materialID);

		Vector3 speed = ((camPos - previousCamPos) * Time.deltaTime);

		speed.z *= decelerateValue;

		float y = Mathf.Repeat (speed.z, 1);

		Vector2 newOffset = new Vector2 (0, offset.y + y);

		rend.sharedMaterial.SetTextureOffset (materialID, newOffset);
		previousCamPos = camPos;
	}
}
