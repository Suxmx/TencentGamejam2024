using Autobind;
using UnityEngine;

//自动生成于：2024/10/20 20:53:09
namespace Tencent
{

	public partial class GameForm
	{

		private RectTransform m_trans_test;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_trans_test = autoBindTool.GetBindComponent<RectTransform>(0);
		}

		private void ReleaseBindComponents()
		{
			//可以根据需要在这里添加代码，位置UIFormCodeGenerator.cs GenAutoBindCode()函数
		}

	}
}
