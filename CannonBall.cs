using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XnaCannon
{
    class CannonBall
    {

        
        public Vector2 Position {get;set;}
        public float Angle { get; set; }
        public float Mass { get; set; }
        public float Power { get; set; }
       

        Texture2D SpriteBall { get; set; }
        Vector2 Momentum;
        Vector2 Gravity;
        Vector2 Wind;
        
        
        
        public CannonBall(Texture2D sprite,Vector2 pos, float force, float ang, float mass, Vector2 wind,Vector2 grav)
        {
            SpriteBall = sprite;
            Position = pos;
            Angle = ang;
            Mass = mass;
            Wind = wind;
            Gravity = grav;
            Momentum =  new Vector2(((float)Math.Cos(ang) * force), (float)(Math.Sin(ang) * force));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 posAux = new Vector2(Position.X - SpriteBall.Width / 2, Position.Y - SpriteBall.Height / 2);
            spriteBatch.Draw(SpriteBall, posAux,Color.White);
        }


        public void Update(GameTime gameTime)
        {
            float secs = (float)gameTime.ElapsedGameTime.TotalSeconds * 9;
            Vector2 accelSecs = (Wind / Mass + Gravity) * secs;
            Position += (Momentum + accelSecs) * secs;
            Momentum += accelSecs;
        }

        public Boolean verifyCollision(Texture2D sprite,Vector2 spritePos)
        {
            Rectangle targetRectangle = new Rectangle((int)spritePos.X, (int)spritePos.Y, sprite.Width, sprite.Height);
            Rectangle cannonBallRectangle = new Rectangle((int)Position.X, (int)Position.Y, SpriteBall.Width, SpriteBall.Height);
            if (cannonBallRectangle.Intersects(targetRectangle))
                return true;
            else
                return false;
        }

        //saiu da tela :)
        public Boolean verifyHomeRun(GraphicsDeviceManager graphics)
        {
            int minX = 0;
            int maxX = graphics.PreferredBackBufferWidth;
            int minY = 0;
            int maxY = graphics.PreferredBackBufferHeight;

            if (Position.X > maxX || Position.X < minX || Position.Y > maxY || Position.Y < minY)
                return true;
            else
                return false;
        }


    }
}
