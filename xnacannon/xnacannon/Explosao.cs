using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Asteroids

{
    class Explosao
    {
        // Animation representing the player
        public Animation ExplosaoAnimation = new Animation();

        // Position of the Player relative to the upper left side of the screen
        public Vector2 Position;

        //Textura
        private Texture2D ExplosaoTexture;

        // State
        public bool Active;


        // Initialize the player
        public void Initialize(ContentManager content, Vector2 position)
        {
            ExplosaoTexture = content.Load<Texture2D>("explosao");
            ExplosaoAnimation.Initialize(ExplosaoTexture, 4, 3, 50, 11, 1.0f, false);


            // Set the starting position of the player around the middle of the screen and to the back
            Position = position;

            // Set to be active
            Active = false;
        }


        // Update the player animation
        public void Update(GameTime gameTime)
        {
            if (Active)
            {
                ExplosaoAnimation.Position = Position;
                ExplosaoAnimation.Update(gameTime);
                if (!ExplosaoAnimation.Active)
                {
                    Active = false;
                    ExplosaoAnimation.Initialize(ExplosaoTexture, 4, 3, 50, 11, 1.0f, false);
                }
            }
  
        }

        // Draw the player
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active) ExplosaoAnimation.Draw(spriteBatch);
        }
    }
}
