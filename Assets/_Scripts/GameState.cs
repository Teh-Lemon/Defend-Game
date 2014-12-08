public static class GameStates
{
    // Curent game state
    public static States Current = States.PLAYING;

    // Game states
    public enum States
    {
        MENU,
        PLAYING,
        GAME_OVER
    }
}