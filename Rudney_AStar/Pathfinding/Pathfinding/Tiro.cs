using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pathfinding
{
    class Tiro
    {
        Vector3 menorDistancia, posicaoDaTorre;
        double cat1, cat2, cat1Norm, cat2Norm, distance;
        float tempoDeTiro, distanciaDoTiro;
        int dano, timerInt;
        float timer = 0;

        //Trocar depois pela própria torre
        public Tiro(Vector3 posicaoDaTorre, float tempoDeTiro, float distanciaDoTiro, int dano)
        {
            this.posicaoDaTorre = posicaoDaTorre;
            this.tempoDeTiro = tempoDeTiro;
            this.distanciaDoTiro = distanciaDoTiro;
            this.dano = dano;
        }

        public void update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            timerInt = (int)timer;

            if (timerInt % tempoDeTiro == 0)
            {
                timer = 0;
                atirar();
            }
        }

        public void atirar()
        {
            Character enemy = inimigoMaisProximo();

            if (distance <= distanciaDoTiro)
                enemy.setLife(dano);
        }

        private Character inimigoMaisProximo()
        {
            Character enemy = null;
            this.distance = 0;

            foreach (Character c in Game1.enemys)
            {
                //Primeira passada
                if (enemy == null)
                {
                    enemy = c;
                    continue;
                }

                double d = calcularDistancia(c);

                //Primeira passada
                if (this.distance == 0)
                    this.distance = d;

                // Se a distancia atual for menor, o novo character está mais próximo da torre;
                else if (this.distance > d)
                {
                    this.distance = d;
                    enemy = c;
                }
            }

            return enemy;
        }


        private double calcularDistancia(Character enemy)
        {
            cat1 = enemy.getPosition().X - posicaoDaTorre.X;
            cat2 = enemy.getPosition().Z - posicaoDaTorre.Z;

            double hip = (cat1 * cat1) + (cat2 * cat2);
            hip = Math.Sqrt(hip);

            return hip;
        }
    }
}
