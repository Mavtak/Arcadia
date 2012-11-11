
namespace SomewhatGeeky.Arcadia.Engine
{
    public class IdGenerator
    {
        private int lastIdUsed;

        public IdGenerator()
        {
            lastIdUsed = 0;
        }

        public string NextString()
        {
            lock (this)
            {
                return (++lastIdUsed).ToString();
            }
        }
    }
}
