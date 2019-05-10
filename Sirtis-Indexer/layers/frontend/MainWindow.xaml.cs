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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SirtisIndexer.layers.backend;
using SirtisIndexer.basic;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Media.Animation;
using SirtisIndexer.layers.frontend;

namespace SirtisIndexer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Declarations       
        private List<uscIndexedItem> objIndexedItens;
        bool blnLoaded;
        #endregion

        #region Constructor
        public MainWindow()
        {

            try
            {
                InitializeComponent();

                blnLoaded = false;

                BMessage.sbInitialize((Brush)FindResource("BaseColor"),
                                      (Brush)FindResource("BackColor"),
                                      (Brush)FindResource("FontColorDark"),
                                      (Brush)FindResource("FontColor"),
                                      (FontFamily)FindResource("Font"),
                                      SirtisIndexerBO.Instance.WorkDirectory + "\\Log");

                objIndexedItens = new List<uscIndexedItem>();

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Events
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadPage();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFolder();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Save();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Filter();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void txtFilter_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    Filter();
                }
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void chkModified_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                Filter();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                About();
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                ClosingProgram(e);
            }
            catch (Exception ex)
            {
                BMessage.Instance.fnErrorMessage(ex);
            }
        }
        #endregion

        #region Functions
        private void LoadPage()
        {
            try
            {
                txtFilter.ItemsSource = new List<string>();
                ((Storyboard)FindResource("WaitStoryboard")).Begin();
                blnLoaded = true;
            }
            catch (Exception ex)
            {
                throw new Exception(Properties.Resources.ResourceManager.GetString("msgError").ToString(), ex);
            }
        }

        private void OpenFolder()
        {
            CommonOpenFileDialog objdialog;

            try
            {

                objdialog = new CommonOpenFileDialog();
                objdialog.Title = "Select Folder";
                objdialog.IsFolderPicker = true;

                if (objdialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    blnLoaded = false;
                    loading.Visibility = Visibility.Visible;
                    SirtisIndexerBO.Instance.ProcessFolder(objdialog.FileName).ContinueWith((result) => {
                        Dispatcher.Invoke(() => 
                            {
                                FolderLoaded();
                            });
                    });
                }

                objdialog = null;

            }
            catch (Exception ex)
            {
                throw new Exception(Properties.Resources.ResourceManager.GetString("msgOpenError").ToString(), ex);
            }
        }

        private void FolderLoaded()
        {
            try
            {

                txtFilter.ItemsSource = SirtisIndexerBO.Instance.AutoCompleteSource;

                stkItens.Children.Clear();
                objIndexedItens.Clear();

                foreach (IndexedItem item in SirtisIndexerBO.Instance.IndexedFolder.Itens)
                {
                    objIndexedItens.Add(new uscIndexedItem(item));
                    objIndexedItens.Last().Margin = new Thickness(2);
                    stkItens.Children.Add(objIndexedItens.Last());
                }


                txtFilter.Text = "";
                blnLoaded = true;
                Clear();
                loading.Visibility = Visibility.Collapsed;

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Filter()
        {
            List<uscIndexedItem> objFilteredResults;

            try
            {

                if (blnLoaded)
                {

                    objFilteredResults = new List<uscIndexedItem>();

                    foreach (uscIndexedItem item in objIndexedItens)
                    {
                        item.Visibility = Visibility.Visible;
                    }

                    //filter modified
                    if (chkModified.IsChecked == true)
                    {
                        objFilteredResults.AddRange(objIndexedItens.FindAll(item => item.Item.Modified != true));
                    }

                    //Filter flat
                    if(txtFilter.Text.Trim() != "")
                    {
                        objFilteredResults.AddRange(objIndexedItens.FindAll(item => !item.Item.Flat.StartsWith(txtFilter.Text.Trim(), StringComparison.InvariantCultureIgnoreCase)));
                    }

                    //Filter value
                    if (txtFilterValue.Text.Trim() != "")
                    {
                        objFilteredResults.AddRange(objIndexedItens.FindAll(item => !item.Item.Value.Trim().ToUpper().Contains(txtFilterValue.Text.Trim().ToUpper())));
                    }

                    //Filter field
                    if (txtFilterField.Text.Trim() != "")
                    {
                        objFilteredResults.AddRange(objIndexedItens.FindAll(item => !item.Item.Alias.Trim().ToUpper().Contains(txtFilterField.Text.Trim().ToUpper())));
                    }

                    //Filter file
                    if (txtFilterFile.Text.Trim() != "")
                    {
                        objFilteredResults.AddRange(objIndexedItens.FindAll(item => !item.Item.ParentFile.Name.Trim().ToUpper().Contains(txtFilterFile.Text.Trim().ToUpper())));
                    }

                    foreach (uscIndexedItem item in objFilteredResults)
                    {
                        item.Visibility = Visibility.Collapsed;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(Properties.Resources.ResourceManager.GetString("msgError").ToString(), ex);
            }
        }

        private void Save()
        {

            List<IndexedItem> objModificated;
            try
            {
                objModificated =  SirtisIndexerBO.Instance.IndexedFolder.Itens.FindAll((item) => { return item.Modified; });
                if(objModificated.Count > 0)
                {
                    loading.Visibility = Visibility.Visible;
                    SirtisIndexerBO.Instance.SaveModifications(objModificated).ContinueWith((result) =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            SetModifications(result.Result);
                        });
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Properties.Resources.ResourceManager.GetString("msgSaveError").ToString(), ex);
            }
        }

        private void SetModifications(bool p_blnSucess)
        {
            List<uscIndexedItem> objModificated;

            try
            {

                loading.Visibility = Visibility.Collapsed;
                if (p_blnSucess)
                {
                    objModificated = objIndexedItens.FindAll((item) => { return item.Item.Modified; });
                    foreach (uscIndexedItem item in objModificated)
                    {
                        item.Item.Modified = false;
                        item.Update();
                    }
                    BMessage.Instance.fnMessage(Properties.Resources.ResourceManager.GetString("msgSaved").ToString(), Properties.Resources.ResourceManager.GetString("AppName").ToString(), MessageBoxButton.OK);
                }
                else
                {
                    BMessage.Instance.fnMessage(Properties.Resources.ResourceManager.GetString("msgSaveError").ToString(), Properties.Resources.ResourceManager.GetString("AppName").ToString(), MessageBoxButton.OK);
                }
            }
            catch (Exception)
            {                
                throw;
            }
        }

        private void Clear()
        {
            try
            {
                txtFilter.Text = "";
                txtFilterField.Text = "";
                txtFilterFile.Text = "";
                chkModified.IsChecked = false ;
                Filter();
            }
            catch (Exception ex)
            {
                throw new Exception(Properties.Resources.ResourceManager.GetString("msgError").ToString(), ex);
            }
        }

        private void About()
        {

            About objAbout;
            try
            {
                objAbout = new About();
                objAbout.ShowDialog();
                objAbout = null;
            }
            catch (Exception ex)
            {
                throw new Exception(Properties.Resources.ResourceManager.GetString("msgError").ToString(), ex);
            }
        }

        private void ClosingProgram(System.ComponentModel.CancelEventArgs e)
        {

            List<uscIndexedItem> objModificated;

            try
            {
                objModificated = objIndexedItens.FindAll((item) => { return item.Item.Modified; });
                if(objModificated.Count() > 0)
                {
                    if(BMessage.Instance.fnMessage(Properties.Resources.ResourceManager.GetString("msgClose").ToString(), Properties.Resources.ResourceManager.GetString("AppName").ToString(), MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                    {
                        e.Cancel = true;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(Properties.Resources.ResourceManager.GetString("msgError").ToString(), ex);
            }
        }
        #endregion

        #region Properties

        #endregion


    }

}
