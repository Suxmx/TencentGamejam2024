using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MirMirror
{
    public class MMDialogue_Data : ScriptableObject
    {
        [SerializeField]
    
        private int m_CharacterID;
        public int CharacterID { get { return m_CharacterID; }set { m_CharacterID = value; } }

        [SerializeField]
        private string m_CharacterName;
        public string CharacterName { get { return m_CharacterName; }set { m_CharacterName = value; } }
        public List<DialoguePartData> m_DialoguePartDatas;

        public void SetCharacterID(int ID)
        {
            m_CharacterID = ID;
        }
        public void SetCharacterName(string Name)
        {
            m_CharacterName = Name;
        }



    }
    [System.Serializable]
    public class DialoguePartData
    {
        public DataStartNode m_StartNode = null;
        public List<DataDialogueNode> m_DataDialogueNodes;
        public List<DataChoieceNode> m_DataChoieceNodes;
        public List<DataEventNode> m_DataEventNodes;

        public DialoguePartData()
        {
            m_StartNode = new DataStartNode();
            m_DataDialogueNodes = new List<DataDialogueNode>();
            m_DataChoieceNodes = new List<DataChoieceNode>();
            m_DataEventNodes = new List<DataEventNode>();
        }

        public void RemoveDataDialogueNode(int dataID)
        {
            for (int i = 0; i < m_DataDialogueNodes.Count; i++)
            {
                if (m_DataDialogueNodes[i].DataNodeID.ID == dataID)
                {
                    m_DataDialogueNodes.RemoveAt(i);
                    break;
                }
            }
        }
        public void RemoveDataChoieceNode(int dataID)
        {
            for (int i = 0; i < m_DataChoieceNodes.Count; i++)
            {
                if (m_DataChoieceNodes[i].DataNodeID.ID == dataID)
                {
                    m_DataChoieceNodes.RemoveAt(i);
                    break;
                }
            }
        }
        public void RemoveDataEventNode(int dataID)
        {
            for (int i = 0; i < m_DataEventNodes.Count; i++)
            {
                if (m_DataEventNodes[i].DataNodeID.ID == dataID)
                {
                    m_DataEventNodes.RemoveAt(i);
                    break;
                }
            }
        }


        public DataDialogueNode GetDataDialogueNode(int dataID)
        {
            for (int i = 0; i < m_DataDialogueNodes.Count; i++)
            {
                if (m_DataDialogueNodes[i].DataNodeID.ID == dataID)
                {
                    return m_DataDialogueNodes[i];
                }
            }
            return null;
        }
        public DataChoieceNode GetDataChoiceNode(int dataID)
        {
            for (int i = 0; i < m_DataChoieceNodes.Count; i++)
            {
                if (m_DataChoieceNodes[i].DataNodeID.ID == dataID)
                {
                    return m_DataChoieceNodes[i];
                }
            }
            return null;
        }
        public DataEventNode GetDataEventNode(int dataID)
        {
            for (int i = 0; i < m_DataEventNodes.Count; i++)
            {
                if (m_DataEventNodes[i].DataNodeID.ID == dataID)
                {
                    return m_DataEventNodes[i];
                }
            }
            return null;
        }

        public DataMMDialogueNode GetFirstNode()
        {
            switch (m_StartNode.NextNodes()[0].m_NODETYPE)
            {
                case NODETYPE.START:

                    return null;
                case NODETYPE.DIALOGUE:
                    return m_DataDialogueNodes[m_StartNode.NextNodes()[0].ID];

                case NODETYPE.CHOICE:
                    return m_DataChoieceNodes[m_StartNode.NextNodes()[0].ID];

                case NODETYPE.EVENT:
                    return m_DataEventNodes[m_StartNode.NextNodes()[0].ID];

                default:
                    return null;
            }
        }
        public DataMMDialogueNode GetNextDialogueNode(DataMMDialogueNode _CurrentNode, int _ChoiceNID = 0)
        {

            if (_CurrentNode.NextNodes().Count == 0)
            {
                return null;
            }
            switch (_CurrentNode.NextNodes()[_ChoiceNID].m_NODETYPE)
            {
                case NODETYPE.START:

                    return null;
                case NODETYPE.DIALOGUE:

                    return m_DataDialogueNodes[_CurrentNode.NextNodes()[_ChoiceNID].ID];

                case NODETYPE.CHOICE:
                    return m_DataChoieceNodes[_CurrentNode.NextNodes()[_ChoiceNID].ID];


                case NODETYPE.EVENT:


                    return m_DataEventNodes[_CurrentNode.NextNodes()[_ChoiceNID].ID];



                default:
                    return null;
            }
        }
        public void ResetDataData()
        {
            m_StartNode = null;
            m_DataChoieceNodes.Clear();
            m_DataDialogueNodes.Clear();
            m_DataEventNodes.Clear();

        }
    }
    [System.Serializable]
    public enum NODETYPE
    {
        START,
        DIALOGUE,
        CHOICE,
        EVENT,



    }
    [System.Serializable]
    public enum SpeakPos
    {
        L,
        R

    }
}
