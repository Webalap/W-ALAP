using LatexMath2MathML;
using System;
using System.Threading;

/**
* @author $Ahtsham Manzoor$
*/

namespace TTS_server_alap_alpha_v1
{
    public class MathML
    {
        private string Output;
        private LatexMathToMathMLConverter lmm;
        public string ConvertLatextToMathMl(string latexExp)
        {
            //For demo Try with following Expression
            String latexExpression = @"\begin{document}"
                                        + latexExp
                                        + @"\end{document}";
            lmm = new LatexMathToMathMLConverter();
            lmm.ValidateResult = true;
            lmm.BeforeXmlFormat += MyEventListener;
            lmm.ExceptionEvent += ExceptionListener;
            lmm.Convert(latexExpression : latexExpression);
            return Output;
        }

        private void ExceptionListener(object sender, ExceptionEventArgs e)
        {
            //this.Output = e.Message;
        }

        private void MyEventListener(object sender, EventArgs e)
        {
            //Console.WriteLine("called .");
            this.Output = lmm.Output;


        }
    }
}
