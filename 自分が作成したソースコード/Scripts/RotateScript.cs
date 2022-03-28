using System.Collections;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
	[SerializeField]
	Transform myTransform;

    [SerializeField, Range(-10.0f,10.0f)]
	[Tooltip("回転速度")]
	private float rotateX, rotateY, rotateZ;

	void Start()
	{
		StartCoroutine(Rotate());
	}

	IEnumerator Rotate()
	{
		while(true)
		{
			Vector3 angle = myTransform.localEulerAngles;
			myTransform.localEulerAngles = new Vector3(angle.x + rotateX, angle.y + rotateY, angle.z + rotateZ);
			yield return null;
		}
	}
}
