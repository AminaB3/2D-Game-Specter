using UnityEngine;

public class Pause : MonoBehaviour
{
    //This class is called when the game is to be paused when speaking to NPCs
    //So the main character can't just run around or away while they speak
    public static bool isGamePaused{get; private set;} = false;
    public static void SetPause(bool pause)
    {
        isGamePaused = pause;
    }
}
