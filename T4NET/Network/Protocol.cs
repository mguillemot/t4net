namespace T4NET.Network
{
    public enum Protocol : ushort 
    {
        // 1-999: System messages
        LOCAL_PLAYER_SIGN_IN = 1,
        LOCAL_PLAYER_SIGN_OUT = 2,

        // 1000-1999: Lobby messages
        REMOTE_PLAYER_SIGN_IN = 1000,
        REMOTE_PLAYER_SIGN_OUT = 1001,
        CHAT_CONTENT = 1002,

        // 2000-2999: Gameplay messages
        PLAYER_INCORPORATE_PIECE = 2000,
        BOARD_FULL_CONTENT = 2001
    }
}
