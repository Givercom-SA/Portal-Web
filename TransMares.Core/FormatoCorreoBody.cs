using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitario.Constante;
using ViewModel.Datos.Embarque.SolicitudFacturacion;

namespace TransMares.Core
{
    public class FormatoCorreoBody
    {

        public string formatoBodyActivarCuenta(string pNombre, string pclave,string ImagenGrupo)
        {
            String HtmlSend = "";

            HtmlSend = $@"
                <!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml'>

                    <head runat='server'>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                        <title></title>
                    </head>

                    <body>
                        <center>
                            <div class='col-lg-12'>
                                <p class='card-text' style='height: 20px;'></p>
                            </div>
                            <div style='width:500px;background:#133bf0;border-radius:  10px 10px 10px 10px;border: 0px solid #000000; '>
                                <div style='height:30px'></div>

                                <table style='height: 407px; border-collapse: collapse' width='421' class='tabb'>
                                    <tbody style=' background:#ffffff '>
                                        <tr>
                                            <td style='padding: 12px;' colspan='3'>
                                                <span style='font-size: 25px; color: white;'>&nbsp; &nbsp; </span>
                                                <img src='{ImagenGrupo}'
                                                    style='height: auto;    width: 40%;'>
                                                <h4><b style='font-family:Arial, Helvetica, sans-serif;'> Estimado {pNombre} </b></h4>
                                                <p style='font-family:Arial, Helvetica, sans-serif;'>Te damos la bienvenida a Transmares Group</p>
                                            <center>
                                                <center>
                                                    <p style='font-family:Arial, Helvetica, sans-serif;'>
                                                        Falta poco para terminar tu solicitud, tu clave para verificación es
                                                        <br /> 
                                                            <strong style='text-size:12px; text-transform: uppercase;'> {pclave} </strong>
                                                        <br />
                                                    </p>

                                                </center>
                            
                                                <br />
                                                    <p style = 'font-family:Arial, Helvetica, sans-serif;' >
                                                        ¡Estamos felices que cuentes con nosotros!
                                                        <br /> El equipo de Transmares te lo agradece
                                                     </p>
                      
                                            </center>
                                            <div class='col-lg-12'>
                                                <p class='card-text' style='height: 20px;'></p>
                                            </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class='col-lg-12'>
                                    <p class='card-text' style='height: 20px;'></p>
                                </div>
                            </div>
                        </center>
                    </body>
                </html>";

            return HtmlSend;
        }

        public string formatoBodyNotificacionArribo(string pNombreClienteEmpresa, string pNumeracionEmbarque, string pEmpresaServicio, string ImagenGrupo)
        {
            String HtmlSend = "";

            HtmlSend = $@"
                <!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml'>

                    <head runat='server'>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                        <title></title>
                    </head>

                    <body>
                        <center>
                            <div class='col-lg-12'>
                                <p class='card-text' style='height: 20px;'></p>
                            </div>
                            <div style='width:500px;background:#133bf0;border-radius:  10px 10px 10px 10px;border: 0px solid #000000; '>
                                <div style='height:30px'></div>

                                <table style='height: 407px; border-collapse: collapse' width='421' class='tabb'>
                                    <tbody style=' background:#ffffff '>
                                        <tr>
                                            <td style='padding: 12px;' colspan='3'>
                                                <span style='font-size: 25px; color: white;'>&nbsp; &nbsp; </span>
                                                <img src='{ImagenGrupo}'
                                                    style='height: auto;    width: 40%;'>
                                                <h4><b style='font-family:Arial, Helvetica, sans-serif;'> Estimado {pNombreClienteEmpresa} </b></h4>
                                                <p style='font-family:Arial, Helvetica, sans-serif;'>
                                                    Se adjunta notificación de arribo correspodientes al número de embarque 546546546 {pNumeracionEmbarque}
                                                </p>
                                                <p>
                                                Attentamente   <br /> 
                                                {pEmpresaServicio.ToUpper()}                                                
                                                </p>
                                                <center>
                                                    <p style = 'font-family:Arial, Helvetica, sans-serif;' >
                                                        ¡Estamos felices que cuentes con nosotros!
                                                        <br /> El equipo de Transmares te lo agradece
                                                     </p>
                                                 </center>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class='col-lg-12'>
                                    <p class='card-text' style='height: 20px;'></p>
                                </div>
                            </div>
                        </center>
                    </body>
                </html>";

            return HtmlSend;
        }

        public string formatoBodyNotificacionFacturacion(string pNombreClienteEmpresa, string pNumeracionEmbarque, string ImagenGrupo)
        {
            String HtmlSend = "";

            HtmlSend = $@"
                <!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml'>

                    <head runat='server'>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                        <title></title>
                    </head>

                    <body>
                        <center>
                            <div class='col-lg-12'>
                                <p class='card-text' style='height: 20px;'></p>
                            </div>
                            <div style='width:500px;background:#133bf0;border-radius:  10px 10px 10px 10px;border: 0px solid #000000; '>
                                <div style='height:30px'></div>

                                <table style='height: 407px; border-collapse: collapse' width='421' class='tabb'>
                                    <tbody style=' background:#ffffff '>
                                        <tr>
                                            <td style='padding: 12px;' colspan='3'>
                                                <span style='font-size: 25px; color: white;'>&nbsp; &nbsp; </span>
                                                <img src='{ImagenGrupo}'
                                                    style='height: auto;    width: 40%;'>
                                                <h4><b style='font-family:Arial, Helvetica, sans-serif;'> Estimado {pNombreClienteEmpresa} </b></h4>
                                                <p style='font-family:Arial, Helvetica, sans-serif;'>
                                                    Se adjunta notificación de facturación correspodiente al Nro. de embarque {pNumeracionEmbarque}.
                                                </p>
                                                
                                                <center>
                                                    <p style = 'font-family:Arial, Helvetica, sans-serif;' >
                                                        ¡Estamos felices que cuentes con nosotros!
                                                        <br /> El equipo de Transmares te lo agradece
                                                     </p>
                                                 </center>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class='col-lg-12'>
                                    <p class='card-text' style='height: 20px;'></p>
                                </div>
                            </div>
                        </center>
                    </body>
                </html>";

            return HtmlSend;
        }


        public string formatoBodyBienvendaUsuarioSecundarioRenovada(string pNombre, string pContrasenia ,string pUrl, string ImagenGrupo)
        {
            String HtmlSend = "";

            HtmlSend = $@"
                <!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml'>

                    <head runat='server'>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                        <title></title>
                    </head>

                    <body>
                        <center>
                            <div class='col-lg-12'>
                                <p class='card-text' style='height: 20px;'></p>
                            </div>
                            <div style='width:500px;background:#133bf0;border-radius:  10px 10px 10px 10px;border: 0px solid #000000; '>
                                <div style='height:30px'></div>

                                <table style='height: 407px; border-collapse: collapse' width='421' class='tabb'>
                                    <tbody style=' background:#ffffff '>
                                        <tr>
                                            <td style='padding: 12px;' colspan='3'>
                                                <span style='font-size: 25px; color: white;'>&nbsp; &nbsp; </span>
                                                <img src='{ImagenGrupo}'
                                                    style='height: auto;    width: 40%;'>
                                                <p style='font-family:Arial, Helvetica, sans-serif;'>  Estimado {pNombre},<br />
                                                    Te damos la bienvenida a Transmares Group.<br />
                                                </p> Su contraseña de acceso es <b>{pContrasenia}</b>

                                                <center>
                                                <p style='font-family:Arial, Helvetica, sans-serif;'>
                                                    Para Confirmar tu correo has click <a href='{pUrl} '>Aqui</a>
                                                </p>
                                            </center>
                                                <center>
                                                    <p style = 'font-family:Arial, Helvetica, sans-serif;' >
                                                        ¡Estamos felices que cuentes con nosotros!
                                                        <br /> El equipo de Transmares te lo agradece
                                                     </p>
                                                 </center>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class='col-lg-12'>
                                    <p class='card-text' style='height: 20px;'></p>
                                </div>
                            </div>
                        </center>
                    </body>

                </html>";

            return HtmlSend;
        }

        public string formatoBodyBienvenidaAprobado(string pRuc, string pRazon, string pNombreRL, string correo, string clave, string imagenGrupo)
        {
            String HtmlSend = "";

            HtmlSend = $@"
                <!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml'>

                    <head runat='server'>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                        <title></title>
                    </head>

                    <body>
                        <center>
                            <div class='col-lg-12'>
                                <p class='card-text' style='height: 20px;'></p>
                            </div>
                            <div style='width:500px;background:#133bf0;border-radius:  10px 10px 10px 10px;border: 0px solid #000000; '>
                                <div style='height:30px'></div>

                                <table style='height: 407px; border-collapse: collapse' width='421' class='tabb'>
                                    <tbody style=' background:#ffffff '>
                                        <tr>
                                            <td align='center' style='padding: 12px;' colspan='3'>
                                                <span style='font-size: 25px; color: white;'>&nbsp; &nbsp; </span>
                                                <img src='{imagenGrupo}'
                                                    style='height: auto;    width: 10%;'>
                                                <h3><b style='font-family:Arial, Helvetica, sans-serif;'> Bienvenido <br /> {pRazon} <br /> {pRuc} </b> </h3>
                                                {pNombreRL}
                                                <p style='font-family:Arial, Helvetica, sans-serif;'>Te damos la bienvenida a la Plataforma
                                                    Web Digital</p>
                                                <center>
                                                    <center>
                                                        <p style='font-family:Arial, Helvetica, sans-serif;'>
                                                            Se ha procesado tu solicitud exitosamente.
                                                            <br /> <strong>Usuario: {correo}</strong>
                                                            <br /> <strong>Contraseña: {clave}</strong>
                                                        </p>
                                                        <p style='font-family:Arial, Helvetica, sans-serif;'>
                                                            Ingresa a la Plataforma y si es tu primer ingreso, le solicitará cambiar su
                                                            clave.
                                                        </p>
                                                        <p>
                                                            <a href='portalweb.tpsac.com.pe'> Acceder </a>
                                                        </ p >
                                                    </center>

                                                    <p style = 'font-family:Arial, Helvetica, sans-serif;' >
                                                        ¡Estamos felices que cuentes con nosotros!
                                                        <br /> El equipo de Transmares te lo agradece
                                                     </p>
                                                 </center>
 
                                                 <div class='col-lg-12'>
                                                    <p class='card-text' style='height: 20px;'></p>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style = 'background-color: #424141; color: white;'
                                                align='center' colspan='3'>
                                               
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class='col-lg-12'>
                                    <p class='card-text' style='height: 20px;'></p>
                                </div>
                            </div>
                        </center>
                    </body>

                </html>";

            return HtmlSend;
        }

        public string formatoBodyBienvenidaRechazada(string pRuc, string pRazon, string pNombreRL, string motivoRechazo, string imagenGrupo)
        {
            String HtmlSend = "";

            HtmlSend = $@"
                <!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml'>

                    <head runat='server'>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                        <title></title>
                    </head>

                    <body>
                        <center>
                            <div class='col-lg-12'>
                                <p class='card-text' style='height: 20px;'></p>
                            </div>
                            <div style='width:500px;background:#133bf0;border-radius:  10px 10px 10px 10px;border: 0px solid #000000; '>
                                <div style='height:30px'></div>

                                <table style='height: 407px; border-collapse: collapse' width='421' class='tabb'>
                                    <tbody style=' background:#ffffff '>
                                        <tr>
                                            <td align='center' style='padding: 12px;' colspan='3'>
                                                <span style='font-size: 25px; color: white;'>&nbsp; &nbsp; </span>
                                                <img src='{imagenGrupo}'
                                                    style='height: auto;    width: 40%;'>
                                                <h3><b style='font-family:Arial, Helvetica, sans-serif;'> {pRazon} <br /> {pRuc} </b> </h3>
                                                {pNombreRL}
                                                <p style='font-family:Arial, Helvetica, sans-serif;'>
                                                    Le informamos que su solicitud ha sido <strong>rechazada</strong> debido a :
                                                </p>
                                                <center>
                                                    <center>
                                                        <p style='font-family:Arial, Helvetica, sans-serif;'>
                                                            <hr/>";


            HtmlSend += $@"{motivoRechazo}";

            HtmlSend += @"<hr/>
                                                        </center>
                                                    </center>
                                                    <p style='font-family:Arial, Helvetica, sans-serif;'>
                                                        El equipo de Transmares se lo agradece
                                                    </p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style='background-color: #424141; color: white;'
                                                    align='center' colspan='3'>
                                                  
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div class='col-lg-12'>
                                        <p class='card-text' style='height: 20px;'></p>
                                    </div>
                                </div>
                            </center>
                        </body>
                    </html>";

            return HtmlSend;
        }

        public string formatoBodyNotificarEmbarque(string NombreCliente, string Mensaje,string nombreLogoEmpresa)
        {
            String HtmlSend = "";
            string imagen = nombreLogoEmpresa;


            HtmlSend = $@"
                <!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml'>

                    <head runat='server'>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                        <title></title>
                    </head>

                    <body>
                        <center>
                            <div class='col-lg-12'>
                                <p class='card-text' style='height: 20px;'></p>
                            </div>
                            <div style='width:500px;background:#133bf0;border-radius:  10px 10px 10px 10px;border: 0px solid #000000; '>
                                <div style='height:30px'></div>

                                <table style='height: 407px; border-collapse: collapse' width='421' class='tabb'>
                                    <tbody style=' background:#ffffff '>
                                        <tr>
                                            <td style='padding: 12px;' colspan='3'>
                                                <span style='font-size: 25px; color: white;'>&nbsp; &nbsp; </span>
                                                   <img src='{imagen}'
                                                    style='height: auto;    width: 40%;'>
                                                <h4><b style='font-family:Arial, Helvetica, sans-serif;'> 
                                                           
                                                <p style='font-family:Arial, Helvetica, sans-serif;'>{Mensaje}</p>
                                                <center>
                                                    <p style = 'font-family:Arial, Helvetica, sans-serif;' >
                                                        ¡Estamos felices que cuentes con nosotros!
                                                        <br /> El equipo de Transmares te lo agradece
                                                     </p>
                                                 </center>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class='col-lg-12'>
                                    <p class='card-text' style='height: 20px;'></p>
                                </div>
                            </div>
                        </center>
                    </body>
                </html>";

            return HtmlSend;
        }

        public string formatoBodyNotificarMemo( string Mensaje, string nombreLogoEmpresa)
        {
            String HtmlSend = "";
            string imagen = nombreLogoEmpresa;

            HtmlSend = $@"
                <!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml'>
                    <head runat='server'>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                        <title></title>
                    </head>
                    <body>
                        <center>
                            <div class='col-lg-12'>
                                <p class='card-text' style='height: 20px;'></p>
                            </div>
                            <div style='width:500px;background:#133bf0;border-radius:  10px 10px 10px 10px;border: 0px solid #000000; '>
                                <div style='height:30px'></div>
                                <table style='height: 407px; border-collapse: collapse' width='421' class='tabb'>
                                    <tbody style=' background:#ffffff '>
                                        <tr>
                                            <td style='padding: 12px;' colspan='3'>
                                                <span style='font-size: 25px; color: white;'>&nbsp; &nbsp; </span>
                                                 <img src='{imagen}'
                                                    style='height: auto;    width: 40%;'><br/>
                                                    <b style='font-family:Arial, Helvetica, sans-serif;'> Estimado cliente, </b>
                                                <p style='font-family:Arial, Helvetica, sans-serif;'>{Mensaje}.</p>
                                                <center>
                                                    <p style = 'font-family:Arial, Helvetica, sans-serif;' >
                                                        ¡Estamos felices que cuentes con nosotros!
                                                        <br /> El equipo de Transmares te lo agradece
                                                     </p>
                                                 </center>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style = 'background-color: #424141; color: white;'
                                                align='center' colspan='3'>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class='col-lg-12'>
                                    <p class='card-text' style='height: 20px;'></p>
                                </div>
                            </div>
                        </center>
                    </body>
                </html>";
            return HtmlSend;
        }

        public string formatoBodySolicitudCreada(string pNroSolicitud, string pMensaje, string nombreLogoEmpresa)
        {
            String HtmlSend = "";
            string imagen = nombreLogoEmpresa;



            HtmlSend = $@"
                <!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml'>

                    <head runat='server'>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                        <title></title>
                    </head>

                    <body>
                        <center>
                            <div class='col-lg-12'>
                                <p class='card-text' style='height: 20px;'></p>
                            </div>
                            <div style='width:500px;background:#133bf0;border-radius:  10px 10px 10px 10px;border: 0px solid #000000; '>
                                <div style='height:30px'></div>

                                <table style='height: 407px; border-collapse: collapse' width='421' class='tabb'>
                                    <tbody style=' background:#ffffff '>
                                        <tr>
                                            <td style='padding: 12px;' colspan='3'>
                                                <span style='font-size: 25px; color: white;'>&nbsp; &nbsp; </span>
                                                 <img src='{imagen}'
                                                    style='height: auto;    width: 40%;'>
                                                <h4><b style='font-family:Arial, Helvetica, sans-serif;'> Estimado usuario, </b></h4>
                                                <p style='font-family:Arial, Helvetica, sans-serif;'>{pMensaje}.</p>
                                                <p style='font-family:Arial, Helvetica, sans-serif;'>
                                                   <br /> <strong>Nro. Solicitud: </strong>{pNroSolicitud}
                                               </p>
                                                <center>
                                                    <p style = 'font-family:Arial, Helvetica, sans-serif;' >
                                                        ¡Estamos felices que cuentes con nosotros!
                                                        <br /> El equipo de Transmares te lo agradece
                                                     </p>
                                                 </center>

                                                 <div class='col-lg-12'>
                                                    <p class='card-text' style='height: 20px;'></p>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style = 'background-color: #424141; color: white;'
                                                align='center' colspan='3'>

                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class='col-lg-12'>
                                    <p class='card-text' style='height: 20px;'></p>
                                </div>
                            </div>
                        </center>
                    </body>
                </html>";

            return HtmlSend;
        }

        public string formatoBodySolicitudDirrecionamientoCreadaOperador(string pNroSolicitud, string pMensaje, string pNroBL, string pNaveViaje, string pConsignatario, string pCantidadCtn, string pDireccionado, string nombreLogoEmpresa)
        {
            String HtmlSend = "";
            string imagen = nombreLogoEmpresa;



            HtmlSend = $@"
                <!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml'>

                    <head runat='server'>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                        <title></title>
                    </head>

                    <body>
                        <center>
                            <div class='col-lg-12'>
                                <p class='card-text' style='height: 20px;'></p>
                            </div>
                            <div style='width:500px;background:#133bf0;border-radius:  10px 10px 10px 10px;border: 0px solid #000000; '>
                                <div style='height:30px'></div>

                                <table style='height: 407px; border-collapse: collapse' width='421' class='tabb'>
                                    <tbody style=' background:#ffffff '>
                                        <tr>
                                            <td style='padding: 12px;' colspan='3'>
                                                <span style='font-size: 25px; color: white;'>&nbsp; &nbsp; </span>
                                                 <img src='{imagen}'
                                                    style='height: auto;    width: 40%;'>
                                                <h4><b style='font-family:Arial, Helvetica, sans-serif;'> Estimado usuario, </b></h4>
                                                <p style='font-family:Arial, Helvetica, sans-serif;'>{pMensaje}.</p>
                                                <p style='font-family:Arial, Helvetica, sans-serif;'>
                                           <strong>N° BL: </strong>{pNroBL} <br>
                                            <strong>Nave/Viaje: </strong>{pNaveViaje} <br>
                                            <strong>Consignatario: </strong>{pConsignatario} <br>
                                            <strong>Total Ctrs.: </strong>{pCantidadCtn} <br>
                                            {pDireccionado}
                                               </p>
                                                <center>
                                                    <p style = 'font-family:Arial, Helvetica, sans-serif;' >
                                                        ¡Estamos felices que cuentes con nosotros!
                                                        <br /> El equipo de Transmares te lo agradece
                                                     </p>
                                                 </center>

                                                 <div class='col-lg-12'>
                                                    <p class='card-text' style='height: 20px;'></p>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style = 'background-color: #424141; color: white;'
                                                align='center' colspan='3'>

                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class='col-lg-12'>
                                    <p class='card-text' style='height: 20px;'></p>
                                </div>
                            </div>
                        </center>
                    </body>
                </html>";

            return HtmlSend;
        }

        public string formatoBodyAsignarCobroReportar(string nroEmbarque, string pMensaje, string nombreLogoEmpresa)
        {
            String HtmlSend = "";
            string imagen = nombreLogoEmpresa;



            HtmlSend = $@"
                <!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml'>

                    <head runat='server'>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                        <title></title>
                    </head>

                    <body>
                        <center>
                            <div class='col-lg-12'>
                                <p class='card-text' style='height: 20px;'></p>
                            </div>
                            <div style='width:500px;background:#133bf0;border-radius:  10px 10px 10px 10px;border: 0px solid #000000; '>
                                <div style='height:30px'></div>

                                <table style='height: 407px; border-collapse: collapse' width='421' class='tabb'>
                                    <tbody style=' background:#ffffff '>
                                        <tr>
                                            <td style='padding: 12px;' colspan='3'>
                                                <span style='font-size: 25px; color: white;'>&nbsp; &nbsp; </span>
                                                 <img src='{imagen}'
                                                    style='height: auto;    width: 40%;'>
                                                <h4><b style='font-family:Arial, Helvetica, sans-serif;'> Estimado operador, </b></h4>
                                                <p style='font-family:Arial, Helvetica, sans-serif;'>{pMensaje}.</p>
                                                <center>
                                                    <p style = 'font-family:Arial, Helvetica, sans-serif;' >
                                                        ¡Estamos felices que cuentes con nosotros!
                                                        <br /> El equipo de Transmares te lo agradece
                                                     </p>
                                                 </center>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style = 'background-color: #424141; color: white;'
                                                align='center' colspan='3'>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class='col-lg-12'>
                                    <p class='card-text' style='height: 20px;'></p>
                                </div>
                            </div>
                        </center>
                    </body>
                </html>";

            return HtmlSend;
        }
        public string formatoBodySolicitudFacturacionInterno(string mensaje,
                                                            SolicitarFacturacionParameterVM model,
                                                              string nombreLogoEmpresa,
                                                              string pNumeroSolicitud,
                                                              string nroEmbarque)
        {
            String HtmlSend = "";

            string imagen = nombreLogoEmpresa;


            CobroClienteVM cobroClienteProvosionSeleleccionado = new CobroClienteVM();

            if (model.ProvisionSeleccionado != null)
            {
                cobroClienteProvosionSeleleccionado = model.CobrosPendientesCliente.Where(x => x.IdFacturacionTercero == model.ProvisionSeleccionado).FirstOrDefault();
            }
            else
            {
                cobroClienteProvosionSeleleccionado = model.CobrosPendientesCliente[0];
            }


            HtmlSend = $@"
                <!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml'>

                    <head runat='server'>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                        <title></title>
                    </head>

                    <body>
                        <center>
                            <div class='col-lg-12'>
                                <p class='card-text' style='height: 20px;'></p>
                            </div>
                            <div style='width:500px;background:#133bf0;border-radius:  10px 10px 10px 10px;border: 0px solid #000000; '>
                                <div style='height:30px'></div>

                                <table style='height: 407px; border-collapse: collapse' width='421'>
                                    <tbody style=' background:#ffffff '>
                                        <tr>
                                            <td style='padding: 12px;' colspan='3'>
                                                <span style='font-size: 25px; color: white;'>&nbsp; &nbsp; </span>
                                                 <img src='{imagen}'
                                                    style='height: auto;    width: 40%;' />
        
                                                         <p style='font-family:Arial, Helvetica, sans-serif;'>
                                                                       {mensaje}
                                                        </p>
                                                <p style='font-family:Arial, Helvetica, sans-serif;'>
                                                                Nor. de Embarque: {nroEmbarque} <br/>
                                                                Fecha de Registro: {DateTime.Now.ToString("dd/MM/yyyy")} <br/>
                                                                Tipo de Pago: {model.TipoPagoString()} <br/>
                                                                Moneda de Solicitud: {cobroClienteProvosionSeleleccionado.CobrosPendientesEmbarque.ElementAt(0)?.Moneda} <br/>
                                                                Monto Total: {  cobroClienteProvosionSeleleccionado.MontoTotal} <br/>   
     
                                                </p>";


            StringBuilder stringBuilderDetalle = new StringBuilder();

            stringBuilderDetalle.AppendLine("  <table style='border: 1px solid #ccc; border-collapse: collapse'> ");

            stringBuilderDetalle.AppendLine("  <thead> <tr>");

            stringBuilderDetalle.AppendLine("  <th style='padding: 15px; border: 1px solid #ccc; border-collapse: collapse'> Concepto </th>");
            stringBuilderDetalle.AppendLine("  <th style='padding: 15px; border: 1px solid #ccc; border-collapse: collapse'> Moneda </th>");
            stringBuilderDetalle.AppendLine("  <th style='padding: 15px; border: 1px solid #ccc; border-collapse: collapse'> Total </th>");

            stringBuilderDetalle.AppendLine("  </tr> </thead> ");

            foreach (var item in cobroClienteProvosionSeleleccionado.CobrosPendientesEmbarque)
            {

                stringBuilderDetalle.AppendLine("  <tr> ");

                stringBuilderDetalle.AppendLine("  <td style='padding: 15px; border: 1px solid #ccc; border-collapse: collapse'> " + item.ConceptoCodigoDescripcion + " </td>");
                stringBuilderDetalle.AppendLine("  <td style='padding: 15px; border: 1px solid #ccc; border-collapse: collapse'> " + item.Moneda + " </td>");
                stringBuilderDetalle.AppendLine("  <td style='padding: 15px; border: 1px solid #ccc; border-collapse: collapse'> " + string.Format("{0}", item.Total) + " </td>");

                stringBuilderDetalle.AppendLine("  </tr> ");


            }

            stringBuilderDetalle.AppendLine("  </table> ");



            HtmlSend += stringBuilderDetalle.ToString();

            HtmlSend+=  @" <center>
                                                    <p style = 'font-family:Arial, Helvetica, sans-serif;' >
                                                        ¡Estamos felices que cuentes con nosotros!
                                                        <br /> El equipo de Transmares te lo agradece
                                                     </p>
                                                 </center>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style = 'background-color: #424141; color: white;'
                                                align='center' colspan='3'>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class='col-lg-12'>
                                    <p class='card-text' style='height: 20px;'></p>
                                </div>
                            </div>
                        </center>
                    </body>
                </html>";

            return HtmlSend;
        }


        public string formatoBodySolicitudMemoInterno(string mensaje,
                                                              string nombreLogoEmpresa,
                                                              string nroEmbarque)
        {
            String HtmlSend = "";

            string imagen = nombreLogoEmpresa;


            HtmlSend = $@"
                <!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml'>

                    <head runat='server'>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                        <title></title>
                    </head>

                    <body>
                        <center>
                            <div class='col-lg-12'>
                                <p class='card-text' style='height: 20px;'></p>
                            </div>
                            <div style='width:500px;background:#133bf0;border-radius:  10px 10px 10px 10px;border: 0px solid #000000; '>
                                <div style='height:30px'></div>

                                <table style='height: 407px; border-collapse: collapse' width='421'>
                                    <tbody style=' background:#ffffff '>
                                        <tr>
                                            <td style='padding: 12px;' colspan='3'>
                                                <span style='font-size: 25px; color: white;'>&nbsp; &nbsp; </span>
                                                 <img src='{imagen}'
                                                    style='height: auto;    width: 40%;' />
        
                                                         <p style='font-family:Arial, Helvetica, sans-serif;'>
                                                                       {mensaje}
                                                        </p>";


            HtmlSend += @" <center>
                                                    <p style = 'font-family:Arial, Helvetica, sans-serif;' >
                                                        ¡Estamos felices que cuentes con nosotros!
                                                        <br /> El equipo de Transmares te lo agradece
                                                     </p>
                                                 </center>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style = 'background-color: #424141; color: white;'
                                                align='center' colspan='3'>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class='col-lg-12'>
                                    <p class='card-text' style='height: 20px;'></p>
                                </div>
                            </div>
                        </center>
                    </body>
                </html>";

            return HtmlSend;
        }

        public string formatoBodySolicitudFacturacionCliente(
                                                          string mensaje,
                                                            SolicitarFacturacionParameterVM model,
                                                              string nombreLogoEmpresa,
                                                              string pNumeroSolicitud,
                                                              string nroEmbarque
                                                              )
        {
            String HtmlSend = "";
            string imagen = nombreLogoEmpresa;


            CobroClienteVM cobroClienteProvosionSeleleccionado = new CobroClienteVM();

            if (model.ProvisionSeleccionado != null)
            {
                cobroClienteProvosionSeleleccionado = model.CobrosPendientesCliente.Where(x => x.IdFacturacionTercero == model.ProvisionSeleccionado).FirstOrDefault();
            }
            else
            {
                cobroClienteProvosionSeleleccionado = model.CobrosPendientesCliente[0];
            }


            HtmlSend = $@"
                <!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml'>
                    <head runat='server'>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                        <title></title>
                    </head>
                    <body>
                        <center>
                            <div class='col-lg-12'>
                                <p class='card-text' style='height: 20px;'></p>
                            </div>
                            <div style='width:500px;background:#133bf0;border-radius:  10px 10px 10px 10px;border: 0px solid #000000; '>
                                <div style='height:30px'></div>
                                <table style='height: 407px; border-collapse: collapse' width='421' class='tabb'>
                                    <tbody style=' background:#ffffff '>
                                        <tr>
                                            <td style='padding: 12px;' colspan='3'>
                                                <span style='font-size: 25px; color: white;'>&nbsp; &nbsp; </span>
                                                 <img src='{imagen}'

                                                    style='height: auto;    width: 40%;' />
                                                                    <p style='font-family:Arial, Helvetica, sans-serif;'>
                                                                       {mensaje}
</p>
                                                <p style='font-family:Arial, Helvetica, sans-serif;'>
                                                                Nor. de Embarque: {nroEmbarque} <br/>
                                                                Fecha de Registro: {DateTime.Now.ToString("dd/MM/yyyy")} <br/>
                                                                Tipo de Pago: {model.TipoPagoString()} <br/>
                                                                Moneda de Solicitud: {cobroClienteProvosionSeleleccionado.CobrosPendientesEmbarque.ElementAt(0)?.Moneda} <br/>
                                                                Monto Total: {cobroClienteProvosionSeleleccionado.MontoTotal} <br/>             
     
                                                </p>";




            StringBuilder stringBuilderDetalle = new StringBuilder();

            stringBuilderDetalle.AppendLine("  <table style='border: 1px solid #ccc; border-collapse: collapse'> ");

            stringBuilderDetalle.AppendLine("  <thead> <tr>");

            stringBuilderDetalle.AppendLine("  <th style='padding: 15px; border: 1px solid #ccc; border-collapse: collapse'> Concepto </th>");
            stringBuilderDetalle.AppendLine("  <th style='padding: 15px; border: 1px solid #ccc; border-collapse: collapse'> Moneda </th>");
            stringBuilderDetalle.AppendLine("  <th style='padding: 15px; border: 1px solid #ccc; border-collapse: collapse'> Total </th>");

            stringBuilderDetalle.AppendLine("  </tr> </thead> ");

            foreach (var item in cobroClienteProvosionSeleleccionado.CobrosPendientesEmbarque) {

                stringBuilderDetalle.AppendLine("  <tr> ");

                stringBuilderDetalle.AppendLine("  <td style='padding: 15px; border: 1px solid #ccc; border-collapse: collapse'> "+ item .ConceptoCodigoDescripcion+ " </td>");
                stringBuilderDetalle.AppendLine("  <td style='padding: 15px; border: 1px solid #ccc; border-collapse: collapse'> "+ item.Moneda + " </td>");
                stringBuilderDetalle.AppendLine("  <td style='padding: 15px; border: 1px solid #ccc; border-collapse: collapse'> "+ string.Format("{0}", item.Total) + " </td>");

                stringBuilderDetalle.AppendLine("  </tr> ");


            }

            stringBuilderDetalle.AppendLine("  </table> ");



            HtmlSend += stringBuilderDetalle.ToString();
            HtmlSend +=@"<center>
                                                    <p style = 'font-family:Arial, Helvetica, sans-serif;' >
                                                        ¡Estamos felices que cuentes con nosotros!
                                                        <br /> El equipo de Transmares te lo agradece
                                                     </p>
                                                 </center>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style = 'background-color: #424141; color: white;'
                                                align='center' colspan='3'>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class='col-lg-12'>
                                    <p class='card-text' style='height: 20px;'></p>
                                </div>
                            </div>
                        </center>
                    </body>
                </html>";

            return HtmlSend;
        }

        public string formatoBodySolicitudMemoCliente(
                                                          string mensaje,
                                                              string nombreLogoEmpresa,
                                                              string nroEmbarque
                                                              )
        {
            String HtmlSend = "";
            string imagen = nombreLogoEmpresa;




            HtmlSend = $@"
                <!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml'>
                    <head runat='server'>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                        <title></title>
                    </head>
                    <body>
                        <center>
                            <div class='col-lg-12'>
                                <p class='card-text' style='height: 20px;'></p>
                            </div>
                            <div style='width:500px;background:#133bf0;border-radius:  10px 10px 10px 10px;border: 0px solid #000000; '>
                                <div style='height:30px'></div>
                                <table style='height: 407px; border-collapse: collapse' width='421' class='tabb'>
                                    <tbody style=' background:#ffffff '>
                                        <tr>
                                            <td style='padding: 12px;' colspan='3'>
                                                <span style='font-size: 25px; color: white;'>&nbsp; &nbsp; </span>
                                                 <img src='{imagen}'

                                                    style='height: auto;    width: 40%;' />
                                                                    <p style='font-family:Arial, Helvetica, sans-serif;'>
                                                                       {mensaje}
</p>
                                                <p style='font-family:Arial, Helvetica, sans-serif;'>
                                                                Nor. de Embarque: {nroEmbarque} <br/>
                                                                
     
                                                </p>";




            HtmlSend += @"<center>
                                                    <p style = 'font-family:Arial, Helvetica, sans-serif;' >
                                                        ¡Estamos felices que cuentes con nosotros!
                                                        <br /> El equipo de Transmares te lo agradece
                                                     </p>
                                                 </center>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style = 'background-color: #424141; color: white;'
                                                align='center' colspan='3'>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class='col-lg-12'>
                                    <p class='card-text' style='height: 20px;'></p>
                                </div>
                            </div>
                        </center>
                    </body>
                </html>";

            return HtmlSend;
        }


        public string formatoBodySolicitudAprobada(string pNroSolicitud, string nombreLogoEmpresa)
        {
            String HtmlSend = "";

            string imagen = nombreLogoEmpresa;

            HtmlSend = $@"
                <!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml'>

                    <head runat='server'>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                        <title></title>
                    </head>

                    <body>
                        <center>
                            <div class='col-lg-12'>
                                <p class='card-text' style='height: 20px;'></p>
                            </div>
                            <div style='width:500px;background:#133bf0;border-radius:  10px 10px 10px 10px;border: 0px solid #000000; '>
                                <div style='height:30px'></div>

                                <table style='height: 407px; border-collapse: collapse' width='421' class='tabb'>
                                    <tbody style=' background:#ffffff '>
                                        <tr>
                                            <td style='padding: 12px;' colspan='3'>
                                                                                      <span style='font-size: 25px; color: white;'>&nbsp; &nbsp; </span>
                                                <img src='{imagen}'
                                                    style='height: auto;    width: 40%;'>
                                                <h4><b style='font-family:Arial, Helvetica, sans-serif;'> Estimado cliente, </b></h4>
                                                <p style='font-family:Arial, Helvetica, sans-serif;'>La solicitud de memo ha sido APROBADO correctamente.</p>
                                                <p style='font-family:Arial, Helvetica, sans-serif;'>
                                                   <br /> <strong>Nro. Solicitud:</strong>{pNroSolicitud}

                                                <center>
                                                    <p style = 'font-family:Arial, Helvetica, sans-serif;' >
                                                        ¡Estamos felices que cuentes con nosotros!
                                                        <br /> El equipo de Transmares te lo agradece
                                                     </p>
                                                 </center>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class='col-lg-12'>
                                    <p class='card-text' style='height: 20px;'></p>
                                </div>
                            </div>
                        </center>
                    </body>
                </html>";
            return HtmlSend;

        }

        public string formatoBodySolicitudDireccionamientoRechazada(string pNroSolicitud, string MotivoRechazo, string nombreLogoEmpresa)
        {
            String HtmlSend = "";

            string imagen = nombreLogoEmpresa;

            HtmlSend = $@"
                <!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml'>

                    <head runat='server'>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                        <title></title>
                    </head>

                    <body>
                        <center>
                            <div class='col-lg-12'>
                                <p class='card-text' style='height: 20px;'></p>
                            </div>
                            <div style='width:500px;background:#133bf0;border-radius:  10px 10px 10px 10px;border: 0px solid #000000; '>
                                <div style='height:30px'></div>

                                <table style='height: 407px; border-collapse: collapse' width='421' class='tabb'>
                                    <tbody style=' background:#ffffff '>
                                        <tr>
                                            <td style='padding: 12px;' colspan='3'>
                                                <span style='font-size: 25px; color: white;'>&nbsp; &nbsp; </span>
                                                <img src='{imagen}'
                                                    style='height: auto;    width: 40%;'>
                                                <h4><b style='font-family:Arial, Helvetica, sans-serif;'> 
                                                 Estimado cliente, </b></h4>
                                                <p style='font-family:Arial, Helvetica, sans-serif;'> 
                                                 Le informamos que su solicitud de direccionamiento Nro. {pNroSolicitud} ha sido <strong>RECHAZADO</strong>. </p>
                                                    <br />  El motivo de rechazo es {MotivoRechazo}.

                                                <center>
                                                    <p style = 'font-family:Arial, Helvetica, sans-serif;' >
                                                        ¡Estamos felices que cuentes con nosotros!
                                                        <br /> El equipo de Transmares te lo agradece
                                                     </p>
                                                 </center>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class='col-lg-12'>
                                    <p class='card-text' style='height: 20px;'></p>
                                </div>
                            </div>
                        </center>
                    </body>
                </html>";




            return HtmlSend;
        }

        public string formatoBodySolicitudMemoRechazada(string pNroSolicitud, IEnumerable<string> listaDocumento,string nombreLogoEmpresa)
        {
            String HtmlSend = "";
            string imagen = nombreLogoEmpresa;

            HtmlSend = $@"
                <!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml'>

                    <head runat='server'>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                        <title></title>
                    </head>

                    <body>
                    <center>
                            <div class='col-lg-12'>
                                <p class='card-text' style='height: 20px;'></p>
                            </div>
                            <div style='width:500px;background:#133bf0;border-radius:  10px 10px 10px 10px;border: 0px solid #000000; '>
                                <div style='height:30px'></div>
                           
                                <table style='height: 407px; border-collapse: collapse' width='421' class='tabb'>
                                    <tbody style=' background:#ffffff '>
                                        <tr>
                                            <td align='center' style='padding: 12px;' colspan='3'>
                                                <span style='font-size: 25px; color: white;'>&nbsp; &nbsp; </span>
                                                <img src='{imagen}'
                                                    style='height: auto;    width: 40%;'>
                                                <h4><b style='font-family:Arial, Helvetica, sans-serif;'> Estimado cliente, </b></h4>
                                                <p style='font-family:Arial, Helvetica, sans-serif;'>
                                                    Le informamos que su solicitud {pNroSolicitud} ha sido <strong>RECHAZADA</strong> debido a observaciones de los siguientes documentos.
                                                </p>
                                                        <p style='font-family:Arial, Helvetica, sans-serif;'>
                                                            <hr/>";
                                                        foreach (var item in listaDocumento)
                                                        {
                                                            var arr = item.Split('|');
                                                            HtmlSend += string.Format(
                                                                "<strong>{0}</strong> <br /> Rechazada por {1} <br /> <br />",
                                                                arr[0].Trim(),
                                                                arr[1]).Trim();
                                                        }
                                                        HtmlSend += @"<hr/>
                                                        <p style='font-family:Arial, Helvetica, sans-serif;'>
                                                            Deberá corregir las observaciones indicadas y volver a presentar su solicitud.
                                                        </p>
                                                    <p style='font-family:Arial, Helvetica, sans-serif;'>
                                                        El equipo de Transmares se lo agradece
                                                    </p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style='background-color: #424141; color: white;'
                                                    align='center' colspan='3'>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                   
                                    <div class='col-lg-12'>
                                        <p class='card-text' style='height: 20px;'></p>
                                    </div>
                                </div>
                      </center>
                        </body>
                    </html>";

            return HtmlSend;
        }

        public string formatoBodySolicitud( string EmpresaSolicito, string Mensaje, string nombreLogoEmpresa)
        {

            String HtmlSend = "";
            string imagen = nombreLogoEmpresa;

            HtmlSend = $@"
                <!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml'>

                    <head runat='server'>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                        <title></title>
                    </head>

                    <body>
                        <center>
                            <div class='col-lg-12'>
                                <p class='card-text' style='height: 20px;'></p>
                            </div>
                            <div style='width:500px;background:#133bf0;border-radius:  10px 10px 10px 10px;border: 0px solid #000000; '>
                                <div style='height:30px'></div>

                                <table style='height: 407px; border-collapse: collapse' width='421' class='tabb'>
                                    <tbody style=' background:#ffffff '>
                                        <tr>
                                            <td style='padding: 12px;' colspan='3'>
                                                <span style='font-size: 25px; color: white;'>&nbsp; &nbsp; </span>
                                                <img src='{imagen}'
                                                    style='height: auto;    width: 40%;'>
                                                <h4><b style='font-family:Arial, Helvetica, sans-serif;'> 
                                                 Estimado {EmpresaSolicito}, </b></h4>
                                                <p style='font-family:Arial, Helvetica, sans-serif;'> {Mensaje} </p>
                                                <center>
                                                    <p style = 'font-family:Arial, Helvetica, sans-serif;' >
                                                        ¡Estamos felices que cuentes con nosotros!
                                                        <br /> El equipo de Transmares te lo agradece
                                                     </p>
                                                 </center>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class='col-lg-12'>
                                    <p class='card-text' style='height: 20px;'></p>
                                </div>
                            </div>
                        </center>
                    </body>
                </html>";

            return HtmlSend;
        }

        public string formatoBodyFacturacionTerceros(string NombreEmpresaRecibe, string nroBL, string nombreLogoEmpresa, string codigoSolicitudFactuTercero)
        {

            String HtmlSend = "";
            string imagen = "http://portalweb.tpsac.com.pe/img/"+ nombreLogoEmpresa ;

            HtmlSend = $@"
                <!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml'>

                    <head runat='server'>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                        <title></title>
                    </head>

                    <body>
                        <center>
                            <div class='col-lg-12'>
                                <p class='card-text' style='height: 20px;'></p>
                            </div>
                            <div style='width:500px;background:#133bf0;border-radius:  10px 10px 10px 10px;border: 0px solid #000000; '>
                                <div style='height:30px'></div>

                                <table style='height: 407px; border-collapse: collapse' width='421' class='tabb'>
                                    <tbody style=' background:#ffffff '>
                                        <tr>
                                            <td style='padding: 12px;' colspan='3'>
                                                <span style='font-size: 25px; color: white;'>&nbsp; &nbsp; </span>
                                                <img src='{imagen}'
                                                    style='height: auto;    width: 40%;'>
                                                <h4><b style='font-family:Arial, Helvetica, sans-serif;'> 
                                                 Estimado colaborador, </b></h4>
                                                <p style='font-family:Arial, Helvetica, sans-serif;'> La empresa <b>{NombreEmpresaRecibe}</b> ha generado una Solicitud de Instrucción de Facturación a Terceros <b>{codigoSolicitudFactuTercero}</b> relacionado al embarque <b>{nroBL}</b>. </p>
                                                <center>
                                                    <p style = 'font-family:Arial, Helvetica, sans-serif;' >
                                                        ¡Estamos felices que cuentes con nosotros!
                                                        <br /> El equipo de Transmares te lo agradece
                                                     </p>
                                                 </center>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class='col-lg-12'>
                                    <p class='card-text' style='height: 20px;'></p>
                                </div>
                            </div>
                        </center>
                    </body>
                </html>";

            return HtmlSend;
        }

        public string formatoBodySolicitudDireccionamientoCreada(string mensajeCliente, string pNroBL, string pNaveViaje, string pConsignatario, string pCantidadCtn, string pDireccionado, string nombreLogoEmpresa)
        {
            String HtmlSend = "";
            string imagen = nombreLogoEmpresa;


            HtmlSend = $@"
                <!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>

<head runat='server'>
   <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
   <title></title>
</head>

<body>
   <center>
      <div class='col-lg-12'>
         <p class='card-text' style='height: 20px;'></p>
      </div>
      <div style='width:500px;background:#133bf0;border-radius:  10px 10px 10px 10px;border: 0px solid #000000; '>
         <div style='height:30px'></div>

         <table style='height: 407px; border-collapse: collapse' width='421' class='tabb'>
            <tbody style=' background:#ffffff '>
               <tr>
                  <td style='padding: 12px;' colspan='3'>
                     <span style='font-size: 25px; color: white;'>&nbsp; &nbsp; </span>
                      <img src='{imagen}'
                        style='height: auto;    width: 40%;'>
                     <h4><b style='font-family:Arial, Helvetica, sans-serif;'> Estimado cliente, </b></h4>
                     <p style='font-family:Arial, Helvetica, sans-serif;'>{mensajeCliente}</p>
                     <p style='font-family:Arial, Helvetica, sans-serif;'>
                        <strong>N° BL: </strong>{pNroBL} <br>
                        <strong>Nave/Viaje: </strong>{pNaveViaje} <br>
                        <strong>Consignatario: </strong>{pConsignatario} <br>
                        <strong>Total Ctrs.: </strong>{pCantidadCtn} <br>
                        {pDireccionado}
                     </p>
                     <p style='font-family:Arial, Helvetica, sans-serif;font-size: 13px;'>
                        *El presente aviso corresponde a la recepción, más no aceptación del direccionamiento, el cual
                        se encuentra en proceso de revisión. Por tanto, para que su procedimiento proceda, es necesario
                        contar con nuestra confirmación.
                     </p>
                     </br>
                     <center>
                        <p style='font-family:Arial, Helvetica, sans-serif;'>
                           ¡Estamos felices que cuentes con nosotros!
                           <br /> El equipo de Transmares te lo agradece.
                        </p>
                     </center>
                     <div class='col-lg-12'>
                        <p class='card-text' style='height: 20px;'></p>
                     </div>
                  </td>
               </tr>
               <tr>
                  <td style='background-color: #424141; color: white;' align='center' colspan='3'>

                  </td>
               </tr>
            </tbody>
         </table>
         <div class='col-lg-12'>
            <p class='card-text' style='height: 20px;'></p>
         </div>
      </div>
   </center>
</body>

</html>";

            return HtmlSend;
        }

        public string formatoBodySolicitudDireccionamientoAprobada(string pNroSolicitud, string nombreLogoEmpresa)
        {
            String HtmlSend = "";


            string imagen = nombreLogoEmpresa;

            HtmlSend = $@"
                <!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml'>

                    <head runat='server'>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                        <title></title>
                    </head>

                    <body>
                        <center>
                            <div class='col-lg-12'>
                                <p class='card-text' style='height: 20px;'></p>
                            </div>
                            <div style='width:500px;background:#133bf0;border-radius:  10px 10px 10px 10px;border: 0px solid #000000; '>
                                <div style='height:30px'></div>

                                <table style='height: 407px; border-collapse: collapse' width='421' class='tabb'>
                                    <tbody style=' background:#ffffff '>
                                        <tr>
                                            <td style='padding: 12px;' colspan='3'>
                                                <span style='font-size: 25px; color: white;'>&nbsp; &nbsp; </span>
                                                <img src='{imagen}'
                                                    style='height: auto;    width: 40%;'>
                                                <h4><b style='font-family:Arial, Helvetica, sans-serif;'> 
                                                 Estimado cliente, </b></h4>
                                                <p style='font-family:Arial, Helvetica, sans-serif;'> 
                                                 Le informamos que su solicitud de direccionamiento Nro. {pNroSolicitud} ha sido <strong>APROBADO</strong>. </p>
                                                <center>
                                                    <p style = 'font-family:Arial, Helvetica, sans-serif;' >
                                                        ¡Estamos felices que cuentes con nosotros!
                                                        <br /> El equipo de Transmares te lo agradece
                                                     </p>
                                                 </center>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class='col-lg-12'>
                                    <p class='card-text' style='height: 20px;'></p>
                                </div>
                            </div>
                        </center>
                    </body>
                </html>";

            return HtmlSend;
        }

        public string formatoBodySolicitudFacturacionAprobadoRechazo(string pNroSolicitud,string pEstado, string nombreLogoEmpresa, string MotivoRechazo)
        {
            String HtmlSend = "";


            HtmlSend = $@"
                <!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml'>

                    <head runat='server'>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                        <title></title>
                    </head>

                    <body>
                        <center>
                            <div class='col-lg-12'>
                                <p class='card-text' style='height: 20px;'></p>
                            </div>
                            <div style='width:500px;background:#133bf0;border-radius:  10px 10px 10px 10px;border: 0px solid #000000; '>
                                <div style='height:30px'></div>

                                <table style='height: 407px; border-collapse: collapse' width='421' class='tabb'>
                                    <tbody style=' background:#ffffff '>
                                        <tr>
                                            <td style='padding: 12px;' colspan='3'>
                                                <span style='font-size: 25px; color: white;'>&nbsp; &nbsp; </span>
                                                <img src='{nombreLogoEmpresa}'
                                                    style='height: auto;    width: 40%;'>
                                                 <p style = 'font-family:Arial, Helvetica, sans-serif;' >
                                                <strong> Estimado cliente,</strong> <br/> <br/>
                                                Le informamos que su solicitud de facturación Nro. {pNroSolicitud} ha sido {pEstado}. <br/><br/>
                                                    {MotivoRechazo}
                                                    </p>            
                                                <center>
                                                    <p style = 'font-family:Arial, Helvetica, sans-serif;' >
                                                        ¡Estamos felices que cuentes con nosotros!
                                                        <br /> El equipo de Transmares te lo agradece
                                                     </p>
                                                 </center>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class='col-lg-12'>
                                    <p class='card-text' style='height: 20px;'></p>
                                </div>
                            </div>
                        </center>
                    </body>
                </html>";

            return HtmlSend;
        }


    }
}
