using System;

namespace Utilitario.Seguridad
{

    public class SeguridadCodigo {

    public string GenerarCadena()
    {
        Random obj = new Random();
        string posibles = Utilitario.Constante.SeguridadConstante.CodigoSeguridad.POSIBLES_CODIGOS;
        int longitud = posibles.Length;
        char letra;
        int longitudnuevacadena = 10;
        string nuevacadena = "";

        for (int i = 0; i < longitudnuevacadena; i++)
        {
            letra = posibles[obj.Next(longitud)];
            nuevacadena += letra.ToString();
        }
        return nuevacadena;
    }
    public string GenerarCadenaLongitud(int plongitud)
    {
        Random obj = new Random();
        string posibles = Utilitario.Constante.SeguridadConstante.CodigoSeguridad.POSIBLES_CODIGOS;
        int longitud = posibles.Length;
        char letra;
        int longitudnuevacadena = plongitud;
        string nuevacadena = "";

        for (int i = 0; i < longitudnuevacadena; i++)
        {
            letra = posibles[obj.Next(longitud)];
            nuevacadena += letra.ToString();
        }
        return nuevacadena;

    }


}
}
