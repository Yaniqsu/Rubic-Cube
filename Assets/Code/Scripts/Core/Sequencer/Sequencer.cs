using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YNQ.Sequencer
{
    public struct SequenceActionData
    {
        public ISequenceAction action;
        public Action onEnd;
    }
    
    public interface ISequenceAction
    {
        public void Start();
        public void Update(float elapsed);
        public bool Completed { get; }
    }

    public class Sequencer
    {
        private readonly Queue<SequenceActionData> _actions = new();
        private readonly MonoBehaviour _mono;
        private Coroutine _handleActionsRoutine;

        public Sequencer(MonoBehaviour mono)
        {
            _mono = mono;
        }

        public void QueueAction(ISequenceAction action, Action onEnd = null)
        {
            _actions.Enqueue(new SequenceActionData()
            {
                action = action,
                onEnd = onEnd
            });

            StartQueue();
        }

        private void StartQueue()
        {
            if (_handleActionsRoutine == null)
                _handleActionsRoutine = _mono.StartCoroutine(HandleActions());
        }

        private IEnumerator HandleActions()
        {
            while (_actions.Count > 0)
            {
                var data = _actions.Dequeue();
                var action = data.action;
                var elapsed = 0f;

                action.Start();

                while (!action.Completed)
                {
                    action.Update(elapsed);

                    elapsed += Time.deltaTime;
                    yield return null;
                }

                data.onEnd?.Invoke();
            }

            _handleActionsRoutine = null;
        }
    } 
}
