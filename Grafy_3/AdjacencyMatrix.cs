using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Grafy_3
{
    class AdjacencyMatrix
    {
        // Brzydkoszybka enkapsulacja
        public int[,] AdjacencyArray;
        private IncidenceMatrix incidenceMatrix;

        // Konstruktor
        public AdjacencyMatrix(int n)
        {
            AdjacencyArray = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    AdjacencyArray[i, j] = 0;
                }
            }
        }

        // Wyświetlanie macierzy na stack panelu 
        // Canvas przekazany, by móc wywołać funkcję DrawGraph(Canvas) później
        // Drugi StackPanel przekazany, by móc później wyświetlić macierz incydencji
        //      (jest ona więc tutaj też na nowo tworzona, bo za każdą zmianą w wyświetlaniu
        //      macierzy sąsiedztwa zmienia sie również macierz incydencji
        public void Display(StackPanel StackPanelForDisplayingAdjacencyMatrix, Canvas MyCanvas, StackPanel StackPanelForDisplayingIncidenceMatrix, StackPanel StackPanelForDisplayingAdjacencylist)
        {
            StackPanelForDisplayingAdjacencyMatrix.Children.Clear();

            string myString = "";

            for (int i = 0; i < AdjacencyArray.GetLength(0); i++)
            {
                for (int j = 0; j < AdjacencyArray.GetLength(1); j++)
                {
                    myString += AdjacencyArray[i, j].ToString() + "  ";
                }
                myString += "\n";
            }

            TextBlock myBlock = new TextBlock();
            myBlock.Text = myString;
            myBlock.FontSize = 16;
            myBlock.FontFamily = new FontFamily("Lucida Console");
            StackPanelForDisplayingAdjacencyMatrix.Children.Add(myBlock);

            DrawGraph(AdjacencyArray.GetLength(0), MyCanvas);

            // Nowa maceirz incydencji
            incidenceMatrix = new IncidenceMatrix(this);
            incidenceMatrix.Display(StackPanelForDisplayingIncidenceMatrix, StackPanelForDisplayingAdjacencylist);

        }

        // Wizualizacja grafu na macierzy
        private void DrawGraph(int num_v, Canvas MyCanvas)
        {
            MyCanvas.Children.Clear();

            var width = MyCanvas.Width;
            var height = MyCanvas.Height;

            Ellipse myEllipse = new Ellipse();
            myEllipse.Height = 400;
            myEllipse.Width = 400;
            myEllipse.Fill = Brushes.Transparent;
            myEllipse.StrokeThickness = 2;
            myEllipse.Stroke = Brushes.LightGray;
            Canvas.SetLeft(myEllipse, width / 2 - 200);
            Canvas.SetTop(myEllipse, height / 2 - 200);
            MyCanvas.Children.Add(myEllipse);

            var r = 200;    //radius

            var x_m = width / 2;    //x middle
            var y_m = height / 2;   //y middle
            

            for (int i = 1; i <= num_v; i++)
            {
                var angle = (2 * Math.PI) / num_v * i;

                var x_oc = r * Math.Cos(angle) + x_m;   //x on cirlce
                var y_oc = r * Math.Sin(angle) + y_m;   //y on circle

                Ellipse smallPoint = new Ellipse();
                smallPoint.Height = 8;
                smallPoint.Width = 8;
                smallPoint.Fill = Brushes.Black;
                smallPoint.StrokeThickness = 1;
                smallPoint.Stroke = Brushes.Black;
                Canvas.SetLeft(smallPoint, x_oc - 3);
                Canvas.SetTop(smallPoint, y_oc - 3);

                TextBlock smallPointNumber = new TextBlock();
                smallPointNumber.Text = i.ToString();
                smallPointNumber.RenderTransform = new TranslateTransform
                {
                    X = (r + 10) * Math.Cos(angle) + x_m,
                    Y = (r + 15) * Math.Sin(angle) + y_m - 8
                };

                for (int j = i; j <= AdjacencyArray.GetLength(0); j++)
                {
                    if (AdjacencyArray[i - 1, j - 1] != 0)                      //      tu zmieniliśmy z == 1 na !=0
                    {
                        var angle_2 = (2 * Math.PI) / num_v * j;

                        var x_oc_2 = r * Math.Cos(angle_2) + x_m;   //x on cirlce
                        var y_oc_2 = r * Math.Sin(angle_2) + y_m;   //y on circle

                        Line myLine = new Line();
                        myLine.Stroke = Brushes.Black;
                        myLine.StrokeThickness = 3;
                        myLine.X1 = x_oc;
                        myLine.Y1 = y_oc;
                        myLine.X2 = x_oc_2;
                        myLine.Y2 = y_oc_2;

                        MyCanvas.Children.Add(myLine);
                    }
                }

                MyCanvas.Children.Add(smallPoint);
                MyCanvas.Children.Add(smallPointNumber);

            }
        }

        //Podgląd m. sasiedztwa
        public void Preview(StackPanel StackPanelForPreview)
        {
            StackPanelForPreview.Children.Clear();

            string myString = "";

            for (int i = 0; i < AdjacencyArray.GetLength(0); i++)
            {
                for (int j = 0; j < AdjacencyArray.GetLength(1); j++)
                {
                    myString += AdjacencyArray[i, j].ToString() + "  ";
                }
                myString += "\n";
            }

            TextBlock titleBlock = new TextBlock();
            titleBlock.Text = "Macierz sąsiedztwa";
            titleBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            titleBlock.FontSize = 48;
            titleBlock.FontFamily = new FontFamily("Times New Roman");
            titleBlock.Margin = new System.Windows.Thickness(0, 20, 0, 50);
            StackPanelForPreview.Children.Add(titleBlock);

            TextBlock myBlock = new TextBlock();
            myBlock.Text = myString;
            myBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            myBlock.FontSize = 16;
            myBlock.FontFamily = new FontFamily("Lucida Console");
            StackPanelForPreview.Children.Add(myBlock);

            StackPanelForPreview.Height = titleBlock.Height + myBlock.Height;
            StackPanelForPreview.Width = titleBlock.Width + myBlock.Width;
        }

        //Podgląd m. incydencji
        internal void PreviewIncidence(StackPanel stackPanelForPreview)
        {
            incidenceMatrix.Preview(stackPanelForPreview);
        }

        //Podgląd listy
        internal void PreviewAdjacencyList(StackPanel stackPanelForPreview)
        {
            incidenceMatrix.PreviewAdjacencyList(stackPanelForPreview);
        }
    }
}
