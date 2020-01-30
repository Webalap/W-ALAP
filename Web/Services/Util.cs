using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Services
{
    public static class Util
    {
        public static string RandomString
        {
            get
            {
                return GenrateRandomString();
            }
        }

        public static int StringLength
        {
            get
            {
                return 10;
            }
        }

        public static string SampleLatex
        {
            get
            {
                #region sample Latex
            string latexDocument = @"\documentclass{article}
\usepackage[utf8]{inputenc}
\usepackage{graphicx}
\graphicspath{{D:/Latex/images/}}
\usepackage{multicol}
\usepackage{pgf-pie}
\usepackage{pgfplots}
\usepackage{tikz}
\usepackage{wrapfig}
\usepackage{mathtools}
\usepackage{color}
\pgfplotsset{compat=1.15}
\usepackage{lipsum}
\usepackage[tmargin=1in,bmargin=1in,lmargin=1.25in,rmargin=1.25in]{geometry}
\begin{document}
\title{Test File}
\date{18 Giugno 2017}
\author{Simone Luconi}
\maketitle
\newpage
{\Huge Big Title}
\newline
\paragraph{Blue paragraph}
\textcolor{blue}{Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec orci tortor, tempus at vulputate vel, cursus vitae quam. Aliquam tincidunt eget odio id posuere. Donec rhoncus consectetur erat, sed tempus erat interdum a. Aliquam dictum imperdiet massa non vestibulum. Quisque rutrum ligula non nisl maximus fringilla. Proin condimentum fermentum accumsan. Morbi venenatis eros sed mi fermentum efficitur. Donec auctor, diam nec gravida egestas, magna libero interdum enim, at venenatis justo libero sed lectus. Nullam lectus nibh, porttitor ut turpis cursus, eleifend iaculis augue. Vivamus mollis, eros nec sagittis accumsan, quam ipsum hendrerit erat, eget fringilla ante nunc sed metus. Sed hendrerit dui ut ultrices faucibus. Nulla velit turpis, pretium a nibh sit amet, dapibus suscipit magna. Sed feugiat a sapien vitae pellentesque. Nam et dolor nec magna ultrices lobortis. Nulla gravida lobortis magna bibendum volutpat.}
\newline
\textbf{Bold text, }
\textit{Italic text, }
\underline{UnderLine text}
\textcolor{red}{Red text}
\fontfamily{cmss}
\selectfont
\paragraph{Change Font: Computer Modern Sans Serif}
Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec orci tortor, tempus at vulputate vel, cursus vitae quam. Aliquam tincidunt eget odio id posuere. Donec rhoncus consectetur erat, sed tempus erat interdum a. Aliquam dictum imperdiet massa non vestibulum. Quisque rutrum ligula non nisl maximus fringilla. Proin condimentum fermentum accumsan. Morbi venenatis eros sed mi fermentum efficitur. Donec auctor, diam nec gravida egestas, magna libero interdum enim, at venenatis justo libero sed lectus. Nullam lectus nibh, porttitor ut turpis cursus, eleifend iaculis augue. Vivamus mollis, eros nec sagittis accumsan, quam ipsum hendrerit erat, eget fringilla ante nunc sed metus. Sed hendrerit dui ut ultrices faucibus. Nulla velit turpis, pretium a nibh sit amet, dapibus suscipit magna. Sed feugiat a sapien vitae pellentesque. Nam et dolor nec magna ultrices lobortis. Nulla gravida lobortis magna bibendum volutpat.
\fontfamily{garamond}
\selectfont
\newpage
\textcolor{red}{{\Huge Formulas (red title)}}
\newline
$\lim_{x \to \infty} \exp(-x) = 0$
\newline
$\frac{n!}{k!(n-k)!} = \binom{n}{k}$
\newline
$\cos (2\theta) = \cos^2 \theta - \sin^2 \theta$
\newline
\newline
{\Huge Table}
\newline
\newline
\begin{tabular}{ | l | c | r | }
\hline
Pizza & Pane & Spaghetti \\ \hline
1 & 2 & 3 \\ \hline
4 & 5 & 6 \\ \hline
\end{tabular}
\paragraph{}
Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec orci tortor, tempus at vulputate vel, cursus vitae quam. Aliquam tincidunt eget odio id posuere. Donec rhoncus consectetur erat, sed tempus erat interdum a. Aliquam dictum imperdiet massa non vestibulum. Quisque rutrum ligula non nisl maximus fringilla. Proin condimentum fermentum accumsan. Morbi venenatis eros sed mi fermentum efficitur. Donec auctor, diam nec gravida egestas, magna libero interdum enim, at venenatis justo libero sed lectus. Nullam lectus nibh, porttitor ut turpis cursus, eleifend iaculis augue. Vivamus mollis, eros nec sagittis accumsan, quam ipsum hendrerit erat, eget fringilla ante nunc sed metus. Sed hendrerit dui ut ultrices faucibus. Nulla velit turpis, pretium a nibh sit amet, dapibus suscipit magna. Sed feugiat a sapien vitae pellentesque. Nam et dolor nec magna ultrices lobortis. Nulla gravida lobortis magna bibendum volutpat.
\newline
\newline
{\Huge Table (no borders) Wrapped}
\newline
\begin{wrapfigure}{R}{0.3\textwidth}
\centering
\begin{tabular}{ l c r }
Pizza & Pane & Spaghetti \\ 
1 & 2 & 3 \\ 
4 & 5 & 6 \\ 
\end{tabular}
\end{wrapfigure}
\paragraph{}
Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec orci tortor, tempus at vulputate vel, cursus vitae quam. Aliquam tincidunt eget odio id posuere. Donec rhoncus consectetur erat, sed tempus erat interdum a. Aliquam dictum imperdiet massa non vestibulum. Quisque rutrum ligula non nisl maximus fringilla. Proin condimentum fermentum accumsan. Morbi venenatis eros sed mi fermentum efficitur. Donec auctor, diam nec gravida egestas, magna libero interdum enim, at venenatis justo libero sed lectus. Nullam lectus nibh, porttitor ut turpis cursus, eleifend iaculis augue. Vivamus mollis, eros nec sagittis accumsan, quam ipsum hendrerit erat, eget fringilla ante nunc sed metus. Sed hendrerit dui ut ultrices faucibus. Nulla velit turpis, pretium a nibh sit amet, dapibus suscipit magna. Sed feugiat a sapien vitae pellentesque. Nam et dolor nec magna ultrices lobortis. Nulla gravida lobortis magna bibendum volutpat.
\newpage
{\Huge Bullet List}
\newline
\begin{itemize}
\item Pizza
\item Pane
\item Pasta
\item Spaghetti
\end{itemize}
{\Huge Enumerate List}
\newline
\begin{enumerate}
\item Pizza
\item Pane
\item Pasta
\item Spaghetti
\end{enumerate}
{\Huge Descriptive List}
\newline
\begin{description}
\item[Pizza] Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec orci tortor, tempus at vulputate vel, cursus vitae quam. Aliquam tincidunt eget odio id posuere. Donec rhoncus c
\item[Pane] Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec orci tortor, tempus at vulputate vel, cursus vitae quam. Aliquam tincidunt eget odio id posuere. Donec rhoncus c
\item[Pasta] Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec orci tortor, tempus at vulputate vel, cursus vitae quam. Aliquam tincidunt eget odio id posuere. Donec rhoncus c
\item[Spaghetti] Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec orci tortor, tempus at vulputate vel, cursus vitae quam. Aliquam tincidunt eget odio id posuere. Donec rhoncus c
\end{description}
\newpage
{\Huge Pie Graph}
\newline
\newline
\begin{center}
\begin{tikzpicture}
\pie[color={red,green,blue,orange}]{10/Pizza,20/Pane,30/Pasta,40/Spaghetti}
\end{tikzpicture}
\end{center}
{\Huge Bar Graph}
\newline
\newline
\begin{center}
\begin{tikzpicture}
\begin{axis}[
symbolic x coords={
Pizza,Pane,Pasta,Spaghetti},
xtick=data]
\addplot[ybar,fill= blue] coordinates {
(Pizza,10)
(Pane,20)
(Pasta,30)
(Spaghetti,40)
};
\end{axis}
\end{tikzpicture}
\end{center}
\newpage
{\Huge Coordinates Graph}
\newline
\begin{center}
\begin{tikzpicture}
\begin{axis}[
title={Temperature dependence of CuSO$_4\cdot$5H$_2$O solubility},
xlabel={Temperature in celsius},
ylabel={Solubility[g per 100 g water]},
xmin=0, xmax=100,
ymin=0, ymax=100,
xtick={0,20,40,60,80,100},
ytick={0,20,40,60,80,100,120},
legend pos=north west,
ymajorgrids = true,
xmajorgrids = false,
grid style = dashed
]
\addplot[
color = blue,
mark = square,
]
coordinates {
(0,23.1)(10,27.5)(20,32)(30,37.8)(40,44.6)(60,61.8)(80,83.8)(100,114)
};
\addlegendentry{CuSO$_4\cdot$5H$_2$O}
\end{axis}
\end{tikzpicture}
\end{center}
{\Huge Math Graph}
\newline
\begin{center}
\begin{tikzpicture}
\begin{axis}[
ymajorgrids = false,
xmajorgrids = false,
]
\addplot[
color = red,
]
{x^2 - 2*x - 1};
\addlegendentry{$x^2 - 2*x - 1$}
\addplot[
color = blue,
]
{x^2 + 2*x + 1};
\addlegendentry{$x^2 + 2*x + 1$}
\end{axis}
\end{tikzpicture}
\end{center}
\end{document}";
                #endregion
                return latexDocument;
            }
        }

        public static string HelloWorldLatex
        {
            get
            {
                return @"\documentclass[12pt]{article}
\begin{document}
Hello latex!
\end{document}";
            }
        }
        public static string MathMLLatex
        {
            get
            {
                return @"\documentclass[12pt]{article}
\begin{document}
Hello latex!
$\frac{ a} { b} \oint_{ d}^{ c} \left(D \right)\sum_{ W}^{ D}$
\end{document}";
            }
        }
        public static string InitialTemplateLatex
        {
            get
            {
                return @"\documentclass{article}
\begin{document}


\section{Title}

\subsection{Subtitle}

Plain text.

\subsection{Another subtitle}

More plain text.


\end{document}";
            }
        }

        private static string GenrateRandomString()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, StringLength)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }




    }
}