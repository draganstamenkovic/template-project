using UnityEngine;
using VContainer;

namespace Registrators
{
    public class ConfigRegistrator
    {
        public static void Register(IContainerBuilder builder)
        {
            var configs = Resources.LoadAll<ScriptableObject>("Configs/");
            foreach (var config in configs)
            {
                builder.RegisterInstance(config)
                    .As(config.GetType())
                    .AsImplementedInterfaces();
            }
        }
    }
}