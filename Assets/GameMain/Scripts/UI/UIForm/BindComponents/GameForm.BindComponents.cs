using Autobind;
using TMPro;
using UnityEngine;

//自动生成于：2024/10/24 18:43:27
namespace GameMain
{

	public partial class GameForm
	{

		private RectTransform m_trans_keys;
		private TextMeshProUGUI m_tmp_curMaterial;
		private RectTransform m_rect_materials;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_trans_keys = autoBindTool.GetBindComponent<RectTransform>(0);
			m_tmp_curMaterial = autoBindTool.GetBindComponent<TextMeshProUGUI>(1);
			m_rect_materials = autoBindTool.GetBindComponent<RectTransform>(2);
		}

		private void ReleaseBindComponents()
		{
			//可以根据需要在这里添加代码，位置UIFormCodeGenerator.cs GenAutoBindCode()函数
		}

	}
}
