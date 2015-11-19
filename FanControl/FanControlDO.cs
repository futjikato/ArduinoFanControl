using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    class FanControlDO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private SerialController serialCntrl;

        private Dictionary<int, byte> pinMap;

        private Dictionary<int, byte> fanSpeed;
        
        public Nullable<byte> Pin1
        {
            get { return GetPinOutput(1); }
            set { if(value.HasValue) setPinByte(1, value.Value); }
        }

        public byte Speed1
        {
            get { return GetPinSpeed(1); }
            set { setPinSpeed(1, value, true); }
        }

        public Nullable<byte> Pin2
        {
            get { return GetPinOutput(2); }
            set { if (value.HasValue) setPinByte(2, value.Value); }
        }

        public byte Speed2
        {
            get { return GetPinSpeed(2); }
            set { setPinSpeed(2, value, true); }
        }

        public Nullable<byte> Pin3
        {
            get { return GetPinOutput(3); }
            set { if (value.HasValue) setPinByte(3, value.Value); }
        }

        public byte Speed3
        {
            get { return GetPinSpeed(3); }
            set { setPinSpeed(3, value, true); }
        }

        public Nullable<byte> Pin4
        {
            get { return GetPinOutput(4); }
            set { if (value.HasValue) setPinByte(4, value.Value); }
        }

        public byte Speed4
        {
            get { return GetPinSpeed(4); }
            set { setPinSpeed(4, value, true); }
        }

        public Nullable<byte> Pin5
        {
            get { return GetPinOutput(5); }
            set { if (value.HasValue) setPinByte(5, value.Value); }
        }

        public byte Speed5
        {
            get { return GetPinSpeed(5); }
            set { setPinSpeed(5, value, true); }
        }

        public Nullable<byte> Pin6
        {
            get { return GetPinOutput(6); }
            set { if (value.HasValue) setPinByte(6, value.Value); }
        }

        public byte Speed6
        {
            get { return GetPinSpeed(6); }
            set { setPinSpeed(6, value, true); }
        }

        public Nullable<byte> Pin7
        {
            get { return GetPinOutput(7); }
            set { if (value.HasValue) setPinByte(7, value.Value); }
        }

        public byte Speed7
        {
            get { return GetPinSpeed(7); }
            set { setPinSpeed(7, value, true); }
        }

        public Nullable<byte> Pin8
        {
            get { return GetPinOutput(8); }
            set { if (value.HasValue) setPinByte(8, value.Value); }
        }

        public byte Speed8
        {
            get { return GetPinSpeed(8); }
            set { setPinSpeed(8, value, true); }
        }

        public FanControlDO(SerialController cntrl)
        {
            serialCntrl = cntrl;
            pinMap = new Dictionary<int, byte>();
            fanSpeed = new Dictionary<int, byte>();
        }

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public Nullable<byte> GetPinOutput(int fan)
        {
            byte output;
            if (pinMap.TryGetValue(fan, out output))
            {
                return output;
            }
            return null;
        }

        private byte GetPinSpeed(int fan)
        {
            byte output;
            if (fanSpeed.TryGetValue(fan, out output))
            {
                return output;
            }
            return 0;
        }

        private void updateFanSpeed(byte fan, byte speed)
        {
            SerialController.ResponseCallback fanCallback = SpeedChangeCallback;
            serialCntrl.Request(2, fanCallback, fan, speed);
        }

        public void setPinByte(int fan, byte pin)
        {
            pinMap.Add(fan, pin);
            OnPropertyChanged(string.Format("Pin{0}", fan));
        }

        public void setPinSpeed(int fan, byte speed, bool andUpdate)
        {
            if (andUpdate)
            {
                byte fanPin;
                if (!pinMap.TryGetValue(fan, out fanPin))
                {
                    return;
                }
                try
                {
                    updateFanSpeed(fanPin, speed);
                }
                catch (InvalidOperationException)
                {
                    // only update value if change was at least send
                    return;
                }
            }

            if (fanSpeed.ContainsKey(fan))
            {
                fanSpeed.Remove(fan);
            }
            fanSpeed.Add(fan, speed);
            OnPropertyChanged(string.Format("Speed{0}", fan));
        }

        public void SpeedChangeCallback(SerialController.Response resp)
        {
            Trace.WriteLine("Speed change send.");
        }
    }
}
