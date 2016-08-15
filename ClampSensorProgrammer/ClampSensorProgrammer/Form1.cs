using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO.Ports;

namespace ClampSensorProgrammer
{
<<<<<<< HEAD
    //one more test
=======
    // test nr3
    //test comment
>>>>>>> origin/master
    public partial class Form1 : Form
    {
        private enum nqCommandType
        {
            get = 0x01,
            set = 0x02,
            response = 0x80
        };
        private enum nqCommands : byte
        {
            readFlash = 0x01,
            writeFlash = 0x02,
            erasePage = 0x03,
            eraseChip = 0x04,

            zWaveInitProgMode = 0x05,
            zWaveVerifyCrc = 0x06,
            zWaveSetLockBits = 0x07,
            zWaveFactoryReset = 0x08,

            reboot = 0x1D,
            factoryReset = 0x1E,

            deviceInfo = 0x20,
// fgslef,gh
            config = 0x21,

            debug = 0xFD,
            hwCheck = 0xFE
        };
        private enum nqCommandStatus : byte
        {
            failed = 0x00,
            success = 0x01,
            invalidValue = 0x02,
            flashNotErased = 0x03,
            unknown = 0xFF
        };

        private string pathToProgrammer, pathToWorkingDirectory;
        private const string firmwareUrl = "";
        private const string serialNumberUrl = "";
        private long serialNumber;

        private SerialPort serialPort;
        private Thread firmwareProgrammerThread;
        private List<string> firmware;
        private int snUpperIdx, snLoweridx;
        private long lastDownloadTime;

        private string bootloaderVersion = "";
        private string applicationVersion = "";
        private long clamp1RmsCurrent, clamp2RmsCurrent, clamp3RmsCurrent;
        private const bool DEBUG = false;

        public Form1()
        {
            InitializeComponent();

            string[] commPorts = SerialPort.GetPortNames();
            if (0 == commPorts.Length)
            {
                commPorts = new string[1];
                commPorts[0] = "None";
                comboComPorts.Items.Add("None");
            }
            else
            {
                List<string> l1 = new List<string>();
                List<string> l2 = new List<string>();
                for (int i = 0; i < commPorts.Length; i++)
                {
                    if (commPorts[i].Length == 4)
                    {
                        l1.Add(commPorts[i]);
                    }
                    else
                    {
                        l2.Add(commPorts[i]);
                    }
                }
              //  l1.Sort();
                l2.Sort();
                comboComPorts.Items.AddRange(l1.ToArray());
                comboComPorts.Items.AddRange(l2.ToArray());
            }
            comboComPorts.SelectedIndex = 0;

            //OpenFileDialog mk3exe = new OpenFileDialog();
            // DialogResult result = mk3exe.ShowDialog();
            // check if MPLab exist on the machine
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432")))
            {

                pathToProgrammer = @"C:\Program Files (x86)\Microchip\MPLABX\v3.35\mplab_ipe\pk3cmd.exe";
                pathToWorkingDirectory = @"C:\Program Files (x86)\Microchip\MPLABX\v3.35\mplab_ipe\";
            }
            else
            {
                pathToProgrammer = @"C:\Program Files (x86)\Microchip\MPLABX\v3.35\mplab_ipe\pk3cmd.exe";
                pathToWorkingDirectory = @"C:\Program Files (x86)\Microchip\MPLABX\v3.35\mplab_ipe\";
            }
            if (File.Exists(pathToProgrammer) == false)
            {
                MessageBox.Show("MPLAB IDE is not installed", "Error");
                btnProgram.Enabled = false;
                updateProgramStatus("MPLAB IDE is required to continue. Install it and restart the program.");
                txtSerialNumber.Enabled = false;
                return;
            }
            /*            // download the firmware
                        string request = "/clamp/nqclamp1_get_firmware";
                        DigestAuthFixer digest = new DigestAuthFixer("https://production.homemanager.tv", "HnCLMHwrjLrb6vZT", "tdFrZlEo2HnBv0Ua");
                        byte[] message;
                        message = digest.GrabBytesResponse(request);
                        TimeSpan span = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
                        lastDownloadTime = (long)span.TotalSeconds;
                        */
            /*
                        string tempFirmware = "";
                        byte[] message;
                        firmware = new List<string>();
                        tempFirmware = File.ReadAllText("firmware_29.01.2016_14.51.07.hex");

                        for (int i = 0; i < tempFirmware.Length; i++)
                        {
                            tempFirmware += (char)message[i];
                        }
                        // split the firmware and save the index of the serial number rows
                        bool done = false;
                        int pos = -1;
                        int count = 0;
                        snUpperIdx = -1;
                        snLoweridx = -1;
                        while (!done)
                        {
                            pos = tempFirmware.IndexOf('\n');
                            if (pos == -1)
                            {
                                done = true;
                            }
                            else
                            {
                                string line;
                                if (tempFirmware[pos - 1] == '\r')
                                {
                                    line = tempFirmware.Substring(0, pos - 1);
                                }
                                else
                                {
                                    line = tempFirmware.Substring(0, pos);
                                }
                                firmware.Add(line);
                                long address = conversions.stringToUint16(line.Substring(3, 4));
                                if (address == 0x9FE8)
                                {
                                    snUpperIdx = count;
                                }
                                if (address == 0x9FEC)
                                {
                                    snLoweridx = count;
                                }
                                count++;
                                tempFirmware = tempFirmware.Substring(pos + 1);
                            }
                        }
                        if (snUpperIdx == -1 || snLoweridx == -1)
                        {
                            txtProgramStatus.AppendText("Firmware file corrupt. Contact NorthQ");
                            return;
                        }
                        if (!Directory.Exists("tempLoad"))
                        {
                            DirectoryInfo di = Directory.CreateDirectory("tempLoad");
                            di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                        }
                        btnProgram.Enabled = true;
                    }
                    */
        }
        /**************************************************************************************/
        //
        /**************************************************************************************/
        public void appendProgramStatus(string message)
        {
            if (InvokeRequired)
            {
                try
                {
                    this.Invoke(new Action<string>(appendProgramStatus), new object[] { message });
                }
                catch (Exception e)
                {

                }
                return;
            }
            if (DEBUG)
            {
                txtProgramStatus.AppendText(message + '\n');
            }
        }
        /**************************************************************************************/
        //
        /**************************************************************************************/
        public void updateProgramStatus(string message)
        {
            if (InvokeRequired)
            {
                try
                {
                    this.Invoke(new Action<string>(updateProgramStatus), new object[] { message });
                }
                catch (Exception e)
                {

                }
                return;
            }
            txtProgramStatus.AppendText(message + '\n');
        }
        /**************************************************************************************/
        //
        /**************************************************************************************/
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult formClose = MessageBox.Show("         Do you really want to exit?", "Application Exit", MessageBoxButtons.YesNo);
            if (formClose == DialogResult.Yes)
            {
                try
                {
                    //worker.Abort();
                }
                catch { }
            }
            else
            {
                e.Cancel = true;
            }
        }
        /**************************************************************************************/
        //
        /**************************************************************************************/
        private void toggleButton(bool value)
        {
            if (InvokeRequired)
            {
                try
                {
                    this.Invoke(new Action<bool>(toggleButton), new object[] { value });
                }
                catch (Exception e)
                {

                }
                return;
            }
            btnProgram.Enabled = value;
        }
        /**************************************************************************************/
        //
        /**************************************************************************************/
        private void updateProgress(string value)
        {
            if (InvokeRequired)
            {
                try
                {
                    this.Invoke(new Action<string>(updateProgress), new object[] { value });
                }
                catch (Exception e)
                {

                }
                return;
            }
            if (value.ToUpper().Equals("BUSY"))
            {
                txtProgress.Text = value;
                txtProgress.BackColor = Color.Yellow;
            }
            if (value.ToUpper().Equals("SUCCESS"))
            {
                txtProgress.Text = value;
                txtProgress.BackColor = Color.Green;
            }
            if (value.ToUpper().Equals("FAILED"))
            {
                txtProgress.Text = value;
                txtProgress.BackColor = Color.Red;
            }
        }
        /**************************************************************************************/
        //
        /**************************************************************************************/
        private void toggleTxtSerialNumber(bool value)
        {
            if (InvokeRequired)
            {
                try
                {
                    this.Invoke(new Action<bool>(toggleTxtSerialNumber), new object[] { value });
                }
                catch (Exception e)
                {

                }
                return;
            }
            txtSerialNumber.Enabled = value;
        }
        /**************************************************************************************/
        //
        /**************************************************************************************/
        private void updateSerialNumber(string value)
        {
            if (InvokeRequired)
            {
                try
                {
                    this.Invoke(new Action<string>(updateSerialNumber), new object[] { value });
                }
                catch (Exception e)
                {

                }
                return;
            }
            txtSerialNumber.Text = value;
        }
        /**************************************************************************************/
        //
        /**************************************************************************************/
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (serialPort != null && serialPort.IsOpen)
            {   // port is active
                serialPort.Close();
                comboComPorts.Enabled = false;
                btnOpen.Text = "Open COM";
            }
            else
            { // port is closed or not initialised
                string comPort = comboComPorts.SelectedItem.ToString();
                if (!comPort.Equals("None"))
                {
                    try
                    {
                        serialPort = new SerialPort(comPort, 125000, Parity.None, 8, StopBits.One);
                        serialPort.Open();
                        serialPort.DiscardInBuffer();
                        serialPort.DataReceived += new SerialDataReceivedEventHandler(serialDataReceived);
                        comboComPorts.Enabled = false;
                        btnOpen.Text = "Close COM";
                    }
                    catch (Exception serialException)
                    {
                        updateProgramStatus("Error opening COM port: " + serialException.Message);
                    }
                }
            }
        }
        /**************************************************************************************/
        //
        /**************************************************************************************/
        private void btnRescan_Click(object sender, EventArgs e)
        {
            string[] commPorts = SerialPort.GetPortNames();

            comboComPorts.Items.Clear();
            if (0 == commPorts.Length)
            {
                commPorts = new string[1];
                commPorts[0] = "None";
                comboComPorts.Items.Add("None");
            }
            else
            {
                List<string> l1 = new List<string>();
                List<string> l2 = new List<string>();
                for (int i = 0; i < commPorts.Length; i++)
                {
                    if (commPorts[i].Length == 4)
                    {
                        l1.Add(commPorts[i]);
                    }
                    else
                    {
                        l2.Add(commPorts[i]);
                    }
                }
                l1.Sort();
                l2.Sort();
                comboComPorts.Items.AddRange(l1.ToArray());
                comboComPorts.Items.AddRange(l2.ToArray());
            }
            comboComPorts.SelectedIndex = 0;
        }
        /**************************************************************************************/
        //   SEND FRAME
        /**************************************************************************************/
        private bool sendFrame(byte[] buffer, int len)
        {
            byte[] uartTxBuffer = new byte[len + 3];
            int uartTxBufferCount, i;
            byte crc = 0xFF;

            uartTxBuffer[0] = 0x01;
            uartTxBuffer[1] = (byte)(len + 1);
            uartTxBufferCount = 2;
            crc ^= uartTxBuffer[1];
            for (i = 0; i < len; i++)
            {
                uartTxBuffer[uartTxBufferCount++] = buffer[i];
                crc ^= buffer[i];
            }
            uartTxBuffer[uartTxBufferCount++] = crc;
            try
            {
                serialPort.Write(uartTxBuffer, 0, uartTxBufferCount);
            }
            catch (Exception e)
            {
                appendProgramStatus("Error sending frame: " + e.Message);
                return false;
            }
            return true;
        }
        /**************************************************************************************/
        //
        //                              serialDataReceived
        //
        /**************************************************************************************/
        private enum recvStep { idle, len, data, crc };
        private enum frameResponse { SOF = 0x01, ACK = 0x06, ERR_CRC = 0x15, ERR_LEN = 0x53 };
        private enum rxFrameStatus { WAIT, SUCCESS, FAIL };
        private int uartRxBufferCount;
        private byte[] uartRxBuffer = new byte[256];
        private byte recvByte;
        private byte recvCrc;
        private byte recvlen;

        private volatile byte rxFrameResponse = 0;
        private volatile rxFrameStatus receivedFrameStatus = rxFrameStatus.WAIT;

        private recvStep receiveStep;
        private void serialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (serialPort.BytesToRead != 0)
            {
                try
                {
                    recvByte = (byte)serialPort.ReadByte();
                }
                catch (Exception serialException)
                {
                    appendProgramStatus("Error reading from COM " + serialException.Message);
                    receiveStep = recvStep.idle;
                }
                switch (receiveStep)
                {
                    case recvStep.idle:
                        switch (recvByte)
                        {
                            case (byte)frameResponse.SOF:
                                uartRxBufferCount = 0;
                                recvCrc = 0xFF;
                                recvlen = 0;
                                receiveStep = recvStep.len;
                                break;
                            case (byte)frameResponse.ACK:
                                rxFrameResponse = (byte)frameResponse.ACK;
                                break;
                            case (byte)frameResponse.ERR_LEN:
                                rxFrameResponse = (byte)frameResponse.ERR_LEN;
                                break;
                            case (byte)frameResponse.ERR_CRC:
                                rxFrameResponse = (byte)frameResponse.ERR_CRC;
                                break;
                        }
                        break;
                    case recvStep.len:
                        recvlen = recvByte;
                        if (recvlen > 254)
                        {
                            receiveStep = recvStep.idle;
                        }
                        else
                        {
                            uartRxBuffer[uartRxBufferCount++] = recvByte;
                            recvCrc ^= recvByte;
                            receiveStep = recvStep.data;
                        }
                        break;
                    case recvStep.data:
                        uartRxBuffer[uartRxBufferCount++] = recvByte;
                        recvCrc ^= recvByte;
                        if (uartRxBufferCount >= recvlen)
                        {
                            receiveStep = recvStep.crc;
                        }
                        break;
                    case recvStep.crc:
                        if (recvByte == recvCrc)
                        {
                            appendProgramStatus("Received: " + conversions.convertBytesToHexString(uartRxBuffer, uartRxBufferCount));
                            receivedFrameStatus = rxFrameStatus.SUCCESS;
                        }
                        else
                        {
                            receivedFrameStatus = rxFrameStatus.FAIL;
                        }
                        receiveStep = recvStep.idle;
                        break;
                    default:
                        receiveStep = recvStep.idle;
                        break;
                }
            }
        }
        /**************************************************************************************/
        //
        /**************************************************************************************/
        private void btnProgram_Click(object sender, EventArgs e)
        {
            string serial = txtSerialNumber.Text.Trim();

            if (!long.TryParse(serial, out serialNumber))
            {
                MessageBox.Show("Invalid serial number", "Error");
                return;
            }
            if (serial.Length != 10)
            {
                MessageBox.Show("Serial number must be 10 digits", "Error");
                return;
            }
            if (serialNumber > 0xFFFFFFFF)
            {
                MessageBox.Show("Serial number must be in the range: 0000000001 - ‭4294967295‬", "Error");
                return;
            }
            if (serialPort == null)
            {
                MessageBox.Show("No COM Ports opened", "Error");
                return;
            }
            if (!serialPort.IsOpen)
            {
                MessageBox.Show("No COM Ports opened", "Error");
                return;
            }
            ThreadStart ts = new ThreadStart(programmerThread);
            firmwareProgrammerThread = new Thread(ts);
            firmwareProgrammerThread.Start();
            toggleButton(false);
            toggleTxtSerialNumber(false);
        }
        /**************************************************************************************/
        // set the chip in application mode
        /**************************************************************************************/
        private bool enterApplicationMode()
        {
            byte[] txBuffer = new byte[2];
            int retries = 0;
            int prev = 0, step = 0;
            int count = 0;
            int attempts = 0;
            bool resetFromBl = false;
            while (retries < 3)
            {
                switch (step)
                {
                    case 0: // request device info
                        txBuffer[0] = (byte)nqCommands.deviceInfo;
                        txBuffer[1] = (byte)nqCommandType.get;
                        rxFrameResponse = 0;
                        receivedFrameStatus = rxFrameStatus.WAIT;
                        step = 2;
                        prev = 0;
                        sendFrame(txBuffer, 2);
                        appendProgramStatus("Sent Device Info Get");
                        break;
                    case 1: // send reboot
                        txBuffer[0] = (byte)nqCommands.reboot;
                        txBuffer[1] = (byte)nqCommandType.set;
                        rxFrameResponse = 0;
                        receivedFrameStatus = rxFrameStatus.WAIT;
                        step = 2;
                        prev = 1;
                        sendFrame(txBuffer, 2);
                        appendProgramStatus("Sent Reboot");
                        break;
                    case 2: // wait for response
                        count = 0;
                        while (rxFrameResponse == 0)
                        {
                            Thread.Sleep(10);
                            count++;
                            if (count > 50)
                            {
                                break;
                            }
                        }
                        if (count >= 50 || rxFrameResponse != (byte)frameResponse.ACK)
                        {
                            retries++;
                            step = prev;
                            continue;
                        }
                        if (resetFromBl)
                        {
                            Thread.Sleep(10);
                            step = 0;
                            resetFromBl = false;
                            continue;
                        }
                        count = 0;
                        while (receivedFrameStatus == rxFrameStatus.WAIT)
                        {
                            Thread.Sleep(10);
                            count++;
                            if (count > 50)
                            {
                                break;
                            }
                        }
                        if (count >= 50 || receivedFrameStatus != rxFrameStatus.SUCCESS)
                        {
                            retries++;
                            step = prev;
                            continue;
                        }
                        if (uartRxBuffer[1] == (byte)nqCommands.deviceInfo)
                        {
                            if (uartRxBuffer[12] != 0xFE)
                            {
                                applicationVersion = "" + uartRxBuffer[5] + "." + uartRxBuffer[6] + "." + uartRxBuffer[7];
                                jackConnStatus = uartRxBuffer[13];
                                return true;
                            }
                            else
                            {
                                resetFromBl = true;
                                attempts++;
                                if (attempts < 3)
                                {   // send reboot command
                                    step = 1;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        if (uartRxBuffer[1] == (byte)nqCommands.reboot)
                        {   // reply from reboot -> send device info get
                            Thread.Sleep(50);
                            step = 0;
                        }
                        else
                        {
                            retries++;
                            step = 0;
                        }
                        break;
                    default:
                        step = 0;
                        break;
                }
            }
            return true;
        }
        /**************************************************************************************/
        // toggle leds
        /**************************************************************************************/
        private bool toggleLed(int led)
        {
            byte[] txBuffer = new byte[3];
            int retries = 0;
            int step = 0;
            int count = 0;
            while (retries < 3)
            {
                switch (step)
                {
                    case 0: // send hw set command
                        txBuffer[0] = (byte)nqCommands.hwCheck;
                        txBuffer[1] = (byte)nqCommandType.set;
                        txBuffer[2] = (byte)led;
                        rxFrameResponse = 0;
                        receivedFrameStatus = rxFrameStatus.WAIT;
                        step = 1;
                        sendFrame(txBuffer, 3);
                        appendProgramStatus("Sent Toggle led " + led);
                        break;
                    case 1: // wait for response
                        count = 0;
                        while (rxFrameResponse == 0)
                        {
                            Thread.Sleep(10);
                            count++;
                            if (count > 50)
                            {
                                break;
                            }
                        }
                        if (count >= 50 || rxFrameResponse != (byte)frameResponse.ACK)
                        {
                            retries++;
                            continue;
                        }
                        count = 0;
                        while (receivedFrameStatus == rxFrameStatus.WAIT)
                        {
                            Thread.Sleep(10);
                            count++;
                            if (count > 50)
                            {
                                break;
                            }
                        }
                        if (count >= 50 || receivedFrameStatus != rxFrameStatus.SUCCESS)
                        {
                            retries++;
                            continue;
                        }
                        if (uartRxBuffer[1] == (byte)nqCommands.hwCheck)
                        {
                            if (uartRxBuffer[2] == (byte)nqCommandType.response)
                            {   // got response
                                if (uartRxBuffer[3] == (byte)nqCommandStatus.success)
                                {
                                    return true;
                                }
                                else
                                {
                                    retries++;
                                    step = 0;
                                }
                            }
                            else
                            {
                                retries++;
                                step = 0;
                            }
                        }
                        else
                        {
                            retries++;
                            step = 0;
                        }
                        break;
                    default:
                        step = 0;
                        break;
                }
            }
            return true;
        }

        /**************************************************************************************/
        // get bootloader info
        /**************************************************************************************/
        private bool getBootloaderInfo()
        {
            byte[] txBuffer = new byte[2];
            int retries = 0;
            int step = 0;
            int count = 0;
            while (retries < 3)
            {
                switch (step)
                {
                    case 0: // send hw set command
                        txBuffer[0] = (byte)nqCommands.deviceInfo;
                        txBuffer[1] = (byte)nqCommandType.get;
                        rxFrameResponse = 0;
                        receivedFrameStatus = rxFrameStatus.WAIT;
                        step = 1;
                        sendFrame(txBuffer, 2);
                        appendProgramStatus("Sent Device Info Get");
                        break;
                    case 1: // wait for response
                        count = 0;
                        while (rxFrameResponse == 0)
                        {
                            Thread.Sleep(10);
                            count++;
                            if (count > 50)
                            {
                                break;
                            }
                        }
                        if (count >= 50 || rxFrameResponse != (byte)frameResponse.ACK)
                        {
                            retries++;
                            continue;
                        }
                        count = 0;
                        while (receivedFrameStatus == rxFrameStatus.WAIT)
                        {
                            Thread.Sleep(10);
                            count++;
                            if (count > 50)
                            {
                                break;
                            }
                        }
                        if (count >= 50 || receivedFrameStatus != rxFrameStatus.SUCCESS)
                        {
                            retries++;
                            continue;
                        }
                        if (uartRxBuffer[1] == (byte)nqCommands.deviceInfo)
                        {
                            if (uartRxBuffer[12] == 0xFE)
                            {
                                bootloaderVersion = "" + uartRxBuffer[5] + "." + uartRxBuffer[6] + "." + uartRxBuffer[7];
                                jackConnStatus = uartRxBuffer[13];
                                return true;
                            }
                            else
                            {
                                retries++;
                                step = 0;
                            }
                        }
                        else
                        {
                            retries++;
                            step = 0;
                        }
                        break;
                    default:
                        step = 0;
                        break;
                }
            }
            return true;
        }
        /**************************************************************************************/
        // get device info
        /**************************************************************************************/
        private bool getDeviceInfo()
        {
            byte[] txBuffer = new byte[2];
            int retries = 0;
            int step = 0;
            int count = 0;
            while (retries < 3)
            {
                switch (step)
                {
                    case 0: // send hw set command
                        txBuffer[0] = (byte)nqCommands.deviceInfo;
                        txBuffer[1] = (byte)nqCommandType.get;
                        rxFrameResponse = 0;
                        receivedFrameStatus = rxFrameStatus.WAIT;
                        step = 1;
                        sendFrame(txBuffer, 2);
                        appendProgramStatus("Sent Device Info Get");
                        break;
                    case 1: // wait for response
                        count = 0;
                        while (rxFrameResponse == 0)
                        {
                            Thread.Sleep(10);
                            count++;
                            if (count > 50)
                            {
                                break;
                            }
                        }
                        if (count >= 50 || rxFrameResponse != (byte)frameResponse.ACK)
                        {
                            retries++;
                            continue;
                        }
                        count = 0;
                        while (receivedFrameStatus == rxFrameStatus.WAIT)
                        {
                            Thread.Sleep(10);
                            count++;
                            if (count > 50)
                            {
                                break;
                            }
                        }
                        if (count >= 50 || receivedFrameStatus != rxFrameStatus.SUCCESS)
                        {
                            retries++;
                            continue;
                        }
                        if (uartRxBuffer[1] == (byte)nqCommands.deviceInfo)
                        {
                            if (uartRxBuffer[12] != 0xFE)
                            {
                                applicationVersion = "" + uartRxBuffer[5] + "." + uartRxBuffer[6] + "." + uartRxBuffer[7];
                                jackConnStatus = uartRxBuffer[13];
                                return true;
                            }
                            else
                            {
                                bootloaderVersion = "" + uartRxBuffer[5] + "." + uartRxBuffer[6] + "." + uartRxBuffer[7];
                                retries++;
                                step = 0;
                            }
                        }
                        else
                        {
                            retries++;
                            step = 0;
                        }
                        break;
                    default:
                        step = 0;
                        break;
                }
            }
            return true;
        }
        /**************************************************************************************/
        // reset to factory defaults
        /**************************************************************************************/
        private bool loadFactoryDefaults()
        {
            byte[] txBuffer = new byte[2];
            int retries = 0;
            int step = 0;
            int count = 0;
            while (retries < 3)
            {
                switch (step)
                {
                    case 0: // send hw set command
                        txBuffer[0] = (byte)nqCommands.factoryReset;
                        txBuffer[1] = (byte)nqCommandType.set;
                        rxFrameResponse = 0;
                        receivedFrameStatus = rxFrameStatus.WAIT;
                        step = 1;
                        sendFrame(txBuffer, 2);
                        appendProgramStatus("Sent Device Info Get");
                        break;
                    case 1: // wait for response
                        count = 0;
                        while (rxFrameResponse == 0)
                        {
                            Thread.Sleep(10);
                            count++;
                            if (count > 50)
                            {
                                break;
                            }
                        }
                        if (count >= 50 || rxFrameResponse != (byte)frameResponse.ACK)
                        {
                            retries++;
                            continue;
                        }
                        count = 0;
                        while (receivedFrameStatus == rxFrameStatus.WAIT)
                        {
                            Thread.Sleep(10);
                            count++;
                            if (count > 50)
                            {
                                break;
                            }
                        }
                        if (count >= 50 || receivedFrameStatus != rxFrameStatus.SUCCESS)
                        {
                            retries++;
                            continue;
                        }
                        if (uartRxBuffer[1] == (byte)nqCommands.factoryReset && uartRxBuffer[2] == (byte)nqCommandType.response && uartRxBuffer[3] == (byte)nqCommandStatus.success)
                        {
                            return true;
                        }
                        else
                        {
                            retries++;
                            step = 0;
                        }
                        break;
                    default:
                        step = 0;
                        break;
                }
            }
            return true;
        }
        /**************************************************************************************/
        // get hardware status
        /**************************************************************************************/
        private bool getHardwareStatus()
        {
            byte[] txBuffer = new byte[2];
            int retries = 0;
            int step = 0;
            int count = 0;
            while (retries < 3)
            {
                switch (step)
                {
                    case 0: // send hw set command
                        txBuffer[0] = (byte)nqCommands.hwCheck;
                        txBuffer[1] = (byte)nqCommandType.get;
                        rxFrameResponse = 0;
                        receivedFrameStatus = rxFrameStatus.WAIT;
                        step = 1;
                        sendFrame(txBuffer, 2);
                        appendProgramStatus("Sent Hardware Status Get");
                        break;
                    case 1: // wait for response
                        count = 0;
                        while (rxFrameResponse == 0)
                        {
                            Thread.Sleep(10);
                            count++;
                            if (count > 50)
                            {
                                break;
                            }
                        }
                        if (count >= 50 || rxFrameResponse != (byte)frameResponse.ACK)
                        {
                            retries++;
                            continue;
                        }
                        count = 0;
                        while (receivedFrameStatus == rxFrameStatus.WAIT)
                        {
                            Thread.Sleep(10);
                            count++;
                            if (count > 50)
                            {
                                break;
                            }
                        }
                        if (count >= 50 || receivedFrameStatus != rxFrameStatus.SUCCESS)
                        {
                            retries++;
                            continue;
                        }
                        if (uartRxBuffer[1] == (byte)nqCommands.hwCheck && uartRxBuffer[2] == (byte)nqCommandType.response && uartRxBuffer[3] == (byte)nqCommandStatus.success)
                        {
                            return (uartRxBuffer[5] == 0x03);
                        }
                        else
                        {
                            retries++;
                            step = 0;
                        }
                        break;
                    default:
                        step = 0;
                        break;
                }
            }
            return true;
        }
        /**************************************************************************************/
        // get adc readings
        /**************************************************************************************/
        private bool getInstantReadings()
        {
            byte[] txBuffer = new byte[3];
            int retries = 0;
            int step = 0;
            int count = 0;
            while (retries < 3)
            {
                switch (step)
                {
                    case 0: // send hw set command
                        txBuffer[0] = (byte)nqCommands.config;
                        txBuffer[1] = (byte)nqCommandType.get;
                        txBuffer[2] = 0x05;
                        rxFrameResponse = 0;
                        receivedFrameStatus = rxFrameStatus.WAIT;
                        step = 1;
                        sendFrame(txBuffer, 3);
                        appendProgramStatus("Sent Config Get: Instant RMS Current");
                        break;
                    case 1: // wait for response
                        count = 0;
                        while (rxFrameResponse == 0)
                        {
                            Thread.Sleep(10);
                            count++;
                            if (count > 50)
                            {
                                break;
                            }
                        }
                        if (count >= 50 || rxFrameResponse != (byte)frameResponse.ACK)
                        {
                            retries++;
                            continue;
                        }
                        count = 0;
                        while (receivedFrameStatus == rxFrameStatus.WAIT)
                        {
                            Thread.Sleep(10);
                            count++;
                            if (count > 50)
                            {
                                break;
                            }
                        }
                        if (count >= 50 || receivedFrameStatus != rxFrameStatus.SUCCESS)
                        {
                            retries++;
                            continue;
                        }
                        if (uartRxBuffer[2] == (byte)nqCommands.config && uartRxBuffer[3] == (byte)nqCommandType.response && uartRxBuffer[4] == (byte)nqCommandStatus.success)
                        {
                            

                            clamp1RmsCurrent = (((long)uartRxBuffer[6]) << 24) + (((long)uartRxBuffer[7]) << 16) + (((long)uartRxBuffer[8]) << 8) + uartRxBuffer[9];
                            clamp2RmsCurrent = (((long)uartRxBuffer[10]) << 24) + (((long)uartRxBuffer[11]) << 16) + (((long)uartRxBuffer[12]) << 8) + uartRxBuffer[13];
                            clamp3RmsCurrent = (((long)uartRxBuffer[14]) << 24) + (((long)uartRxBuffer[15]) << 16) + (((long)uartRxBuffer[16]) << 8) + uartRxBuffer[17];
                          
                            return true;
                        }
                        else
                        {
                            retries++;
                            step = 0;
                        }
                        break;
                    default:
                        step = 0;
                        break;
                }
            }
            return true;
        }

        private void txtProgress_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtProgramStatus_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSerialNumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboComPorts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /**************************************************************************************/
        //
        /**************************************************************************************/
        private int jackConnStatus;
        private void programmerThread()
        {
            string line;
            string txtBoxSerialNumber = txtSerialNumber.Text.Trim();
            updateProgress("busy");
            // Step 1: check if the serial number exists in the database and the firmware is up to date
            string request = "/clamp/check_unique?serial=" + txtBoxSerialNumber;
            DigestAuthFixer digest = new DigestAuthFixer("https://production.homemanager.tv", "HnCLMHwrjLrb6vZT", "tdFrZlEo2HnBv0Ua");
            string response = digest.GrabResponse(request);
            /*        if(response.Equals("{\"success\":0}"))
                    {
                        MessageBox.Show("Duplicate Serial Number", "Error");
                        updateProgress("Failed");
                        updateProgramStatus("Duplicate serial number");
                        toggleButton(true);
                        toggleTxtSerialNumber(true);
                        return;
                    }
                   */
            request = "/clamp/nqclamp1_get_firmware?download=" + lastDownloadTime;
            try
            {
                /* byte[] message = digest.GrabBytesResponse(request);
                 TimeSpan span = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
                 lastDownloadTime = (long)span.TotalSeconds;
                 */
                string tempFirmware = "";
                firmware = new List<string>();
                tempFirmware = File.ReadAllText("firmware_20.07.2016_12.50.32.hex");

                //string lala = "";
                /* for (int i = 0; i < message.Length; i++)
                 {
                     tempFirmware += (char)message[i];
                 }
                 */
                // split the firmware and save the index of the serial number rows
                bool done = false;
                int pos = -1;
                int count = 0;
                snUpperIdx = -1;
                snLoweridx = -1;
                while (!done)
                {
                    pos = tempFirmware.IndexOf('\n');
                    if (pos == -1)
                    {
                        done = true;
                    }
                    else
                    {

                        if (tempFirmware[pos - 1] == '\r')
                        {
                            line = tempFirmware.Substring(0, pos - 1);
                        }
                        else
                        {
                            line = tempFirmware.Substring(0, pos);
                        }
                        firmware.Add(line);
                        long address = conversions.stringToUint16(line.Substring(3, 4));
                        if (address == 0x9FE8)
                        {
                            snUpperIdx = count;
                        }
                        if (address == 0x9FEC)
                        {
                            snLoweridx = count;
                        }
                        count++;
                        tempFirmware = tempFirmware.Substring(pos + 1);
                    }
                }
                if (snUpperIdx == -1 || snLoweridx == -1)
                {
                    txtProgramStatus.AppendText("Firmware file corrupt. Contact NorthQ");
                    return;
                }
            }
            catch (Exception ee)
            {

            }

            // Step 2: update the firmware with the serial number
            long oldSn = serialNumber;
            line = firmware.ElementAt(snUpperIdx);
            string replacementSn;
            int crc = 0;
            replacementSn = conversions.convertByteToHexString((int)(serialNumber >> 24));
            replacementSn = conversions.convertByteToHexString((int)(serialNumber >> 16)) + replacementSn;
            line = line.Substring(0, 9) + replacementSn + "0000";
            for (int i = 1; i < line.Length; i += 2)
            {
                crc += conversions.stringToByte(line.Substring(i, 2));
            }
            crc = (byte)(0x100 - (crc & 0xFF));
            line += conversions.convertByteToHexString(crc);
            firmware.RemoveAt(snUpperIdx);
            firmware.Insert(snUpperIdx, line.ToLower());

            line = firmware.ElementAt(snLoweridx);
            crc = 0;
            replacementSn = conversions.convertByteToHexString((int)(serialNumber >> 8));
            replacementSn = conversions.convertByteToHexString((int)(serialNumber)) + replacementSn;
            line = line.Substring(0, 9) + replacementSn + "0000";
            for (int i = 1; i < line.Length; i += 2)
            {
                crc += conversions.stringToByte(line.Substring(i, 2));
            }
            crc = (byte)(0x100 - (crc & 0xFF));
            line += conversions.convertByteToHexString(crc);
            firmware.RemoveAt(snLoweridx);
            firmware.Insert(snLoweridx, line.ToLower());

            File.WriteAllLines(System.Environment.CurrentDirectory + "\\tempLoad\\load.hex", firmware);
            // Step 3: program the device
            updateProgramStatus("Programming device started...");
            string firmwareDirectory = System.Environment.CurrentDirectory + "\\tempLoad\\load.hex";
            string statusMessage = "Success. Serial number is: " + serialNumber;
            string programmingStatus = "failed";
            Process p = new Process();
            p.StartInfo.FileName = pathToProgrammer;
            p.StartInfo.Arguments = @"-P24F32KA301 -E -F" + firmwareDirectory + " -M -Y -L";
            p.StartInfo.WorkingDirectory = pathToWorkingDirectory;
            p.Start();
            p.WaitForExit();
            switch (p.ExitCode)
            {
                case 0:
                    programmingStatus = "success";
                    serialNumber++;
                    string serial = "" + serialNumber;
                    int digitCount = 0;
                    while (serialNumber != 0)
                    {
                        digitCount++;
                        serialNumber = serialNumber / 10;
                    }
                    for (int i = digitCount; i < 10; i++)
                    {
                        serial = "0" + serial;
                    }
                    updateSerialNumber(serial);
                    break;
                case 1:
                    statusMessage = "Error: Invalid command line argument";
                    break;
                case 2:
                    statusMessage = "Error: Communication with PicKit failed";
                    break;
                case 3:
                    statusMessage = "Error: Selected Operation Failed";
                    break;
                case 4:
                    statusMessage = "Error: Unknown Runtime Failure";
                    break;
                case 5:
                    statusMessage = "Error: Invalid Device Detected";
                    break;
                case 6:
                    statusMessage = "Error: SQTP Failed";
                    break;
                default:
                    statusMessage = "Error: Unknown error: " + p.ExitCode;
                    break;
            }
            if (File.Exists(System.Environment.CurrentDirectory + "\\tempLoad\\load.hex"))
            {
                File.Delete(System.Environment.CurrentDirectory + "\\tempLoad\\load.hex");
            }
            updateProgramStatus("Programming device completed...");
            updateProgramStatus("Testing device started...");

            // Step 4: test the hardware 
            jackConnStatus = 0;
            DialogResult dr;
            bool hardwareTest = true;
            /*  if(p.ExitCode != 0)
              {
                  MessageBox.Show("Exit Code fail!");
                  hardwareTest = false;
                  applicationVersion = "";
                  bootloaderVersion = "";
              }
              */

            if (hardwareTest)
            {
                hardwareTest = getBootloaderInfo();
                MessageBox.Show("Connect all JACKS to the CLAMPS", "Info");
                hardwareTest = enterApplicationMode();
                hardwareTest = jackConnStatus == 0x07;
                appendProgramStatus("Jack conn: " + jackConnStatus);
                if (!hardwareTest)
                {
                    int attempt = 0;
                    while (attempt < 3 && hardwareTest == false)
                    {
                        MessageBox.Show("Try to reconnect all JACKS again and press OK("+attempt+"try)", "JACK connections", MessageBoxButtons.OK);
                        hardwareTest = getDeviceInfo();
                        hardwareTest = jackConnStatus == 0x00;
                        appendProgramStatus("Jack conn: " + jackConnStatus);
                        attempt++;
                    }
                }
            }
            if (hardwareTest)
            {
                hardwareTest = toggleLed(1);
            }
            if (hardwareTest)
            {
                dr = MessageBox.Show("Is the BLUE LED on ?", "LED", MessageBoxButtons.YesNo);
                if (dr == DialogResult.No)
                {
                    hardwareTest = false;
                }
            }
            if (hardwareTest)
            {
                hardwareTest = toggleLed(2);
            }
            if (hardwareTest)
            {
                dr = MessageBox.Show("Is the RED LED on ?", "LED", MessageBoxButtons.YesNo);
                if (dr == DialogResult.No)
                {
                    hardwareTest = false;
                }
            }
            if (hardwareTest)
            {
                hardwareTest = getHardwareStatus();
                MessageBox.Show("Press the button for 1 seccond and THEN press OK!", "BUTTON", MessageBoxButtons.OK);
      
            }

            if (hardwareTest)
            {
                MessageBox.Show("Clamp the clapms around the cord, Rotate the switch on position 1 and press OK", "Clamp check", MessageBoxButtons.OK);
                hardwareTest = getInstantReadings();

                if ((0 >= clamp1RmsCurrent || clamp1RmsCurrent < 1000) || (0 >= clamp1RmsCurrent || clamp1RmsCurrent < 1000) || (0 >= clamp1RmsCurrent || clamp1RmsCurrent < 1000))
                {
                    hardwareTest = false;

                }
            }
                
            
            if (hardwareTest)
            {
                MessageBox.Show("Disconnect all JACKS from the CLAMS", "Info");
                hardwareTest = getDeviceInfo();
                hardwareTest = jackConnStatus == 0x00;
                appendProgramStatus("Jack conn: " + jackConnStatus);
                if (!hardwareTest)
                {
                    int attempt = 0;
                    while (attempt < 3 && hardwareTest == false)
                    {
                        MessageBox.Show("Try to reconnect and disconnect all JACKS again and press OK("+attempt+"try)", "JACK disconnection", MessageBoxButtons.OK);
                        hardwareTest = getDeviceInfo();
                        hardwareTest = jackConnStatus == 0x00;
                        appendProgramStatus("Jack conn: " + jackConnStatus);
                        attempt++;
                    }
                }
                
            }



            if (hardwareTest)
            {
                hardwareTest = loadFactoryDefaults();
                request = "/clamp/commit_produced?serial=" + txtBoxSerialNumber + "&bt=" + bootloaderVersion + "&fw=" ;
                try
                {
                    response = digest.GrabResponse(request);
                }
                catch (Exception e1)
                {

                }
                Thread.Sleep(250);
            }
            else
            {
                programmingStatus = "Failed";
                statusMessage = "Failed to test device";
            }
            updateProgramStatus("Testing device completed");
            // Step 5: update UI with status
            updateProgress(programmingStatus);
            updateProgramStatus(statusMessage);
            toggleButton(true);
            toggleTxtSerialNumber(true);
        }
    }
}
