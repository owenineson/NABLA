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
using System.IO;
using System.IO.Pipes;
using System.Xaml;
using Microsoft.Win32;


namespace NABLA.ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            PipeServer ps = new PipeServer();
            FlowDocument netlistDocument = RichTextBox_NetlistInput.Document;

            string netlistString = new TextRange(netlistDocument.ContentStart, netlistDocument.ContentEnd).Text;

            ps.WriteToPipe(netlistString);

            TextBox_Output.Text = ps.ReadFromPipe();

        }

        private void MenuItem_MasterControl_File_Save_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_MasterControl_File_Open_Clicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Netlist Files (*.txt)|*.txt";


            if (openFileDialog.ShowDialog() == true)
            {
                System.Uri uri1 = new Uri(@openFileDialog.FileName);

                System.Uri uri2 = new Uri(@"C:\NABLA.sim");



                Uri relativeUri = uri2.MakeRelativeUri(uri1);



                Console.WriteLine(relativeUri.ToString());

                RichTextBox_NetlistInput.Document = Application.LoadComponent(new Uri(openFileDialog.FileName)) as FlowDocument;
            }
        }
    }
}
