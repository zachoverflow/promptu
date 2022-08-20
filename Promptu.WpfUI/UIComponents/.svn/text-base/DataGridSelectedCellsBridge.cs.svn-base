using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel;
using System.Windows.Controls;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class DataGridSelectedCellsBridge : IEnumerable<CellAddress>
    {
        private DataGrid dataGrid;

        public DataGridSelectedCellsBridge(DataGrid dataGrid)
        {
            if (dataGrid == null)
            {
                throw new ArgumentNullException("dataGrid");
            }

            this.dataGrid = dataGrid;
        }

        public IEnumerator<CellAddress> GetEnumerator()
        {
            foreach (DataGridCellInfo cell in this.dataGrid.SelectedCells)
            {
                yield return new CellAddress(dataGrid.Items.IndexOf(cell.Item), dataGrid.Columns.IndexOf(cell.Column));
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
