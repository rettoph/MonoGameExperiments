using Autofac;
using Guppy.Attributes;
using Guppy.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslucentInstancedPrimitives.Loaders
{
    [AutoLoad]
    internal sealed class MainLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            // throw new NotImplementedException();
        }
    }
}
