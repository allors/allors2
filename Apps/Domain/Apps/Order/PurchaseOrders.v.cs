// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrders.v.cs" company="Allors bvba">
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
    public partial class PurchaseOrders
    {
        public const string PurchaseOrderTemplateEn =
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
        $if(this.IsProvisional)$
        <h1>
            Provisional purchase order
        </h1>
        $else$
        <h1>
            Purchase Order
        </h1>
        $endif$
    </div>
    <div>
        <table width=""100%"">
        <tr>
            <td>
            $if(this.ExistShipToBuyer)$
                $if(this.ShipToBuyer.ExistLogoImage)$
                    <img src = ""\\media\\i_$this.ShipToBuyer.LogoImage.Id$"" alt=""company logo"" width=""200"" />
                $endif$
                <p>
                    $this.ShipToBuyer.Name;format=""xml-encode""$
                </p>
                $partyContact(this.ShipToBuyer)$
                $if(this.ShipToBuyer.ExistTaxNumber)$
                    Tax number: $this.ShipToBuyer.TaxNumber$<br />
                $endif$
                $this.ShipToBuyer.BankAccounts:{bankAccount|$bank(bankAccount)$}$
            $endif$
            </td>
            <td>
                <span style=""font-weight: bold"">Billing Address</span>
                $if(this.ExistBillToPurchaser)$
                <p>
                    $this.BillToPurchaser.PartyName;format=""xml-encode""$<br />
                    $if(this.ExistBillToContactMechanism)$
                        $if(this.BillToContactMechanism.IsPostalAddress)$
                            $this.BillToContactMechanism.FormattedFullAddress$
                        $else$
                            $this.BillToContactMechanism.Description;format=""xml-encode""$
                        $endif$
                    $endif$
	            </p>
                $if(!this.BillToPurchaser.IsPerson)$
                    $if(this.BillToPurchaser.ExistTaxNumber)$
                    Tax number: $this.BillToPurchaser.TaxNumber$
                    $endif$
                $endif$
                $endif$
            </td>
	        <td>
            $if(this.ExistTakenViaSupplier)$
                <span style=""font-weight: bold"">Order from</span>
                <p>
                    $this.TakenViaSupplier.PartyName;format=""xml-encode""$<br />
                    $if(this.ExistTakenViaContactMechanism)$
                        $this.TakenViaContactMechanism.Description$
                    $endif$
                </p>
                $if(!this.TakenViaSupplier.IsPerson)$
                    $if(this.TakenViaSupplier.ExistTaxNumber)$
                    Tax number: $this.TakenViaSupplier.TaxNumber$
                    $endif$
                $endif$
            $endif$
	        </td>
	    </tr>
        </table>
    </div>
     <br />
     <div> 
        <h4>
            Order numer:&#160;$this.OrderNumber$<br />
            Order date:&#160;&#160;$this.ShortOrderDateString$
        </h4>
        $if(this.ExistMessage)$
        <p class=""message"">
            $this.Message;format=""xml-encode""$
        </p>
        $endif$
    </div>
    <br />
    <div> 
        <table width=""100%"">
        <tr>
            <td class=""header"" width=""8%"">
                Our reference
            </td>
            <td class=""header"" width=""8%"">
                Your reference
            </td>
            <td class=""header"" width=""60%"">
                Description
            </td>
            <td class=""header"" style=""text-align: right"" width=""8%"">
                Quantity
            </td>
            <td class=""header"" style=""text-align: right"" width=""8%"">
                Unit Price
            </td>
            <td class=""header"" style=""text-align: right"" width=""8%"">
                Price
            </td>
        </tr>
        $this.ValidOrderItems:{orderItem|$orderItem(orderItem)$}$
        <tr>          
            <td colspan=""6"">
            </td>
        </tr>
        <tr>          
            <td colspan=""4"">
            </td>
            <td class=""header"">
                Subtotal
            </td>
            <td class=""field"">
            $if(this.ExistTotalBasePrice)$
                $this.GetTotalBasePriceAsCurrencyString$
            $else$
                $this.GetNothingAsCurrencyString$
            $endif$
            </td>
        </tr>
        <tr>          
            <td colspan=""4"">
            </td>
            <td class=""header"">
                VAT
            </td>
            <td class=""field"">
            $if(this.ExistTotalVat)$
                $this.GetTotalVatAsCurrencyString$
            $else$
                $this.GetNothingAsCurrencyString$
            $endif$
            </td>
        </tr>
        <tr>
            <td colspan=""5"">
            </td>
            <td>
                <hr />
            </td>
        </tr>
        <tr>
            <td colspan=""4"">
            </td>
            <td class=""header"">
                TOTAL
            </td>
            <td class=""field"">
            $if(this.ExistTotalIncVat)$
                $this.GetTotalIncVatAsCurrencyString$
            $else$
                $this.GetNothingAsCurrencyString$
            $endif$
            </td>
        </tr>
        </table>
    </div>
</body>
</html>
>>

partyContact(party) ::= <<
$if(party.ExistShippingAddress)$
    $party.ShippingAddress.FormattedFullAddress$<br /><br />  
$endif$
$if(party.ExistGeneralPhoneNumber)$
    Telephone: $party.GeneralPhoneNumber.Description;format=""xml-encode""$<br />
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

orderItem(item) ::= <<
<tr>
    <td width=""8%"">
    $if(item.ExistProduct)$
        $if(item.Product.ExistSku)$
            $item.Product.Sku$
        $endif$
    $endif$
    </td>
    <td width=""8%"">
        $item.SupplierReference$
    </td>
    <td>
    $if(item.ExistProduct)$
        $if(item.Product.ExistName)$
            $item.Product.Name;format=""xml-encode""$
        $else$
            $item.Description;format=""xml-encode""$
        $endif$
    $elseif(item.ExistPart)$
        $if(item.Part.ExistName)$
            $item.Part.Name;format=""xml-encode""$
        $else$
            $item.Description;format=""xml-encode""$
        $endif$
    $else$
        $item.Description;format=""xml-encode""$
    $endif$
    </td>
    <td class=""field"">
        $item.QuantityOrdered$
    </td>
    <td class=""field"">
        $item.GetUnitBasePriceAsCurrencyString$
    </td>
    <td class=""field"">
        $item.GetTotalExVatAsCurrencyString$
    </td>
</tr>
>>
";

        public const string PurchaseOrderTemplateNl =
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
        $if(this.IsProvisional)$
        <h1>
            Voorlopige aankoop order
        </h1>
        $else$
        <h1>
            Aankoop Order
        </h1>
        $endif$
    </div>
    <div>
        <table width=""100%"">
        <tr>
            <td>
            $if(this.ExistShipToBuyer)$
                $if(this.ShipToBuyer.ExistLogoImage)$
                    <img src = ""\\media\\i_$this.ShipToBuyer.LogoImage.Id$"" alt=""company logo"" width=""200"" />
                $endif$
                <p>
                    $this.ShipToBuyer.Name;format=""xml-encode""$
                </p>
                $partyContact(this.ShipToBuyer)$
                $if(this.ShipToBuyer.ExistTaxNumber)$
                    BTW-nummer: $this.ShipToBuyer.TaxNumber$<br />
                $endif$
                $this.ShipToBuyer.BankAccounts:{bankAccount|$bank(bankAccount)$}$
            $endif$
            </td>
            <td>
                <span style=""font-weight: bold"">Factuuradres</span>
                $if(this.ExistBillToPurchaser)$
                <p>
                    $this.BillToPurchaser.PartyName;format=""xml-encode""$<br />
                    $if(this.ExistBillToContactMechanism)$
                        $if(this.BillToContactMechanism.IsPostalAddress)$
                            $this.BillToContactMechanism.FormattedFullAddress$
                        $else$
                            $this.BillToContactMechanism.Description;format=""xml-encode""$
                        $endif$
                    $endif$
	            </p>
                $if(!this.BillToPurchaser.IsPerson)$
                    $if(this.BillToPurchaser.ExistTaxNumber)$
                    BTW-nummer: $this.BillToPurchaser.TaxNumber$
                    $endif$
                $endif$
                $endif$
            </td>
	        <td>
            $if(this.ExistTakenViaSupplier)$
                <span style=""font-weight: bold"">Leverancier</span>
                <p>
                    $this.TakenViaSupplier.PartyName;format=""xml-encode""$<br />
                    $if(this.ExistTakenViaContactMechanism)$
                        $this.TakenViaContactMechanism.Description$
                    $endif$
                </p>
                $if(!this.TakenViaSupplier.IsPerson)$
                    $if(this.TakenViaSupplier.ExistTaxNumber)$
                    BTW-nummer: $this.TakenViaSupplier.TaxNumber$
                    $endif$
                $endif$
            $endif$
	        </td>
	    </tr>
        </table>
    </div>
     <br />
     <div> 
        <h4>
            Ordernumber:&#160;$this.OrderNumber$<br />
            Orderdatum :&#160;&#160;$this.ShortOrderDateString$
        </h4>
        $if(this.ExistMessage)$
        <p class=""message"">
            $this.Message;format=""xml-encode""$
        </p>
        $endif$
    </div>
    <br />
    <div> 
        <table width=""100%"">
        <tr>
            <td class=""header"" width=""8%"">
                Onze referentie
            </td>
            <td class=""header"" width=""8%"">
                Uw referentie
            </td>
            <td class=""header"" width=""60%"">
                Omschrijving
            </td>
            <td class=""header"" style=""text-align: right"" width=""8%"">
                Aantal
            </td>
            <td class=""header"" style=""text-align: right"" width=""8%"">
                Prijs per eenheid
            </td>
            <td class=""header"" style=""text-align: right"" width=""8%"">
                Prijs
            </td>
        </tr>
        $this.ValidOrderItems:{orderItem|$orderItem(orderItem)$}$
        <tr>          
            <td colspan=""6"">
            </td>
        </tr>
        <tr>          
            <td colspan=""4"">
            </td>
            <td class=""header"">
                Subtotaal
            </td>
            <td class=""field"">
            $if(this.ExistTotalBasePrice)$
                $this.GetTotalBasePriceAsCurrencyString$
            $else$
                $this.GetNothingAsCurrencyString$
            $endif$
            </td>
        </tr>
        <tr>          
            <td colspan=""4"">
            </td>
            <td class=""header"">
                BTW
            </td>
            <td class=""field"">
            $if(this.ExistTotalVat)$
                $this.GetTotalVatAsCurrencyString$
            $else$
                $this.GetNothingAsCurrencyString$
            $endif$
            </td>
        </tr>
        <tr>
            <td colspan=""5"">
            </td>
            <td>
                <hr />
            </td>
        </tr>
        <tr>
            <td colspan=""4"">
            </td>
            <td class=""header"">
                TOTAAL
            </td>
            <td class=""field"">
            $if(this.ExistTotalIncVat)$
                $this.GetTotalIncVatAsCurrencyString$
            $else$
                $this.GetNothingAsCurrencyString$
            $endif$
            </td>
        </tr>
        </table>
    </div>
</body>
</html>
>>

partyContact(party) ::= <<
$if(party.ExistShippingAddress)$
    $party.ShippingAddress.FormattedFullAddress$<br /><br />  
$endif$
$if(party.ExistGeneralPhoneNumber)$
    Telefoon: $party.GeneralPhoneNumber.Description;format=""xml-encode""$<br />
$endif$
$if(party.ExistGeneralFaxNumber)$
    Fax     : $party.GeneralFaxNumber.Description;format=""xml-encode""$<br />
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

orderItem(item) ::= <<
<tr>
    <td width=""8%"">
    $if(item.ExistProduct)$
        $if(item.Product.ExistSku)$
            $item.Product.Sku$
        $endif$
    $endif$
    </td>
    <td width=""8%"">
        $item.SupplierReference$
    </td>
    <td>
    $if(item.ExistProduct)$
        $if(item.Product.ExistName)$
            $item.Product.Name;format=""xml-encode""$
        $else$
            $item.Description;format=""xml-encode""$
        $endif$
    $elseif(item.ExistPart)$
        $if(item.Part.ExistName)$
            $item.Part.Name;format=""xml-encode""$
        $else$
            $item.Description;format=""xml-encode""$
        $endif$
    $else$
        $item.Description;format=""xml-encode""$
    $endif$
    </td>
    <td class=""field"">
        $item.QuantityOrdered$
    </td>
    <td class=""field"">
        $item.GetUnitBasePriceAsCurrencyString$
    </td>
    <td class=""field"">
        $item.GetTotalExVatAsCurrencyString$
    </td>
</tr>
>>
";
    }
}