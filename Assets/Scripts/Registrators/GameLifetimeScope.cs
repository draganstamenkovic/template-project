using Registrators;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        ConfigRegistrator.Register(builder);
        GuiRegistrator.Register(builder);

        builder.RegisterEntryPoint<Bootstrap>();
    }
}
