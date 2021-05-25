using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoGame.GameEngine.Configuration
{
    public abstract class AbstractFactory
    {
        public virtual ILudoProvider BuildLudoProvider() => new LudoProvider();
    }
}
