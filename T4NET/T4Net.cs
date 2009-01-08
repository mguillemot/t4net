using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace T4NET
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class T4Net : Game
    {
        private readonly Board m_board = new Board(10, 20);
        private readonly HashSet<Keys> m_previouslyPressedKeys = new HashSet<Keys>();
        private BasicEffect basicEffect;
        private VertexDeclaration basicEffectVertexDeclaration;
        private VertexPositionNormalTexture[] cubeVertices;
        private GraphicsDeviceManager graphics;
        private Piece m_currentPiece;

        private Texture2D m_darkBlueBlock;
        private Texture2D m_greenlock;
        private double m_lastAutoDrop;
        private double m_lastMove;
        private Texture2D m_lightBlueBlock;
        private Texture2D m_orangeBlock;
        private Texture2D m_redBlock;
        private Texture2D m_violetBlock;
        private Texture2D m_yellowBlock;
        private Matrix projectionMatrix;
        private SpriteBatch spriteBatch;
        private VertexBuffer vertexBuffer;
        private Matrix viewMatrix;
        private Matrix worldMatrix;

        public T4Net()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            RegenPiece();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            basicEffect = new BasicEffect(GraphicsDevice, null);
            basicEffect.VertexColorEnabled = true;
            worldMatrix = Matrix.CreateTranslation(100.0f, 50.0f, 0.0f);
            viewMatrix = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 1.0f), Vector3.Zero, Vector3.Up);
            projectionMatrix = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width,
                                                                  GraphicsDevice.Viewport.Height, 0, 1.0f,
                                                                  1000.0f);
            basicEffect.World = worldMatrix;
            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;

            m_darkBlueBlock = Content.Load<Texture2D>("DarkBlueBlock");
            m_lightBlueBlock = Content.Load<Texture2D>("LightBlueBlock");
            m_greenlock = Content.Load<Texture2D>("GreenBlock");
            m_orangeBlock = Content.Load<Texture2D>("OrangeBlock");
            m_redBlock = Content.Load<Texture2D>("RedBlock");
            m_violetBlock = Content.Load<Texture2D>("VioletBlock");
            m_yellowBlock = Content.Load<Texture2D>("YellowBlock");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            var keyboardState = Keyboard.GetState(PlayerIndex.One);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || keyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            var keysPressed = new HashSet<Keys>();
            var keysReleased = new HashSet<Keys>();
            foreach (var key in keyboardState.GetPressedKeys())
            {
                if (!m_previouslyPressedKeys.Contains(key))
                {
                    keysPressed.Add(key);
                }
            }
            foreach (var key in m_previouslyPressedKeys)
            {
                if (keyboardState.IsKeyUp(key))
                {
                    keysReleased.Add(key);
                }
            }
            m_previouslyPressedKeys.Clear();
            m_previouslyPressedKeys.UnionWith(keyboardState.GetPressedKeys());

            if (keysPressed.Contains(Keys.C))
            {
                RegenPiece();
            }

            if (m_currentPiece != null)
            {
                var totalSeconds = gameTime.TotalGameTime.TotalSeconds;
                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    if ((totalSeconds - m_lastMove) > 0.3)
                    {
                        m_lastMove = totalSeconds;
                        if (m_board.CanMoveRight(m_currentPiece))
                        {
                            m_currentPiece.X = m_currentPiece.X + 1;
                        }
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.Left))
                {
                    if ((totalSeconds - m_lastMove) > 0.3)
                    {
                        m_lastMove = totalSeconds;
                        if (m_board.CanMoveLeft(m_currentPiece))
                        {
                            m_currentPiece.X = m_currentPiece.X - 1;
                        }
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.Down))
                {
                    if ((totalSeconds - m_lastMove) > 0.1)
                    {
                        m_lastMove = totalSeconds;
                        m_lastAutoDrop = totalSeconds;
                        if (m_board.CanMoveDown(m_currentPiece))
                        {
                            m_currentPiece.Y = m_currentPiece.Y + 1;
                        }
                        else
                        {
                            m_board.Incorporate(m_currentPiece);
                            RegenPiece();
                        }
                    }
                }
                else
                {
                    m_lastMove = -1;
                }

                if (keysPressed.Contains(Keys.X))
                {
                    m_board.RotateRight(m_currentPiece);
                }
                else if (keysPressed.Contains(Keys.W))
                {
                    m_board.RotateLeft(m_currentPiece);
                }

                if ((totalSeconds - m_lastAutoDrop) > 0.4)
                {
                    m_lastAutoDrop = totalSeconds;
                    if (m_board.CanMoveDown(m_currentPiece))
                    {
                        //m_currentPiece.Y = m_currentPiece.Y + 1;
                    }
                    else
                    {
                        m_board.Incorporate(m_currentPiece);
                        RegenPiece();
                    }
                }
            }

            base.Update(gameTime);
        }

        private void RegenPiece()
        {
            m_currentPiece = Piece.RandomPiece();
            m_currentPiece.X = 5;
            m_currentPiece.Y = 2;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            const int BLOCK_SIZE = 20;
            GraphicsDevice.Clear(Color.CornflowerBlue);
            var grid = new VertexPositionColor[(m_board.HSize + m_board.VSize + 2)*2];
            int idx = 0;
            for (int i = 0; i <= m_board.HSize; i++)
            {
                grid[idx].Position = new Vector3(i*BLOCK_SIZE, 0, 0);
                grid[idx].Color = Color.Black;
                grid[idx + 1].Position = new Vector3(i*BLOCK_SIZE, m_board.VSize*BLOCK_SIZE, 0);
                grid[idx + 1].Color = Color.Black;
                idx += 2;
            }
            for (int j = 0; j <= m_board.VSize; j++)
            {
                grid[idx].Position = new Vector3(0, j*BLOCK_SIZE, 0);
                grid[idx].Color = Color.Black;
                grid[idx + 1].Position = new Vector3(m_board.HSize*BLOCK_SIZE, j*BLOCK_SIZE, 0);
                grid[idx + 1].Color = Color.Black;
                idx += 2;
            }
            basicEffectVertexDeclaration = new VertexDeclaration(GraphicsDevice, VertexPositionColor.VertexElements);
            vertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.SizeInBytes*grid.Length,
                                            BufferUsage.None);
            vertexBuffer.SetData(grid);
            GraphicsDevice.VertexDeclaration = basicEffectVertexDeclaration;
            GraphicsDevice.Vertices[0].SetSource(vertexBuffer, 0, VertexPositionColor.SizeInBytes);
            basicEffect.Begin();
            foreach (var pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Begin();
                GraphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, m_board.HSize + m_board.VSize + 2);
                pass.End();
            }
            basicEffect.End();
            spriteBatch.Begin();
            for (int i = 0; i < m_board.HSize; i++)
            {
                for (int j = 0; j < m_board.VSize; j++)
                {
                    if (m_board.Content[i][j] != 0)
                    {
                        spriteBatch.Draw(m_lightBlueBlock,
                                         new Rectangle(100 + BLOCK_SIZE*i + 1, 50 + BLOCK_SIZE*j + 1,
                                                       BLOCK_SIZE - 1, BLOCK_SIZE - 1), Color.White);
                    }
                }
            }
            if (m_currentPiece != null)
            {
                foreach (var b in m_currentPiece.CurrentBlocks)
                {
                    int x = m_currentPiece.X + b.X;
                    int y = m_currentPiece.Y + b.Y;
                    Texture2D block = m_darkBlueBlock;// (b.X == 0 && b.Y == 0) ? m_redBlock : m_darkBlueBlock;
                    spriteBatch.Draw(block,
                                     new Rectangle(100 + BLOCK_SIZE*x + 1,
                                                   50 + BLOCK_SIZE*y + 1,
                                                   BLOCK_SIZE - 1, BLOCK_SIZE - 1), Color.White);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}