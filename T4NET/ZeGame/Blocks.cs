using System.Collections.Generic;
using T4NET.ZeGame;

namespace T4NET.ZeGame
{
    public static class Blocks
    {
        public static readonly List<Block> SPECIAL_BLOCKS = new List<Block>
                                                                {
                                                                    Block.BONUS_C,
                                                                    Block.BONUS_N,
                                                                    Block.MALUS_A
                                                                };

        public static readonly List<Block> STANDARD_BLOCKS = new List<Block>
                                                                 {
                                                                     Block.DARK_BLUE,
                                                                     Block.GREEN,
                                                                     Block.LIGHT_BLUE,
                                                                     Block.ORANGE,
                                                                     Block.RED,
                                                                     Block.VIOLET,
                                                                     Block.YELLOW
                                                                 };
    }
}