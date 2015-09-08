//------------------------------------------------------------------------------------------------- 
// <copyright file=""""ExpectedInvoiceLayout.cs"""" company=""""Allors bvba"""">
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
    public class ExpectedInvoiceLayout
    {
        public const string SalesInvoiceEnglisch =
@"<?xml version=""1.0"" encoding=""ISO-8859-1""?>
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
                <img src = ""\media\i_9c41233f-9480-4df9-9421-63ed61f80623"" alt=""company logo"" width=""200"" />
                <p>
                    Allors bvba
                </p>
                    Kleine Nieuwedijkstraat 2<br />2800 Mechelen<br />Belgium<br /><br />  
                    Phone number: +32  3301 3301<br />

                    Tax number: 11111111<br />
                <p>
                    Bank: 
                         &#xA0;Fortis België
                        IBAN: BE23 3300 6167 6391<br />
                        BIC: GEBABEBB<br />
                </p>
            </td>
            <td>
                <span style=""font-weight: bold"">Billing Address</span>
                <p>
                    ACME<br />
                            invoices@acme.com
	            </p>
                    Tax number: 22222222
            </td>
	        <td>
                <span style=""font-weight: bold"">Shipping Address</span>
                <p>
                    ACME<br />
                        4000 Warner Blvd<br />CA 91505 Burbank<br />United States
                </p>
                    Tax number: 22222222
            </td>
	    </tr>
        </table>
     </div>
     <br />
     <div> 
        <h4>
            Invoice number:&#160;1<br />
            Invoice date  :&#160;22/02/2011<br />
            Duedate       :&#160;22/02/2011<br />
        </h4>
        <p class=""message"">
            Thanks for your order.
        </p>
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
        <tr>
            <td width=""8%"">
                    10101
            </td>
            <td>
                    good
            </td>
            <td class=""field"">
                3
            </td>
            <td class=""field"">
                €9,722.58
            </td>
            <td class=""field"">
                €29,167.74
            </td>
            <td class=""field"">
                21%
            </td>
            <td class=""field"">
                €35,292.96
            </td>
        </tr>
        <tr>
            <td class=""message"" colspan=""6"">
                Orderitem message<br />
            </td>
        </tr>
        <tr>
            <td width=""8%"">
                    10101
            </td>
            <td>
                    good
            </td>
            <td class=""field"">
                3
            </td>
            <td class=""field"">
                €9,722.58
            </td>
            <td class=""field"">
                €29,167.74
            </td>
            <td class=""field"">
                21%
            </td>
            <td class=""field"">
                €35,292.96
            </td>
        </tr>

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
                €58,335.48
            </td>
        </tr>
        <tr>
            <td colspan=""5"">
            </td>
            <td class=""header"">
                Shipping &amp; Handling
            </td>
            <td class=""field"">
                €7.50
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
                Total before Tax
            </td>
            <td class=""field"">
                €58,342.98
            </td>
        </tr>
        <tr>
            <td colspan=""5"">
            </td>
            <td class=""header"">
                Sales Tax
            </td>
            <td class=""field"">
                €12,252.02
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
                €70,595.00
            </td>
        </tr>
        </table>
    </div>
</body>
</html>
";
    }
}
