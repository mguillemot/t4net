namespace T4NET.Controls
{
    public static class Functions
    {
        public static readonly Function[] ALL =
            new[]
                {
                    Function.GAME_LEFT,
                    Function.GAME_RIGHT,
                    Function.GAME_UP,
                    Function.GAME_DOWN,
                    Function.GAME_ROTATE_R,
                    Function.GAME_ROTATE_L,
                    Function.GAME_BONUS_LEFT,
                    Function.GAME_BONUS_SELF,
                    Function.GAME_BONUS_RIGHT,
                    Function.GAME_MENU,
                    Function.MENU_UP,
                    Function.MENU_DOWN,
                    Function.MENU_LEFT,
                    Function.MENU_RIGHT,
                    Function.MENU_VALIDATE,
                    Function.MENU_CANCEL,
                    Function.CHEAT_RESET_BOARD,
                    Function.DEBUG_CONSOLE,
                    Function.DEBUG_CREATE_SESSION,
                    Function.DEBUG_JOIN_SESSION
                };
    }
}