namespace T4NET.Controls
{
    public enum Function
    {
        // Game functions
        GAME_LEFT,
        GAME_RIGHT,
        GAME_UP,
        GAME_DOWN,
        GAME_ROTATE_R,
        GAME_ROTATE_L,
        GAME_BONUS_LEFT,
        GAME_BONUS_SELF,
        GAME_BONUS_RIGHT,
        GAME_MENU,

        // Menus
        MENU_UP,
        MENU_DOWN,
        MENU_LEFT,
        MENU_RIGHT,
        MENU_VALIDATE,
        MENU_CANCEL,

        // Cheats
        CHEAT_RESET_BOARD,

        // Debug
        DEBUG_CONSOLE,
        DEBUG_CREATE_SESSION,
        DEBUG_JOIN_SESSION
    }
}