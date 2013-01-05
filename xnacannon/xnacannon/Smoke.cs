using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaCannon
{
    class Smoke
    {
        Texture2D smokeTexture;
        List<Vector2> smokeList;
        Random randomizer;
        Vector2 smokePos;

        public Smoke(Vector2 ballPos,Texture2D smoke)
        {
            smokeList = new List<Vector2>();
            smokePos = Vector2.Zero;
            smokeTexture = smoke;
            randomizer = new Random();
        }


        public void Update(GameTime gametime,Vector2 ballPos)
        {
            Vector2 smokePos = ballPos;
            for (int i = 0; i < 5; i++)
            {
                Vector2 vecAux = new Vector2(randomizer.Next(20) - 10, randomizer.Next(20) - 10);
                smokePos = Vector2.Add(smokePos, vecAux);
                smokeList.Add(smokePos);
            }
        }

        public void Draw(SpriteBatch render)
        {
            foreach (Vector2 smokePos in smokeList)
                render.Draw(smokeTexture, smokePos, null, Color.White, 0, new Vector2(40, 35), 0.2f, SpriteEffects.None, 1);
        }
    }
}
