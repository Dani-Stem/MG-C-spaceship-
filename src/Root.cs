using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace MonoGame
{
    class Root : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int winheight = 0;
        int winwidth = 0;

        // Background
        Rectangle recBg;
        Texture2D textureBg;

        //spaceship
        Rectangle recSS;
        Texture2D textureSS;
        int SSSpeed = 10;

        //asstriods
        Rectangle recAss;
        Texture2D textureAss;

        // Apples Aka Asteriods
        List<Apple> apples;
        Rectangle recApple;
        Texture2D textureApple;
        Random applernd = new Random();
        float Movespeed = 1.5f;
        int time = 0;
        int timeout = 1500;
        TimeSpan ptime, atime;

        //Bullets
        List<Bullet> bullets;
        Rectangle recBullet;
        Texture2D textureBullet;
        Random bulletrnd = new Random();
        float Shotspeed = 3f;
        //scoreboard
        int scoreBoard = 0;
        SpriteFont scoreFont;

        //gameplay
        int lives = 0;

        //gameover
        Rectangle recgameOver;
        Texture2D gameOver;

        //gamestart
        Rectangle recgameStart;
        Texture2D gameStart;

        // Sound Effects
        List<SoundEffect> soundEffects;
        // Keyboard Inputs
        KeyboardState key;

        // Scores
        //creat a list like soundefects but for scores
        List<Score> highScore = new List<Score>() ;

        //Test Code
        Random rnd = new Random(6556456);

        public Root()
        {
            graphics = new GraphicsDeviceManager(this);

            // Window
            graphics.PreferredBackBufferWidth = 1160;
            graphics.PreferredBackBufferHeight = 740;

            // Fullscreen Window (Max Resolution)
            //graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            // Fullscreen (Max Resolution)
            //graphics.IsFullScreen = true;

            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            soundEffects = new List<SoundEffect>();

            Log.Clear();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            recBg = new Rectangle(0,0,GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            recgameOver = new Rectangle(0,0,GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            recgameStart = new Rectangle(0,0,GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            recSS = new Rectangle(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height/2, 100,100);
            recApple = new Rectangle(applernd.Next(GraphicsDevice.Viewport.Width - 32), 0, 32,36);
            recBullet = new Rectangle(bulletrnd.Next(GraphicsDevice.Viewport.Width - 32),0, 32,36);
            winheight = GraphicsDevice.Viewport.Height;
            winwidth = GraphicsDevice.Viewport.Width;

            apples = new List<Apple>();
            bullets = new List<Bullet>();

            atime = TimeSpan.FromSeconds(1.5f);
            soundEffects.Add(Content.Load<SoundEffect>("basketdrop"));
            Log.Print("Terminal Active\n");
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textureBg = Content.Load<Texture2D>("space");
            textureApple = Content.Load<Texture2D>("ass");
            textureBullet = Content.Load<Texture2D>("bullet");
            scoreFont = Content.Load<SpriteFont>("scoreFont");
            textureSS = Content.Load<Texture2D>("ship");
            gameOver = Content.Load<Texture2D>("gameover");
            gameStart = Content.Load<Texture2D>("gamestart");


            highScore.Add(new Score() { PlayerName = "Gina" , PlayerScore = rnd.Next(100,999) });
            highScore.Add(new Score() { PlayerName = "Dani" , PlayerScore = rnd.Next(100,999) });
            highScore.Add(new Score() { PlayerName = "Xam" , PlayerScore = rnd.Next(100,999) });
            highScore.Add(new Score() { PlayerName = "Dani" , PlayerScore = rnd.Next(100,999) });

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();



            highScore.Sort();

            if(lives <= 0){

               if(Keyboard.GetState().IsKeyDown(Keys.Space)){
                   lives = 5;
                   scoreBoard = 0;
               }

            }else{

            key = Keyboard.GetState();
            // TODO: Add your update logic here
            Inputs();

            int x, y;
            Random ran = new Random();
            x = ran.Next(0, GraphicsDevice.Viewport.Width - 64);
            y = ran.Next(0, 0);

            if (gameTime.TotalGameTime - ptime > atime)
            {
                ptime = gameTime.TotalGameTime;
                AddApples(new Vector2(x, y));
                Movespeed++;
            }

            Updateapples();

            UpdateBullets();

            Collision();

            }

            base.Update(gameTime);
        }
        protected void Inputs(){
            if(key.IsKeyDown(Keys.Right)){
                recSS.X += SSSpeed;
            }

            if(key.IsKeyDown(Keys.Left)){
                recSS.X -= SSSpeed;
            }

            if(key.IsKeyDown(Keys.Up)){
                recSS.Y -= SSSpeed;
            }

            if(key.IsKeyDown(Keys.Down)){
                recSS.Y += SSSpeed;
            }

            if(recSS.Y < 0){
                recSS.Y =0;
            }

            if(key.IsKeyDown(Keys.Space)){

                AddBullets(new Vector2(recSS.X, recSS.Y));

            }

            if(recSS.Y > winheight - 100){
                recSS.Y = winheight - 100;
            }

            if(recSS.X < 0){
                recSS.X = 0;
            }

            if(recSS.X > winwidth - 100){
                recSS.X = winwidth - 100;
            }
        }

        protected void Collision()
        {

            for (int i = 0; i < bullets.Count; i++)
            {
                recBullet = new Rectangle
               ((int)bullets[i].Position.X - bullets[i].Width / 2, (int)bullets[i].Position.Y - bullets[i].Height / 2,
               bullets[i].Width, bullets[i].Height);


                for (int j = 0; j < apples.Count; j++)
                {
                    recApple = new Rectangle
               ((int)apples[j].Position.X - apples[j].Width / 2, (int)apples[j].Position.Y - apples[j].Height / 2,
               apples[j].Width, apples[j].Height);

                    if(recBullet.Intersects(recApple))
                    {
                        soundEffects[0].CreateInstance().Play();
                        bullets.RemoveAt(i);
                        apples.RemoveAt(j);
                        scoreBoard += 10;

                    }
                }
            }
            for (int i = 0; i < apples.Count; i++)
            {
                recApple = new Rectangle
               ((int)apples[i].Position.X - apples[i].Width / 2, (int)apples[i].Position.Y - apples[i].Height / 2,
               apples[i].Width, apples[i].Height);

                if(recApple.Intersects(recSS))
                {
                    soundEffects[0].CreateInstance().Play();
                    apples.RemoveAt(i);
                    recApple.Y = 0;
                    recApple.X = applernd.Next(GraphicsDevice.Viewport.Width - 100);

                    scoreBoard -= 10;
                    lives -=1;

                }

                if (recApple.Y > GraphicsDevice.Viewport.Height)
                {
                    apples.RemoveAt(i);
                    //applemissed += 1;
                    //applenum -= 1;
                    recApple.X = applernd.Next(GraphicsDevice.Viewport.Width);
                    recApple.Y = 0;
                }

                if (recBullet.Y < 0)
                {
                    bullets.RemoveAt(i);
                    //applemissed += 1;
                    //applenum -= 1;
                    recBullet.X = bulletrnd.Next(GraphicsDevice.Viewport.Width);
                    recBullet.Y = 0;
                }

            }
        }

        private void AddApples(Vector2 position)
        {
            Apple apple = new Apple();
            apple.Initialize(GraphicsDevice.Viewport, textureApple, position, Movespeed);
            apples.Add(apple);
        }

        private void AddBullets (Vector2 position)
        {
            Bullet bullet = new Bullet();
            bullet.Initialize(GraphicsDevice.Viewport, textureBullet, position, Shotspeed);
            bullets.Add(bullet);
        }

        private void RemoveApples()
        {

        }

        private void Updateapples()
        {
            for (int i = apples.Count - 1; i >= 0; i--)
            {
                apples[i].Update();

                if (apples[i].Active == false)
                {
                    apples.RemoveAt(i);
                }
            }

        }

        private void UpdateBullets()
        {
            for(int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update();

                if (bullets[i].Active == false)
                {
                    bullets.RemoveAt(i);
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LemonChiffon);

            if(lives <= 0){
                spriteBatch.Begin();
                spriteBatch.Draw(gameStart, recgameOver, Color.White);
                int scoreIncrease = 30;
                foreach (Score aScore in highScore)
                {
                    spriteBatch.DrawString(scoreFont, "High Scores:" + aScore.ToString(), new Vector2(10, scoreIncrease += 50), Color.Pink);

                }
                spriteBatch.End();
            }else{
                spriteBatch.Begin();
                spriteBatch.Draw(textureBg, recBg, Color.White);
                spriteBatch.Draw(textureSS, recSS, Color.White);
                spriteBatch.DrawString(scoreFont, "Your Score:" + scoreBoard.ToString(), new Vector2(10,10), Color.White);
                spriteBatch.DrawString(scoreFont,"LIVES: " + lives.ToString(), new Vector2(10, 30), Color.White);
                for (int i = 0; i < apples.Count; i++)
                {
                    apples[i].Draw(spriteBatch);
                }
                for (int i = 0; i < bullets.Count; i++)
                {
                    bullets[i].Draw(spriteBatch);
                }

                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
