using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace T4NET.Graphic
{
    public class BoardDisplay
    {
        private const int BLOCK_SIZE = 20;

        private static readonly Dictionary<int, Texture2D> s_blockTextures = new Dictionary<int, Texture2D>();

        private readonly Board m_board;
        private BasicEffect m_basicEffect;

        private GraphicsDevice m_device;
        private VertexPositionColor[] m_grid;
        private SpriteBatch m_spriteBatch;
        private VertexBuffer m_vertexBuffer;
        private VertexDeclaration m_vertexDeclaration;

        public BoardDisplay(Board board)
        {
            m_board = board;
        }

        public int UnscaledWidth
        {
            get { return m_board.HSize*BLOCK_SIZE; }
        }

        public int UnscaledHeight
        {
            get { return m_board.VSize*BLOCK_SIZE; }
        }

        public static void LoadContent(ContentManager content)
        {
            s_blockTextures[1] = content.Load<Texture2D>("DarkBlueBlock");
            s_blockTextures[2] = content.Load<Texture2D>("GreenBlock");
            s_blockTextures[3] = content.Load<Texture2D>("LightBlueBlock");
            s_blockTextures[4] = content.Load<Texture2D>("OrangeBlock");
            s_blockTextures[5] = content.Load<Texture2D>("RedBlock");
            s_blockTextures[6] = content.Load<Texture2D>("VioletBlock");
            s_blockTextures[7] = content.Load<Texture2D>("YellowBlock");
        }

        public void Initialize(GraphicsDevice device, BasicEffect basicEffect)
        {
            m_device = device;
            m_basicEffect = basicEffect;

            // Init grid
            m_grid = new VertexPositionColor[(m_board.HSize + m_board.VSize + 2)*2];
            int idx = 0;
            for (int i = 0; i <= m_board.HSize; i++)
            {
                m_grid[idx].Position = new Vector3(i*BLOCK_SIZE, BLOCK_SIZE, 0);
                m_grid[idx].Color = Color.Black;
                m_grid[idx + 1].Position = new Vector3(i*BLOCK_SIZE, m_board.VSize*BLOCK_SIZE, 0);
                m_grid[idx + 1].Color = Color.Black;
                idx += 2;
            }
            for (int j = 1; j <= m_board.VSize; j++)
            {
                m_grid[idx].Position = new Vector3(0, j*BLOCK_SIZE, 0);
                m_grid[idx].Color = Color.Black;
                m_grid[idx + 1].Position = new Vector3(m_board.HSize*BLOCK_SIZE, j*BLOCK_SIZE, 0);
                m_grid[idx + 1].Color = Color.Black;
                idx += 2;
            }

            // Init vertex buffer
            m_vertexBuffer = new VertexBuffer(device, VertexPositionColor.SizeInBytes*m_grid.Length,
                                              BufferUsage.None);
            m_vertexBuffer.SetData(m_grid);
            m_vertexDeclaration = new VertexDeclaration(m_device, VertexPositionColor.VertexElements);

            // Init 2D batch
            m_spriteBatch = new SpriteBatch(device);
        }

        public void Draw(Point origin, float scale)
        {
            m_device.VertexDeclaration = m_vertexDeclaration;
            m_device.Vertices[0].SetSource(m_vertexBuffer, 0, VertexPositionColor.SizeInBytes);
            m_basicEffect.World = Matrix.CreateScale(scale)*Matrix.CreateTranslation(origin.X, origin.Y, 0.0f);
            m_basicEffect.Begin();
            foreach (EffectPass pass in m_basicEffect.CurrentTechnique.Passes)
            {
                pass.Begin();
                m_device.DrawPrimitives(PrimitiveType.LineList, 0, m_grid.Length/2);
                pass.End();
            }
            m_basicEffect.End();

            m_spriteBatch.Begin();
            var scaledBlockSize = (int) (BLOCK_SIZE*scale-1);
            for (int i = 0; i < m_board.HSize; i++)
            {
                for (int j = 0; j < m_board.VSize; j++)
                {
                    if (m_board.Content[i][j] != 0)
                    {
                        var drawX = (int) (origin.X + 1 + BLOCK_SIZE*i*scale);
                        var drawY = (int) (origin.Y + 1 + BLOCK_SIZE*j*scale);
                        m_spriteBatch.Draw(s_blockTextures[m_board.Content[i][j]],
                                           new Rectangle(drawX, drawY, scaledBlockSize, scaledBlockSize),
                                           Color.White);
                    }
                }
            }
            if (m_board.CurrentPiece != null)
            {
                foreach (Point b in m_board.CurrentPiece.CurrentBlocks)
                {
                    int x = m_board.CurrentPiece.X + b.X;
                    int y = m_board.CurrentPiece.Y + b.Y;
                    var drawX = (int) (origin.X + 1 + BLOCK_SIZE*x*scale);
                    var drawY = (int) (origin.Y + 1 + BLOCK_SIZE*y*scale);
                    m_spriteBatch.Draw(s_blockTextures[m_board.CurrentPiece.Color],
                                       new Rectangle(drawX, drawY, scaledBlockSize, scaledBlockSize), Color.White);
                }
            }
            m_spriteBatch.End();
        }
    }
}