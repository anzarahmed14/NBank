using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOLNBank
{
    public class ExcelImportError
    {
        public int RowNo { get; set; }
        public string ColumnName { get; set; }
        public string ErrorMessage { get; set; }
        public string CellValue { get; set; }
    }

}
