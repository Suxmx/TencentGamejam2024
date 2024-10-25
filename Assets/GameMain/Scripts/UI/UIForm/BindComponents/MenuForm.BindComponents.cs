using Autobind;
using UnityEngine;
using UnityEngine.UI;

//自动生成于：2024/10/25 16:58:52
namespace GameMain
{

	public partial class MenuForm
	{

		private Button m_btn_newgame;
		private Button m_btn_setting;
		private Button m_btn_exit;
		private RectTransform m_rect_selector;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_btn_newgame = autoBindTool.GetBindComponent<Button>(0);
			m_btn_setting = autoBindTool.GetBindComponent<Button>(1);
			m_btn_exit = autoBindTool.GetBindComponent<Button>(2);
			m_rect_selector = autoBindTool.GetBindComponent<RectTransform>(3);
		}

		private void ReleaseBindComponents()
		{
			//可以根据需要在这里添加代码，位置UIFormCodeGenerator.cs GenAutoBindCode()函数
		}

	}
}
