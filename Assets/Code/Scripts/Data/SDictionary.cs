using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
[JsonConverter(typeof(SDictionaryJsonConverter))]
public class SDictionary<T1, T2> : IEnumerable<TableRow<T1,T2>>
{
    [SerializeField] private TableRow<T1, T2>[] m_array;

    public SDictionary()
    {
        m_array = Array.Empty<TableRow<T1, T2>>();
    }
    
    public int Count => m_array.Length;
    
    public SDictionary(params (T1 key, T2 value)[] values) 
    {
        m_array = new TableRow<T1, T2>[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            m_array[i] = new TableRow<T1, T2>(values[i].key, values[i].value);
        }
    }
    
    public SDictionary(Dictionary<T1, T2> dictionary) 
    {
        m_array = new TableRow<T1, T2>[dictionary.Count];
        for (int i = 0; i < m_array.Length; i++)
        {
            var element = dictionary.ElementAt(i);
            
            m_array[i] = new TableRow<T1, T2>(element.Key, element.Value);
        }
    }

    public T2 this[T1 key]
    {
        get => m_array.FirstOrDefault(row => row.key.Equals(key)).value;
        set
        {
            for (var index = 0; index < m_array.Length; index++)
            {
                if (m_array[index].key.Equals(key))
                {
                    m_array[index].value = value;
                    return;
                }
            }
        }
    }
    
    public bool TryAdd(T1 key, T2 value)
    {
        if (m_array.Any(row => row.key.Equals(key)))
            return false;
        
        Array.Resize(ref m_array, m_array.Length + 1);
        m_array[^1] = new TableRow<T1, T2>(key, value);

        return true;
    }

    public bool TryGetValue(T1 key, out T2 value)
    {
        value = default;
        
        if (m_array.All(row => !row.key.Equals(key)))
            return false;
        
        value = m_array.FirstOrDefault(row => row.key.Equals(key)).value;
        return true;
    }

    public void Add(T1 key, T2 value)
    {
        TryAdd(key, value);
    }
    
    public void Add(TableRow<T1, T2> row)
    {
        TryAdd(row.key, row.value);
    }

    public void Remove(T1 key)
    {
        for (int i = 0; i < m_array.Length; i++)
        {
            if (m_array[i].key.Equals(key))
            {
                m_array[i] = m_array[^1];
                
                Array.Resize(ref m_array, m_array.Length - 1);
                return;
            }
        }
    }

    public IEnumerator<TableRow<T1, T2>> GetEnumerator()
        => ((IEnumerable<TableRow<T1, T2>>)m_array)
            .GetEnumerator();

    public Dictionary<T1, T2> ToDictionary()
        => m_array.ToDictionary(row => row.key, row => row.value);

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerable<T1> Keys => m_array.Select(row => row.key);
    public IEnumerable<T2> Values => m_array.Select(row => row.value);
}