// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PackagingSlips.v.cs" company="Allors bvba">
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
    public partial class PackagingSlips
    {
        private const string PackagingSlipTemplateEn =
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
           PACK LIST
        </h1>
    </div>
    <div>
        <table width=""100%"">
        <tr>
            <td>
            $if(this.GetShipment.BillFromInternalOrganisation.ExistLogoImage)$
                <img src = ""\\media\\i_$this.GetShipment.BillFromInternalOrganisation.LogoImage.Id$"" alt=""company logo"" width=""200"" />
            $endif$
            $if(this.GetShipment.ExistBillFromInternalOrganisation)$
                <p>
                    $this.GetShipment.BillFromInternalOrganisation.Name;format=""xml-encode""$
                </p>
                $partyContact(this.GetShipment.BillFromInternalOrganisation)$
                $if(this.GetShipment.BillFromInternalOrganisation.ExistTaxNumber)$
                    Tax number: $this.GetShipment.BillFromInternalOrganisation.TaxNumber$<br />
                $endif$
            $endif$
            </td>
	        <td>
            $if(this.GetShipment.ExistShipToParty)$
                <span style=""font-weight: bold"">Shipping Address</span>
                <p>
                    $this.GetShipment.ShipToParty.PartyName;format=""xml-encode""$<br />
                    $if(this.GetShipment.ExistShipToAddress)$
                        $this.GetShipment.ShipToAddress.FormattedFullAddress$
                    $endif$
                </p>
            $endif$
            </td>
	    </tr>
        </table>
     </div>
     <br />
     <div> 
        <h4>
            Shipment number:&#160;$this.GetShipment.ShipmentNumber$<br />
            Estimated shipping date:&#160;$this.GetShipment.ShortShipDateString$<br />
        </h4>
    </div>
    <br />
    <div> 
        <table width=""100%"">
        <tr>
            <td class=""header"" width=""8%"">
                Article
            </td>
            <td class=""header"" width=""60%"">
                Description
            </td>
            <td class=""header"" style=""text-align: right"" width=""8%"">
                Quantity
            </td>
        </tr>
        $this.ShipmentPackageWhereDocument.PackagingContents:{content|$packagingContent(content)$}$
        <tr>          
            <td colspan=""3"">
            </td>
        </tr>
        <tr>
            <td colspan=""2"">
            </td>
            <td class=""field"">
                TOTAL $this.ShipmentPackageWhereDocument.TotalQuantity$
            </td>
        </tr>
        </table>
    </div>
</body>
</html>
>>

partyContact(party) ::= <<
$if(party.ExistBillingAddress)$
    $party.BillingAddress.FormattedFullAddress$<br /><br />  
$endif$
$if(party.ExistGeneralPhoneNumber)$
    Phone number: $party.GeneralPhoneNumber.Description;format=""xml-encode""$<br />
$endif$
$if(party.ExistGeneralFaxNumber)$
    Fax number: $party.GeneralFaxNumber.Description;format=""xml-encode""$<br />
$endif$
>>

bank(bankAccount) ::= <<
<p>
    Bank: 
    $if(bankAccount.ExistBank)$
         &#xA0;$bankAccount.Bank.Name;format=""xml-encode""$
    $endif$
    $if(bankAccount.ExistIban)$
        IBAN: $bankAccount.Iban$<br />
    $endif$
    $if(bankAccount.Bank.ExistBic)$
        BIC: $bankAccount.Bank.Bic$<br />
    $endif$
</p>
>>

packagingContent(item) ::= <<
<tr>
    <td width=""8%"">
    $if(item.ShipmentItem.ExistGood)$
        $if(item.ShipmentItem.Good.ExistSku)$
            $item.ShipmentItem.Good.Sku$
        $endif$
    $endif$
    </td>
    <td>
        $item.ContentDescription;format=""xml-encode""$
    </td>
    <td class=""field"">
        $item.Quantity$
    </td>
</tr>
>>
";

        private const string PackagingSlipTemplateNl =
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
           PAKLIJST
        </h1>
    </div>
    <div>
        <table width=""100%"">
        <tr>
            <td>
            $if(this.GetShipment.BillFromInternalOrganisation.ExistLogoImage)$
                <img src = ""\\media\\i_$this.GetShipment.BillFromInternalOrganisation.LogoImage.Id$"" alt=""company logo"" width=""200"" />
            $endif$
            $if(this.GetShipment.ExistBillFromInternalOrganisation)$
                <p>
                    $this.GetShipment.BillFromInternalOrganisation.Name;format=""xml-encode""$
                </p>
                $partyContact(this.GetShipment.BillFromInternalOrganisation)$
                $if(this.GetShipment.BillFromInternalOrganisation.ExistTaxNumber)$
                    Tax number: $this.GetShipment.BillFromInternalOrganisation.TaxNumber$<br />
                $endif$
            $endif$
            </td>
	        <td>
            $if(this.GetShipment.ExistShipToParty)$
                <span style=""font-weight: bold"">Afleveradres</span>
                <p>
                    $this.GetShipment.ShipToParty.PartyName;format=""xml-encode""$<br />
                    $if(this.GetShipment.ExistShipToAddress)$
                        $this.GetShipment.ShipToAddress.FormattedFullAddress$
                    $endif$
                </p>
            $endif$
            </td>
	    </tr>
        </table>
     </div>
     <br />
     <div> 
        <h4>
            Zending nummer:&#160;$this.GetShipment.ShipmentNumber$<br />
            Verzend datum:&#160;$this.GetShipment.ShortShipDateString$<br />
        </h4>
    </div>
    <br />
    <div> 
        <table width=""100%"">
        <tr>
            <td class=""header"" width=""8%"">
                Artikel
            </td>
            <td class=""header"" width=""60%"">
                Omschrijving
            </td>
            <td class=""header"" style=""text-align: right"" width=""8%"">
                Aantal
            </td>
        </tr>
        $this.ShipmentPackageWhereDocument.PackagingContents:{content|$packagingContent(content)$}$
        <tr>          
            <td colspan=""3"">
            </td>
        </tr>
        <tr>
            <td colspan=""2"">
            </td>
            <td class=""field"">
                TOTAAL $this.ShipmentPackageWhereDocument.TotalQuantity$
            </td>
        </tr>
        </table>
    </div>
</body>
</html>
>>

partyContact(party) ::= <<
$if(party.ExistBillingAddress)$
    $party.BillingAddress.FormattedFullAddress$<br /><br />  
$endif$
$if(party.ExistGeneralPhoneNumber)$
    Telefoon: $party.GeneralPhoneNumber.Description;format=""xml-encode""$<br />
$endif$
$if(party.ExistGeneralFaxNumber)$
    Fax: $party.GeneralFaxNumber.Description;format=""xml-encode""$<br />
$endif$
>>

bank(bankAccount) ::= <<
<p>
    Bank: 
    $if(bankAccount.ExistBank)$
         &#xA0;$bankAccount.Bank.Name;format=""xml-encode""$
    $endif$
    $if(bankAccount.ExistIban)$
        IBAN: $bankAccount.Iban$<br />
    $endif$
    $if(bankAccount.Bank.ExistBic)$
        BIC: $bankAccount.Bank.Bic$<br />
    $endif$
</p>
>>

packagingContent(item) ::= <<
<tr>
    <td width=""8%"">
    $if(item.ShipmentItem.ExistGood)$
        $if(item.ShipmentItem.Good.ExistSku)$
            $item.ShipmentItem.Good.Sku$
        $endif$
    $endif$
    </td>
    <td>
        $item.ContentDescription;format=""xml-encode""$
    </td>
    <td class=""field"">
        $item.Quantity$
    </td>
</tr>
>>
";
    }
}