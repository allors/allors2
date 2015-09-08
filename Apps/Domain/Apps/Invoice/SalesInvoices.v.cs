// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesInvoices.v.cs" company="Allors bvba">
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
    public partial class SalesInvoices
    {
        private const string AppsSalesInvoiceTemplateEn =
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
    	td.small {font-size:0.875em;}
</style>
</head>
<body>
    <div>
        <h1>
           Invoice
        </h1>
    </div>
    <div>
        <table width=""100%"">
        <tr>
            <td>
            $if(this.BilledFromInternalOrganisation.ExistLogoImage)$
                <img src = ""\\media\\i_$this.BilledFromInternalOrganisation.LogoImage.UniqueId$"" alt=""company logo"" width=""200"" />
            $endif$
            $if(this.ExistBilledFromInternalOrganisation)$
                <p>
                    $this.BilledFromInternalOrganisation.Name;format=""xml-encode""$
                </p>
                $partyContact(this.BilledFromInternalOrganisation)$
                $if(this.BilledFromInternalOrganisation.ExistTaxNumber)$
                    Tax number: $this.BilledFromInternalOrganisation.TaxNumber$<br />
                $endif$
                $this.BilledFromInternalOrganisation.BankAccounts:{bankAccount|$bank(bankAccount)$}$
            $endif$
            </td>
            <td>
                <span style=""font-weight: bold"">Billing Address</span>
                $if(this.ExistBillToCustomer)$
                <p>
                    $this.BillToCustomer.PartyName$<br />
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
            Invoice number:&#160;$this.InvoiceNumber$<br />
            Invoice date  :&#160;$this.ShortInvoiceDateString$<br />
            Duedate       :&#160;$this.ShortDueDateString$<br />
        </h4>
        $if(this.ExistCustomerReference)$
        <h4>
            Your reference:&#160;$this.CustomerReference;format=""xml-encode""$<br />
        </h4>
        $endif$
        $if(this.ExistSalesOrder)$
        <h4>
            Invoice for order:&#160;$this.SalesOrder.OrderNumber$ on $this.SalesOrder.ShortOrderDateString$<br />
        </h4>
        $endif$
        $if(this.ExistShipment)$
            Shipment:&#160;$this.Shipment.Carrier.PartyName;format=""xml-encode""$&#160;$this.Shipment.ShipmentMethod.Name;format=""xml-encode""$<br />
        $endif$
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
            <td class=""header"" width=""52%"">
                Description
            </td>
            <td class=""header"" style=""text-align: right"" width=""8%"">
                Quantity
            </td>
            <td class=""header"" style=""text-align: right"" width=""8%"">
                Unit Price<br />
            </td>
            <td class=""header"" style=""text-align: right"" width=""8%"">
                Our Price<br />(excl. VAT)
            </td>
            <td class=""header"" style=""text-align: right"" width=""8%"">
                VAT Rate
            </td>
            <td class=""header"" style=""text-align: right"" width=""8%"">
                Total Price
            </td>
        </tr>
        $this.SalesInvoiceItems:{invoiceItem|$invoiceItem(invoiceItem)$}$
        <tr>          
            <td colspan=""7"">
            </td>
        </tr>
        <tr>          
            <td colspan=""5"">
            </td>
            <td class=""header"">
                Subtotal
            </td>
            <td class=""field"">
            $if(this.ExistTotalBasePrice)$
                $this.GetSubTotalAsCurrencyString$
            $else$
                $this.GetNothingAsCurrencyString$
            $endif$
            </td>
        </tr>
        $if(this.HasFee)$
        <tr>
            <td colspan=""5"">
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
            <td colspan=""5"">
            </td>
            <td class=""header"">
                Shipping &amp; Handling
            </td>
            <td class=""field"">
                $this.GetTotalShippingAndHandlingAsCurrencyString$
            </td>
        </tr>
        $endif$
        $if(this.hasFee || this.HasShippingAndHandling)$
        <tr>
            <td colspan=""6"">
            </td>
            <td>
                <hr />
            </td>
        </tr>
        <tr>
            <td colspan=""5"">
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
            <td colspan=""5"">
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
            <td colspan=""6"">
            </td>
            <td>
                <hr />
            </td>
        </tr>
        <tr>
            <td colspan=""5"">
            </td>
            <td class=""header"">
                TOTAL
            </td>
            <td class=""field"">
            $if (this.ExistTotalIncVat)$
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

invoiceItem(item) ::= <<
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
        $item.Quantity$
    </td>
    <td class=""field"">
        $item.GetCalculatedUnitPriceAsCurrencyString$
    </td>
    <td class=""field"">
        $item.GetTotalExVatAsCurrencyString$
    </td>
    <td class=""field"">
        $item.VatRateAsString$%
    </td>
    <td class=""field"">
        $item.GetTotalIncVatAsCurrencyString$
    </td>
</tr>
$if(item.ExistMessage)$
<tr>
    <td class=""message"" colspan=""6"">
        $item.Message;format=""xml-encode""$<br />
    </td>
</tr>
$endif$
>>
";

        private const string AppsSalesInvoiceTemplateNl =
@"main(this) ::= <<<?xml version=""1.0"" encoding=""ISO-8859-1""?>
<html xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""nl"">
<head>
    <title></title>
    <style type=""text/css"" xml:space=""preserve"">
        td.header {height:10.8pt;border:solid #999999 1.0pt;background:#DADFD7;font-weight: bold;}
        td.message {padding-left: 20px}
        td.field {text-align: right; width=""8%""}
    	td {vertical-align: top;}
    	td.small {font-size:0.875em;}
</style>
</head>
<body>
    <div>
        <h1>
           Factuur
        </h1>
    </div>
    <div>
        <table width=""100%"">
        <tr>
            <td>
            $if(this.BilledFromInternalOrganisation.ExistLogoImage)$
                <img src = ""\\media\\i_$this.BilledFromInternalOrganisation.LogoImage.UniqueId$"" alt=""company logo"" width=""200"" />
            $endif$
            $if(this.ExistBilledFromInternalOrganisation)$
                <p>
                    $this.BilledFromInternalOrganisation.Name;format=""xml-encode""$
                </p>
                $partyContact(this.BilledFromInternalOrganisation)$
                $if(this.BilledFromInternalOrganisation.ExistTaxNumber)$
                    BTW-nummer: $this.BilledFromInternalOrganisation.TaxNumber$<br />
                $endif$
                $this.BilledFromInternalOrganisation.BankAccounts:{bankAccount|$bank(bankAccount)$}$
            $endif$
            </td>
            <td>
                <span style=""font-weight: bold"">Factuuradres</span>
                $if(this.ExistBillToCustomer)$
                <p>
                    $this.BillToCustomer.PartyName$<br />
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
                    $this.ShipToCustomer.PartyName$<br />
                    $if(this.ExistShipToAddress)$
                        $this.ShipToAddress.FormattedFullAddress$
                    $endif$
                </p>
                $if(!this.ShipToCustomer.IsPerson)$
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
            Factuurnummer:&#160;$this.InvoiceNumber$<br />
            Factuurdatum :&#160;$this.ShortInvoiceDateString$<br />
            Vervaldatum  :&#160;$this.ShortDueDateString$<br />
        </h4>
        $if(this.ExistCustomerReference)$
        <h4>
            Uw referentie:&#160;$this.CustomerReference;format=""xml-encode""$<br />
        </h4>
        $endif$
        $if(this.ExistSalesOrder)$
        <h4>
            Factuur voor order:&#160;$this.SalesOrder.OrderNumber$ d.d. $this.SalesOrder.ShortOrderDateString$<br />
        </h4>
        $endif$
        $if(this.ExistShipment)$
            Transport:&#160;$this.Shipment.Carrier.PartyName;format=""xml-encode""$&#160;$this.Shipment.ShipmentMethod.Name;format=""xml-encode""$<br />
        $endif$
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
            <td class=""header"" width=""52%"">
                Omschrijving
            </td>
            <td class=""header"" style=""text-align: right"" width=""8%"">
                Aantal
            </td>
            <td class=""header"" style=""text-align: right"" width=""8%"">
                Prijs per eenheid<br />
            </td>
            <td class=""header"" style=""text-align: right"" width=""8%"">
                Onze prijs<br />(excl. BTW)
            </td>
            <td class=""header"" style=""text-align: right"" width=""8%"">
                BTW percentage
            </td>
            <td class=""header"" style=""text-align: right"" width=""8%"">
                Prijs
            </td>
        </tr>
        $this.SalesInvoiceItems:{invoiceItem|$invoiceItem(invoiceItem)$}$
        <tr>          
            <td colspan=""7"">
            </td>
        </tr>
        <tr>          
            <td colspan=""5"">
            </td>
            <td class=""header"">
                Subtotaal
            </td>
            <td class=""field"">
            $if(this.ExistTotalBasePrice)$
                $this.GetSubTotalAsCurrencyString$
            $else$
                $this.GetNothingAsCurrencyString$
            $endif$
            </td>
        </tr>
        $if(this.HasFee)$
        <tr>
            <td colspan=""5"">
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
            <td colspan=""5"">
            </td>
            <td class=""header"">
                Verzendkosten
            </td>
            <td class=""field"">
                $this.GetTotalShippingAndHandlingAsCurrencyString$
            </td>
        </tr>
        $endif$
        $if(this.HasFee || this.HasShippingAndHandling)$
        <tr>
            <td colspan=""6"">
            </td>
            <td>
                <hr />
            </td>
        </tr>
        <tr>
            <td colspan=""5"">
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
            <td colspan=""5"">
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
            <td colspan=""6"">
            </td>
            <td>
                <hr />
            </td>
        </tr>
        <tr>
            <td colspan=""5"">
            </td>
            <td class=""header"">
                TOTAAL
            </td>
            <td class=""field"">
            $if (this.ExistTotalIncVat)$
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

invoiceItem(item) ::= <<
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
        $item.Quantity$
    </td>
    <td class=""field"">
        $item.GetCalculatedUnitPriceAsCurrencyString$
    </td>
    <td class=""field"">
        $item.GetTotalExVatAsCurrencyString$
    </td>
    <td class=""field"">
        $item.VatRateAsString$%
    </td>
    <td class=""field"">
        $item.GetTotalIncVatAsCurrencyString$
    </td>
</tr>
$if(item.ExistMessage)$
<tr>
    <td class=""message"" colspan=""6"">
        $item.Message;format=""xml-encode""$<br />
    </td>
</tr>
$endif$
>>
";
        
        protected string SalesInvoiceTemplateEn
        {
            get
            {
                return AppsSalesInvoiceTemplateEn;
            }
        }

        protected string SalesInvoiceTemplateNl
        {
            get
            {
                return AppsSalesInvoiceTemplateNl;
            }
        }
    }
}