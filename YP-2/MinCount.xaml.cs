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
using System.Windows.Shapes;

namespace YP_2
{
    /// <summary>
    /// Логика взаимодействия для MinCount.xaml
    /// </summary>
    public partial class MinCount : Window
    {
        public MinCount(double max)
        {
            InitializeComponent();
            TbNewMinCount.Text = max.ToString();

        }

        double newMinCount = 0;

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            newMinCount = Convert.ToDouble(TbNewMinCount.Text);
            this.Close();
        }

        public double NewMinCount
        {
            get
            {
                return newMinCount;
            }
        }
    }
}
