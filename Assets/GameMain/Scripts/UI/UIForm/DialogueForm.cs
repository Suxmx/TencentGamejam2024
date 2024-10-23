using System.Collections.Generic;
using Framework;
using MirMirror;

namespace GameMain
{
    public partial class DialogueForm : UGuiForm
    {
        private MMDialogue_Data _curDialogueData;
        private DataMMDialogueNode _currentNode = null;
        private int _curWordIndex;
        private List<string> _curWords;
        public override void OnInit()
        {
            base.OnInit();
            GetBindComponents(gameObject);
        }

        private void RegisterEvents()
        {
            
        }

        private void RemoveEvents()
        {
            
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            RegisterEvents();
            _currentNode = null;
            _curDialogueData = (MMDialogue_Data)userData;
        }

        public override void OnClose()
        {
            base.OnClose();
            RemoveEvents();
        }

        private void StartDialogue()
        {
            _currentNode = _curDialogueData.m_DialoguePartDatas[0].GetFirstNode();
            ParseNode(_currentNode);
        }

        private void ParseNode(DataMMDialogueNode node)
        {
            switch (node.DataNodeID.m_NODETYPE)
            {
                case NODETYPE.DIALOGUE:
                    
                    break;
            }
        }
    }
}