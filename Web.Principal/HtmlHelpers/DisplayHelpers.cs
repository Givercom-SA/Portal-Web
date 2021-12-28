using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Security.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Principal.HtmlHelpers
{
    public static partial class DisplayHelpers
    {
        public static IHtmlContent Encriptar(this IHtmlHelper htmlHelper, string valor)
          => new HtmlString(Encriptador.EncriptarTexto(valor));

        public static IHtmlContent FormatoMontoSoles(this IHtmlHelper htmlHelper, decimal valor)
         => new HtmlString(string.Format("{0} {1}", "S/. ", string.Format("{0:###,###,###,##0.00}", valor)));

        public static IHtmlContent FormatoFechaEstandar(this IHtmlHelper htmlHelper, DateTime valor)
        => new HtmlString(valor.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));

        
        public static IHtmlContent IntegerFormat(this IHtmlHelper htmlHelper, long value)
            => new HtmlString(string.Format("{0}", string.Format("{0:###,###,###,###}", value)));

     

     
    }

}
