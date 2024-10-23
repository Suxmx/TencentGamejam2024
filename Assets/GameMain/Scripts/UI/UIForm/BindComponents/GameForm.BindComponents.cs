using Autobind;
using TMPro;
using UnityEngine;

//自动生成于：2024/10/23 15:28:46
namespace GameMain
{

	public partial class GameForm
	{

		private RectTransform m_rect_cusor;
		private RectTransform m_trans_keys;
		private TextMeshProUGUI m_tmp_curMaterial;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_rect_cusor = autoBindTool.GetBindComponent<RectTransform>(0);
			m_trans_keys = autoBindTool.GetBindComponent<RectTransform>(1);
			m_tmp_curMaterial = autoBindTool.GetBindComponent<TextMeshProUGUI>(2);
		}

		private void ReleaseBindComponents()
		{
			//可以根据需要在这里添加代码，位置UIFormCodeGenerator.cs GenAutoBindCode()函数
		}

	}
}
