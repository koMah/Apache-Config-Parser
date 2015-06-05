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
            ApacheConfigParser parser = new ApacheConfigParser();
            ConfigNode config = parser.Parse(@"D:\wamp\bin\apache\apache2.4.9\conf\extra\httpd-vhosts.conf");

            foreach (ConfigNode child in config.getChildren()) {
                if (child.getName().Equals("VirtualHost")) {
                    Console.WriteLine(child.getContent());
                    foreach (ConfigNode child2 in child.getChildren())
                    {
                        Console.WriteLine(child2.getName());
                        Console.WriteLine(child2.getContent());
                    }
                }
            }
        }
    }
}
