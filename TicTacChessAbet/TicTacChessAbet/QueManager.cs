using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace serialPortDot6Test
{
    //!IMPORTANT!
    // SerialPortLib nuget package is needed for this code
    internal class QueManager
    {
        SerialPort serialPort;
        List<string> log = new List<string>();
        bool isPickedUp = false;

        public QueManager(string _portName)
        {
            serialPort = new SerialPort();
            serialPort.PortName = _portName;
            serialPort.BaudRate = Convert.ToInt32(115200);
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            serialPort.Open();

            if (!serialPort.IsOpen)
            {
                try
                {
                    serialPort.Open();
                    Thread.Sleep(200);
                }
                catch
                {
                    MessageBox.Show("error cant open serialPort");
                }
            }
        }

        //this code handels the recieved the data from the arduino
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string data = sp.ReadExisting();
            log.Add(data);
        }

        //this is a mostly debugging method used for sending costum messages 
        public void SendCostum(string _messege)
        {
            log.Add(_messege);
            WriteArduino(_messege);
        }

        //this code will return the machine to its resting position
        public void ReturnToStartPos()
        {
            WriteArduino("HS:0");
            Thread.Sleep(1000);
            while (true)
            {
                if (GetLastLines("HS:Ready"))
                {
                    break;
                }
                Thread.Sleep(100);
            }
            WriteArduino("RS:0");
            Thread.Sleep(1000);
            while (true)
            {
                if (GetLastLines("RS:Ready"))
                {
                    break;
                }
                Thread.Sleep(100);
            }
        }

        //this code will ready the machine by zeroing and positioning the machine
        public void Ready()
        {
            WriteArduino("ZS:0");

            while (true)
            {
                if (GetLastLines("Ready-LT"))
                {
                    break;
                }
                Thread.Sleep(100);
            }

            log.Add("moving to");

            WriteArduino("VS:950");
            while (true)
            {
                if (GetLastLines("VS:Ready"))
                {
                    break;
                }
                Thread.Sleep(100);
            }
        }

        //this will return the private log
        public List<string> returnLog()
        {
            return log;
        }

        //this method will move to horizontal position and rotate to
        //the rotation stated by the parameters
        public void MoveTo(int _hoz, int _rot)
        {
            WriteArduino("RS:" + _rot.ToString());
            Thread.Sleep(1000);
            while (true)
            {
                if (GetLastLines("RS:Ready"))
                {
                    break;
                }
                Thread.Sleep(100);
            }
            Thread.Sleep(1000);
            WriteArduino("HS:" + _hoz.ToString());
            while (true)
            {
                if (GetLastLines("HS:Ready"))
                {
                    break;
                }
                Thread.Sleep(100);
            }
        }

        //this method will pick or drop a piece based on the bool isPickedUp
        //if a piece is picked up then it will drop the piece
        //if it isnt then it will lower the arm and pick it up
        public void PickOrDrop()
        {
            if (!isPickedUp)
            {
                WriteArduino("VS:1150");
                Thread.Sleep(500);
                while (true)
                {
                    if (GetLastLines("VS:Ready"))
                    {
                        break;
                    }
                    Thread.Sleep(100);
                }
                WriteArduino("CS:1");
                Thread.Sleep(500);
                WriteArduino("SS:1");
                isPickedUp = true;
            }
            else
            {
                WriteArduino("CS:0");
                Thread.Sleep(500);
                WriteArduino("SS:0");
                isPickedUp = false;

                Thread.Sleep(2000);
                WriteArduino("VS:950");
                while (true)
                {
                    if (GetLastLines("VS:Ready"))
                    {
                        break;
                    }
                    Thread.Sleep(100);
                }
            }
        }

        //this function gets the last 3 lines from the log (if its long enough)
        //and checks if it has substring of the varable search 
        private bool GetLastLines(string search)
        {
            if (log.Count < 4)
            {
                return false;
            }
            string result = "";

            for (int i = log.Count - 3; i < log.Count; i++)
            {
                result += log[i];
            }
            if (result.Contains(search))
            {
                return true;
            }
            return false;
        }

        //this method will write information to the arduino through the serialPort
        private void WriteArduino(string _command)
        {
            int m_length = _command.Length;
            char[] m_data = new char[m_length];

            String m_carriageReturn = "\r";
            char[] m_cr = new char[2];

            String m_newLine = "\n";
            char[] m_nl = new char[2];

            for (int m_index = 0; m_index < m_length; m_index++)
            {
                m_data[m_index] = Convert.ToChar(_command[m_index]);
            }

            for (int m_index = 0; m_index < 1; m_index++)
            {
                m_cr[m_index] = Convert.ToChar(m_carriageReturn[m_index]);
            }

            for (int m_index = 0; m_index < 1; m_index++)
            {
                m_nl[m_index] = Convert.ToChar(m_newLine[m_index]);
            }

            if (serialPort.IsOpen == true)
            {
                serialPort.Write(m_data, 0, m_length);
            }
            else
            {
                Console.Error.WriteLine("no port selected");
            }
        }

        /*
        this is a list of all the commands the the arduino understands

        VS:0000 == Vertical
        HS:0000 == Horizontal
        RS:0000 == Rotation
        CS:0 == Compressor OFF / CS:1 Compressor ON
        SS:0 == Suction OFF / SS:1 Suction ON
        ZS:0 == Zero all
        ZS:1 == Zero Vertical
        ZS:2 == Zero Horizontal
        ZS:3 == Zero Rotation
        */

    }
}
