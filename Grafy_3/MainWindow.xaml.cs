using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace Grafy_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //deklaracje wartości publicznych, widoczne w całym programie i w każdej funckji
        private AdjacencyMatrix adjacencyMatrix;
        public List<ComboBox> ListOfLeftComboBoxes;
        public List<ComboBox> ListOfRightComboBoxes;
        public int LeftComboBoxValue = 0;



        public MainWindow()
        {
            InitializeComponent();
        }

        // Losowanie grafu G(n,p)
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (Number_Of_Vertex.Text != "0" && Number_Of_Vertex.Text != "")           // jesli cokolwiek jest w polu podania wierzcholkow
            {
                var howManyVertex = Int32.Parse(Number_Of_Vertex.Text);     // rzutowanie wartosci na inta, bo tak jest fajnie
                adjacencyMatrix = new AdjacencyMatrix(howManyVertex);       // stworzenie macierzy sasiedztwa 
                Number_Of_Vertex.Background = Brushes.White;                // tlo pola białe- dobrze wpisaliśmy, jeeej!
                
                StackPanelWithConnections.Children.Clear();                 //?

                bool success = false;                                       // sprawdza czy wylosowaliśmy graf spójny
                while (!success)
                    success = Connected_Graph(howManyVertex,adjacencyMatrix);


            }
            else
            {
                Number_Of_Vertex.Background = Brushes.OrangeRed;
                
            }

            adjacencyMatrix.Display(StackPanelForDisplayingAdjacencyMatrix, MyCanvas, StackPanelForDisplayingIncidenceMatrix, StackPanelForDisplayingAdjacencylist);


         }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////   rzeczy Artura z 1. zestawu, podobno wszystko dobrze i nie ruszać    //////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            private void GenerateConnections(int num_v, int num_e)
            {

                StackPanelWithConnections.Children.Clear();

                adjacencyMatrix = new AdjacencyMatrix(num_v);
                adjacencyMatrix.Display(StackPanelForDisplayingAdjacencyMatrix, MyCanvas, StackPanelForDisplayingIncidenceMatrix, StackPanelForDisplayingAdjacencylist);


                TextBlock tmpBlock = new TextBlock();
                tmpBlock.Text = "Uzupełnij połączenia:";
                tmpBlock.VerticalAlignment = VerticalAlignment.Center;
                tmpBlock.Margin = new Thickness(10, 20, 0, 20);
                StackPanelWithConnections.Children.Add(tmpBlock);

                int j = 0;

                ListOfLeftComboBoxes = new List<ComboBox>();
                ListOfRightComboBoxes = new List<ComboBox>();

                for (int i = 0; i < num_e; i++)
                {
                    StackPanel stackPanelForConn = new StackPanel();
                    stackPanelForConn.Orientation = Orientation.Horizontal;
                    stackPanelForConn.Margin = new Thickness(20, 0, 0, 0);





                    StackPanelWithConnections.Children.Add(stackPanelForConn);

                    j += 2;
                }

            }

            // Uzupełnienie ComboBoxa danymi
            private void SetComboBox(int e, ComboBox comboBox)
            {
                comboBox.Width = 40;
                for (int i = 1; i <= e; i++)
                {
                    ComboBoxItem newItem = new ComboBoxItem();
                    newItem.Content = i;
                    comboBox.Items.Add(newItem);
                }
            }

            // Jeśli rozwiniemy listę w "połączenie od: "
            private void FromComboBox_DropDownOpened(object sender, EventArgs e)
            {
                ComboBox myComboBox = sender as ComboBox;

                if (myComboBox.SelectedItem != null)
                {
                    ComboBoxItem typeItemLeft = (ComboBoxItem)myComboBox.SelectedItem;
                    int previousLeftValue = Int32.Parse(typeItemLeft.Content.ToString());

                    var comboBoxRight = ListOfLeftComboBoxes.Find(x => (int)x.Tag == (int)myComboBox.Tag + 1);
                    ComboBoxItem typeItemRight = (ComboBoxItem)comboBoxRight.SelectedItem;
                    int previosuRightValue = Int32.Parse(typeItemRight.Content.ToString());

                    adjacencyMatrix.AdjacencyArray[previousLeftValue - 1, previosuRightValue - 1] = 0;
                    adjacencyMatrix.AdjacencyArray[previosuRightValue - 1, previousLeftValue - 1] = 0;

                }

            }



            // Rowniez sprawdza Num_of_E, nie tylko Num_of_V
            private void Num_of_V_PreviewTextInput(object sender, TextCompositionEventArgs e)
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }


            private void StackPanelForDisplayingAdjacencyMatrix_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            {
                StackPanelForPreview.Visibility = Visibility.Visible;
                PreviewScrollViewer.Visibility = Visibility.Visible;
                adjacencyMatrix.Preview(StackPanelForPreview);
            }

            private void StackPanelForDisplayingIncidenceMatrix_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            {
                StackPanelForPreview.Visibility = Visibility.Visible;
                PreviewScrollViewer.Visibility = Visibility.Visible;
                adjacencyMatrix.PreviewIncidence(StackPanelForPreview);
            }

            private void StackPanelForDisplayingAdjacencylist_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            {
                StackPanelForPreview.Visibility = Visibility.Visible;
                PreviewScrollViewer.Visibility = Visibility.Visible;
                adjacencyMatrix.PreviewAdjacencyList(StackPanelForPreview);
            }

            private void StackPanelForPreview_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            {
                StackPanel stackPanelForPreview = sender as StackPanel;
                stackPanelForPreview.Visibility = Visibility.Hidden;
                PreviewScrollViewer.Visibility = Visibility.Hidden;

            }

        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////

        bool Connected_Graph(int howManyVertex, AdjacencyMatrix adjacencyMatrix)
        {

            ///////////////////////////////////////////////////////////////////////
            ////////////////        losowanie grafu     /////////////////////////////
            ///////////////////////////////////////////////////////////////////////

            //fragmant kodu Artura, zestaw 1.
            // tworzenie macierzy sąsiedztwa, w której są wagi krawędzi 

            Random r = new Random();    // rand do prawdopodobienstwa
            Random r1 = new Random();   // rand do wag krawędzi

            for (int i = 0; i < howManyVertex; i++)
            {
                for (int j = i + 1; j < howManyVertex; j++)
                {
                    int probability = r.Next(0, 100);

                    if (probability > 40)
                    {
                        adjacencyMatrix.AdjacencyArray[i, j] = 0;
                        adjacencyMatrix.AdjacencyArray[j, i] = 0;
                    }
                    else
                    {
                        adjacencyMatrix.AdjacencyArray[i, j] = r1.Next(1, 10);
                        adjacencyMatrix.AdjacencyArray[j, i] = r1.Next(1, 10);
                    }
                    adjacencyMatrix.AdjacencyArray[i, i] = 0;
                }
            }


            ////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////        sprawdzanie jaka jest najwieksza spojna skladowa   /////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Fragment z kodu Pauli, zestaw 2.

            int v = howManyVertex;

            Stack<int> stos = new Stack<int>();
            int cn = 0;
            int[] c = new int[v];
            for (int i = 0; i < v; i++)
            {
                c[i] = 0;
            }
            for (int i = 0; i < v; i++)
            {
                if (c[i] > 0)
                {
                    continue;
                }
                cn++;
                stos.Push(i);
                c[i] = cn;
                while (stos.Count > 0)
                {
                    int vv = stos.Pop();
                    List<int> neighbours = new List<int>();
                    for (int j = 0; j < v; j++)
                    {
                        if (adjacencyMatrix.AdjacencyArray[vv, j] != 0)         // zmiana z == 1 do != 0
                        {
                            neighbours.Add(j);
                        }
                    }
                    for (int j = 0; j < neighbours.Count; j++)
                    {
                        if (c[neighbours[j]] > 0)
                        {
                            continue;
                        }
                        stos.Push(neighbours[j]);
                        c[neighbours[j]] = cn;
                    }

                }


                if (c.Count(x => x == cn) == v)
                    break;
            }

            int max = 0, maxval = 0;
            for (int i = 1; i <= cn; i++)
            {
                if (c.Count(x => x == i) > max)
                {
                    max = c.Count(x => x == i);
                    maxval = i;
                }
            }

            List<int> doWypisania = new List<int>();
            int ile = 0;
            for (int i = 0; i < v; i++)
            {
                if (c[i] == maxval)
                {
                    doWypisania.Add(i + 1);
                    ile++;
                }
            }
            
            // jesli NSS nie jest równe liczbie wierzcholkow to zwracamy false- funkcja bedzie wykonywała się kolejny raz, 
            // aż wygeneruje taki graf losowy dla którego NSS = liczbie wierzcholkow 
            if (max != howManyVertex)
                return false;
             return true;


        }

    }


}






