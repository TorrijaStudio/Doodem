using System;

namespace HelloWorld.formulas
{
    public struct Reward
    {
        private int A;
        private int B;
        private float C;

        public Reward(int a, int b, float c)
        {
            A = a;
            B = b;
            C = c;
        }

        public int GetReward(int numRondas)
        {
            return (int)(A + Math.Abs(B * Math.Pow(numRondas - 1,C)));
        }
    }
}