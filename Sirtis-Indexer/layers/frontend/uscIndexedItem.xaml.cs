using BControls;
using SirtisIndexer.basic;
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

namespace SirtisIndexer.layers.frontend
{
    /// <summary>
    /// Interaction logic for uscIndexedItem.xaml
    /// </summary>
    public partial class uscIndexedItem : UserControl
    {

        #region Declarations

        #endregion

        #region Constructor
        public uscIndexedItem()
        {
            try
            {
                InitializeComponent();
                Item = new IndexedItem();
                Update();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public uscIndexedItem(IndexedItem p_objItem)
        {
            try
            {
                InitializeComponent();
                Item = p_objItem;
                Update();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Events
        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Info();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void txtValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                UpdateField();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        #endregion

        #region Functions
        public void Update()
        {
            lblFlat.Text = Item.Flat;
            txtValue.Text = Item.Value;
            lblFile.Text = Item.ParentFile.Name;
            if (Item.Modified)
            {
                lblModifiedIndicator.Content = "*";
            }
            else{
                lblModifiedIndicator.Content = "";
            }
        }

        private void Info()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(Properties.Resources.ResourceManager.GetString("msgError").ToString(), ex);
            }
        }

        private void UpdateField()
        {
            try
            {

                if (!txtValue.Text.Equals(Item.Value))
                {
                    Item.Value = txtValue.Text;
                    Item.Modified = true;
                    lblModifiedIndicator.Content = "*";                    
                }

            }
            catch (Exception ex)
            {
                throw new Exception(Properties.Resources.ResourceManager.GetString("msgError").ToString(), ex);
            }
        }
        #endregion

        #region Properties
        public IndexedItem Item { get; set; }
        #endregion

        
    }
}
