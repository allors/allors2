// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PickLists.v.cs" company="Allors bvba">
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
    public partial class PickLists
    {
        public const string PickListTemplateNl =
@"main(this) ::= <<
<?xml version=""1.0"" encoding=""ISO-8859-1""?>
<html xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""nl"">
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
        $if(this.IsNegativePickList)$
        <h1>
            Wijziging voor zending: $this.CustomerShipmentCorrection.ShipmentNumber$
        </h1>
        $else$
            $if(this.ExistPickListItems)$
        <h1>
            Verzamellijst in $this.PickListItems.first.InventoryItem.Facility.Name;format=""xml-encode""$
        </h1>
            $endif$
            $if(this.ExistShipToParty)$
        <h2>
            Zending nummer(s): $this.ShipToParty.PendingCustomerShipments:{customerShipment|$customerShipment.ShipmentNumber$, }$
        </h2>
            $endif$
        $endif$
        $if(this.ExistShipToParty)$
        <div> 
            Zending voor     :$this.ShipToParty.PartyName;format=""xml-encode""$<br />
        </div>
        $endif$
        $if(this.ExistPicker)$
        <h2>
            Picker: $this.Picker.PartyName;format=""xml-encode""$
        </h2>
        $endif$
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
                Pak
            </td>
        </tr>
        $this.PickListItems:{item|$picklistitem(item)$}$
        </table>
    </div>
</body>
</html>
>>

picklistitem(item) ::= <<
<tr>
    <td class=""field"">
    $if(item.InventoryItem.ExistGood)$
        $if(item.InventoryItem.Good.ExistSku)$
            $item.InventoryItem.Good.Sku$
        $endif$
    $endif$
    </td>
    <td class=""field"">
        $if(item.InventoryItem.ExistGood)$
        $item.InventoryItem.Good.Name;format=""xml-encode""$
        $else$
        $item.InventoryItem.Part.Name;format=""xml-encode""$
        $endif$
    </td>
    <td class=""field"">
        $item.RequestedQuantity$
    </td>
</tr>
>>
";

        public const string PickListTemplateEn =
@"main(this) ::= <<
<?xml version=""1.0"" encoding=""ISO-8859-1""?>
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
        $if(this.IsNegativePickList)$
        <h1>
            Change in shipment: $this.CustomerShipmentCorrection.ShipmentNumber$
        </h1>
        $else$
            $if(this.ExistPickListItems)$
        <h1>
            Picklist in $this.PickListItems.first.InventoryItem.Facility.Name;format=""xml-encode""$
        </h1>
            $endif$
            $if(this.ExistShipToParty)$
        <h2>
            Shipment number(s): $this.ShipToParty.PendingCustomerShipments:{customerShipment|$customerShipment.ShipmentNumber$, }$
        </h2>
            $endif$
        $endif$
        $if(this.ExistShipToParty)$
        <div> 
            Ship to           :$this.ShipToParty.PartyName;format=""xml-encode""$<br />
        </div>
        $endif$
        $if(this.ExistPicker)$
        <h2>
            Picker: $this.Picker.PartyName;format=""xml-encode""$
        </h2>
        $endif$
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
        $this.PickListItems:{item|$picklistitem(item)$}$
        </table>
    </div>
</body>
</html>
>>

picklistitem(item) ::= <<
<tr>
    <td class=""field"">
    $if(item.InventoryItem.ExistGood)$
        $if(item.InventoryItem.Good.ExistSku)$
            $item.InventoryItem.Good.Sku$
        $endif$
    $endif$
    </td>
    <td class=""field"">
        $if(item.InventoryItem.ExistGood)$
        $item.InventoryItem.Good.Name;format=""xml-encode""$
        $else$
        $item.InventoryItem.Part.Name;format=""xml-encode""$
        $endif$
    </td>
    <td class=""field"">
        $item.RequestedQuantity$
    </td>
</tr>
>>
";
    }
}