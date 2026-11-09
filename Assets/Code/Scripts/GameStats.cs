using UnityEngine;

public class GameStats
{
    public int size;
    public EventData<float> time = new(0);
    public EventData<int> moves = new(0);
}
