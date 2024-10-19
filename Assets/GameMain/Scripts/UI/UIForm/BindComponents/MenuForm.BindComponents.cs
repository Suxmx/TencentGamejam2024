using Autobind;
using UnityEngine;
using UnityEngine.UI;

//自动生成于：2024/9/29 2:02:09
namespace GameMain
{

	public partial class MenuForm
	{

		private Button m_btn_start;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_btn_start = autoBindTool.GetBindComponent<Button>(0);
		}

		private void ReleaseBindComponents()
		{
			//可以根据需要在这里添加代码，位置UIFormCodeGenerator.cs GenAutoBindCode()函数
		}

	}
}
