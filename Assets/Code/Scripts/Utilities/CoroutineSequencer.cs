using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineSequencer
{
    private struct RoutineData
    {
        public IEnumerator enumerator;
        public Action onEnd;
    }
    
    private readonly Queue<RoutineData> _queue;
    private readonly MonoBehaviour _mono;
    private Coroutine _sequenceRoutine;

    public CoroutineSequencer(MonoBehaviour mono)
    {
        _mono = mono;
        _queue = new Queue<RoutineData>();
    }

    public void AddCoroutine(IEnumerator enumerator, Action onEnd = null)
    {
        _queue.Enqueue(new RoutineData { enumerator = enumerator, onEnd = onEnd });

        StartSequence();
    }

    private void StartSequence()
    {
        if(_sequenceRoutine == null) 
            _sequenceRoutine = _mono.StartCoroutine(SequenceRoutine());
    }

    private IEnumerator SequenceRoutine()
    {
        while(_queue.Count > 0)
        {
            var data = _queue.Dequeue();
            
            yield return data.enumerator;

            data.onEnd?.Invoke();
        }

        _sequenceRoutine = null;
    }
}
