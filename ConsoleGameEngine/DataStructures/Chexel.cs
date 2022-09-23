namespace ConsoleGameEngine.DataStructures
{
    public struct Chexel
    {
        public char character;
        public Vec3 foreground;
        public Vec3 background;

        public Chexel(char character, Vec3 foreground, Vec3 background)
        {
            this.character = character;
            this.foreground = foreground;
            this.background = background;
        }
    }

}
