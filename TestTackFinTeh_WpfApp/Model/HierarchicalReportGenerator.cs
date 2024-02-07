using ClosedXML.Excel;
using Microsoft.Win32;
using TestTaskFinTeh_Model;

namespace TestTackFinTeh_WpfApp.Model
{
    internal class HierarchicalReportGenerator
    {
        public HierarchicalReportGenerator(LinkTree linkTree, int maxLevelHierarchy)
        {
            _linkTree = linkTree;
            MaxLevelHierarchy = maxLevelHierarchy;
        }

        private LinkTree _linkTree;

        private int MaxLevelHierarchy { get; }
        public void Generate()
        {
            var wb = new XLWorkbook();
            IXLWorksheet sh = CreateTable(wb);
            СompletionTable(sh, _linkTree.RootProduct);
            SaveFile(wb);
        }

        private IXLWorksheet CreateTable(XLWorkbook wb)
        {
            IXLWorksheet sh = wb.Worksheets.Add("Иерархия продуктов");

            sh.Cell(1, 1).SetValue("Изделие                 ");
            sh.Cell(1, 2).SetValue("Кол-во                  ");
            sh.Cell(1, 3).SetValue("Стоимость               ");
            sh.Cell(1, 4).SetValue("Цена                    ");
            sh.Cell(1, 5).SetValue("Кол-во входящих         ");

            sh.Cell(1, 1).Style.Font.Bold = true;
            sh.Cell(1, 2).Style.Font.Bold = true;
            sh.Cell(1, 3).Style.Font.Bold = true;
            sh.Cell(1, 4).Style.Font.Bold = true;
            sh.Cell(1, 5).Style.Font.Bold = true;
            sh.Columns().AdjustToContents();

            return sh;
        }
        private void SaveFile(XLWorkbook wb)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".xls";
            saveFileDialog.Filter = "Text file (*.xls)|*.xlsx";
            bool resultDialog = (bool)saveFileDialog.ShowDialog()!;
            if (resultDialog)
                wb.SaveAs(saveFileDialog.FileName);
        }
        private void СompletionTable(IXLWorksheet sheet, List<LinkNode> thisLevelLink)
        {
            СompletionTable(sheet, 1, 0, _linkTree.RootProduct);
        }
        private int СompletionTable(IXLWorksheet sheet, int numberLine, int levelHierarchy, List<LinkNode> thisLevelLink)
        {
            levelHierarchy++;
            foreach (LinkNode node in thisLevelLink)
            {
                СompletionLine(sheet, numberLine += 1, levelHierarchy, node);

                if (node.DownProducts.Count != 0 && levelHierarchy < MaxLevelHierarchy)
                    numberLine = СompletionTable(sheet, numberLine, levelHierarchy, node.DownProducts);
            }
            return numberLine;
        }
        private void СompletionLine(IXLWorksheet sheet, int numberLine, int levelHierarchy, LinkNode linkNode)
        {
            string indent = "";
            for (int i = 1; i < levelHierarchy; i++)
            {
                indent += "       ";
            }
            for (int j = 1; j <= 5; j++)
            {
                sheet.Cell(numberLine, 1).SetValue(indent + linkNode.Link.Product.Name);
                sheet.Cell(numberLine, 2).SetValue(linkNode.Link.Count);
                sheet.Cell(numberLine, 3).SetValue(linkNode.Cost);
                sheet.Cell(numberLine, 4).SetValue(linkNode.Link.Product.Price);
                sheet.Cell(numberLine, 5).SetValue(linkNode.CountIncoming);
            }
        }
    }
}
