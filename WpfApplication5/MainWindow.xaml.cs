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

namespace WpfApplication5
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            /*
            try
            {
                VHostManager isn = VHostManager.GetInstance;

            } catch (Exception EXT)
            {
                Console.WriteLine(EXT.ToString());
            }*/
            try
            {
                ApacheConfigParser parser = new ApacheConfigParser();
                ConfigNode config = parser.Parse(@"D:\wamp\bin\apache\apache2.4.9\conf\extra\httpd-vhosts.conf");

                foreach (ConfigNode child in config.GetChildren())
                {
                    Dump.AppendText(child.ToString() + Environment.NewLine + Environment.NewLine);



                    /* if (child.GetName().Equals("VirtualHost")) {
                         Dump.AppendText(child.ToString() + Environment.NewLine + Environment.NewLine);*/
                    /* Console.WriteLine(child.GetContent() + Environment.NewLine);
                     foreach (ConfigNode child2 in child.GetChildren())
                     {
                         Dump.AppendText(child2.GetName() + child2.GetContent() + Environment.NewLine);
                         Console.WriteLine(child2.GetName() + Environment.NewLine);
                         Console.WriteLine(child2.GetContent() + Environment.NewLine);
                     }
                     Dump.AppendText(Environment.NewLine);*/
                    /* }*/
                }
            } catch (Exception Exp)
            {
                Console.WriteLine(Exp.ToString());
            }

        }
    }
}
