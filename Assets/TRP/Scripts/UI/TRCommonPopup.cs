using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using ThreeRabbitPackage.DesignPattern;

public class TRCommonPopup : TRPopup
{
	[Header("Image")]
	public Image bg_img;
	public Image item_img;

	[Header("Text")]
	public Text title_txt;
	public Text message_txt;
	public Text confirm_txt;
	public Text cancel_txt;

	[Header("Button")]
	public Button confirm_btn;
	public Button cancel_btn;

	[Header("Enable Object")]
	public GameObject title;
	public GameObject item;
	public GameObject message;
	public GameObject buttonGroup;


	public void Awake()
	{
		base.Open();

		title.SetActive(false);
		item.SetActive(false);
		message.SetActive(false);
		buttonGroup.SetActive(false);
		confirm_btn.gameObject.SetActive(false);
		cancel_btn.gameObject.SetActive(false);
	}

	public void OnDestroy()
	{
		base.Close();
	}

	public static CommonPopupBuilder Instantiate(Transform parent)
	{
		return new CommonPopupBuilder(Instantiate(Resources.Load<GameObject>("Prefabs/TRCommonPopup"), parent));
	}

	public class CommonPopupBuilder : TRBuilder<TRCommonPopup>
	{
		public CommonPopupBuilder(GameObject obj) : base(obj) { }

		public CommonPopupBuilder SetTitle(string title)
		{
			component.title.SetActive(true);
			component.title_txt.text = title;
			return this;
		}

		public CommonPopupBuilder SetMessage(string message)
		{
			component.message.SetActive(true);
			component.message_txt.text = message;
			return this;
		}

		public CommonPopupBuilder SetConfirm(UnityAction<GameObject> confirmAction, string confirmText)
		{
			component.buttonGroup.SetActive(true);
			component.confirm_btn.gameObject.SetActive(true);
			component.confirm_btn.onClick.AddListener(() =>
			{
				confirmAction?.Invoke(this.gameObject);
			});
			component.confirm_txt.text = confirmText;
			return this;
		}

		public CommonPopupBuilder SetCancel(UnityAction<GameObject> cancelAction, string cancelText)
		{
			component.buttonGroup.SetActive(true);
			component.cancel_btn.gameObject.SetActive(true);
			component.cancel_btn.onClick.AddListener(() => 
			{
				cancelAction?.Invoke(this.gameObject);
			});
			component.cancel_txt.text = cancelText;
			return this;
		}

		public CommonPopupBuilder SetItemImage(Sprite itemSprite)
		{
			component.item.SetActive(true);
			component.item_img.sprite = itemSprite;
			return this;
		}

		public GameObject Build()
		{
			return this.gameObject;
		}
	}
}
