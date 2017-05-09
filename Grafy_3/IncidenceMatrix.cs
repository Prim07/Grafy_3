using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Grafy_3
{
    class IncidenceMatrix
    {
        // Brzydkoszybka enkapsulacja
        public int[,] IncidenceArray;
        public AdjacencyList adjacencyList;

        public IncidenceMatrix(AdjacencyMatrix sourceMatrix)
        {
            int num_v = sourceMatrix.AdjacencyArray.GetLength(1);
            int num_e = 0;

            // Zliczanie liczby wierzcholkow w macierzy sasiedztwa
            for (int i = 0; i < num_v; i++)
                for (int j = i + 1; j < num_v; j++)
                    if (sourceMatrix.AdjacencyArray[i, j] == 1)
                        num_e++;

            IncidenceArray = new int[num_v, num_e];

            // Zerowanie macierzy incydencji
            for (int i = 0; i < num_v; i++)
                for (int j = 0; j < num_e; j++)
                    IncidenceArray[i, j] = 0;


            int k = 0;

            // Konwersja Macierzy sąsiedztwa -> incydencji
            for (int i = 0; i < num_v; i++)
                for (int j = i + 1; j < num_v; j++)
                    if (sourceMatrix.AdjacencyArray[i, j] == 1)
                    {
                        IncidenceArray[i, k] = -1;
                        IncidenceArray[j, k] = 1;
                        k++;
                    }

        }

        public void Display(StackPanel StackPanelForDisplayingIncidenceMatrix, StackPanel StackPanelForDisplayingAdjacencylist)
        {
            StackPanelForDisplayingIncidenceMatrix.Children.Clear();

            string myString = "";

            for (int i = 0; i < IncidenceArray.GetLength(0); i++)
            {
                for (int j = 0; j < IncidenceArray.GetLength(1); j++)
                {
                    if (IncidenceArray[i, j] < 0)
                        myString += IncidenceArray[i, j].ToString() + "  ";
                    else
                        myString += " " + IncidenceArray[i, j].ToString() + "  ";
                }
                myString += "\n";
            }

            TextBlock myBlock = new TextBlock();
            myBlock.Text = myString;
            myBlock.FontSize = 16;
            myBlock.FontFamily = new FontFamily("Lucida Console");
            StackPanelForDisplayingIncidenceMatrix.Children.Add(myBlock);

            // Nowa lista sąsiedztwa
            adjacencyList = new AdjacencyList(this);
            adjacencyList.Display(StackPanelForDisplayingAdjacencylist);

        }

        internal void Preview(StackPanel StackPanelForPreview)
        {
            StackPanelForPreview.Children.Clear();

            string myString = "";

            for (int i = 0; i < IncidenceArray.GetLength(0); i++)
            {
                for (int j = 0; j < IncidenceArray.GetLength(1); j++)
                {
                    if(IncidenceArray[i, j] < 0)
                        myString += IncidenceArray[i, j].ToString() + "  ";
                    else
                        myString += " " + IncidenceArray[i, j].ToString() + "  ";
                }
                myString += "\n";
            }

            TextBlock titleBlock = new TextBlock();
            titleBlock.Text = "Macierz incydencji";
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

        internal void PreviewAdjacencyList(StackPanel stackPanelForPreview)
        {
            adjacencyList.Preview(stackPanelForPreview);
        }
    }
}
