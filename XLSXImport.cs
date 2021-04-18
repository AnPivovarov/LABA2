using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace LABA2
{
    public class XLSXImport
    {
        public static IEnumerable<Threat> EnumerateThreats(string xlsxpath)
        {
            var workbook = new XLWorkbook(xlsxpath);
            var worksheet = workbook.Worksheets.Worksheet(1);
            int row = 3;
            while (worksheet.Cell(row, 1).GetValue<string>() != "")
            {
                string threatConf = "";
                if (worksheet.Cell(row, 6).GetValue<int>() == 1)
                {
                    threatConf = "Да";
                }
                else { threatConf = "Нет"; }

                string threatInteg = "";
                if (worksheet.Cell(row, 7).GetValue<int>() == 1)
                {
                    threatInteg = "Да";
                }
                else { threatInteg = "Нет"; }

                string threatAccess = "";
                if (worksheet.Cell(row, 8).GetValue<int>() == 1)
                {
                    threatAccess = "Да";
                }
                else { threatAccess = "Нет"; }
                var threat = new Threat
                {
                    ThreatID = "УБИ." + worksheet.Cell(row, 1).GetValue<string>(),
                    ThreatName = worksheet.Cell(row, 2).GetValue<string>(),
                    ThreatDescription = worksheet.Cell(row, 3).GetValue<string>(),
                    ThreatSource = worksheet.Cell(row, 4).GetValue<string>(),
                    ThreatObject = worksheet.Cell(row, 5).GetValue<string>(),
                    ThreatConf = threatConf,
                    ThreatInteg = threatInteg,
                    ThreatAccess = threatAccess,
                };
                yield return threat;
                row++;
            }
        }
    }
}
