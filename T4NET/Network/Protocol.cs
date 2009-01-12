namespace T4NET.Network
{
    public static class Protocol
    {
        // 1-999: System messages

        // 1000-1999: Lobby messages
        public const ushort CHAT_CONTENT = 1000;

        // 2000-2999: Gameplay messages
        public const ushort PLAYER_INCORPORATE_PIECE = 2000;
    }
}
