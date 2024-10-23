using Autobind;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//自动生成于：2024/10/23 20:42:54
namespace GameMain
{

	public partial class DialogueForm
	{

		private TextMeshProUGUI m_tmp_leftName;
		private TextMeshProUGUI m_tmp_content;
		private RectTransform m_trans_leftTri;
		private RectTransform m_trans_rightTri;
		private Button m_btn_next;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_tmp_leftName = autoBindTool.GetBindComponent<TextMeshProUGUI>(0);
			m_tmp_content = autoBindTool.GetBindComponent<TextMeshProUGUI>(1);
			m_trans_leftTri = autoBindTool.GetBindComponent<RectTransform>(2);
			m_trans_rightTri = autoBindTool.GetBindComponent<RectTransform>(3);
			m_btn_next = autoBindTool.GetBindComponent<Button>(4);
		}

		private void ReleaseBindComponents()
		{
			//可以根据需要在这里添加代码，位置UIFormCodeGenerator.cs GenAutoBindCode()函数
		}

	}
}
