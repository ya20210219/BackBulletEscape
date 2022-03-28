using UnityEngine.UI;
using UnityEngine;

public class InGameUIScript : MonoBehaviour,ISetImageActive
{
	[SerializeField]
	Image	bannerImage;
	[SerializeField]
	Button	optionButton;
	[SerializeField]
	Image	stage1Image;
	[SerializeField]
	Image	stage2Image;
	[SerializeField]
	Image	stage3Image;
	void Start()
	{
		
	}

	void Update()
	{

	}

	public void SetImageActive(bool isEnable)
	{
		bannerImage.gameObject.SetActive(isEnable);
		optionButton.gameObject.SetActive(isEnable);
		stage1Image.gameObject.SetActive(isEnable);
		stage2Image.gameObject.SetActive(isEnable);
		stage3Image.gameObject.SetActive(isEnable);
	}

	public void SetStageNumImage(SceneController.SceneType type)
	{
		switch(type)
		{
			case SceneController.SceneType.Stage1:
			stage2Image.gameObject.SetActive(false);
			stage3Image.gameObject.SetActive(false);
			break;
						
			case SceneController.SceneType.Stage2:
			stage1Image.gameObject.SetActive(false);
			stage3Image.gameObject.SetActive(false);
			break;
		
			case SceneController.SceneType.Stage3:
			stage1Image.gameObject.SetActive(false);
			stage2Image.gameObject.SetActive(false);
			break;

			default:
			break;
		}
	}
}
