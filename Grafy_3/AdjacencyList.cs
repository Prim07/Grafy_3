using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Grafy_3
{
    class AdjacencyList
    {
        //Brzydkoszybka enkapsulacja
        //n list gdzie n to liczba wierzchołków
        //public List<int>[] ArrayOfLists;
        public List<List<int>> ListOfLists;

        public AdjacencyList(IncidenceMatrix sourceMatrix)
        {

            // Pobranie liczby krawędzi i wierzchołków
            int num_e = sourceMatrix.IncidenceArray.GetLength(1);
            int num_v = sourceMatrix.IncidenceArray.GetLength(0);

            // Zmienne pomocnicze
            int tmp1 = 0, tmp2 = 0;

            // Alkokowanei pamięci pod listę list
            ListOfLists = new List<List<int>>();

            // Czyścimy wszystkie listy w tablicy
            for (int i = 0; i < num_v; i++)
                ListOfLists.Add(new List<int>());

            //Konwersja Macierzy Incydencji -> Lista sąsiedztwa
            for (int j = 0; j < num_e; j++)
            {
                for (int i = 0; i < num_v; i++)
                {
                    if (sourceMatrix.IncidenceArray[i, j] == 1)
                        tmp2 = i;
                    else if (sourceMatrix.IncidenceArray[i, j] == -1)
                        tmp1 = i;
                }
                ListOfLists[tmp2].Add(tmp1 + 1);
                ListOfLists[tmp1].Add(tmp2 + 1);
            }
            
        }

        public void Display(StackPanel StackPanelForDisplayingAdjacencyList)
        {
            StackPanelForDisplayingAdjacencyList.Children.Clear();

            string myString = "";

            for (int i = 0; i < ListOfLists.Count; i++)
            {
                myString += (i+1).ToString();
                for (int j = 0; j < ListOfLists[i].Count; j++)
                {
                    myString += " → " +  ListOfLists[i][j].ToString();
                }
                myString += " ↴\n";
            }

            TextBlock myBlock = new TextBlock();
            myBlock.Text = myString;
            myBlock.FontSize = 16;
            myBlock.FontFamily = new FontFamily("Lucida Console");
            StackPanelForDisplayingAdjacencyList.Children.Add(myBlock);
        }

        internal void Preview(StackPanel StackPanelForPreview)
        {
            StackPanelForPreview.Children.Clear();

            string myString = "";

            for (int i = 0; i < ListOfLists.Count; i++)
            {
                myString += (i + 1).ToString();
                for (int j = 0; j < ListOfLists[i].Count; j++)
                {
                    myString += " → " + ListOfLists[i][j].ToString();
                }
                myString += " ↴\n";
            }

            TextBlock titleBlock = new TextBlock();
            titleBlock.Text = "Lista sąsiedztwa";
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
    }
}
