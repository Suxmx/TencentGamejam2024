using Autobind;
using TMPro;
using UnityEngine;

//自动生成于：2024/10/25 1:58:14
namespace GameMain
{

	public partial class GameForm
	{

		private RectTransform m_trans_keys;
		private TextMeshProUGUI m_tmp_curMaterial;
		private RectTransform m_rect_materials;
		private CanvasGroup m_group_guide;
		private TextMeshProUGUI m_tmp_guideTxt;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_trans_keys = autoBindTool.GetBindComponent<RectTransform>(0);
			m_tmp_curMaterial = autoBindTool.GetBindComponent<TextMeshProUGUI>(1);
			m_rect_materials = autoBindTool.GetBindComponent<RectTransform>(2);
			m_group_guide = autoBindTool.GetBindComponent<CanvasGroup>(3);
			m_tmp_guideTxt = autoBindTool.GetBindComponent<TextMeshProUGUI>(4);
		}

		private void ReleaseBindComponents()
		{
			//可以根据需要在这里添加代码，位置UIFormCodeGenerator.cs GenAutoBindCode()函数
		}

	}
}
