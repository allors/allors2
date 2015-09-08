// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseShipments.v.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    public partial class PurchaseShipments
    {
        public const string PurchaseShipmentTemplateEn =
            @"main(this) ::= <<
<?xml version=""1.0"" encoding=""ISO-8859-1""?>
<html xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""en"">
<head>
    <title></title>
    <style type=""text/css"" xml:space=""preserve"">
        td.header {height:10.8pt;border:solid #999999 1.0pt;background:#DADFD7;font-weight: bold;}
        td.message {padding-left: 20px}
        td.field {text-align: right; width=""8%""}
    	td {vertical-align: top;}
</style>
</head>
<body>
    <div>
        <h1>
           PURCHASE SHIPMENT
        </h1>
    </div>
    <div>
    $if(this.ExistShipToParty)$
        To      : $this.ShipToParty.PartyName;format=""xml-encode""$<br />
    $endif$
    $if(this.ExistFacility)$
        Facility: $this.Facility.Name;format=""xml-encode""$<br />
    $endif$
    $if(this.ExistShipFromParty)$
        From    : $this.ShipFromParty.PartyName;format=""xml-encode""$<br />
    $endif$
        Date    : $this.DateString;format=""xml-encode""$<br />
     </div>
    <br />
    <div> 
        <table width=""100%"">
        <tr>
            <td class=""header"" width=""8%"">
                SKU
            </td>
            <td class=""header"" width=""60%"">
                Name
            </td>
            <td class=""header"" style=""text-align: right"" width=""8%"">
                Quantity
            </td>
        </tr>
        $this.ShipmentItems:{item|$shipmentItems(item)$}$
        </table>
    </div>
</body>
</html>
>>

shipmentItems(item) ::= <<
<tr>
    <td width=""8%"">
    $if(item.ExistGood)$
        $if(item.Good.ExistSku)$
            $item.Good.Sku$
        $endif$
    $endif$
    </td>
    <td>
    $if(item.ExistGood)$
        $item.Good.Name;format=""xml-encode""$
    $endif$
    </td>
    <td class=""field"">
        $item.Quantity$
    </td>
</tr>
>>
";
        public const string PurchaseShipmentTemplateNl =
            @"main(this) ::= <<
<?xml version=""1.0"" encoding=""ISO-8859-1""?>
<html xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""en"">
<head>
    <title></title>
    <style type=""text/css"" xml:space=""preserve"">
        td.header {height:10.8pt;border:solid #999999 1.0pt;background:#DADFD7;font-weight: bold;}
        td.message {padding-left: 20px}
        td.field {text-align: right; width=""8%""}
    	td {vertical-align: top;}
</style>
</head>
<body>
    <div>
        <h1>
           LEVERANCIER ZENDING
        </h1>
    </div>
    <div>
    $if(this.ExistShipToParty)$
        Aan     : $this.ShipToParty.PartyName;format=""xml-encode""$<br />
    $endif$
    $if(this.ExistFacility)$
        Magazijn: $this.Facility.Name;format=""xml-encode""$<br />
    $endif$
    $if(this.ExistShipFromParty)$
        Van     : $this.ShipFromParty.PartyName;format=""xml-encode""$<br />
    $endif$
        Datum   : $this.DateString;format=""xml-encode""$<br />
     </div>
    <br />
    <div> 
        <table width=""100%"">
        <tr>
            <td class=""header"" width=""8%"">
                SKU
            </td>
            <td class=""header"" width=""60%"">
                Naam
            </td>
            <td class=""header"" style=""text-align: right"" width=""8%"">
                Aantal
            </td>
        </tr>
        $this.ShipmentItems:{item|$shipmentItems(item)$}$
        </table>
    </div>
</body>
</html>
>>

shipmentItems(item) ::= <<
<tr>
    <td width=""8%"">
    $if(item.ExistGood)$
        $if(item.Good.ExistSku)$
            $item.Good.Sku$
        $endif$
    $endif$
    </td>
    <td>
    $if(item.ExistGood)$
        $item.Good.Name;format=""xml-encode""$
    $endif$
    </td>
    <td class=""field"">
        $item.Quantity$
    </td>
</tr>
>>
";
    }
}