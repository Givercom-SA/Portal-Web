using System;

namespace Utilitario.Constante
{
    public static class EmbarqueConstante
    {

        public static class Banco
        {

            public const string BCP = "BANCO DE CREDITO";
            public const string SKB = "SCOTIABANK";
            public const string IBK = "INTERBANK";

            public const string BCP_CODIGO = "BCP";
            public const string SKB_CODIGO = "SKB";
            public const string IBK_CODIGO = "IBK";

        }

      
        public static class EmbarqueEstadoAgenteAduanas
        {

            public const int PENDIENTE = 1;
            public const int APROBADO = 2;
            public const int RECHAZADO = 3;
            public const int ANULADO = 4;

        }

        public static class EmbarqueMostrarOpcionRegFacturacionTercero
        {

            public static int NO = 0;
            public static int SI_INGRESO = 1;
            public static int SI_NOINGRESA_MENSAJE = 2;


        }



        public static class DireccionamientoModalidad
        {

            public static int DIFERIDO= 1;
            public static int SADA_DESCARGA_DIRECTA = 2;
            public static int SADA_PUNTO_LLEGADA = 3;


        }


        public static class ProcesoSistema
        {

            public static int ASIGNACION_PROCESO = 13;
            public static int LIBERACION_CARGA = 17;
            


        }


        public static class CondicionTransmares
        {


            public static string LCL = "01";
            public static string FCL = "01";

        }
        public static class AgenteAduanaActualizaTipoOpereacion
        {


            public static int Asignar = 1;
            public static int Anular =2;

        }
        public static class TipoDocumento
        {
            
   
            public static string DNI = "DNI";
            public static string RUC = "RUC";
          
        }

        public static class EntidadTransmaresTipoDocumento
        {
            public static string DNI = "0";
            public static string RUC = "1";

        }

        public static class EntidadTransmaresOpcion
        {
            public static string REGISTRO_INSTRUCCION_FACTU_TERCERO = "0";
            public static string GESTION_DIRECCIONAMIENTO = "1";
            


        }
        public static class EntidadTransmaresTipoEntidad
        {
            public static string REGISTRO_INSTRUCCION_FACTU_TERCERO = "0";
            public static string GESTION_DIRECCIONAMIENTO_MODALIDAD_SADA_DD = "1";
            public static string GESTION_DIRECCIONAMIENTO_MODALIDAD_NO_SADA_DD = "2";
            

        }




        public static class TipoCondicion
        {

            public static string CONDICION_FCL = "01";
            public static string CONDICION_LCL = "02";

        }

        public static class TipoPadre
        {

            public static string MASTER = "MASTER";
            public static string HOUSE = "HOUSE";

        }

        public static class TipoEntidad
        {
            public static string CLIENTE_FINAL = "TC01";
            public static string BROKER = "TC02";
            public static string AGENTE_ADUANAS = "TC03";
            public static string CLIENTE_FORWARDER = "TC04";

        }
 
        public static class TipoPerfil
        {
            public static string INTERNO = "TP01";
            public static string EXTERNO = "TP02";
            public static string SISTEMAS = "TC03";
      

        }


        public static class TipoEntidadTransmares
        {
            public static string CLIENTE_FINAL = "CL";
            public static string BROKET = "CB";
            public static string AGENTE_ADUANAS = "AA"; 
            public static string CLIENTE_FORWARDER = "WS";

        }
        public static class TipoPago
        {
            public static string CONTADO = "CO";
            public static string CREDITO= "CR";
            
        }

        public static class Trazabilidad
        {
            public static string MARITIMO = "M";
            public static string AEREO = "A";
            public static string IMPORTACION = "I";
            public static string EXPORTACION = "E";

        }

        public static class Estado
        {
            public static string TODO = "T";
            public static string ACTIVO = "A";
            public static string INACTIVO = "I";
            

        }

        public static class MetodoPago
        {
            public static string TRANSFERENCIA = "TR";
            public static string TARJETA = "PT";

        }

        public static class ErroresFormato
        {
            public static string DATO_FORMATO_INCORRECTO = "tiene formato incorrecto";
            public static string DATO_NO_VALIDO = "no valido";
            public static string NUMERO_NO_VALIDO = "es un numero no valido";
            public static string LETRAS_NO_VALIDA = "tiene letras no validas";
            public static string ALFANUMERICO_NO_VALIDO = "es un alfanumerico no valido";
            public static string LONGITUD_NO_VALIDA = "tiene una longitud no valida";
            public static string TIPO_NO_VALIDO = "tipo no valido";
            public static string TIPO_VIA_NO_VALIDO = "tipo no valido";
            public static string TIPO_LOCALIDAD_NO_VALIDO = "tipo no valido";
            public static string TIPO_TRABAJADOR_NO_VALIDO = "no valido";
            public static string UBIGEO_NO_VALIDO = "no valido";
            public static string FECHA_NO_VALIDA = "no valida";
            public static string FECHA_FORMATO = "formato no valido";
            public static string DISCADO_NO_VALIDO = "tiene discado no valido";
            public static string TELEF_NO_VALIDO = "no valido";
            public static string ORIGEN_ONP_NO_VALIDO = "no valido";
            public static string RUC_NO_VALIDO = "no valido";
            public static string FECINICLAB_MAYOR_ACTUAL = "mayor a la fecha actual";
            public static string FECINICLAB_ANTERIOR_FECNAC = "anterior o igual a la fecha de nacimiento";
            public static string FECHA_NACIMIENTO = "no valido";
            public static string CORREO_NO_VALIDO = "no valido";
            public static string INDEP_NO_VALIDO = "no valido";
            public static string EDAD_NO_PERMITIDA_DNI_MENOR = "no permitida para LAT";
            public static string EDAD_NO_PERMITIDA_DNI = "no permitida para DNI";
        }



        public static class Validacion
        {
            //Validacion 
            public static string SI_CUMPLE = "S";
            public static string NO_CUMPLE = "N";

            //Validaciones RENIEC
            public static string RENIEC_HAB_SERVICIO = "4";
            public static string RENIEC_EMPL_CANT_MAX = "3";

            public static string VALIDACION_OB_TIPDOC = "OB_TIPDOC";
            public static string VALIDACION_OB_NUMDOC = "OB_NUMDOC";
            public static string VALIDACION_OB_NOM = "OB_NOM";
            public static string VALIDACION_OB_APEPAT = "OB_APEPAT";
            public static string VALIDACION_OB_APEMAT = "OB_APEMAT";
            public static string VALIDACION_OB_FECHNAC = "OB_FECHNAC";
            public static string VALIDACION_OB_RUC = "OB_RUC";
            public static string VALIDACION_OB_RAZSOC = "OB_RAZSOC";
            public static string VALIDACION_OB_FECHINICLAB = "OB_FECHINICLAB";
        }

        public static class Patrones
        {
            public static string REGEXP_DNI = "^[\\d|\\D]{8}";
            public static string REGEXP_ALFA_NUMERICO = "^[a-zA-Z0-9][\\w+\\s\\w+]*";
            public static string REGEXP_NUMERICO = "^\\d*";
            public static string REGEXP_CARNET_EXTRANJERIA = "^[\\d|\\D]{9}";
            public static string REGEXP_DNI_MENOR = "^[\\d|\\D]{20}";
            public static string REGEXP_NOMBRE_NO_VALIDO = "[[^\\p{Punct}&&[^0-9]]|[']]+";
            public static string REGEXP_CORREO = "^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$";
        }



        public class EstadoGeneral
        {

            public static string PENDIENTE = "SP";
            public static string RECHAZADO = "SR";
            public static string APROBADO = "SA";

        }

        public static class Campos
        {

            public static string CAMPO_TIPO_DOCUMENTO = "Tipo de Documento";
            public static string CAMPO_NUMERO_DOCUMENTO = "Numero de Documento";
            public static string CAMPO_NOMBRE_COMPLETO = "Nombre Completo";
            public static string CAMPO_APELLIDO_PATERNO = "Apellido Paterno";
            public static string CAMPO_APELLIDO_MATERNO = "Apellido Materno";
            public static string CAMPO_FECHA_NACIMIENTO = "Fecha Nacimiento";
            public static string CAMPO_CORREO = "Correo Electronico";
            public static string CAMPO_TELEFONO_FIJO = "Telefono Fijo";
            public static string CAMPO_TELEFONO_MOVIL = "Telefono Movil";
            public static string CAMPO_UBIGEO = "Codigo de Ubigeo";
            public static string CAMPO_TIPO_VIA = "Tipo Via";
            public static string CAMPO_NOMBRE_VIA = "Nombre Via";
            public static string CAMPO_TIPO_LOCALIDAD = "Tipo Localidad";
            public static string CAMPO_NOMBRE_LOCALIDAD = "Nombre Localidad";
            public static string CAMPO_USUARIO_AGENTE = "Usuario Agente";


            public static string CAMPO_RUC = "Ruc";
            public static string CAMPO_RAZON_SOCIAL = "Razon Social";
            public static string CAMPO_CANAL = "Canal";

            public static int CAMPO_NOMBRE_COMPLETO_LONGITUD_MIN = 3;
            public static int CAMPO_NOMBRE_COMPLETO_LONGITUD_MAX = 61;

            public static int CAMPO_APELLIDO_LONGITUD_MIN = 1;
            public static int CAMPO_APELLIDO_LONGITUD_MAX = 40;

            public static int CAMPO_TELEFONO_MOVIL_LONGITUD = 9;

            public static string VALOR_EDAD = "Edad";
        }





    }
}