using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClampSensorProgrammer
{
    class conversions
    {
        /*************************************************************************************************************/
        //
        /*************************************************************************************************************/
        public static byte stringToByte(string s)
        {
            int retVal = 0;
            int i;

            s = s.ToUpper();

            for (i = 0; i < 2; i++)
            {
                retVal <<= 4;
                if (s[i] >= '0' && s[i] <= '9')
                {
                    retVal = retVal + (s[i] - '0');
                }
                else
                if (s[i] >= 'A' && s[i] <= 'F')
                {
                    retVal = retVal + (s[i] - 55);
                }
            }
            return (byte)retVal;
        }
        /**************************************************************************************/
        //
        /**************************************************************************************/
        public static UInt16 stringToUint16(string s)
        {
            int retVal = 0;
            int i;
            s = s.ToUpper();
            for (i = 0; i < 4; i++)
            {
                retVal <<= 4;
                if (s[i] >= '0' && s[i] <= '9')
                {
                    retVal = retVal + (s[i] - '0');
                }
                else
                if (s[i] >= 'A' && s[i] <= 'F')
                {
                    retVal = retVal + (s[i] - 55);
                }
            }
            return (UInt16)retVal;
        }
        /**************************************************************************************/
        //
        /**************************************************************************************/
        public static UInt32 stringToUint32(string s)
        {
            int retVal = 0;
            int i;
            s = s.ToUpper();
            for (i = 0; i < 8; i++)
            {
                retVal <<= 4;
                if (s[i] >= '0' && s[i] <= '9')
                {
                    retVal = retVal + (s[i] - '0');
                }
                else
                if (s[i] >= 'A' && s[i] <= 'F')
                {
                    retVal = retVal + (s[i] - 55);
                }
            }
            return (UInt32)retVal;
        }
        /**************************************************************************************/
        //
        /**************************************************************************************/
        public static string convertBytesToHexString(byte[] buffer, int len)
        {
            string result = "";
            byte b;
            for (int i = 0; i < len; i++)
            {
                b = (byte)((buffer[i] >> 4) & 0x0F);
                if (b < 10)
                {
                    result += (char)(b + '0');
                }
                else
                {
                    switch (b)
                    {
                        case 10:
                            result += 'A';
                            break;
                        case 11:
                            result += 'B';
                            break;
                        case 12:
                            result += 'C';
                            break;
                        case 13:
                            result += 'D';
                            break;
                        case 14:
                            result += 'E';
                            break;
                        case 15:
                            result += 'F';
                            break;
                    }
                }

                b = (byte)(buffer[i] & 0x0F);
                if (b < 10)
                {
                    result += (char)(b + '0');
                }
                else
                {
                    switch (b)
                    {
                        case 10:
                            result += 'A';
                            break;
                        case 11:
                            result += 'B';
                            break;
                        case 12:
                            result += 'C';
                            break;
                        case 13:
                            result += 'D';
                            break;
                        case 14:
                            result += 'E';
                            break;
                        case 15:
                            result += 'F';
                            break;
                    }
                }
                result += ' ';
            }
            return result;
        }
        /**************************************************************************************/
        //
        /**************************************************************************************/
        public static string convertByteToHexString(int input)
        {
            string result = "";
            int b;

            b = (input >> 4) & 0x0F;
            if (b < 10)
            {
                result += (char)(b + '0');
            }
            else
            {
                switch (b)
                {
                    case 10:
                        result += 'A';
                        break;
                    case 11:
                        result += 'B';
                        break;
                    case 12:
                        result += 'C';
                        break;
                    case 13:
                        result += 'D';
                        break;
                    case 14:
                        result += 'E';
                        break;
                    case 15:
                        result += 'F';
                        break;
                }
            }

            b = input & 0x0F;
            if (b < 10)
            {
                result += (char)(b + '0');
            }
            else
            {
                switch (b)
                {
                    case 10:
                        result += 'A';
                        break;
                    case 11:
                        result += 'B';
                        break;
                    case 12:
                        result += 'C';
                        break;
                    case 13:
                        result += 'D';
                        break;
                    case 14:
                        result += 'E';
                        break;
                    case 15:
                        result += 'F';
                        break;
                }
            }
            return result;
        }
    }
}

