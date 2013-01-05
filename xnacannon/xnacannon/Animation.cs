// Animation.cs
//Using declarations
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids

{
    public class Animation
    {
        // The image representing the collection of images used for animation
        Texture2D spriteStrip;

        // The scale used to display the sprite strip
        public float scale;

        // The time since we last updated the frame
        int elapsedTime;

        // The time we display a frame until the next one
        int frameTime;

        // The number of frames that the animation contains
        int frameCountT;

        // The number of frames that the animation contains in X
        int frameCountX;

        // The number of frames that the animation contains in Y
        int frameCountY;

        // The index of the current frame we are displaying
        int currentFrame;

        // The index of the current frame we are displaying
        int currentFrameX;

        // The index of the current frame we are displaying
        int currentFrameY;

        // The color of the frame we will be displaying
        public Color color;

        // The area of the image strip we want to display
        Rectangle sourceRect = new Rectangle();

        // Width of a given frame
        public int FrameWidth;

        // Height of a given frame
        public int FrameHeight;

        // The state of the Animation
        public bool Active;

        // Determines if the animation will keep playing or deactivate after one run
        public bool Looping;

        // Width of a given frame
        public Vector2 Position;

        // Origem
        public Vector2 Origin;

        // Angulo do Sprite
        public float angle;

        public void Initialize(Texture2D texture,
        int frameCountX, int frameCountY,
        int frametime,int frameCountT, float scale, bool looping)
        {
            // Keep a local copy of the values passed in
            this.color = Color.White;
            this.FrameWidth = texture.Width / frameCountX;
            this.FrameHeight = texture.Height / frameCountY;
            this.frameCountX = frameCountX;
            this.frameCountY = frameCountY;
            this.frameTime = frametime;
            this.frameCountT = frameCountT;
            this.scale = scale;

            Looping = looping;
            Position = Vector2.Zero;
            spriteStrip = texture;

            // Set the time to zero
            elapsedTime = 0;
            currentFrame = 0;

            // Set the Animation to active by default
            Active = true;
        }


        public void Update(GameTime gameTime)
        {
            // Do not update the game if we are not active
            if (Active == false)
                return;

            // Update the elapsed time
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            // If the elapsed time is larger than the frame time
            // we need to switch frames
            if (elapsedTime > frameTime)
            {
                // Move to the next frame
                currentFrame++; 
                currentFrameX++;

                if (currentFrameX == frameCountX) 
                { 
                    currentFrameY++;
                    currentFrameX = 0;
                    if (currentFrameY == frameCountY) {currentFrameY = 0; }
                }
                

                // If the currentFrame is equal to frameCount reset currentFrame to zero
                if (currentFrame == frameCountT)
                {
                    currentFrame = 0;
                    currentFrameY = 0;
                    currentFrameX = 0;

                    // If we are not looping deactivate the animation
                    if (Looping == false)
                        Active = false;
                }

                // Reset the elapsed time to zero
                elapsedTime = 0;
            }


            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            sourceRect = new Rectangle(currentFrameX * FrameWidth, currentFrameY * FrameHeight, FrameWidth, FrameHeight);

            
            // Origem do Sprite
            Origin = new Vector2(FrameWidth / 2, FrameHeight / 2);

        }


        // Draw the Animation Strip
        public void Draw(SpriteBatch spriteBatch)
        {
            // Only draw the animation when we are active
            if (Active)
            {
                //spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color);
                spriteBatch.Draw(spriteStrip, Position, sourceRect, color, angle, Origin, scale,SpriteEffects.None ,0 );

            }
        }
    }
}
