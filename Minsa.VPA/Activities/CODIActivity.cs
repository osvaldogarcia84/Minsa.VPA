using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Minsa.VPA.Modelos;
using Minsa.VPA.Proveedores;
using Minsa.VPA.Servicios;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minsa.VPA.Activities
{
    [Activity(Label = "Cobro CODI")]
    public class CODIActivity : Activity
    {
        ImageView imgQrCodi;
        Button RegresarRecibos;
        private ResultadoDeOperacionGenerico<Vendedor> vendedor;
        public Transacciones transacciones;
        public static string LlaveCODI = "LlaveCODI";
        ObtieneQRPago obtieneQRPago = new ObtieneQRPago();
        ProveedorDeVenta proveedorDeVenta = new ProveedorDeVenta();
        public ResultadoDeOperacionGenerico<ObtieneQRPago> resultado;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            vendedor =
                Servicios.ProveedorDeSerializado
                    .Obtener<ResultadoDeOperacionGenerico<Vendedor>>(
                        Intent.Obtener<string>(MainActivity.LlaveUsuario));

            transacciones =
                ProveedorDeSerializado.Obtener<Transacciones>(
                    Intent.Obtener<string>(LlaveCODI));

            SetContentView(Resource.Layout.CODI);

            imgQrCodi =
                FindViewById<ImageView>(Resource.Id.imgQrCodi);

            RegresarRecibos =
                FindViewById<Button>(Resource.Id.RegresarRecibosValores3);

            RegresarRecibos.Click += RegresarRecibos_Click;

            try
            {
                if (transacciones == null)
                {
                    Toast.MakeText(
                        this,
                        "No se encontró la información de la transacción.",
                        ToastLength.Long)
                        .Show();

                    Finish();
                    return;
                }
                //var importe = 10.00;
                var data = new DataReferenciaPagoCODI
                {
                    referencia =
                        transacciones.Cliente +
                        transacciones.Factura +
                        "XXXXXXXXXXXXX",
                        importe = Convert.ToDecimal(transacciones.Importe), //transacciones.Importe,
                        fuenteSolicita = "VPA"
                };

                var resultado = await proveedorDeVenta.SolicitarQrCodi(data);

                if (resultado == null)
                {
                    Toast.MakeText(
                        this,
                        "No se recibió respuesta del servicio CODI.",
                        ToastLength.Long)
                        .Show();

                    return;
                }

                //if (resultado.Tipo != TipoDeResultado.Exito)
                //{
                //    Toast.MakeText(
                //        this,
                //        resultado.Mensaje,
                //        ToastLength.Long)
                //        .Show();

                //    return;
                //}

                if (resultado.Valor == null)
                {
                    Toast.MakeText(
                        this,
                        "La respuesta CODI no contiene información.",
                        ToastLength.Long)
                        .Show();

                    return;
                }

                //if (resultado.Valor.JsonCodiEsError)
                //{
                //    string errorCodi =
                //        resultado.Valor.ObtenerErrorCodi();

                //    Toast.MakeText(
                //        this,
                //        "CODI regresó un error: " + errorCodi,
                //        ToastLength.Long)
                //        .Show();

                //    return;
                //}

                //if (!resultado.Valor.JsonCodiEsObjeto)
                //{
                //    Toast.MakeText(
                //        this,
                //        "La respuesta CODI no contiene un objeto válido.",
                //        ToastLength.Long)
                //        .Show();

                //    return;
                //}

                jsonResponseCODI respuestaCodi =
     resultado.Valor.ObtenerRespuestaCodi();

                if (respuestaCodi == null)
                {
                    string contenidoRecibido =
                        resultado.Valor.JsonResposeCODI != null
                            ? resultado.Valor.JsonResposeCODI.ToString()
                            : "NULL";

                    System.Diagnostics.Debug.WriteLine(
                        "No se convirtió jsonResposeCODI. Contenido: " +
                        contenidoRecibido);

                    Toast.MakeText(
                        this,
                        "No fue posible convertir la respuesta CODI.",
                        ToastLength.Long)
                        .Show();

                    return;
                }

                if (respuestaCodi.ReturnCodes == null)
                {
                    Toast.MakeText(
                        this,
                        "CODI no regresó el nodo returnCodes.",
                        ToastLength.Long)
                        .Show();

                    return;
                }

                if (respuestaCodi.ReturnCodes.ReturnCode != "00")
                {
                    Toast.MakeText(
                        this,
                        "CODI rechazó la solicitud: " +
                        respuestaCodi.ReturnCodes.Description,
                        ToastLength.Long)
                        .Show();

                    return;
                }

                if (string.IsNullOrWhiteSpace(
                    respuestaCodi.ImageResponse))
                {
                    Toast.MakeText(
                        this,
                        "CODI no regresó el nodo imageResponse.",
                        ToastLength.Long)
                        .Show();

                    return;
                }

                System.Diagnostics.Debug.WriteLine(
                    "Referencia CODI: " + respuestaCodi.Ref);

                CargarImagenBase64(
                    respuestaCodi.ImageResponse);
            }
            catch (Exception ex)
            {
                Toast.MakeText(
                    this,
                    "Error al generar el QR CODI: " +
                    ObtenerMensajeExcepcion(ex),
                    ToastLength.Long)
                    .Show();
            }
        }
        private void CargarImagenBase64(string base64)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(base64))
                {
                    Toast.MakeText(
                        this,
                        "La imagen CODI está vacía.",
                        ToastLength.Long)
                        .Show();

                    return;
                }

                base64 = base64.Trim();

                /*
                 * La API regresa:
                 *
                 * data:image/png;base64,iVBORw0...
                 *
                 * Eliminamos el prefijo y conservamos únicamente
                 * el contenido Base64.
                 */
                int posicionComa = base64.IndexOf(',');

                if (posicionComa >= 0)
                {
                    base64 =
                        base64.Substring(posicionComa + 1);
                }

                /*
                 * Eliminamos posibles saltos de línea o espacios
                 * agregados durante la transmisión.
                 */
                base64 = base64
                    .Replace("\r", string.Empty)
                    .Replace("\n", string.Empty)
                    .Replace(" ", string.Empty);

                byte[] bytes =
                    Convert.FromBase64String(base64);

                Bitmap bitmap =
                    BitmapFactory.DecodeByteArray(
                        bytes,
                        0,
                        bytes.Length);

                if (bitmap == null)
                {
                    Toast.MakeText(
                        this,
                        "No fue posible convertir la imagen CODI.",
                        ToastLength.Long)
                        .Show();

                    return;
                }

                imgQrCodi.SetImageBitmap(bitmap);
            }
            catch (FormatException)
            {
                Toast.MakeText(
                    this,
                    "La imagen CODI no tiene un formato Base64 válido.",
                    ToastLength.Long)
                    .Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(
                    this,
                    "Error al cargar el QR CODI: " +
                    ObtenerMensajeExcepcion(ex),
                    ToastLength.Long)
                    .Show();
            }
        }
        private void RegresarRecibos_Click(object sender, EventArgs e)
        {
            var activity = new Android.Content.Intent(this, typeof(ContenidoActivity));
            activity.PutExtra(MainActivity.LlaveUsuario, ProveedorDeSerializado.Generar(vendedor));
            //activity.PutExtra(LlaveVentaCompleta, ProveedorDeSerializado.Generar(ClientesProspecto));
            SetResult(Result.Ok, activity);
            Finish();
        }

        //private void CargarImagenBase64(string base64)
        //{
        //    try
        //    {
        //        // Por si viene como: data:image/png;base64,XXXX
        //        if (base64.Contains(","))
        //            base64 = base64.Substring(base64.IndexOf(",") + 1);

        //        byte[] bytes = Convert.FromBase64String(base64);

        //        Bitmap bitmap = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);

        //        if (bitmap == null)
        //        {
        //            Toast.MakeText(this, "No se pudo generar el QR CODI.", ToastLength.Long).Show();
        //            return;
        //        }

        //        imgQrCodi.SetImageBitmap(bitmap);
        //    }
        //    catch (Exception ex)
        //    {
        //        Toast.MakeText(this, "Error al cargar CODI: " + ex.Message, ToastLength.Long).Show();
        //    }
        //}
        private static string ObtenerMensajeExcepcion(Exception ex)
        {
            if (ex == null)
                return "Error no identificado.";

            var mensajes = new StringBuilder();

            Exception excepcionActual = ex;

            while (excepcionActual != null)
            {
                if (mensajes.Length > 0)
                {
                    mensajes.Append(
                        " | InnerException: ");
                }

                mensajes.Append(
                    excepcionActual.Message);

                excepcionActual =
                    excepcionActual.InnerException;
            }

            return mensajes.ToString();
        }



    }
}