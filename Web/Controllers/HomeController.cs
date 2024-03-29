﻿using System.Web.Mvc;
using Web.Services;
using Web.ViewModels;
using NAudio.Lame;
using NAudio.Wave;
using Document = LatexDocument.Document;
using System.IO;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using System.Globalization;
using System.Threading;
using System;
using System.Web;
using Web.Models;
using TTS_server_alap_alpha_v1;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        #region Dependencies
        public TTSServices tTSServices { get; set; }

        // For installing packages
        string InstallPaclageCommand = @"""C:\Program Files\MiKTeX 2.9\miktex\bin\x64\mpm"" --admin --verbose --package-level=complete --upgrade";
        public TTSSettings TTSSettings
        {
            get
            {
                if (Session["TTSSettings"] == null)
                {
                    Session["TTSSettings"] = new TTSSettings();
                }
                return (TTSSettings)(Session["TTSSettings"] ?? new TTSSettings());
            }
            set
            {
                Session["TTSSettings"] = value;
            }
        }
        public HomeController()
        {
            tTSServices = new TTSServices();
        }
        #endregion


        public string LatexFolderPath
        {
            get
            {
                return Server.MapPath(Settings.LatexfolderRelativePath);
            }
        }
        public string UniqueFileName
        {
            get
            {
                if (Session["FileName"] == null)
                {
                    Session["FileName"] = Util.RandomString;
                    return Session["FileName"].ToString();
                }
                else
                {
                    return Session["FileName"].ToString();
                }
            }
        }
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [HttpPost]
        public ActionResult CompileLatex(LatexEditorViewModel model)
        {
            var PdfFile = PdfLatexService.CompileLatex(model.DocumentText, LatexFolderPath, UniqueFileName);
            PdfFile.LogFileName = PdfFile.LogFileName != string.Empty ? GetLogString(PdfFile.LogFileName) : string.Empty;
            return Json(PdfFile, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSampleLatex()
        {
            string latexDocumentFileName = Util.InitialTemplateLatex;
            return Json(latexDocumentFileName,
                JsonRequestBehavior.AllowGet);

        }

        public ActionResult Compiler()
        {
            return View();
        }

        public ActionResult ByChar()
        {
            TTSSettings.ByChar = true;
            TTSSettings.ByWord = false;
            return Json(TTSSettings, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ByWord()
        {
            TTSSettings.ByChar = false;
            TTSSettings.ByWord = true;
            return Json(TTSSettings, JsonRequestBehavior.AllowGet);
        }


        private string GetLogString(string filename)
        {
            try
            {
                return System.IO.File.ReadAllText(Server.MapPath(filename));
            }
            catch (Exception)
            {

            }
            return string.Empty;
        }

        public ActionResult UpgradePackages()
        {
            try
            {
                ExecuteCmd exe = new ExecuteCmd();
                //exe.ExecuteCommandSync(command);
                //throw new Exception();
                // Execute the command asynchronously.
                exe.ExecuteCommandAsync(InstallPaclageCommand);
                return Json(new
                {
                    error = true,
                    message = @"Packages are updating."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exp)
            {
                return Json(new
                {
                    error = false,
                    message = @"Packages are not updating. Please try again."
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetFilterLatexSourceCodeUsingOpenDetex(string document)
        {
            document = document.Replace("`~`", "\n");
            MathMode mathObj = new MathMode(document);
            string DataRecived = mathObj.filterLatexSourceCodeUsingOpenDetex();
            return Json(DataRecived,JsonRequestBehavior.AllowGet);
        }
        public ActionResult Speak(string message)
        {
            if (String.IsNullOrEmpty(message) || !TTSSettings.State)
                //message = "Type something in first";
                return null;
            //else
            //    message = message.Replace(@"`!", @"\r\n");
            message = TTSServices.ConvertMode(message, TTSSettings);
            return TextToMp3(message);
        }
        public ActionResult ToggleCursorMode()
        {
            if (TTSSettings.CursorMode)
                TTSSettings.CursorMode = false;
            else
                TTSSettings.CursorMode = true;
            string promptClinet = TTSSettings.CursorMode ? Settings.EnableCursorMode : Settings.DisableCursorMode;
            return Json(promptClinet, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TogglePdfMode()
        {
            if (TTSSettings.PdfMode)
                TTSSettings.PdfMode = false;
            else
                TTSSettings.PdfMode = true;
            string promptClinet = TTSSettings.PdfMode ? Settings.EnablePdfMode : Settings.DisablePdfMode;
            // string str = TTSServices.ConvertMode(message, TTSSettings);
            return Json(promptClinet, JsonRequestBehavior.AllowGet);
            
        }
        public ActionResult ToggleTTSState()
        {
            string returnMessage = string.Empty;
            if (TTSSettings.State)
            {
                TTSSettings.State = false;
                returnMessage = Settings.TurnOffCompleteTTSSystem;
            }
            else
            {
                TTSSettings.State = true;
                returnMessage = Settings.TurnOnCompleteTTSSystem;
            }
            return Json(returnMessage,JsonRequestBehavior.AllowGet);
        }
        public FileResult TextToMp3(string text)
        {
            //Primary memory stream for storing mp3 audio
            var mp3Stream = new MemoryStream();
            //Speech format
            var speechAudioFormatConfig = new SpeechAudioFormatInfo(samplesPerSecond: 8000, bitsPerSample: AudioBitsPerSample.Sixteen, channel: AudioChannel.Stereo);
            //Naudio's wave format used for mp3 conversion. Mirror configuration of speech config.
            var waveFormat = new WaveFormat(speechAudioFormatConfig.SamplesPerSecond, speechAudioFormatConfig.BitsPerSample, speechAudioFormatConfig.ChannelCount);
            try
            {
                //Build a voice prompt to have the voice talk slower and with an emphasis on words
                var prompt = new PromptBuilder { Culture = CultureInfo.CreateSpecificCulture("en-US") };
                prompt.StartVoice(prompt.Culture);
                prompt.StartSentence();
                prompt.StartStyle(new PromptStyle() { Emphasis = PromptEmphasis.Reduced, Rate = PromptRate.Medium });
                prompt.AppendText(text);
                prompt.EndStyle();
                prompt.EndSentence();
                prompt.EndVoice();

                //Wav stream output of converted text to speech
                using (var synthWavMs = new MemoryStream())
                {
                    //Spin off a new thread that's safe for an ASP.NET application pool.
                    var resetEvent = new ManualResetEvent(false);
                    ThreadPool.QueueUserWorkItem(arg =>
                    {
                        try
                        {
                            //initialize a voice with standard settings
                            var siteSpeechSynth = new SpeechSynthesizer();
                            //Set memory stream and audio format to speech synthesizer
                            siteSpeechSynth.SetOutputToAudioStream(synthWavMs, speechAudioFormatConfig);
                            //build a speech prompt
                            siteSpeechSynth.Speak(prompt);
                        }
                        catch (Exception ex)
                        {
                            //This is here to diagnostic any issues with the conversion process. It can be removed after testing.
                            Response.AddHeader("EXCEPTION", ex.GetBaseException().ToString());
                        }
                        finally
                        {
                            resetEvent.Set();//end of thread
                        }
                    });
                    //Wait until thread catches up with us
                    WaitHandle.WaitAll(new WaitHandle[] { resetEvent });
                    //Estimated bitrate
                    var bitRate = (speechAudioFormatConfig.AverageBytesPerSecond * 8);
                    //Set at starting position
                    synthWavMs.Position = 0;
                    //Be sure to have a bin folder with lame dll files in there.  They also need to be loaded on application start up via Global.asax file
                    using (var mp3FileWriter = new LameMP3FileWriter(outStream: mp3Stream, format: waveFormat, bitRate: bitRate))
                        synthWavMs.CopyTo(mp3FileWriter);
                }
            }
            catch (Exception ex)
            {
                Response.AddHeader("EXCEPTION", ex.GetBaseException().ToString());
            }
            finally
            {
                //Set no cache on this file
                Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                //required for chrome and safari
                Response.AppendHeader("Accept-Ranges", "bytes");
                //Write the byte length of mp3 to the client
                Response.AddHeader("Content-Length", mp3Stream.Length.ToString(CultureInfo.InvariantCulture));
            }
            //return the converted wav to mp3 stream to a byte array for a file download
            return File(mp3Stream.ToArray(), "audio/mp3");
        }


    }
}
