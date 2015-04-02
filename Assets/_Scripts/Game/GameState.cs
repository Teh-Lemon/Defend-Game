public static class GameStates
{
    // Curent game state
    public static States Current = States.LAUNCH;

    // Game states
    public enum States
    {
        LAUNCH,
        MENU,
        PLAYING,
        GAME_OVER
    }

    public enum MeteorTypes
    {
        NORMAL,
        BIG,
        FAST
    }
}