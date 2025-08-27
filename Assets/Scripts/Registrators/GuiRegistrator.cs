using VContainer;

namespace Registrators
{
    public class GuiRegistrator
    {
        public static void Register(IContainerBuilder builder)
        {
            ScreenControllersRegistrator.Register(builder);
        }
    }
}