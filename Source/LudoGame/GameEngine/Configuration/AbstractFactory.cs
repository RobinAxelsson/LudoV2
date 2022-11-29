namespace LudoGame.GameEngine.Configuration
{
    public abstract class AbstractFactory
    {
        public virtual ILudoProvider BuildLudoProvider() => new LudoProvider();
    }
}
