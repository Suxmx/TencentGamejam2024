using Autobind;
using UnityEngine;
using UnityEngine.UI;

//自动生成于：2024/10/25 12:35:49
namespace GameMain
{

	public partial class FakeMenuForm
	{

		private Button m_btn_newgame;
		private Button m_btn_setting;
		private Button m_btn_makers;
		private Button m_btn_exit;
		private RectTransform m_rect_selector;
		private RectTransform m_trans_loading;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_btn_newgame = autoBindTool.GetBindComponent<Button>(0);
			m_btn_setting = autoBindTool.GetBindComponent<Button>(1);
			m_btn_makers = autoBindTool.GetBindComponent<Button>(2);
			m_btn_exit = autoBindTool.GetBindComponent<Button>(3);
			m_rect_selector = autoBindTool.GetBindComponent<RectTransform>(4);
			m_trans_loading = autoBindTool.GetBindComponent<RectTransform>(5);
		}

		private void ReleaseBindComponents()
		{
			//可以根据需要在这里添加代码，位置UIFormCodeGenerator.cs GenAutoBindCode()函数
		}

	}
}
