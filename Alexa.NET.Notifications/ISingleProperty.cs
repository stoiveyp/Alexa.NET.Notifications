using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.Notifications
{
    internal interface ISingleProperty<T>
    {
        T SpecifiedProperty { get; set; }
    }
}
