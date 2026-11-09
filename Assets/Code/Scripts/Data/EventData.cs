using System;

public class EventData<T>
{
    private T _value;

    public event Action<T> OnValueChanged;

    public T Value
    {
        get => _value;
        set
        {
            _value = value;
            OnValueChanged?.Invoke(_value);
        }
    }

    public string StrValue => _value.ToString();

    public EventData(T startValue)
    {
        Value = startValue;
    }
}
