using Registrators;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        AudioRegistrator.Register(builder);
        CamerasRegistrator.Register(builder);
        ConfigRegistrator.Register(builder);
        DataRegistrator.Register(builder);
        GameplayRegistrator.Register(builder);
        GuiRegistrator.Register(builder);
        HelpersRegistrator.Register(builder);
        InputsRegistrator.Register(builder);

        builder.RegisterEntryPoint<Bootstrap>();
    }
}
