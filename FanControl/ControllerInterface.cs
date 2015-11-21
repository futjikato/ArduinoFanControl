using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public delegate void ResponseCallback(Response resp);

    public interface ControllerInterface
    {
        void Request(byte requestByte, ResponseCallback responseCallback);
        void Request(byte requestByte, ResponseCallback responseCallback, byte fan, byte speed);
    }
}
