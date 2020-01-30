using System.Collections.Generic;
using Web.ViewModels;
using Document = LatexDocument.Document;

namespace Web.Services
{
    public class PdfLatexService
    {
        public static LatexCompilerResponse CompileLatex(string DocumentText, string LatexFolderPath, string FileName)
        {
            Document lt = new Document(Settings.LatexExecutablePath, LatexFolderPath);
            lt.RecreateDocument(DocumentText);
            string fileName = FileName;
            string logFileName = string.Empty;
            if (lt.CreatePdf(fileName, true))
            {
                fileName = string.Format(@"{0}{1}.pdf", Settings.LatexfolderRelativePath, fileName);
            }
            else
            {
                logFileName = string.Format(@"{0}{1}.log", Settings.LatexfolderRelativePath, fileName);
                return new LatexCompilerResponse {
                    FileName = string.Empty,
                    Status = false ,
                    LogFileName = logFileName
                };
            }
            return new LatexCompilerResponse {
                FileName = fileName ,
                Status = true ,
                LogFileName = logFileName
            };
        }
        
        
    }
}