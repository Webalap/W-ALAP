using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Configuration;

namespace TTS_server_alap_alpha_v1
{
    class Program
    {
        const int PORT_NO = 5000;
        static string ValueByKey = "";
        static bool isError = false;    
        static bool isMisSpelled = false;
        static bool isSpeicalCommand = false;
        static bool isVoiceChange = false;
        const string SERVER_IP = "127.0.0.1";
        static Dictionary<string, string> lookupTable = new Dictionary<string, string>();
        static List<string> keyList = new List<string>(lookupTable.Keys);
        public static string strAppendLatexDocument = "";
        public static bool isPDfAccMode = false;
        static string currentVoice = "Microsoft David Desktop";
        static int currentSpeed = 0;
        static string focusedUIElement = "";
        static void Main(string[] args)
        {
            try
            {
                while (true)
                    SetServer();
            }catch(Exception ex)
            {
                Console.WriteLine("Resetting server" + ex.Message);
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = TTSProperties.CMND_KILL_TTS;
                process.StartInfo = startInfo;
                process.Start();
                SetServer();
            } 
        }

        private static void SetServer()
        {
            try
            {
                TTS.setReader();
                IPAddress localAdd = IPAddress.Parse(SERVER_IP);

                TcpListener listener = new TcpListener(localAdd, PORT_NO);
                Console.WriteLine(ConfigurationManager.AppSettings["ServerInitalState"]);
                listener.Start();

                //---incoming client connected--- 
                TcpClient client = listener.AcceptTcpClient();

                listener.Stop();
                bool isVrbstyChar = false;
                bool isPDFMode = false;
                bool IsLatexDocument = false;

                while (client.Connected)
                {
                    NetworkStream nwStream = client.GetStream();

                    byte[] buffer = new byte[client.ReceiveBufferSize];

                    //---read incoming stream--- 
                    int bytesRead = 0;
                    string dataReceived = string.Empty;

                    try
                    {
                        bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
                    }
                    catch (IOException e)
                    {
                        // break;
                    }
                    //---convert the data received into a string---
                    dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    Console.WriteLine("Recevie Data From Client: "+dataReceived);

                    if (dataReceived.Length == 1)
                    {
                        CancelCurrentStream();
                        dataReceived = CurserMode.ParseSource(dataReceived);
                        TTS.Speak(dataReceived);
                    }
                    else if (dataReceived.Equals(ConfigurationManager.AppSettings["DataReceived_Next"]) 
                            || dataReceived.Equals(ConfigurationManager.AppSettings["DataReceived_Stop"]))
                    {
                        if (TTS.getReader().State == SynthesizerState.Paused)
                        {
                            TTS.getReader().Dispose();
                            TTS.setReader();
                        }
                        else
                            TTS.getReader().SpeakAsyncCancelAll();
                        if (dataReceived.Equals(ConfigurationManager.AppSettings["DataReceived_Next"]))
                        {
                            isError = true;
                        }
                        dataReceived = string.Empty;
                        isPDFMode = false;
                        //isVrbstyChar = false;
                        isMisSpelled = false;
                    }
                    else if (dataReceived.Equals(ConfigurationManager.AppSettings["DataReceived_LatexDcmnt"]))
                    {
                        dataReceived = string.Empty;
                        IsLatexDocument = true;
                    }
                    else if (IsLatexDocument)
                    {
                        if (dataReceived.ToLower().Contains("\\end{document}") || dataReceived.Equals("\\end{document}"))
                        {
                            dataReceived = strAppendLatexDocument + dataReceived;
                            strAppendLatexDocument = string.Empty;
                            PdfAccessibilityMode obj = new PdfAccessibilityMode(dataReceived);
                            obj.ConvertLatexEquationToText();

                            //---write back the text to the client---
                            byte[] hey = Encoding.ASCII.GetBytes(PdfAccessibilityMode.LatexDocument);
                            Console.WriteLine("Sending back Client : " + PdfAccessibilityMode.LatexDocument);
                            nwStream.Write(hey, 0, hey.Length);
                            PdfAccessibilityMode.LatexDocument = string.Empty;
                            dataReceived = "";
                            IsLatexDocument = false;
                            nwStream.Flush();
                            //client.Close();
                            //Thread.Sleep(5000);
                            //nwStream.Dispose();
                            //listener.Stop();
                            //nwStream.Dispose();
                            //nwStream.Flush();
                            //listener.Stop();
                            //client.Close();
                            //listener.Start();
                        }
                        else
                        {
                            strAppendLatexDocument = dataReceived + "\n";
                        }
                    }
                    else if (isPDFMode)
                    {
                        PdfMode mathObj = new PdfMode(dataReceived);
                        mathObj.filterLatexSourceCodeUsingOpenDetex();
                        dataReceived = "";
                        isPDFMode = false;
                    }

                    else if (dataReceived.Equals(ConfigurationManager.AppSettings["DataReceived_PDFMode"]))
                    {
                        CancelCurrentStream();
                        isPDFMode = true;
                        dataReceived = string.Empty;
                        Console.WriteLine(TTSProperties.MSG_PDF_MODE_ENABLED);
                    }
                    else if (dataReceived.Equals(ConfigurationManager.AppSettings["DataReceived_Pause"]))
                    {
                        TTS.TTS_Pause();
                        dataReceived = string.Empty;
                        Console.WriteLine(TTSProperties.MSG_TTS_PAUSED);
                    }
                    else if (dataReceived.Equals(ConfigurationManager.AppSettings["DataReceived_Resume"]))
                    {
                        TTS.TTS_Resume();
                        dataReceived = string.Empty;
                        Console.WriteLine(TTSProperties.MSG_TTS_RESUMED);
                    }
                    else if (dataReceived.Equals(ConfigurationManager.AppSettings["DataReceived_WordVrbsty"]))
                    {
                        isVrbstyChar = false;
                        dataReceived = string.Empty;
                    }
                    else if (dataReceived.Equals(ConfigurationManager.AppSettings["DataReceived_CharVrbsty"]))
                    {
                        isVrbstyChar = true;
                        dataReceived = string.Empty;
                    }
                    else if (dataReceived.Equals(ConfigurationManager.AppSettings["DataReceived_SpecialCmnd"]))
                    {
                        CancelCurrentStream();
                        isSpeicalCommand = true;
                        dataReceived = string.Empty;
                    }
                    else if (dataReceived.Equals(ConfigurationManager.AppSettings["DataReceived_IncreaseSpeed"]))
                    {
                        int speed = TTS.getReader().Rate + 1;
                        TTS.TTS_SetSpeed(speed);
                        dataReceived = string.Empty; 
                        Console.WriteLine(TTSProperties.MSG_SPEED_INCREASED);
                    }
                    else if (dataReceived.Equals(ConfigurationManager.AppSettings["DataReceived_DecreaseSpeed"]))
                    {
                        int speed = TTS.getReader().Rate - 1;
                        TTS.TTS_SetSpeed(speed);
                        dataReceived = string.Empty;
                        Console.WriteLine(TTSProperties.MSG_SPEED_DECREASED);
                    }
                    else if (CurserMode.IsCurserMode(dataReceived))
                    {
                        string editorCmndValue = ConfigurationManager.AppSettings["DataReceived_EditorCmnd"];
                        if (dataReceived.Contains(editorCmndValue))
                        {
                            dataReceived = dataReceived.Contains(editorCmndValue) ? dataReceived.Replace(editorCmndValue, "") : dataReceived;
                            if (isVrbstyChar)
                            {
                                CancelCurrentStream();
                                //dataReceived = Encoding.Default.GetString(buffer, 0, bytesRead);
                                TTS.VBRCharModeTTSSpeak(dataReceived);
                                //isVrbstyChar = false;
                                continue;
                            }
                        }
                        dataReceived = CurserMode.ParseSource(dataReceived);
                        CancelCurrentStream();
                        TTS.Speak(dataReceived);
                    }
                    else if (dataReceived.Equals(ConfigurationManager.AppSettings["DataReceived_ChangeVoice"]))
                    {
                        isVoiceChange = true;
                        dataReceived = string.Empty;
                    }
                    else if (isVoiceChange)
                    {
                        isVoiceChange = false;
                        TTS.TTS_ChangeVoice(dataReceived);
                        dataReceived = string.Empty;
                        Console.WriteLine(TTSProperties.MSG_VOICE_CHANGED);
                    }
                    else if (!string.IsNullOrEmpty(dataReceived))
                    {
                        LookupTable();
                        //dataReceived = TextLineParsing(dataReceived);
                        if (isMisSpelled)
                        {
                            dataReceived = "misspelled \r\n" + dataReceived;
                            isMisSpelled = false;
                        }
                        if (isVrbstyChar && !isError && !isSpeicalCommand)
                        {
                            CancelCurrentStream();
                            //dataReceived = Encoding.Default.GetString(buffer, 0, bytesRead);
                            TTS.VBRCharModeTTSSpeak(dataReceived);
                            //isVrbstyChar = false;
                            continue;
                        }
                        dataReceived = TextLineParsing(dataReceived);
                        CancelCurrentStream();
                        TTS.Speak(dataReceived);
                        isError = false;
                        isSpeicalCommand = false;
                    }
                }
            }catch(Exception ex) 
            {
                throw ex;
            }
        }

        private static void CancelCurrentStream()
        {
            if (TTS.getReader().State == SynthesizerState.Speaking)
            {
                TTS.getReader().SpeakAsyncCancelAll();
                TTS.setReader();
            }
        }

        private static string TextLineParsing(string DataStr)
        {
            string ParsedString = string.Empty;         
            string[] Lines = DataStr.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach(string Line in Lines)
            {
                if (Line.Contains("\\"))
                {
                    string TempStr = LatexParsing(Line);
                    ParsedString = ParsedString + TempStr;
                }
                else
                {
                    ParsedString = ParsedString + Line;
                }
            }
            ParsedString = ParsedString.Replace(@"{", lookupTable[@"{"]).Replace(@"}", lookupTable[@"}"]);
            return ParsedString;
        }

        private static string LatexParsing(string dataReceived)
        {
            string[] LatexErrorText = dataReceived.Split(' ');
            foreach (string word in LatexErrorText)
            {
                if (word.Contains("\\") || dataReceived.Contains(@"^") || dataReceived.Contains(@"}") || dataReceived.Contains(@"{"))
                {                   
                    bool hasValue = lookupTable.TryGetValue(word, out ValueByKey);
                    if (hasValue)
                    {
                        dataReceived = dataReceived.Replace(word, ValueByKey);
                        //isMisSpelled = false;
                    } 
                }
                else if (isError)
                    isMisSpelled = true;
            }
            return dataReceived;
        }
        private static void LookupTable()
        {
            lookupTable = LookUpFile.GetLookTable();
        }   
    }
}
