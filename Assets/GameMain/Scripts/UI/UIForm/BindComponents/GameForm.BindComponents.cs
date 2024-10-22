using Autobind;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//自动生成于：2024/10/22 21:01:10
namespace GameMain
{

	public partial class GameForm
	{

		private Image m_img_cusor;
		private RectTransform m_trans_keys;
		private TextMeshProUGUI m_tmp_curMaterial;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_img_cusor = autoBindTool.GetBindComponent<Image>(0);
			m_trans_keys = autoBindTool.GetBindComponent<RectTransform>(1);
			m_tmp_curMaterial = autoBindTool.GetBindComponent<TextMeshProUGUI>(2);
		}

		private void ReleaseBindComponents()
		{
			//可以根据需要在这里添加代码，位置UIFormCodeGenerator.cs GenAutoBindCode()函数
		}

	}
}
