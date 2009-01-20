using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace T4NET.Graphic
{
    public class ConsoleDisplay
    {
        private static SpriteFont s_systemFont;

        private GraphicsDevice m_device;
        private BasicEffect m_basicEffect;
        private VertexPositionColor[] m_grid;
        private SpriteBatch m_spriteBatch;
        private VertexBuffer m_vertexBuffer;
        private VertexDeclaration m_vertexDeclaration;

        public static void LoadContent(ContentManager content)
        {
            s_systemFont = content.Load<SpriteFont>("SystemFont");
        }

        public int CharacterHeight
        {
            get { return 44; } // TODO rendre ça dépendant de la résolution
        }

        public int CharacterWidth
        {
            get { return 157; }  // TODO rendre ça dépendant de la résolution
        }

        public int ConsoleHeight
        {
            get { return 720; } // TODO rendre ça paramétrable
        }

        public void Initialize(GraphicsDevice device)
        {
            m_device = device;
            m_basicEffect = new BasicEffect(device, null)
                {
                VertexColorEnabled = true,
                World = Matrix.Identity,
                View = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 1.0f), Vector3.Zero, Vector3.Up),
                Projection = Matrix.CreateOrthographicOffCenter(0, device.Viewport.Width,
                                device.Viewport.Height, 0,
                                1.0f,
                                1000.0f)
                };

            // Grid: console background
            m_grid = new VertexPositionColor[9];
            m_grid[0].Position = new Vector3(0, device.Viewport.Height - ConsoleHeight, 0);
            m_grid[0].Color = Color.Black;
            m_grid[0].Color.A = 150;
            m_grid[1].Position = new Vector3(device.Viewport.Width, device.Viewport.Height - ConsoleHeight, 0);
            m_grid[1].Color = Color.Black;
            m_grid[1].Color.A = 150;
            m_grid[2].Position = new Vector3(device.Viewport.Width, device.Viewport.Height, 0);
            m_grid[2].Color = Color.Black;
            m_grid[2].Color.A = 150;
            m_grid[3].Position = new Vector3(0, device.Viewport.Height, 0);
            m_grid[3].Color = Color.Black;
            m_grid[3].Color.A = 150;
            // Grid: tilesafe area
            m_grid[4].Position = new Vector3(device.Viewport.TitleSafeArea.X, device.Viewport.TitleSafeArea.Y, 0);
            m_grid[4].Color = Color.Red;
            m_grid[5].Position = new Vector3(device.Viewport.TitleSafeArea.X + device.Viewport.TitleSafeArea.Width, device.Viewport.TitleSafeArea.Y, 0);
            m_grid[5].Color = Color.Red;
            m_grid[6].Position = new Vector3(device.Viewport.TitleSafeArea.X + device.Viewport.TitleSafeArea.Width, device.Viewport.TitleSafeArea.Y + device.Viewport.TitleSafeArea.Height, 0);
            m_grid[6].Color = Color.Red;
            m_grid[7].Position = new Vector3(device.Viewport.TitleSafeArea.X, device.Viewport.TitleSafeArea.Y + device.Viewport.TitleSafeArea.Height, 0);
            m_grid[7].Color = Color.Red;
            m_grid[8] = m_grid[4];

            // Init vertex buffer
            m_vertexBuffer = new VertexBuffer(device, VertexPositionColor.SizeInBytes*m_grid.Length,
                                              BufferUsage.None);
            m_vertexBuffer.SetData(m_grid);
            m_vertexDeclaration = new VertexDeclaration(m_device, VertexPositionColor.VertexElements);

            // Init 2D batch
            m_spriteBatch = new SpriteBatch(device);
        }

        public void Draw()
        {
            // Background
            m_device.VertexDeclaration = m_vertexDeclaration;
            m_device.Vertices[0].SetSource(m_vertexBuffer, 0, VertexPositionColor.SizeInBytes);
            m_device.RenderState.CullMode = CullMode.None;
            m_basicEffect.Begin();
            foreach (EffectPass pass in m_basicEffect.CurrentTechnique.Passes)
            {
                pass.Begin();
                m_device.DrawPrimitives(PrimitiveType.TriangleFan, 0, 2);
                m_device.DrawPrimitives(PrimitiveType.LineStrip, 4, 4);
                pass.End();
            }
            m_basicEffect.End();

            // Text
            m_spriteBatch.Begin();
            m_spriteBatch.DrawString(s_systemFont, Console.GetFormattedText(CharacterHeight), new Vector2(16, m_device.Viewport.Height - ConsoleHeight + 3), Color.LightGreen);
            m_spriteBatch.End();
        }
    }
}