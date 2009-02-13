using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using T4NET.Controls;
using T4NET.Menus;

namespace T4NET.Graphic
{
    public class MenuDisplay
    {
        private static SpriteFont s_font;
        private static Texture2D s_block;
        private static readonly BlockFont s_blockFont = new BlockFont();

        public static void LoadContent(ContentManager content)
        {
            s_font = content.Load<SpriteFont>("MenuFont");
            s_block = content.Load<Texture2D>("RedBlock");
        }

        private readonly Menu m_menu;
        private GraphicsDevice m_device;
        private SpriteBatch m_spriteBatch;
        private BasicEffect m_basicEffect;
        private VertexPositionColor[] m_grid;
        private VertexBuffer m_vertexBuffer;
        private VertexDeclaration m_vertexDeclaration;

        public MenuDisplay(Menu menu)
        {
            m_menu = menu;
        }

        public Menu Menu
        {
            get { return m_menu; }
        }

        public void Initialize(GraphicsDevice device)
        {
            m_device = device;
            m_spriteBatch = new SpriteBatch(device);
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

            // Init grid
            m_grid = new VertexPositionColor[4];
            //m_grid[0].Position = new Vector3(200, 80, 0);
            m_grid[0].Position = new Vector3(device.Viewport.TitleSafeArea.X, device.Viewport.TitleSafeArea.Y, 0);
            m_grid[0].Color = Color.Black;
            m_grid[0].Color.A = 200;
            //m_grid[1].Position = new Vector3(device.Viewport.Width - 200, 80, 0);
            m_grid[1].Position = new Vector3(device.Viewport.TitleSafeArea.X + device.Viewport.TitleSafeArea.Width, device.Viewport.TitleSafeArea.Y, 0);
            m_grid[1].Color = Color.Black;
            m_grid[1].Color.A = 200;
            //m_grid[2].Position = new Vector3(device.Viewport.Width - 200, device.Viewport.Height - 80, 0);
            m_grid[2].Position = new Vector3(device.Viewport.TitleSafeArea.X + device.Viewport.TitleSafeArea.Width, device.Viewport.TitleSafeArea.Y + device.Viewport.TitleSafeArea.Height, 0);
            m_grid[2].Color = Color.Black;
            m_grid[2].Color.A = 200;
            //m_grid[3].Position = new Vector3(200, device.Viewport.Height - 80, 0);
            m_grid[3].Position = new Vector3(device.Viewport.TitleSafeArea.X, device.Viewport.TitleSafeArea.Y + device.Viewport.TitleSafeArea.Height, 0);
            m_grid[3].Color = Color.Black;
            m_grid[3].Color.A = 200;

            // Init vertex buffer
            m_vertexBuffer = new VertexBuffer(device, VertexPositionColor.SizeInBytes * m_grid.Length,
                                              BufferUsage.None);
            m_vertexBuffer.SetData(m_grid);
            m_vertexDeclaration = new VertexDeclaration(m_device, VertexPositionColor.VertexElements);

        }

        public void Draw()
        {
            if (m_menu.Active)
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
                    pass.End();
                }
                m_basicEffect.End();

                // Text
                m_spriteBatch.Begin();
                var pos = new Vector2(230, 100);
                foreach (var entry in m_menu.Entries)
                {
                    //m_spriteBatch.DrawString(s_font, entry.Title, pos,
                    //                         m_menu.SelectedEntry == entry ? Color.Red : Color.DarkRed);
                    pos.Y += 60;
                }
                m_spriteBatch.DrawString(s_font, "@", new Vector2(0,-8), Color.Red);
                string txt = "ABCDEFGHIJKLMNOPQRS\nTUVWXYZ0123456789";
                int cursor = 0;
                int line = 0;
                var corner = new Point(100, 100);
                const int SIZE = 10;
                foreach (var c in txt)
                {
                    if (c == '\n')
                    {
                        line += 6;
                        cursor = 0;
                    }
                    else
                    {
                        var matrix = s_blockFont.GetMatrix(c);
                        int max = 0;
                        foreach (var point in matrix)
                        {
                            m_spriteBatch.Draw(s_block,
                                               new Rectangle(corner.X + (cursor + point.X)*SIZE, corner.Y + (line + point.Y)*SIZE,
                                                             SIZE, SIZE), Color.White);
                            if (point.X > max)
                            {
                                max = point.X;
                            }
                        }
                        cursor += max + 2;
                    }
                }
                m_spriteBatch.End();
            }
        }
    }
}
