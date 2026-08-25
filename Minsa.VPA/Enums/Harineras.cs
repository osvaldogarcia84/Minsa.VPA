using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Minsa.VPA.Enums
{
    public class Harineras
    {
        public static Harinera ObtenerHarinaera(string codigo)
        {
            Harinera harinera;
            switch (codigo)
            {
                case "001Min":
                    harinera = Harinera.Minsa;
                    break;
                case "002Mas":
                    harinera = Harinera.Maseca;
                    break;
                case "003Agr":
                    harinera = Harinera.Agroinsa;
                    break;
                case "006Bla":
                    harinera = Harinera.Blancas;
                    break;
                case "OPTIMASA":
                    harinera = Harinera.Optimasa;
                    break;
                case "007Har":
                    harinera = Harinera.Harimasa;
                    break;
                case "005Mac":
                    harinera = Harinera.Macsa;
                    break;
                case "011Hgr":
                    harinera = Harinera.Harigro;
                    break;
                case "Mz":
                    harinera = Harinera.Maiz;
                    break;
                case "Ma":
                    harinera = Harinera.Masa;
                    break;
                case "021Amz":
                    harinera = Harinera.Maizza;
                    break;
                case "015Con":
                    harinera = Harinera.Omalli;
                    break;
                case "013Ana":
                    harinera = Harinera.Rindemasa;
                    break;
                case "012Als":
                    harinera = Harinera.Maximasa;
                    break;
                case "033MiMa":
                    harinera = Harinera.MiMasita;
                    break;
                case "032MasH":
                    harinera = Harinera.MasHarina;
                    break;
                case "031Maix":
                    harinera = Harinera.Maixico;
                    break;
                case "030CamMx":
                    harinera = Harinera.DelCampoMexicano;
                    break;
                case "023Nat":
                    harinera = Harinera.Naturelo;
                    break;
                case "020Han":
                    harinera = Harinera.HarinaYCerealesAnahuac;
                    break;
                case "022Sbl":
                    harinera = Harinera.SanBlas;
                    break;
                case "019Gp":
                    harinera = Harinera.MaizGranosPatron;
                    break;
                case "018Por":
                    harinera = Harinera.MaizPortimex;
                    break;
                case "017Agv":
                    harinera = Harinera.Agrovision;
                    break;
                case "016Nu3":
                    harinera = Harinera.Masabor;
                    break;
                case "014Cam":
                    harinera = Harinera.ProductosDelCampo;
                    break;
                default:
                    harinera = Harinera.Otros;
                    break;
            }

            return harinera;
        }
    }
    public enum Harinera
    {
        Minsa = 48,
        Maseca = 4,
        Agroinsa = 6,
        Blancas = 11,
        Optimasa = 14,
        Harimasa = 17,
        Macsa = 20,
        Maiz = 46,
        Masa = 47,
        Harigro = 49,
        Maizza = 51,
        Omalli = 52,
        Rindemasa = 53,
        Maximasa = 54,
        MiMasita = 55,
        MasHarina = 56,
        Maixico = 57,
        DelCampoMexicano = 58,
        Naturelo = 59,
        HarinaYCerealesAnahuac = 60,
        SanBlas = 61,
        MaizGranosPatron = 62,
        MaizPortimex = 63,
        Agrovision = 64,
        Masabor = 65,
        ProductosDelCampo = 66,
        Otros = 67
    }
}