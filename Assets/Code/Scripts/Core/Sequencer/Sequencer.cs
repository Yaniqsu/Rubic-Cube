using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YNQ.Sequencer
{
    public interface ISequenceAction
    {
        public void Start();
        public void Update(float elapsed);
        public bool Completed { get; }
    }

    public class Sequencer
    {
        private readonly Queue<ISequenceAction> _actions = new();
        private readonly MonoBehaviour _mono;
        private Coroutine _handleActionsRoutine;

        public Sequencer(MonoBehaviour mono)
        {
            _mono = mono;
        }

        public void QueueAction(ISequenceAction action)
        {
            _actions.Enqueue(action);

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
                var action = _actions.Dequeue();
                var elapsed = 0f;

                action.Start();

                while (!action.Completed)
                {
                    action.Update(elapsed);

                    elapsed += Time.deltaTime;
                    yield return null;
                }
            }

            _handleActionsRoutine = null;
        }
    } 
}
