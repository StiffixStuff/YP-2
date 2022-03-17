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
using Word = Microsoft.Office.Interop.Word;

namespace YP_2
{
    /// <summary>
    /// Логика взаимодействия для GoodsList.xaml
    /// </summary>
    public partial class GoodsList : Page
    {
        List<Material> ServiceStart = Base.CT.Material.ToList();
        public GoodsList()
        {
            InitializeComponent();
            Goods.ItemsSource = ServiceStart;
            CbFilt.Items.Add("Все типы");
            List<MaterialType> mt = Base.CT.MaterialType.ToList();
            for (int i = 0; i < mt.Count; i++)
            {
                CbFilt.Items.Add(mt[i].Title);
            }
            CbFilt.SelectedIndex = 0;
            TbCount.Text = "Записей: " + ServiceStart.Count().ToString() + " из " + ServiceStart.Count().ToString();
        }

        private void TbSupplier_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            int index = Convert.ToInt32(tb.Uid);
            List<MaterialSupplier> mtList = Base.CT.MaterialSupplier.Where(x => x.MaterialID == index).ToList();
            string str = "";
            foreach (MaterialSupplier item in mtList)
            {
                str += item.Supplier.Title + ", ";
            }
            if (mtList.Count == 0)
            {
                tb.Text = "Поставщики: отсутствуют";
            }
            else
            {
                tb.Text = "Поставщики: " + str.Substring(0, str.Length - 2);
            }
        }

        List<Material> MatFilter = new List<Material>();

        List<Material> MatSearch = new List<Material>();
        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TbSearch.Text != String.Empty)
            {
                MatSearch = ServiceStart.Where(x => x.Title.Contains(TbSearch.Text)).ToList();
                FilterSort();
            }
            else
            {
                FilterSort();
            }
        }
        private void CbFilt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterSort();
        }
        private void CbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterSort();
        }
        private void FilterSort()
        {
            int filterIndex = CbFilt.SelectedIndex;

            if (TbSearch.Text != String.Empty)
            {
                if (filterIndex != 0)
                {
                    MatFilter = MatSearch.Where(x => x.MaterialTypeID == filterIndex).ToList();
                }
                else
                {
                    MatFilter = MatSearch;
                }
            }
            else
            {
                if (filterIndex != 0)
                {
                    MatFilter = ServiceStart.Where(x => x.MaterialTypeID == filterIndex).ToList();
                }
                else
                {
                    MatFilter = ServiceStart;
                }
            }

            switch (CbSort.SelectedIndex)
            {
                case 0:
                    MatFilter.Sort((x, y) => x.Title.CompareTo(y.Title));
                    break;
                case 1:
                    MatFilter.Sort((x, y) => x.Title.CompareTo(y.Title));
                    MatFilter.Reverse();
                    break;
                case 2:
                    MatFilter.Sort((x, y) => x.Cost.CompareTo(y.Cost));
                    break;
                case 3:
                    MatFilter.Sort((x, y) => x.Cost.CompareTo(y.Cost));
                    MatFilter.Reverse();
                    break;
                case 4:
                    MatFilter.Sort((x, y) => x.CountInStock.CompareTo(y.CountInStock));
                    break;
                case 5:
                    MatFilter.Sort((x, y) => x.CountInStock.CompareTo(y.CountInStock));
                    MatFilter.Reverse();
                    break;
            }
            Goods.ItemsSource = MatFilter;
            Goods.Items.Refresh();
            TbCount.Text = "Записей: " + MatFilter.Count().ToString() + " из " + ServiceStart.Count().ToString();
        }
        private void AddClick(object sender, RoutedEventArgs e)
        {
            Edit editWindow = new Edit();
            editWindow.ShowDialog();
            Goods.Items.Refresh();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Button B = (Button)sender;
            int id = Convert.ToInt32(B.Uid);
            Material MaterialEdit = Base.CT.Material.FirstOrDefault(y => y.ID == id);
            Edit editWindow = new Edit(MaterialEdit);
            editWindow.ShowDialog();
            Goods.Items.Refresh();
        }

        private void EditMinBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedList = Goods.SelectedItems;
            double maxMc = 0;
            foreach (Material mC in selectedList)
            {
                if (mC.MinCount > maxMc)
                {
                    maxMc = mC.MinCount;
                }
            }
            MinCount mCWin = new MinCount(maxMc);
            mCWin.ShowDialog();
            if (mCWin.NewMinCount > 0)
            {
                foreach (Material mC in selectedList)
                {
                    mC.MinCount = mCWin.NewMinCount;
                }
                Goods.Items.Refresh();
            }
        }

        private void Goods_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Goods.SelectedIndex != -1)
            {
                EditMinBtn.Visibility = Visibility.Visible;
            }
            else
            {
                EditMinBtn.Visibility = Visibility.Hidden;
            }
        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            Word wordWindow = new Word();
            wordWindow.ShowDialog();
        }
    }
}
