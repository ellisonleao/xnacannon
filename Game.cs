using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XnaCannon
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice device;

        Texture2D background;
        Texture2D foreground;
        Texture2D cannon;
        Texture2D cannonBall;
        Texture2D score;
        Texture2D smokeTexture;
        Texture2D menu;
        SpriteFont font,player,hit;
        CannonBall ball;
        Smoke smoke;
        Vector2 gravity;
        Vector2 wind;

        int screenHeight;
        int screenWidth;
        int currentPlayer;
        Cannon[] cannons;
        bool cannonBallFlying;
        bool collided;
        bool gameOver;
        bool showMenu;
        bool battleSongStart;
        int winnerPos;
        int pos;
        Random rand;
 
        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            AudioManager.Initialize(this);
        }


        protected override void Initialize()
        {

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "XnaCannon";
            device = graphics.GraphicsDevice;
            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;
            wind = new Vector2(5.0f, 0);
            gravity = new Vector2(0, 10.0f);
            showMenu = true;
            
            
            base.Initialize();
        }


        protected void LoadCannons()
        {
            currentPlayer = 0;
            cannons = new Cannon[2]; 

            cannons[0] = new Cannon(cannon,new Vector2(cannon.Width + 10, screenHeight - 100));
            cannons[1] = new Cannon(cannon,new Vector2(screenWidth - cannon.Width - 10 , screenHeight - 100));
        }


        protected override void LoadContent()
        {
            
            //texturas
            #region texturas
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("images/background");
            foreground = Content.Load<Texture2D>("images/foreground");
            cannon = Content.Load<Texture2D>("images/cannon");
            cannonBall = Content.Load<Texture2D>("images/ball");
            score = Content.Load<Texture2D>("images/score");
            smokeTexture = Content.Load<Texture2D>("images/smoke");
            menu = Content.Load<Texture2D>("images/menu");
            font = Content.Load<SpriteFont>("fonts/font");
            player = Content.Load<SpriteFont>("fonts/player");
            hit = Content.Load<SpriteFont>("fonts/hit"); 
            #endregion

            //sons
            AudioManager.LoadSounds();
            

            LoadCannons();
            
        }


        protected override void UnloadContent()
        {
            
        }

        private void CheckGameOver(int pos)
        {
            if (cannons[pos].Health <= 0)
            {
                cannons[pos].IsAlive = false;
                winnerPos = currentPlayer;
                gameOver = true;
            }
            else
            { 
                //muda player
                currentPlayer = currentPlayer == 0 ? 1 : 0;
            }
        }


        private void UpdateWind()
        {
            rand = new Random();
            wind = new Vector2(rand.Next(-5, 10), rand.Next(0, 10));
        }
 
        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            KeyboardState keybState = Keyboard.GetState();
            cannons[currentPlayer].UpdateCannon(keybState);

            if (showMenu)
            {
                AudioManager.PlaySound("menu");
            }

            if (keybState.IsKeyDown(Keys.Enter))
            {
                showMenu = false;
                AudioManager.StopSound("menu");
                battleSongStart = true;
            }

            if (battleSongStart)
            {
                AudioManager.PlaySound("battle");
            }

            if (keybState.IsKeyDown(Keys.Space))
            {
                cannonBallFlying = true;
                ball = new CannonBall(cannonBall,
                        cannons[currentPlayer].getCannonOffset(),
                        cannons[currentPlayer].Power,
                        MathHelper.ToRadians(cannons[currentPlayer].Angle),
                        cannons[currentPlayer].BallMass,
                        wind,
                        gravity);
                smoke = new Smoke(ball.Position,smokeTexture);
                AudioManager.PlaySound("launch");
            }

            if (cannonBallFlying)
            {
                pos = currentPlayer;   
                if (currentPlayer == 0)
                {
                    collided = ball.verifyCollision(cannon, cannons[currentPlayer + 1].Position);
                    pos = 1; 
                }
                else
                {
                    collided = ball.verifyCollision(cannon, cannons[currentPlayer - 1].Position);
                    pos = 0;
                }

                //smoke trail
                smoke.Update(gameTime, ball.Position);
                
                
                //remove health
                if (collided)
                {
                    AudioManager.PlaySound("hitcannon");
                    cannons[pos].Health -= cannons[currentPlayer].GetDamage();
                    CheckGameOver(pos);
                    UpdateWind();
                    cannonBallFlying = false;
                }

                //checa colisao com terreno e bola fora do terreno
                if (ball.verifyCollision(foreground, new Vector2(0, screenHeight - 70)) || ball.verifyHomeRun(graphics))
                {
                    AudioManager.PlaySound("hitground");
                    UpdateWind();
                    cannonBallFlying = false;
                }

                //retira o total de bala
                if (!cannonBallFlying)
                {
                    if (cannons[currentPlayer].BallMass == 1)
                        cannons[currentPlayer].ballTypes["heavy"]--;
                    else if (cannons[currentPlayer].BallMass == 2)
                        cannons[currentPlayer].ballTypes["medium"]--;
                }
                    
                
                ball.Update(gameTime);
            }
                

            base.Update(gameTime);
        }


        private void DrawCannonBall()
        {
            if (cannonBallFlying)
            {
                ball.Draw(spriteBatch);
                smoke.Draw(spriteBatch);
            }
        }

        private void DrawCannons()
        {
            for (int i = 0; i < cannons.Length; i++)
            { 
                if (cannons[i].IsAlive)
                    cannons[i].Draw(spriteBatch);
            }
        }

        private void DrawWorld()
        {
            Rectangle screen = new Rectangle(0, 0, screenWidth, screenHeight);
            spriteBatch.Draw(background, screen, Color.White);
            spriteBatch.Draw(foreground, new Rectangle(0,screenHeight - 70,foreground.Width,foreground.Height), Color.White);
        }

        /// <summary>
        /// bllalasdasd
        /// </summary>
        /// <returns></returns>
        private string DrawWindDirection()
        {
            return wind.X > 0 ? "->" : "<-";
        }

        private void DrawForces()
        {
            spriteBatch.DrawString(player, "WIND  " + DrawWindDirection(), new Vector2(screenWidth / 2 - 50, 10), Color.AliceBlue);
            
            
            spriteBatch.DrawString(player, "UP + DOWN for Angle", new Vector2(screenWidth / 2 - 70, 60), Color.Brown);
            spriteBatch.DrawString(player, "SPACE - Shoot.", new Vector2(screenWidth / 2 - 70, 80), Color.Brown);
            spriteBatch.DrawString(player, "QWE - Choose Weapon", new Vector2(screenWidth / 2 - 70, 100), Color.Brown);

            //TODO Refatorar.

            #region gambi
            //player 1
            int remainingBalls1 = cannons[0].RemainingBalls();
            spriteBatch.Draw(score, new Rectangle(60, 60, score.Width, score.Height), Color.White);
            spriteBatch.DrawString(player, "Player : 1" , new Vector2(score.Width/2, score.Height/2 - 10), Color.Wheat);
            spriteBatch.DrawString(font, "Power :" , new Vector2(80, 120), Color.Wheat);
            spriteBatch.Draw(score, new Rectangle(140, 120,(int) cannons[0].Power, 20), Color.Blue);
            spriteBatch.DrawString(font, "Mass :" + cannons[0].BallMass.ToString(), new Vector2(80, 140), Color.Wheat);
            if (cannons[0].BallMass == 1 || cannons[0].BallMass == 2)
                spriteBatch.DrawString(font, "Left ( " + remainingBalls1.ToString() + " )", new Vector2(160, 140), Color.Wheat);
            spriteBatch.DrawString(font, "Health :", new Vector2(80, 160), Color.Wheat);
            spriteBatch.Draw(score, new Rectangle(140, 160, cannons[0].Health, 20), Color.Blue);

            //player 2
            int remainingBalls2 = cannons[1].RemainingBalls();
            spriteBatch.Draw(score, new Rectangle(screenWidth - score.Width - 60, 60, score.Width, score.Height), Color.White);
            spriteBatch.DrawString(player, "Player : 2", new Vector2(screenWidth - score.Width - 10, score.Height / 2 - 10), Color.Wheat);
            spriteBatch.DrawString(font, "Power :", new Vector2(screenWidth - score.Width - 30, 120), Color.Wheat);
            spriteBatch.Draw(score, new Rectangle(screenWidth - score.Width + 30, 120, (int)cannons[1].Power, 20), Color.Red);
            spriteBatch.DrawString(font, "Mass :" + cannons[1].BallMass.ToString(), new Vector2(screenWidth - score.Width - 30, 140), Color.Wheat);
            if (cannons[1].BallMass == 1 || cannons[1].BallMass == 2)
                spriteBatch.DrawString(font, "Left ( " + remainingBalls2.ToString() + " )", new Vector2(screenWidth - score.Width + 40, 140), Color.Wheat);
            spriteBatch.DrawString(font, "Health :" , new Vector2(screenWidth - score.Width - 30, 160), Color.Wheat);
            spriteBatch.Draw(score, new Rectangle(screenWidth - score.Width + 30, 160, (int)cannons[1].Health, 20), Color.Red);
            #endregion


        }

        private void DrawWinner()
        {
            Rectangle rect = new Rectangle(350, 200, 400, 200);
            spriteBatch.Draw(score, rect, Color.Green);
            spriteBatch.DrawString(hit, "GAME OVER", new Vector2(400,230), Color.Wheat);
            spriteBatch.DrawString(player, "Winner - Player " + (winnerPos + 1).ToString(), new Vector2(400,290), Color.White);
        }

        private void DrawMenu()
        {
            Rectangle screen = new Rectangle(0, 0, screenWidth, screenHeight);
            spriteBatch.Draw(menu, screen, Color.White);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();


            if (!gameOver)
            {
                if (!showMenu)
                {
                    DrawWorld();
                    DrawForces();
                    DrawCannons();
                    DrawCannonBall();
                }
                else
                {
                    DrawMenu();
                }


            }
            else
            {
                DrawWorld();
                DrawWinner();
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
