using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MORT
{
    interface ITransform
    {
        void ForceTransparency();
        void DoUpdate(bool isTranslating);
    }
}
