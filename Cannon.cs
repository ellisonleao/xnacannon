using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XnaCannon
{
    class Cannon
    {
        public Texture2D Sprite { get; set; }
        public Vector2 Position{ get; set;}
        public bool IsAlive{ get;set; }
        public float Angle { get; set; }
        public float Power{ get;set; }
        public int Health { get; set; }
        public Vector2 Offset { get; set; }
        public int BallMass { get; set; }
        public Dictionary<string, int> ballTypes { get; set;}

        public bool releaseCannonBall { get; set; }
        
        public Cannon(Texture2D Spri,Vector2 Pos) 
        {
            Sprite = Spri;
            Position = Pos;
            IsAlive = true;
            Health = 100;
            BallMass = 3;
            releaseCannonBall = false;
            ballTypes = new Dictionary<string, int>();
            ballTypes.Add("heavy", 3);
            ballTypes.Add("medium", 5);
        }

        public float getRotateAngle()
        {
            return MathHelper.ToRadians(Angle);
        }

        public Vector2 getCannonOffset()
        {
            float size = (Sprite.Width) - 20;
            Vector2 direction = new Vector2(((float)Math.Cos(getRotateAngle()) * size), (float)(Math.Sin(getRotateAngle()) * size));
            return Vector2.Add(Position, direction);
        }


        public int RemainingBalls()
        {
            int remaining = 0;
            switch (BallMass)
            {
                case 1:
                    remaining = ballTypes["heavy"];
                    break;
                case 2:
                    remaining = ballTypes["medium"];
                    break;
            }
            return remaining;
        }

        public int GetDamage()
        {
            int damage = 0;
            switch (BallMass)
            {
                case 1:
                    damage = 5;
                    break;
                case 2:
                    damage = 10;
                    break;
                case 3:
                    damage = 20;
                    break;
            }
            return damage;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 cannonOrigin = new Vector2(30, Sprite.Height / 2);
            spriteBatch.Draw(Sprite, Position, null, Color.White, getRotateAngle(), cannonOrigin, 1.0f, SpriteEffects.None, 0f);
        }


        public void UpdateCannon(KeyboardState keybState)
        {
            if (keybState.IsKeyDown(Keys.Left))
                Angle -= 1;
            if (keybState.IsKeyDown(Keys.Right))
                Angle += 1;

            Angle = MathHelper.Clamp(Angle, -180, 0);


            //TODO melhorar a logica do lancamento
            if (keybState.IsKeyDown(Keys.Space) && !releaseCannonBall)
            {
                releaseCannonBall = true; 
            }

            if (keybState.IsKeyDown(Keys.Space) && releaseCannonBall)
            {
                Power += 1;
                if (Power == 100)
                    Power = 0;
            }

            Power = MathHelper.Clamp(Power, 0, 100);

            
            //seleciona o tipo de bomba
            //Q pesada
            if (keybState.IsKeyDown(Keys.Q))
            {
                
                BallMass = ballTypes["heavy"] > 0 ? 1 : 3;
            }

            //W media
            if (keybState.IsKeyDown(Keys.W))
            {
                BallMass = ballTypes["medium"] > 0 ? 2 : 3;
            }

            //E pequena
            if (keybState.IsKeyDown(Keys.E))
            {
                BallMass = 3;
            }
        }
       
    }
}
