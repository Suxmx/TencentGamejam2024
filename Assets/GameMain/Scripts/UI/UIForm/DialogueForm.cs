using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Args;
using MirMirror;
using UnityEngine;

namespace GameMain
{
    public partial class DialogueForm : UGuiForm
    {
        private const float _textSpeed = 10;
        private MMDialogue_Data _curDialogueData;
        private DataMMDialogueNode _currentNode = null;
        private int _curWordIndex;
        private int _choiceId;
        private List<string> _curWords;
        private bool _onClickNext;
        private Tween _textTween;

        public override void OnInit()
        {
            base.OnInit();
            GetBindComponents(gameObject);
        }

        private void RegisterEvents()
        {
            m_btn_next.onClick.AddListener(OnClickNext);
        }

        private void RemoveEvents()
        {
            m_btn_next.onClick.RemoveListener(OnClickNext);
        }

        private void OnClickNext()
        {
            _onClickNext = true;
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            AGameManager.FreeCursor(true);
            RegisterEvents();
            _currentNode = null;
            _curDialogueData = (MMDialogue_Data)userData;
            GameEntry.Event.Fire(this, OnDialoguePlayArg.Create(true));
            StartCoroutine(nameof(ParseMM_DialogueData));
        }

        public override void OnClose()
        {
            base.OnClose();
            RemoveEvents();
            GameEntry.Event.Fire(this, OnDialoguePlayArg.Create(false));
            AGameManager.LockCursor();
        }

        private IEnumerator ParseMM_DialogueData()
        {
            _choiceId = -1;
            _currentNode = _curDialogueData.m_DialoguePartDatas[0].GetFirstNode();


            while (true)
            {
                switch (_currentNode.DataNodeID.m_NODETYPE)
                {
                    case NODETYPE.START:
                        _currentNode = _curDialogueData.m_DialoguePartDatas[0].GetFirstNode();
                        break;
                    case NODETYPE.DIALOGUE:
                        for (int i = 0; i < (_currentNode as DataDialogueNode).Words.Count; i++)
                        {
                            _curWordIndex = i;
                            ContinueDialogue(_currentNode as DataDialogueNode);

                            while (_onClickNext == false)
                            {
                                yield return null;
                            }

                            bool endTween = false;
                            if (_textTween is not null && _textTween.active)
                            {
                                _textTween.Kill();
                                m_tmp_content.text = (_currentNode as DataDialogueNode).Words[_curWordIndex];
                                endTween = true;
                                _onClickNext = false;
                            }

                            if (endTween)
                            {
                                while (_onClickNext == false)
                                {
                                    yield return null;
                                }
                            }

                            _onClickNext = false;
                            yield return null;
                        }


                        _currentNode = _curDialogueData.m_DialoguePartDatas[0].GetNextDialogueNode(_currentNode);
                        break;
                    case NODETYPE.CHOICE:
                        _choiceId = -1;
                        ShowChoiceDialogue(_currentNode as DataChoieceNode);
                        while (_choiceId < 0)
                        {
                            yield return null;
                        }


                        _currentNode = _curDialogueData.m_DialoguePartDatas[0]
                            .GetNextDialogueNode(_currentNode, _choiceId);
                        _choiceId = -1;
                        break;
                    // case NODETYPE.EVENT:
                    //     TriggerEvent((_currentNode as DataEventNode).EventKey);
                    //
                    //     _currentNode = _curDialogueData.m_DialoguePartDatas[0].GetNextDialogueNode(_currentNode);
                    //     break;
                    default:
                        break;
                }


                if (_currentNode == null)
                {
                    EndDialogue();
                    break;
                }

                yield return null;
            }
        }

        /// <summary>
        /// 继续剧情对话node
        /// </summary>
        /// <param name="node"></param>
        private void ContinueDialogue(DataDialogueNode node)
        {
            m_tmp_leftName.text = node.ChrName;
            m_tmp_content.text = "";
            float time = node.Words[_curWordIndex].Length / _textSpeed;
            _textTween = m_tmp_content.DOText(node.Words[_curWordIndex], time);
        }

        private void ShowChoiceDialogue(DataChoieceNode node)
        {
        }

        private void EndDialogue()
        {
            GameEntry.UI.CloseUIForm(UIFormId.DialogueForm);
        }
    }
}