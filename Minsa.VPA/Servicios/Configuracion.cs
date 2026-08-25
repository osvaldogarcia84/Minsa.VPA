using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Minsa.VPA.Servicios
{
    public  class Configuracion
    {
        // ========================================== Version ==========================================================

        public const string Version = "228";

        public const string Compania = "207";

        //=========================================== Preproduccion207 Servidor ALOM =================================================

         public const string ServidorDesarrollo = "http://alom.minsa.com.mx/WSVPA_DES/Modulos.svc";

        //=========================================== Produccion207 Servidor WSVPA ====================================================

        //public const string ServidorDesarrollo = "http://wsvpa1.minsa.com.mx/WCFVPA/Modulos.svc"; version 238 YA NO DESCOMENTAR

        //public const string ServidorDesarrollo = "http://wsvpa1.minsa.com.mx/WSVPAV4/Modulos.svc"; // version 244

        // =========================================== Produccion207 Servidor ALOM =====================================================

        // public const string ServidorDesarrollo = "http://alom.minsa.com.mx/WSVPA_PROD/Modulos.svc";

        // =========================================== URL CODI PRUEBAS ================================================================

        public const string ServidorCODI_DESA = "http://taak/codi/";

        public const string UsuarioCODI = "admin";
        public const string PasswordCODI = "+C@tedra-$21X.";

        public static bool ConexionClienteServidor(string direction)
        {
            bool conx = false;
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply resp;
                    resp = ping.Send(direction, 4000);

                    if (resp.Status == IPStatus.Success)
                    {
                        conx = true;
                    }
                }
            }
            catch (Exception)
            {
                conx = false;
            }
            return conx;
        }
    }
}