using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

[Serializable]
public struct TableRow<T1,T2>
{
    public T1 key;
    public T2 value;
    
    [JsonConstructor]
    public TableRow(T1 key, T2 value)
    {
        this.key = key;
        this.value = value;
    }
    
    public Dictionary<T1, T2> ToDictionary(TableRow<T1, T2>[] array)
    {
        return array.ToDictionary(row => row.key, row => row.value);
    }
}

public static class TableRowUtilities
{
    public static Dictionary<T1, T2> ToDictionary<T1, T2>(TableRow<T1, T2>[] dictionary)
        => dictionary.ToDictionary(tableRow => tableRow.key, tableRow => tableRow.value);
    
    public static TableRow<T1, T2>[] FromDictionary<T1, T2>(Dictionary<T1, T2> dictionary)
        => dictionary
            .Select(e => new TableRow<T1, T2>(e.Key, e.Value))
            .ToArray();
    
    public static TableRow<T1, T2>[] FromDictionary<T1, T2>(SDictionary<T1, T2> dictionary)
        => dictionary
            .Select(e => new TableRow<T1, T2>(e.key, e.value))
            .ToArray();
}