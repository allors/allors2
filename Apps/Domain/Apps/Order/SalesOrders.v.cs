// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesOrders.v.cs" company="Allors bvba">
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
    public partial class SalesOrders
    {
        public const string SalesOrderTemplateEn =
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
        $if(this.IsProposal)$
        <h1>
            Proposal
        </h1>
        $else$
        <h1>
            Order Confirmation
        </h1>
        $endif$
    </div>
    <div>
        <table width=""100%"">
        <tr>
            <td>
            $if(this.TakenByInternalOrganisation.ExistLogoImage)$
                <img src = ""\\media\\i_$this.TakenByInternalOrganisation.LogoImage.UniqueId$"" alt=""company logo"" width=""200"" />
            $endif$
            $if(this.ExistTakenByInternalOrganisation)$
                <p>
                    $this.TakenByInternalOrganisation.Name;format=""xml-encode""$
                </p>
                $partyContact(this.TakenByInternalOrganisation)$
                $if(this.TakenByInternalOrganisation.ExistTaxNumber)$
                    Tax number: $this.TakenByInternalOrganisation.TaxNumber$<br />
                $endif$
                $this.TakenByInternalOrganisation.BankAccounts:{bankAccount|$bank(bankAccount)$}$
            $endif$
            </td>
            <td>
                <span style=""font-weight: bold"">Billing Address</span>
                $if(this.ExistBillToCustomer)$
                <p>
                    $this.BillToCustomer.PartyName;format=""xml-encode""$<br />
                    $if(this.ExistBillToContactMechanism)$
                        $if(this.BillToContactMechanism.IsPostalAddress)$
                            $this.BillToContactMechanism.FormattedFullAddress$
                        $else$
                            $this.BillToContactMechanism.Description;format=""xml-encode""$
                        $endif$
                    $endif$
	            </p>
                $if(!this.BillToCustomer.IsPerson)$
                    $if(this.BillToCustomer.ExistTaxNumber)$
                    Tax number: $this.BillToCustomer.TaxNumber$
                    $endif$
                $endif$
                $endif$
            </td>
	        <td>
            $if(this.ExistShipToCustomer)$
                <span style=""font-weight: bold"">Shipping Address</span>
                <p>
                    $this.ShipToCustomer.PartyName;format=""xml-encode""$<br />
                    $if(this.ExistShipToAddress)$
                        $this.ShipToAddress.FormattedFullAddress$
                    $endif$
                </p>
                $if(!this.ShipToCustomer.IsPerson)$
                    $if(this.ShipToCustomer.ExistTaxNumber)$
                    Tax number: $this.ShipToCustomer.TaxNumber$
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
            Order number:&#160;$this.OrderNumber$<br />
            Order date:&#160;&#160;$this.ShortOrderDateString$<br />
        </h4>
        $if(this.ExistCustomerReference)$
        <h4>
            Your reference:&#160;$this.CustomerReference;format=""xml-encode""$<br />
        </h4>
        $endif$
        Shipment method:&#160;$this.ShipmentMethod.Name$<br />
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
                Article
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
                Price Adjustment
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
        $if(this.HasDiscount)$
        <tr>
            <td colspan=""4"">
            </td>
            <td class=""header"">
                Discount
            </td>
            <td class=""field"">
                $this.GetTotalDiscountAsCurrencyString$
            </td>
        </tr>
        $endif$
        $if(this.HasSurcharge)$
        <tr>
            <td colspan=""4"">
            </td>
            <td class=""header"">
                Surcharge
            </td>
            <td class=""field"">
                $this.GetTotalSurchargeAsCurrencyString$
            </td>
        </tr>
        $endif$
        $if(this.HasFee)$
        <tr>
            <td colspan=""4"">
            </td>
            <td class=""header"">
                Fee
            </td>
            <td class=""field"">
                $this.GetTotalFeeAsCurrencyString$
            </td>
        </tr>
        $endif$
        $if(this.HasShippingAndHandling)$
        <tr>
            <td colspan=""4"">
            </td>
            <td class=""header"">
                Shipping &amp; Handling
            </td>
            <td class=""field"">
                $this.GetTotalShippingAndHandlingAsCurrencyString$
            </td>
        </tr>
        $endif$
        $if(this.HasShippingAndHandling || this.HasFee || this.HasSurcharge || this.HasDiscount)$
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
                Total before Tax
            </td>
            <td class=""field"">
                $this.GetTotalExVatAsCurrencyString$
            </td>
        </tr>
        $endif$
        <tr>          
            <td colspan=""4"">
            </td>
            <td class=""header"">
                Sales Tax
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

orderItem(item) ::= <<
<tr>
    <td width=""8%"">
    $if(item.ExistProduct)$
        $if(item.Product.ExistSku)$
            $item.Product.Sku$
        $endif$
    $endif$
    </td>
    <td>
    $if(item.ExistProduct)$
        $if(item.Product.ExistName)$
            $item.Product.Name;format=""xml-encode""$
        $else$
            $item.Description;format=""xml-encode""$
        $endif$
    $elseif(item.ExistProductFeature)$
        $if(item.ProductFeature.ExistDescription)$
            $item.ProductFeature.Description;format=""xml-encode""$
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
        $item.GetPriceAdjustmentAsCurrencyString$
    </td>
    <td class=""field"">
        $item.GetTotalExVatAsCurrencyString$
    </td>
</tr>
$if(item.ExistMessage || item.ExistAssignedShipToParty || item.ExistAssignedShipToAddress)$
<tr>
    <td class=""message"" colspan=""6"">
    $if(item.ExistMessage)$
        $item.Message;format=""xml-encode""$<br />
    $endif$
    $if(item.ExistAssignedShipToParty || item.ExistAssignedShipToAddress)$
        Item shipped to: &#xA0;
        $if(item.ItemDifferentShippingParty)$
            $item.ItemDifferentShippingParty.PartyName;format=""xml-encode""$
        $endif$
        $if(item.ItemDifferentShippingAddress)$
            $item.ItemDifferentShippingAddress.FullAddress;format=""xml-encode""$  
        $endif$
    $endif$
    </td>
</tr>
$endif$
>>
";

        public const string SalesOrderTemplateNl =
@"main(this) ::= <<
<?xml version=""1.0"" encoding=""ISO-8859-1""?>
<html xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""nl"">
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
        $if(this.IsProposal)$
        <h1>
            Offerte
        </h1>
        $else$
        <h1>
            Orderbevestiging
        </h1>
        $endif$
    </div>
    <div>
        <table width=""100%"">
        <tr>
            <td>
            $if(this.TakenByInternalOrganisation.ExistLogoImage)$
                <img src = ""\\media\\i_$this.TakenByInternalOrganisation.LogoImage.UniqueId$"" alt=""company logo"" width=""200"" />
            $endif$
            $if(this.ExistTakenByInternalOrganisation)$
                <p>
                    $this.TakenByInternalOrganisation.Name;format=""xml-encode""$
                </p>
                $partyContact(this.TakenByInternalOrganisation)$
                $if(this.TakenByInternalOrganisation.ExistTaxNumber)$
                    BTW-nummer: $this.TakenByInternalOrganisation.TaxNumber$<br />
                $endif$
                $this.TakenByInternalOrganisation.BankAccounts:{bankAccount|$bank(bankAccount)$}$
            $endif$
            </td>
            <td>
                <span style=""font-weight: bold"">Factuuradres</span>
                $if(this.ExistBillToCustomer)$
                <p>
                    $this.BillToCustomer.PartyName;format=""xml-encode""$<br />
                    $if(this.ExistBillToContactMechanism)$
                        $if(this.BillToContactMechanism.IsPostalAddress)$
                            $this.BillToContactMechanism.FormattedFullAddress$
                        $else$
                            $this.BillToContactMechanism.Description;format=""xml-encode""$
                        $endif$
                    $endif$
	            </p>
                $if(!this.BillToCustomer.IsPerson)$
                    $if(this.BillToCustomer.ExistTaxNumber)$
                    BTW-nummer: $this.BillToCustomer.TaxNumber$
                    $endif$
                $endif$
                $endif$
            </td>
	        <td>
            $if(this.ExistShipToCustomer)$
                <span style=""font-weight: bold"">Afleveradres</span>
                <p>
                    $this.ShipToCustomer.PartyName;format=""xml-encode""$<br />
                    $if(this.ExistShipToAddress)$
                        $this.ShipToAddress.FormattedFullAddress$
                    $endif$
                </p>
                $if(!this.BillToCustomer.IsPerson)$
                    $if(this.ShipToCustomer.ExistTaxNumber)$
                    BTW-nummer: $this.ShipToCustomer.TaxNumber$
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
            Ordernummer:&#160;$this.OrderNumber$<br />
            Orderdatum:&#160;&#160;$this.ShortOrderDateString$<br />
        </h4>
        $if(this.ExistCustomerReference)$
        <h4>
            Uw Referentie:&#160;$this.CustomerReference;format=""xml-encode""$<br />
        </h4>
        $endif$
        Verzend methode:&#160;$this.ShipmentMethod.Name$<br />
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
                Artikel
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
                Prijs correctie
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
        $if(this.HasDiscount)$
        <tr>
            <td colspan=""4"">
            </td>
            <td class=""header"">
                Korting
            </td>
            <td class=""field"">
                $this.GetTotalDiscountAsCurrencyString$
            </td>
        </tr>
        $endif$
        $if(this.HasSurcharge)$
        <tr>
            <td colspan=""4"">
            </td>
            <td class=""header"">
                Toeslag
            </td>
            <td class=""field"">
                $this.GetTotalSurchargeAsCurrencyString$
            </td>
        </tr>
        $endif$
        $if(this.HasFee)$
        <tr>
            <td colspan=""4"">
            </td>
            <td class=""header"">
                Fee
            </td>
            <td class=""field"">
                $this.GetTotalFeeAsCurrencyString$
            </td>
        </tr>
        $endif$
        $if(this.HasShippingAndHandling)$
        <tr>
            <td colspan=""4"">
            </td>
            <td class=""header"">
                Verzendkosten
            </td>
            <td class=""field"">
                $this.GetTotalShippingAndHandlingAsCurrencyString$
            </td>
        </tr>
        $endif$
        $if(this.HasShippingAndHandling || this.HasFee || this.HasSurcharge || this.HasDiscount)$
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
                Totaal excl. BTW
            </td>
            <td class=""field"">
                $this.GetTotalExVatAsCurrencyString$
            </td>
        </tr>
        $endif$
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
    <td>
    $if(item.ExistProduct)$
        $if(item.Product.ExistName)$
            $item.Product.Name;format=""xml-encode""$
        $else$
            $item.Description;format=""xml-encode""$
        $endif$
    $elseif(item.ExistProductFeature)$
        $if(item.ProductFeature.ExistDescription)$
            $item.ProductFeature.Description;format=""xml-encode""$
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
        $item.GetPriceAdjustmentAsCurrencyString$
    </td>
    <td class=""field"">
        $item.GetTotalExVatAsCurrencyString$
    </td>
</tr>
$if(item.ExistMessage || item.ExistAssignedShipToParty || item.ExistAssignedShipToAddress)$
<tr>
    <td class=""message"" colspan=""6"">
    $if(item.ExistMessage)$
        $item.Message;format=""xml-encode""$<br />
    $endif$
    $if(item.ExistAssignedShipToParty || item.ExistAssignedShipToAddress)$
        Afleveradres voor dit item: &#xA0;
        $if(item.ItemDifferentShippingParty)$
            $item.ItemDifferentShippingParty.Name;format=""xml-encode""$;
        $endif$
        $if(item.ItemDifferentShippingAddress)$
            $item.ItemDifferentShippingAddress.FullAddress;format=""xml-encode""$  
        $endif$
    $endif$
    </td>
</tr>
$endif$
>>
";
    }
}