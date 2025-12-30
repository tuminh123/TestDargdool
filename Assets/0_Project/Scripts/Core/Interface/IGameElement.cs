
namespace Core
{
    public interface IGameElement
    {
        public bool HasSetup { get; }
        public void EnableSetup();
        public void DisableSetup();
        public void Enable();
        public void Disable();
    }
}
