public static class GameStates
{
    // Curent game state
    public static States Current = States.LAUNCH;

    // Game states
    public enum States
    {
        LAUNCH,
        MENU,
        GAME_START,
        PLAYING,
        GAME_OVER,
        PAUSED
    }

    public enum MeteorTypes
    {
        NORMAL,
        BIG,
        FAST
    }
}