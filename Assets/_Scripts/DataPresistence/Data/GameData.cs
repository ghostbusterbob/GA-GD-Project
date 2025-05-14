using UnityEngine;

[System.Serializable]

public class GameData
{
    public int reload;

    // The values defined in this constructor will be the default values
    // The game starts with when there's no data to load


    public GameData()
    {
       this.reload = 0;
    }
}

