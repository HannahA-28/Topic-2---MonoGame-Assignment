using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Topic_2___MonoGame_Assignment
{
    enum Screen
    {
        Title,
        Field
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        Screen screen;
        KeyboardState keyboardState;
        Random generator;
        Rectangle window;
        Texture2D titleTexture;
        Texture2D sportsBackgroundTexture;
        List<Texture2D> textures;
        List<Rectangle> ballRects;
        List<Texture2D> ballTextures;
        float seconds;
        float respawnTime;
        MouseState mouseState;
        SpriteFont spriteFont;
        string message;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            window = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            generator = new Random();
            textures = new List<Texture2D>();
            ballRects = new List<Rectangle>();

            for (int i = 0; i < 30; i++)
            {
                ballRects.Add
                    (
                        new Rectangle(generator.Next(window.Width - 25),
                        generator.Next(window.Height - 25), 25, 25)
                    );
            }

            ballTextures = new List<Texture2D>();

            seconds = 0f;
            respawnTime = 3f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            titleTexture = Content.Load<Texture2D>("titleScreen");
            sportsBackgroundTexture = Content.Load<Texture2D>("Images/sportsBackground");
            for (int i = 1; i <= 10; i++)
                textures.Add(Content.Load<Texture2D>("Images/ball" + i));

            for (int i = 0; i < ballRects.Count; i++)
                ballTextures.Add(textures[generator.Next(textures.Count)]);

            spriteFont = Content.Load<SpriteFont>("spriteFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            keyboardState = Keyboard.GetState();

            if (screen == Screen.Title)
            {
                message = "Click ENTER for the next screen.";
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    screen = Screen.Field;
                }
            }

            mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                for (int i = 0; i < ballRects.Count; i++)
                {
                    if (ballRects[i].Contains(mouseState.Position))
                    {
                        ballRects.RemoveAt(i);
                        ballTextures.RemoveAt(i);
                    }
                }
            }

            seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (seconds > respawnTime)
            {
                ballRects.Add
                    (
                        new Rectangle(generator.Next(window.Width - 25),
                        generator.Next(window.Height - 25), 25, 25)
                    );
                ballTextures.Add(textures[generator.Next(textures.Count)]);

                seconds = 0f;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            
            if (screen == Screen.Title)
            {
                _spriteBatch.Draw(titleTexture, window, Color.White);
                _spriteBatch.DrawString(spriteFont, "Hannah Anderson", new Vector2(250, 200), Color.Blue);
                _spriteBatch.DrawString(spriteFont, message, new Vector2(10, 10), Color.Black);
                _spriteBatch.DrawString(spriteFont, "Click the left mouse button", new Vector2(10, 50), Color.Black);
                _spriteBatch.DrawString(spriteFont, "to get rid of the balls!", new Vector2(10, 90), Color.Black);
            }
            else if (screen == Screen.Field)
            {
                _spriteBatch.Draw(sportsBackgroundTexture, window, Color.White);
                _spriteBatch.Draw(sportsBackgroundTexture, window, Color.White);
                     for (int i = 0; i < ballRects.Count; i++)
                _spriteBatch.Draw(ballTextures[i], ballRects[i], Color.White);
            }



            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
