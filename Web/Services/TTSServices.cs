using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Text;
using TTS_server_alap_alpha_v1;
using Web.Models;

namespace Web.Services
{
    public class TTSServices
    {
        public bool IsError { get; set; }
        public bool IsMisSpelled { get; set; }
        public bool IsVrbstyChar { get; set; }
        public bool IsPDFMode { get; set; }
        public Dictionary<string,string> LookupTable { get; set; }
        public List<string> KeyList { get; set; }

        private string ValueByKey;
        public TTSServices()
        {
            IsError = false;
            IsMisSpelled = false;
            IsVrbstyChar = false;
            IsPDFMode = false;
            LookupTable = new Dictionary<string, string>();
            LookupTable = GetLookupTable();
            KeyList = new List<string>(LookupTable.Keys);
        }
        public void Speak(string dataReceived)
        {
            TTS.setReader();
            if (dataReceived.Equals("**(tNext)**") || dataReceived.Equals("**(tStop)**"))
            {
                if (TTS.getReader().State == SynthesizerState.Paused)
                {
                    TTS.getReader().Dispose();
                    TTS.setReader();
                }
                else
                    TTS.getReader().SpeakAsyncCancelAll();
                if (dataReceived.Equals("**(tNext)**"))
                {
                    IsError = true;
                }

                dataReceived = "";
            }
            else if (IsPDFMode)
            {
                PdfMode mathObj = new PdfMode(dataReceived);
                mathObj.filterLatexSourceCodeUsingOpenDetex();
                dataReceived = "";

            }
            else if (CurserMode.IsCurserMode(dataReceived))
            {
                dataReceived = CurserMode.ParseSource(dataReceived);
                TTS.Speak(dataReceived);
            }
            else if (dataReceived.Equals("**(tPDFMode)**"))
            {
                if (TTS.getReader().State == SynthesizerState.Speaking)
                {
                    TTS.getReader().SpeakAsyncCancelAll();
                    TTS.setReader();
                }
                IsPDFMode = true;
                dataReceived = "";
                Console.WriteLine("PDF Mode enabled!");
            }
            else if (dataReceived.Equals("**(tPAUSE)**"))
            {
                TTS.TTS_Pause();
                dataReceived = "";
                Console.WriteLine("TTS Paused!");
            }
            else if (dataReceived.Equals("**(tRESUME)**"))
            {
                TTS.TTS_Resume();
                dataReceived = "";
                Console.WriteLine("TTS Resumed!");
            }
            else if (dataReceived.Equals("**(VBRSTYWord)**"))
            {
                IsVrbstyChar = false;
                dataReceived = "";
            }
            else if (dataReceived.Equals("**VBRSTYChar**"))
            {
                IsVrbstyChar = true;
                dataReceived = "";
            }

            else if (!string.IsNullOrEmpty(dataReceived))
            {
                LookupTable = GetLookupTable();
                dataReceived = TextLineParsing(dataReceived);
                if (IsMisSpelled)
                {
                    dataReceived = "misspelled \r\n" + dataReceived;
                    IsMisSpelled = false;
                }
                if (IsVrbstyChar && !IsError)
                {
                    TTS.VBRCharModeTTSSpeak(dataReceived);
                    IsVrbstyChar = false;
                    return;
                }
                TTS.Speak(dataReceived);
                IsError = false;
            }

        }
        private Dictionary<string, string> GetLookupTable()
        {
            return LookUpFile.GetLookTable();
        }
        private string TextLineParsing(string DataStr)
        {
            string ParsedString = "";
            string[] Lines = DataStr.Split(new string[] { @"\r\n" }, StringSplitOptions.None);
            foreach (string Line in Lines)
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
            //ParsedString = ParsedString.Replace(@"{", LookupTable[@"{"]).Replace(@"}", LookupTable[@"}"]);
            return ParsedString;
        }

        private string LatexParsing(string dataReceived)
        {
            string[] LatexErrorText = dataReceived.Split(' ');
            foreach (string word in LatexErrorText)
            {
                if (word.Contains("\\") || dataReceived.Contains(@"^") || dataReceived.Contains(@"}") || dataReceived.Contains(@"{"))
                {
                    bool hasValue = LookupTable.TryGetValue(word, out ValueByKey);
                    if (hasValue)
                    {
                        dataReceived = dataReceived.Replace(word, ValueByKey);
                        //isMisSpelled = false;
                    }
                    else if (IsError)
                        IsMisSpelled = true;
                }
            }
            return dataReceived;
        }

        public static string ConvertMode(string text, TTSSettings tTSSettings)
        {
            if (tTSSettings.PdfMode)
            {
               // string mathML = MathML.ConvertLatextToMathMl(text);
                PdfMode pdf = new PdfMode(text);
                string str = pdf.filterLatexSourceCodeUsingOpenDetex();
                //PDFModelToDO
                //string pdfString = new PdfMode(mathML)
                //text = MathML
            }
            if (tTSSettings.ByWord)
            {
                text = text.Replace("{", " Start Curly Bracket ");
                text = text.Replace("}", " End Curly Bracket ");
                text = text.Replace("[", " Start Square Bracket ");
                text = text.Replace("}", " End Square Bracket ");
                text = text.Replace("!", " Sign of Explanation ");
                text = text.Replace("(", " Start parenthesis ");
                text = text.Replace(")", " End parenthesis ");
            }
            if (tTSSettings.ByChar && !Settings.IsCommandText(text))
            {
                StringBuilder stringBuilder = new StringBuilder();
                var charArray = text.ToCharArray();
                foreach (var item in charArray)
                {
                    if (item.Equals(' '))
                    {
                        stringBuilder.Append(string.Format("space "));
                    }
                    else if (item.Equals('{'))
                    {
                        stringBuilder.Append(string.Format("Start Curly Bracket "));
                    }
                    else if (item.Equals('}'))
                    {
                        stringBuilder.Append(string.Format("End Curly Bracket "));
                    }
                    else if (item.Equals('['))
                    {
                        stringBuilder.Append(string.Format("Start Square Bracket "));
                    }
                    else if (item.Equals(']'))
                    {
                        stringBuilder.Append(string.Format("End Square Bracket "));
                    }
                    else if (item.Equals('!'))
                    {
                        stringBuilder.Append(string.Format("Sign of Explanation "));
                    }
                    else if (item.Equals('('))
                    {
                        stringBuilder.Append(string.Format("Start parenthesis"));
                    }
                    else if (item.Equals(')'))
                    {
                        stringBuilder.Append(string.Format("End parenthesis"));
                    }
                    else
                    {
                        stringBuilder.Append(string.Format("{0} ", item));
                    }
                    
                }
                
                return stringBuilder.ToString();
            }
            else
            {
                
            }
            return text;
        }
    }
}