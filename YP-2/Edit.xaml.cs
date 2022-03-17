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
using Microsoft.Win32;

namespace YP_2
{
    /// <summary>
    /// Логика взаимодействия для Edit.xaml
    /// </summary>
    public partial class Edit : Window
    {
        string path;
        bool IsCreate = false;
        List<MaterialSupplier> MS = Base.CT.MaterialSupplier.ToList();
        public Edit()
        {
            InitializeComponent();
            CbMaterialType.ItemsSource = Base.CT.MaterialType.ToList();
            CbMaterialType.SelectedValuePath = "ID";
            CbMaterialType.DisplayMemberPath = "Title";

            CbSupplier.ItemsSource = Base.CT.Supplier.ToList();
            CbSupplier.SelectedValuePath = "ID";
            CbSupplier.DisplayMemberPath = "Title";
            IsCreate = true;
            LbSupliers.SelectedValuePath = "ID";
            LbSupliers.DisplayMemberPath = "Title";
        }
        Material MaterialEdit = new Material();
        public Edit(Material editImport)
        {
            InitializeComponent();
            MaterialEdit = editImport;

            CbMaterialType.ItemsSource = Base.CT.MaterialType.ToList();
            CbMaterialType.SelectedValuePath = "ID";
            CbMaterialType.DisplayMemberPath = "Title";
            CbMaterialType.SelectedIndex = MaterialEdit.MaterialTypeID - 1;

            TbTitle.Text = MaterialEdit.Title;
            TbCountInStock.Text = MaterialEdit.CountInStock.ToString();
            TbCountInPack.Text = MaterialEdit.CountInPack.ToString();
            TbUnit.Text = MaterialEdit.Unit.ToString();
            TbCost.Text = MaterialEdit.Cost.ToString();
            TbMinCount.Text = MaterialEdit.MinCount.ToString();

            if (MaterialEdit.Image != null)
            {
                BitmapImage BI = new BitmapImage(new Uri(MaterialEdit.Image, UriKind.RelativeOrAbsolute));
                MaterialImage.Source = BI;
            }

            CbSupplier.ItemsSource = Base.CT.Supplier.ToList();
            CbSupplier.SelectedValuePath = "ID";
            CbSupplier.DisplayMemberPath = "Title";


            List<Supplier> s = new List<Supplier>();

            foreach (MaterialSupplier t in Base.CT.MaterialSupplier)
            {
                if (t.MaterialID == MaterialEdit.ID)
                {
                    List<Supplier> tempS = Base.CT.Supplier.Where(x => x.ID == t.SupplierID).ToList();
                    s.AddRange(tempS);
                }
            }

            foreach (Supplier sup in s)
            {
                LbSupliers.Items.Add(sup);
            }
            LbSupliers.SelectedValuePath = "ID";
            LbSupliers.DisplayMemberPath = "Title";
        }

        private void SupplierAddBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Supplier> sup = Base.CT.Supplier.ToList();
            for (int i = 0; i < LbSupliers.Items.Count; i++)
            {
                if (CbSupplier.DisplayMemberPath[CbSupplier.SelectedIndex] == LbSupliers.DisplayMemberPath[i])
                {
                    MessageBox.Show("Поставщик уже добавлен", "Редактирование", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            LbSupliers.Items.Add(sup.FirstOrDefault(x => x.ID == CbSupplier.SelectedIndex + 1));
        }

        private void SupplierRemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            LbSupliers.Items.RemoveAt(LbSupliers.SelectedIndex);
        }

        private void EditImageBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog OFD = new OpenFileDialog();
                OFD.ShowDialog();
                path = OFD.FileName;
                int n = path.IndexOf("materials");
                path = path.Substring(n);
                BitmapImage img = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
                MaterialImage.Source = img;
            }
            catch
            {
                MessageBox.Show("Картинка успешно добавлена", "Редактирование", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MaterialEdit.Title = TbTitle.Text;
                MaterialEdit.MaterialTypeID = CbMaterialType.SelectedIndex + 1;
                MaterialEdit.CountInStock = Convert.ToSingle(TbCountInStock.Text);
                MaterialEdit.Unit = TbUnit.Text;
                MaterialEdit.CountInPack = Convert.ToInt32(TbCountInPack.Text);
                MaterialEdit.MinCount = Convert.ToInt32(TbMinCount.Text);
                MaterialEdit.Cost = Convert.ToDecimal(TbCost.Text);
                MaterialEdit.Description = TbDescription.Text;
                MaterialEdit.Image = path;
                if (IsCreate == true)
                {
                    Base.CT.Material.Add(MaterialEdit);
                }
                Base.CT.SaveChanges();
                List<MaterialSupplier> materialSuppliersOld = MS.Where(x => x.MaterialID == MaterialEdit.ID).ToList();
                if (materialSuppliersOld.Count != 0)
                {
                    foreach (MaterialSupplier ms in materialSuppliersOld)
                    {
                        Base.CT.MaterialSupplier.Remove(ms);
                    }
                }
                foreach (Supplier t in LbSupliers.Items)
                {
                    MaterialSupplier ms = new MaterialSupplier();
                    ms.MaterialID = MaterialEdit.ID;
                    ms.SupplierID = t.ID;
                    Base.CT.MaterialSupplier.Add(ms);
                }
                Base.CT.SaveChanges();
                MessageBox.Show("Данные записаны", "Редактирование", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                this.Close();
                Frames.FrameMain.Navigate(new GoodsList());
            }
            catch
            {
                MessageBox.Show("Не удалось записать данные, повторите попытку", "Редактирование", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsCreate == true)
            {
                MessageBox.Show("Невозможно удалить запись, так как она еще не существует", "Редактирование", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBoxResult.Yes == MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Редактирование", MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                Base.CT.Material.Remove(MaterialEdit);
                Base.CT.SaveChanges();
                MessageBox.Show("Запись успешно удалена!", "Редактирование", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                this.Close();
                Frames.FrameMain.Navigate(new GoodsList());
            }
            else
            {
                return;
            }
        }
    }
}
