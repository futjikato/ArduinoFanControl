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
                resp.answerBytes = new byte[] { 13, 12, 11, 10, 9, 8, 7, 6, 0 };
            }
            else
            {
                resp.answerBytes = new byte[] { 1, 0 };
            }
            responseCallback(resp);
        }
    }
}
