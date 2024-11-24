namespace formulas
{
    public struct PieceTotem
    {
        private int head;
        private int body;
        private int feet;

        public PieceTotem(int h, int b, int f)
        {
            head = h;
            body = b;
            feet = f;
        }

        public int GetHead()
        {
            return head;
        }
        public int GetBody()
        {
            return body;
        }
        public int GetFeet()
        {
            return feet;
        }
    }
}