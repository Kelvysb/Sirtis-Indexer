/*
Copyright 2018 Kelvys B. Pantaleão

This file is part of Sirtis Indexer

Sirtis Indexer Is free software: you can redistribute it And/Or modify
it under the terms Of the GNU General Public License As published by
the Free Software Foundation, either version 3 Of the License, Or
(at your option) any later version.

This program Is distributed In the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty Of
MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License For more details.

You should have received a copy Of the GNU General Public License
along with this program.  If Not, see <http://www.gnu.org/licenses/>.

Este arquivo é parte Do programa Sirtis Indexer

Sirtis Indexer é um software livre; você pode redistribuí-lo e/ou 
modificá-lo dentro dos termos da Licença Pública Geral GNU como 
publicada pela Fundação Do Software Livre (FSF); na versão 3 da 
Licença, ou(a seu critério) qualquer versão posterior.

Este programa é distribuído na esperança de que possa ser  útil, 
mas SEM NENHUMA GARANTIA; sem uma garantia implícita de ADEQUAÇÃO
a qualquer MERCADO ou APLICAÇÃO EM PARTICULAR. Veja a
Licença Pública Geral GNU para maiores detalhes.

Você deve ter recebido uma cópia da Licença Pública Geral GNU junto
com este programa, Se não, veja <http://www.gnu.org/licenses/>.

'GitHub: https://github.com/Kelvysb/Sirtis-Indexer
*/

using BControls;
using SirtisIndexer.basic;
using System;
using System.Windows;
using System.Windows.Controls;

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
                Copy();
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

        private void Copy()
        {
            try
            {
                Clipboard.SetText(Item.ParentFile.Path);
                BMessage.Instance.fnMessage(Properties.Resources.ResourceManager.GetString("msgCopy").ToString(), Properties.Resources.ResourceManager.GetString("AppName").ToString(), MessageBoxButton.OK);
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
