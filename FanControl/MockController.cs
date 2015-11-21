using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    class MockController : ControllerInterface
    {
        public void Request(byte requestByte, ResponseCallback responseCallback)
        {
            Request(requestByte, responseCallback, 0, 0);
        }

        public void Request(byte requestByte, ResponseCallback responseCallback, byte fan, byte speed)
        {
            Response resp = new Response();

            if (requestByte == 1)
            {
                resp.answerBytes = new byte[] { 3, 5, 6, 9, 10, 11, 0 };
            }
            else
            {
                resp.answerBytes = new byte[] { 1, 0 };
            }
            responseCallback(resp);
        }
    }
}
