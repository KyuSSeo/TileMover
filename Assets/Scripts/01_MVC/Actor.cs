using UnityEngine;
public class Actor
{
    public PlayerType player;
    public PlayerType computer;

    public PlayerType current 
    {
        get 
        {
            return computer != PlayerType.None ? computer : player; 
        }
    }
}
