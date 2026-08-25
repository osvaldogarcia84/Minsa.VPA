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
using SQLite;
using Android.Util;
using System.IO;
using Minsa.VPA.Modelos;

namespace Minsa.VPA.Proveedores
{
    public class ProveedorLocalDatabase
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        String databaseName = "DBVPA.db";
        public bool createDataBase()
        {
            try
            {
               using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, databaseName)))
                {                  
                    connection.CreateTable<ClienteMovil>();
                    connection.CreateTable<GeoLocacion>();
                    connection.CreateTable<GeoLocacionSupervisores>();
                }                   
                    return true;                
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx: ", ex.Message);
                return false;
            }
        }
        public bool insertGeolocacion(GeoLocacion geoLocacion)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, databaseName)))
                {
                    connection.Insert(geoLocacion);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool insertGeolocacionSupervisores(GeoLocacionSupervisores geoLocacionSupervisores)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, databaseName)))
                {
                    connection.Insert(geoLocacionSupervisores);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool insertIntoTableRutas(ClienteMovil cliente)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, databaseName)))
                {                
                    connection.Insert(cliente);               
                    return true;
                }
            }
            catch(Exception ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public List<GeoLocacion> SelectGeolocation()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, databaseName)))
                {
                    return connection.Table<GeoLocacion>().ToList();

                }
            }
            catch(Exception ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        public List<GeoLocacionSupervisores> SelectGeolocationSupervisores()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, databaseName)))
                {
                    return connection.Table<GeoLocacionSupervisores>().ToList();

                }
            }
            catch (Exception ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        public List<ClienteMovil> SelectClientes()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, databaseName)))
                {
                    return connection.Table<ClienteMovil>().ToList();

                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        public bool deleteGeoLocacion()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, databaseName)))
                {
                    connection.Query<GeoLocacion>("delete from GeoLocacion");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool deleteGeoLocacionSupervisores()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, databaseName)))
                {
                    connection.Query<GeoLocacionSupervisores>("delete from GeoLocacionSupervisores");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
    }
}