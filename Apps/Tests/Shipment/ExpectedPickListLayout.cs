//------------------------------------------------------------------------------------------------- 
// <copyright file="ExpectedPickListLayout.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the MediaTests type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    public class ExpectedPickListLayout
    {
        public static string pickListEnglisch =
           @"<?xml version=""1.0"" encoding=""ISO-8859-1""?>
<html xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""en"">
<head>
    <title></title>
    <style type=""text/css"" xml:space=""preserve"">
        td.header {height:10.8pt;border:solid #999999 1.0pt;background:#DADFD7;font-weight: bold;}
        td.message {padding-left: 20px}
        td.field {text-align: left}
    	td {vertical-align: top;}
</style>
</head>
<body>
    <div>
        <h1>
            Picklist in facility
        </h1>
        <h2>
            Shipment number(s): 1, 
        </h2>
        <div> 
            Ship to           :person1<br />
        </div>
    </div>
    <div> 
        <table width=""100%"">
        <tr>
            <td class=""header"" width=""10%"">
                SKU
            </td>
            <td class=""header"" width=""80%"">
                Product
            </td>
            <td class=""header"" width=""10%"">
                To Pick
            </td>
        </tr>
        <tr>
            <td class=""field"">
                    10101
            </td>
            <td class=""field"">
                good1
            </td>
            <td class=""field"">
                3
            </td>
        </tr><tr>
            <td class=""field"">
                    10102
            </td>
            <td class=""field"">
                good2
            </td>
            <td class=""field"">
                5
            </td>
        </tr>
        </table>
    </div>
</body>
</html>
";

        public static string negativePickListEnglisch =
           @"<?xml version=""1.0"" encoding=""ISO-8859-1""?>
<html xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""en"">
<head>
    <title></title>
    <style type=""text/css"" xml:space=""preserve"">
        td.header {height:10.8pt;border:solid #999999 1.0pt;background:#DADFD7;font-weight: bold;}
        td.message {padding-left: 20px}
        td.field {text-align: left}
    	td {vertical-align: top;}
</style>
</head>
<body>
    <div>
        <h1>
            Change in shipment: 1
        </h1>
        <div> 
            Ship to           :person1<br />
        </div>
    </div>
    <div> 
        <table width=""100%"">
        <tr>
            <td class=""header"" width=""10%"">
                SKU
            </td>
            <td class=""header"" width=""80%"">
                Product
            </td>
            <td class=""header"" width=""10%"">
                To Pick
            </td>
        </tr>
        <tr>
            <td class=""field"">
                    10101
            </td>
            <td class=""field"">
                good1
            </td>
            <td class=""field"">
                -2
            </td>
        </tr>
        </table>
    </div>
</body>
</html>";
    }
}
