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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirtisIndexer.basic
{

    public class IndexedFolder
    {

        #region Functions
        public override string ToString()
        {
            string strReturn;

            try
            {
                strReturn = Path + " => " + (Char)13;

                foreach (IndexedFile file in Files)
                {
                    strReturn = strReturn + (Char)9 + file.ToString() + (Char)13;
                }

                return strReturn;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Properties
        public DateTime Tiemstamp { get; set; }
        public string Path { get; set; }
        public List<IndexedFile> Files { get; set; }
        public List<IndexedItem> Itens { get; set; }
        #endregion
    }

    public class IndexedFile
    {

        #region Functions
        public override string ToString()
        {
            string strReturn;

            try
            {
                strReturn = Name  + " => " + (Char)13;

                foreach (IndexedItem item in Items)
                {
                    strReturn = strReturn + (Char)9 + item.ToString() + (Char)13;
                }
                return strReturn;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Properties
        public string Alias { get; set; }
        public string Extension { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public List<IndexedItem> Items { get; set; }
        public string Raw { get; set; }
        #endregion
    }

    public class IndexedItem
    {

        #region Functions
        public override string ToString()
        {
            try
            {
                return Flat + " => " + Value;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Properties
        public DateTime Tiemstamp { get; set; }
        public string Alias { get; set; }
        public string Flat { get; set; }
        public string Value { get; set; }
        public int Index { get; set; }
        public bool Modified { get; set; }
        public IndexedFile ParentFile { get; set; }
        #endregion
    }


}
