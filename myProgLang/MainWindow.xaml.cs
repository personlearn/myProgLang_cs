using myProgLang.stone;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace myProgLang
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            string str = textBox1.Text;
            byte[] strinput = Encoding.ASCII.GetBytes(str);
            MemoryStream stream = new MemoryStream(strinput);
            Lexer l = new Lexer(stream);
            for (Token t; (t = l.read()) != Token.EOF;)
            {
                sb.Append("=>" + t.getText());
            }
            textBox2.Text = sb.ToString();
        }
    }
}
