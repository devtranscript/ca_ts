using NReco.VideoConverter;
using NReco.VideoInfo;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;

namespace ca_ts
{
    internal class Program
    {
        private static string str_Id = null;

        private static void Main(string[] args)
        {
            string str_ffmpg = null;
            foreach (Process p in Process.GetProcesses())
            {
                if (p.ProcessName == "ffmpeg.exe" || p.ProcessName == "ffmpeg")
                {
                    str_ffmpg = p.ProcessName;
                    break;
                }
                else
                {
                }
            }

            if (str_ffmpg == "ffmpeg")
            {
            }
            else
            {
                RecorreyCargaVideos();
                //caducidad_mat();
                //recorrer();
                //carga_manual();
            }
        }

        private static void RecorreyCargaVideos()
        {
            DateTime dt_fr, dt_frc;
            string ruta_ini, usr_ini, clv_ini, ruta_Destino = null, file_XML = null,
                pathXMLfiles = "*.jvl",
                pathWMVfiles = "*.wmv",
                pathMP4files = "*.mp4",
                pathASFfiles = "*.asf";
            int id_RutaVideos = 0, bol_SearchXMLfiles = 0;
            double differenceInMinutes;

            using (var md_ft = new bd_tsEntities())
            {
                var i_ft = (from c in md_ft.inf_fecha_transformacion
                            select c).ToList();

                if (i_ft.Count == 0)
                {
                    Console.WriteLine("Sin fecha de transformacion, favor de agregar");
                }
                else
                {
                    var i_rv = (from c in md_ft.inf_ruta_videos
                                select c).ToList();

                    if (i_rv.Count == 0)
                    {
                        Console.WriteLine("Sin rutas de videos, favor de agregar");
                    }
                    else
                    {
                        dt_fr = DateTime.Parse(i_ft[0].horario.ToString());
                        dt_frc = DateTime.Now;

                        differenceInMinutes = (double)(dt_frc - dt_fr).Minutes;
                        if (dt_frc >= dt_fr && differenceInMinutes == 0.0)
                        {
                            Console.WriteLine("Coincide la hora para la carga");

                            foreach (var f_rv in i_rv)
                            {
                                ruta_ini = f_rv.desc_ruta_ini;
                                usr_ini = f_rv.ruta_user_ini;
                                clv_ini = f_rv.ruta_pass_ini;
                                ruta_Destino = f_rv.desc_ruta_fin;
                                id_RutaVideos = f_rv.id_ruta_videos;

                                var networkPath = ruta_ini;
                                var credentials = new NetworkCredential(usr_ini, clv_ini);
                                try
                                {
                                    using (new networkconnection(networkPath, credentials))
                                    {
                                        var dir_list = Directory.GetDirectories(networkPath);
                                        foreach (var dir in dir_list)
                                        {
                                            if (bol_SearchXMLfiles == 0)
                                            {
                                                DirectoryInfo dir_p = new DirectoryInfo(dir.ToString());
                                                if (dir_p.Exists)
                                                {
                                                    foreach (FileInfo fil_asf in dir_p.GetFiles(pathASFfiles))
                                                    {
                                                        file_XML = fil_asf.FullName.ToString();
                                                        RegistraDatosVideo(file_XML, ruta_Destino, id_RutaVideos, bol_SearchXMLfiles);
                                                    }
                                                    foreach (FileInfo fil_wmv in dir_p.GetFiles(pathWMVfiles))
                                                    {
                                                        file_XML = fil_wmv.FullName.ToString();
                                                        RegistraDatosVideo(file_XML, ruta_Destino, id_RutaVideos, bol_SearchXMLfiles);
                                                    }
                                                    foreach (FileInfo fil_mp4 in dir_p.GetFiles(pathMP4files))
                                                    {
                                                        file_XML = fil_mp4.FullName.ToString();
                                                        RegistraDatosVideo(file_XML, ruta_Destino, id_RutaVideos, bol_SearchXMLfiles);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                DirectoryInfo dir_p = new DirectoryInfo(dir.ToString());
                                                if (dir_p.Exists)
                                                {
                                                    foreach (FileInfo fil_p in dir_p.GetFiles(pathXMLfiles))
                                                    {
                                                        file_XML = fil_p.FullName.ToString();
                                                        log_err(file_XML, ruta_Destino, id_RutaVideos);
                                                    }
                                                    var dir_ext = Directory.GetDirectories(dir_p.ToString());
                                                    foreach (var dir_n in dir_ext)
                                                    {
                                                        var lis_jvl = Directory.GetFiles(dir_n, "*jvl");
                                                        if (lis_jvl.Length > 0)
                                                        {
                                                            string ff = lis_jvl[0].ToString();
                                                            log_err_ext(ff, ruta_Destino, id_RutaVideos);
                                                        }
                                                        else
                                                        {

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Sin acceso a la ruta de red, {0}", e.ToString());
                                    Console.WriteLine("Favor de revisar o contactar a soporte");
                                }
                            }
                        }
                        else // No Coincide la hora para la carga
                        {
                            using (var md_ft_f = new bd_tsEntities())
                            {
                                var i_ft_f = (from c in md_ft_f.inf_master_jvl
                                              select c).ToList();

                                if (i_ft_f.Count == 0)
                                {
                                }
                                else
                                {
                                    foreach (var f_rv in i_rv)
                                    {
                                        ruta_ini = f_rv.desc_ruta_ini;
                                        usr_ini = f_rv.ruta_user_ini;
                                        clv_ini = f_rv.ruta_pass_ini;
                                        ruta_Destino = f_rv.desc_ruta_fin;
                                        id_RutaVideos = f_rv.id_ruta_videos;

                                        var networkPath = ruta_ini;
                                        var credentials = new NetworkCredential(usr_ini, clv_ini);
                                        try
                                        {
                                            using (new networkconnection(networkPath, credentials))
                                            {
                                                var dir_list = Directory.GetDirectories(networkPath);
                                                foreach (var dir in dir_list)
                                                {
                                                    if (bol_SearchXMLfiles == 0)
                                                    {

                                                    }
                                                    else
                                                    {
                                                        DirectoryInfo dir_p = new DirectoryInfo(dir.ToString());
                                                        if (dir_p.Exists)
                                                        {
                                                            foreach (FileInfo fil_p in dir_p.GetFiles(pathXMLfiles))
                                                            {
                                                                file_XML = fil_p.FullName.ToString();
                                                                p_nuevos(file_XML, ruta_Destino, id_RutaVideos);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine("Sin acceso a la ruta de red, {0}", e.ToString());
                                            Console.WriteLine("Favor de revisar o contactar a soporte");
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }

        private static void RegistraDatosVideo(string file_XML, string ruta_fin, int id_rv, int bol_SearchXMLfiles)
        {
            string dir_p, dir_m, nom_vid = null, err_estructura = null,
                pathWMVfiles = "*.wmv",
                pathMP4files = "*.mp4",
                pathASFfiles = "*.asf";
            Guid str_MasterId;
            int err_c = 0;
            string str_Title = null, str_Department, str_IsSealed, str_Exhibits;

            FileInfo fi_vid = new FileInfo(file_XML);
            nom_vid = Path.GetFileNameWithoutExtension(file_XML); ;
            dir_p = fi_vid.Directory.Name;

            DataSet dataSet = new DataSet();
            //int num = (int)dataSet.ReadXml(file_XML);

            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = new DataTable();
            DataTable dataTable3 = new DataTable();
            DataTable dataTable4 = new DataTable();

            DataTable table = dataSet.Tables["Master"];
            Guid str_idcontrol = Guid.NewGuid();

            foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
            {
                str_MasterId = Guid.Parse(row[0].ToString());
                str_Id = row[1].ToString();
                str_Title = row[2].ToString();
                str_Department = row[3].ToString();
                str_IsSealed = row[4].ToString();
                str_Exhibits = row[6].ToString();
            }

            if (str_Id == nom_vid)
            {
            }
            else
            {
                err_c = 1;
            }

            if (str_Id == dir_p)
            {
            }
            else
            {
                err_c = err_c + 2 + 1;
            }

            var dir_list = Directory.GetDirectories(Path.GetDirectoryName(file_XML));

            foreach (var dir in dir_list)
            {
                var list = Directory.GetFiles(dir, pathWMVfiles);
                if (list.Length > 0)
                {
                    DirectoryInfo di = new DirectoryInfo(dir);
                    dir_m = di.Name;

                    if (str_Id == dir_m)
                    {
                    }
                    else
                    {
                        err_c = err_c + 1;
                    }
                }

                var list_asf = Directory.GetFiles(dir, pathASFfiles);
                if (list_asf.Length > 0)
                {
                    DirectoryInfo di = new DirectoryInfo(dir);
                    dir_m = di.Name;

                    if (str_Id == dir_m)
                    {
                    }
                    else
                    {
                        err_c = err_c + 1;
                    }
                }
            }
            switch (err_c)
            {
                case 1:
                    err_estructura = "err_nom_vid";
                    break;

                case 2:
                    err_estructura = "err_dir_princial";
                    break;

                case 3:
                    err_estructura = "err_dir_media";
                    break;

                case 4:
                    err_estructura = "err_nom_jvl / err_dir_princial";
                    break;

                case 5:
                    err_estructura = "err_nom_jvl / err_dir_princial / err_dir_media";
                    break;

                default:
                    err_estructura = "Sin Errores";
                    break;
            }

            if (err_estructura == "Sin Errores")
            {
                using (var edm_master = new bd_tsEntities())
                {
                    var i_master = (from c in edm_master.inf_master_jvl
                                    where c.sesion == nom_vid
                                    select c).ToList();

                    if (i_master.Count == 0)
                    {
                        inf_master_jvl infMaster = new inf_master_jvl()
                        {
                            id_control_exp = str_idcontrol,
                            sesion = str_Id,
                            titulo = str_Title,
                            err_carga = err_estructura,
                            id_estatus_exp = 2,
                            id_estatus_qa = 1,
                            id_ruta_videos = id_rv,
                            fecha_registro = DateTime.Now
                        };
                        edm_master.inf_master_jvl.Add(infMaster);
                        edm_master.SaveChanges();

                        string dir_c = Path.GetDirectoryName(file_XML);
                        string dir_root = ruta_fin + "\\" + nom_vid;

                        if (Directory.Exists(dir_root))
                        {
                            Directory.CreateDirectory(dir_root);
                            file_library.CopyDirectory(dir_c, dir_root, false);
                        }
                        else
                        {
                            FFMpegConverter ffMpegConverter = new FFMpegConverter();
                            FFProbe ffProbe = new FFProbe();

                            Directory.CreateDirectory(dir_root);
                            file_library.CopyDirectory(dir_c, dir_root, false);

                            var dir_wmv = Directory.GetDirectories(dir_root);
                            foreach (var dir in dir_wmv)
                            {
                                var lis_wmv = Directory.GetFiles(dir, pathWMVfiles);
                                if (lis_wmv.Length > 0)
                                {
                                    foreach (var l_wmv in lis_wmv)
                                    {
                                        FileInfo fi_wmv = new FileInfo(l_wmv);
                                        var videoInfo = ffProbe.GetMediaInfo(fi_wmv.ToString());
                                        string dur_wmv = videoInfo.Duration.Hours + ":" + videoInfo.Duration.Minutes + ":" + videoInfo.Duration.Seconds;

                                        var i_media = (from c in edm_master.inf_exp_mat
                                                       where c.nom_archivo == fi_wmv.Name
                                                       select c).ToList();

                                        if (i_media.Count == 0)
                                        {
                                            var g_media = new inf_exp_mat
                                            {
                                                ruta_archivo = fi_wmv.ToString().Replace(".wmv", ".mp4"),

                                                duracion = dur_wmv,
                                                nom_archivo = fi_wmv.Name.Replace(".wmv", ""),
                                                id_est_mat = 2,
                                                id_control_exp = str_idcontrol,
                                                fecha_registro = DateTime.Now,
                                            };

                                            edm_master.inf_exp_mat.Add(g_media);
                                            edm_master.SaveChanges();

                                            try
                                            {
                                                ffMpegConverter.ConvertMedia(fi_wmv.ToString(), fi_wmv.ToString().Replace(".wmv", ".mp4"), Format.mp4);
                                                File.Delete(fi_wmv.ToString());

                                                using (var act_media = new bd_tsEntities())
                                                {
                                                    var a_exp = (from c in act_media.inf_master_jvl
                                                                 where c.id_control_exp == str_idcontrol
                                                                 select c).FirstOrDefault();

                                                    a_exp.id_estatus_exp = 1;
                                                    act_media.SaveChanges();

                                                    var a_media = (from c in act_media.inf_exp_mat
                                                                   where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                                   select c).FirstOrDefault();

                                                    a_media.id_est_mat = 1;
                                                    act_media.SaveChanges();
                                                }
                                            }
                                            catch
                                            {
                                                using (var act_media = new bd_tsEntities())
                                                {
                                                    var a_media = (from c in act_media.inf_exp_mat
                                                                   where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                                   select c).FirstOrDefault();

                                                    a_media.id_est_mat = 3;

                                                    act_media.SaveChanges();
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            var dir_asf = Directory.GetDirectories(dir_root);
                            foreach (var dir in dir_asf)
                            {
                                var lis_asf = Directory.GetFiles(dir, pathASFfiles);
                                if (lis_asf.Length > 0)
                                {
                                    foreach (var l_asf in lis_asf)
                                    {
                                        FileInfo fi_asf = new FileInfo(l_asf);
                                        var videoInfo = ffProbe.GetMediaInfo(fi_asf.ToString());
                                        string dur_asf = videoInfo.Duration.Hours + ":" + videoInfo.Duration.Minutes + ":" + videoInfo.Duration.Seconds;

                                        var i_media = (from c in edm_master.inf_exp_mat
                                                       where c.nom_archivo == fi_asf.Name
                                                       select c).ToList();

                                        if (i_media.Count == 0)
                                        {
                                            var g_media = new inf_exp_mat
                                            {
                                                ruta_archivo = fi_asf.ToString().Replace(".asf", ".mp4"),

                                                duracion = dur_asf,
                                                nom_archivo = fi_asf.Name.Replace(".asf", ""),
                                                id_est_mat = 2,
                                                id_control_exp = str_idcontrol,
                                                fecha_registro = DateTime.Now,
                                            };

                                            edm_master.inf_exp_mat.Add(g_media);
                                            edm_master.SaveChanges();

                                            try
                                            {
                                                ffMpegConverter.ConvertMedia(fi_asf.ToString(), fi_asf.ToString().Replace(".asf", ".mp4"), Format.mp4);
                                                File.Delete(fi_asf.ToString());

                                                using (var act_media = new bd_tsEntities())
                                                {
                                                    var a_exp = (from c in act_media.inf_master_jvl
                                                                 where c.id_control_exp == str_idcontrol
                                                                 select c).FirstOrDefault();

                                                    a_exp.id_estatus_exp = 1;
                                                    act_media.SaveChanges();

                                                    var a_media = (from c in act_media.inf_exp_mat
                                                                   where c.nom_archivo == fi_asf.Name.Replace(".asf", "")
                                                                   select c).FirstOrDefault();

                                                    a_media.id_est_mat = 1;
                                                    act_media.SaveChanges();
                                                }
                                            }
                                            catch
                                            {
                                                using (var act_media = new bd_tsEntities())
                                                {
                                                    var a_media = (from c in act_media.inf_exp_mat
                                                                   where c.nom_archivo == fi_asf.Name.Replace(".asf", "")
                                                                   select c).FirstOrDefault();

                                                    a_media.id_est_mat = 3;
                                                    act_media.SaveChanges();
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        string dir_c = Path.GetDirectoryName(file_XML);
                        string dir_root = ruta_fin + "\\" + nom_vid;

                        FFMpegConverter ffMpegConverter = new FFMpegConverter();
                        FFProbe ffProbe = new FFProbe();

                        var dir_wmv = Directory.GetDirectories(dir_c);
                        foreach (var dir in dir_wmv)
                        {
                            var lis_wmv = Directory.GetFiles(dir, pathWMVfiles);
                            if (lis_wmv.Length > 0)
                            {
                                foreach (var l_wmv in lis_wmv)
                                {
                                    FileInfo fi_wmv = new FileInfo(l_wmv);
                                    var videoInfo = ffProbe.GetMediaInfo(fi_wmv.ToString());
                                    string dur_wmv = videoInfo.Duration.Hours + ":" + videoInfo.Duration.Minutes + ":" + videoInfo.Duration.Seconds;

                                    var i_media = (from c in edm_master.inf_exp_mat
                                                   where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                   select c).ToList();

                                    if (i_media.Count == 0)
                                    {
                                    }
                                    else
                                    {
                                        var g_media = new inf_exp_mat

                                        {
                                            ruta_archivo = fi_wmv.ToString().Replace(".wmv", ".mp4"),

                                            duracion = dur_wmv,
                                            nom_archivo = fi_wmv.Name.Replace(".wmv", ""),
                                            id_est_mat = 2,
                                            id_control_exp = str_idcontrol,
                                            fecha_registro = DateTime.Now,
                                        };

                                        edm_master.inf_exp_mat.Add(g_media);
                                        edm_master.SaveChanges();

                                        try
                                        {
                                            ffMpegConverter.ConvertMedia(fi_wmv.ToString(), fi_wmv.ToString().Replace(".wmv", ".mp4"), Format.mp4);
                                            File.Delete(fi_wmv.ToString());

                                            using (var act_media = new bd_tsEntities())
                                            {
                                                var a_media = (from c in act_media.inf_exp_mat
                                                               where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                               select c).FirstOrDefault();

                                                a_media.id_est_mat = 1;
                                                act_media.SaveChanges();
                                            }
                                        }
                                        catch
                                        {
                                            using (var act_media = new bd_tsEntities())
                                            {
                                                var a_media = (from c in act_media.inf_exp_mat
                                                               where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                               select c).FirstOrDefault();

                                                a_media.id_est_mat = 3;
                                                act_media.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        var dir_asf = Directory.GetDirectories(dir_c);
                        foreach (var dir in dir_asf)
                        {
                            var lis_asf = Directory.GetFiles(dir, pathASFfiles);
                            if (lis_asf.Length > 0)
                            {
                                foreach (var l_asf in lis_asf)
                                {
                                    FileInfo fi_asf = new FileInfo(l_asf);
                                    var videoInfo = ffProbe.GetMediaInfo(fi_asf.ToString());
                                    string dur_asf = videoInfo.Duration.Hours + ":" + videoInfo.Duration.Minutes + ":" + videoInfo.Duration.Seconds;

                                    var i_media = (from c in edm_master.inf_exp_mat
                                                   where c.nom_archivo == fi_asf.Name.Replace(".asf", "")
                                                   select c).ToList();

                                    if (i_media.Count == 0)
                                    {
                                    }
                                    else
                                    {
                                        var g_media = new inf_exp_mat

                                        {
                                            ruta_archivo = fi_asf.ToString().Replace(".asf", ".mp4"),

                                            duracion = dur_asf,
                                            nom_archivo = fi_asf.Name.Replace(".asf", ""),
                                            id_est_mat = 2,
                                            id_control_exp = str_idcontrol,
                                            fecha_registro = DateTime.Now,
                                        };

                                        edm_master.inf_exp_mat.Add(g_media);
                                        edm_master.SaveChanges();

                                        try
                                        {
                                            ffMpegConverter.ConvertMedia(fi_asf.ToString(), fi_asf.ToString().Replace(".asf", ".mp4"), Format.mp4);
                                            File.Delete(fi_asf.ToString());

                                            using (var act_media = new bd_tsEntities())
                                            {
                                                var a_media = (from c in act_media.inf_exp_mat
                                                               where c.nom_archivo == fi_asf.Name.Replace(".asf", "")
                                                               select c).FirstOrDefault();

                                                a_media.id_est_mat = 1;
                                                act_media.SaveChanges();
                                            }
                                        }
                                        catch
                                        {
                                            using (var act_media = new bd_tsEntities())
                                            {
                                                var a_media = (from c in act_media.inf_exp_mat
                                                               where c.nom_archivo == fi_asf.Name.Replace(".asf", "")
                                                               select c).FirstOrDefault();

                                                a_media.id_est_mat = 3;
                                                act_media.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }
            else
            {
                using (var edm_master = new bd_tsEntities())
                {
                    var i_master = (from c in edm_master.inf_master_jvl
                                    where c.sesion == nom_vid
                                    select c).ToList();

                    if (i_master.Count == 0)
                    {
                        inf_master_jvl infMaster = new inf_master_jvl()
                        {
                            id_control_exp = str_idcontrol,
                            sesion = str_Id,
                            titulo = str_Title,
                            err_carga = err_estructura,
                            id_estatus_exp = 3,
                            id_estatus_qa = 1,
                            id_ruta_videos = id_rv,
                            fecha_registro = DateTime.Now
                        };
                        edm_master.inf_master_jvl.Add(infMaster);
                        edm_master.SaveChanges();
                    }
                    else
                    {
                    }
                }
            }
        }

        private static void carga_manual()
        {
            Guid id_control_esp;

            using (var md_ft = new bd_tsEntities())
            {
                var i_ft = (from c in md_ft.inf_master_jvl
                            where c.id_estatus_exp == 2
                            select c).ToList();

                if (i_ft.Count == 0)
                {
                    Console.WriteLine("Sin registros para la fecha de carga, favor de agregar");
                }
                else
                {
                    id_control_esp = i_ft[0].id_control_exp;
                    foreach (var f_rv in i_ft)
                    {
                        var i_em = (from c in md_ft.inf_exp_mat
                                    where c.id_control_exp == id_control_esp
                                    select c).ToList();

                        foreach (var i_n in i_em)
                        {
                            FileInfo fi_wmv = new FileInfo(i_n.ruta_archivo.Replace(".mp4", ".wmv"));
                            try
                            {
                                FFMpegConverter ffMpegConverter = new FFMpegConverter();
                                FFProbe ffProbe = new FFProbe();
                                ffMpegConverter.ConvertMedia(fi_wmv.ToString(), fi_wmv.ToString().Replace(".wmv", ".mp4"), Format.mp4);
                                File.Delete(fi_wmv.ToString());

                                using (var act_media = new bd_tsEntities())
                                {
                                    var a_exp = (from c in act_media.inf_master_jvl
                                                 where c.id_control_exp == id_control_esp
                                                 select c).FirstOrDefault();

                                    a_exp.id_estatus_exp = 1;

                                    act_media.SaveChanges();

                                    var a_media = (from c in act_media.inf_exp_mat
                                                   where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                   select c).FirstOrDefault();

                                    a_media.id_est_mat = 1;

                                    act_media.SaveChanges();
                                }
                            }
                            catch
                            {
                                using (var act_media = new bd_tsEntities())
                                {
                                    var a_media = (from c in act_media.inf_exp_mat
                                                   where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                   select c).FirstOrDefault();

                                    a_media.id_est_mat = 3;

                                    act_media.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void recorrer()
        {
            DateTime dt_fr, dt_frc;
            string ruta_ini, usr_ini, clv_ini, ruta_fin = null, usr_fin, clv_fin, file_jvl = null, dir_m, jvl_nom, err_estructura = null; ;
            int int_dcv, id_rv = 0;
            double differenceInMinutes;

            using (var md_ft = new bd_tsEntities())
            {
                var i_ft = (from c in md_ft.inf_fecha_transformacion
                            select c).ToList();

                if (i_ft.Count == 0)
                {
                    Console.WriteLine("Sin registros para la fecha de carga, favor de agregar");
                }
                else
                {
                    var i_rv = (from c in md_ft.inf_ruta_videos
                                select c).ToList();

                    if (i_rv.Count == 0)
                    {
                        Console.WriteLine("Sin registros para las rutas de videos, favor de agregar");
                    }
                    else
                    {
                        dt_fr = DateTime.Parse(i_ft[0].horario.ToString());
                        dt_frc = DateTime.Now;

                        differenceInMinutes = (double)(dt_frc - dt_fr).Minutes;
                        if (dt_frc >= dt_fr && differenceInMinutes == 0.0)
                        {
                            Console.WriteLine("Coincide la fecha para la carga");

                            foreach (var f_rv in i_rv)
                            {
                                ruta_ini = f_rv.desc_ruta_ini;
                                usr_ini = f_rv.ruta_user_ini;
                                clv_ini = f_rv.ruta_pass_ini;
                                ruta_fin = f_rv.desc_ruta_fin;
                                id_rv = f_rv.id_ruta_videos;

                                var networkPath = ruta_ini;
                                var credentials = new NetworkCredential(usr_ini, clv_ini);
                                try
                                {
                                    using (new networkconnection(networkPath, credentials))
                                    {
                                        var dir_list = Directory.GetDirectories(networkPath);
                                        foreach (var dir in dir_list)
                                        {
                                            DirectoryInfo dir_p = new DirectoryInfo(dir.ToString());
                                            foreach (FileInfo fil_p in dir_p.GetFiles("*.jvl"))
                                            {
                                                file_jvl = fil_p.FullName.ToString();

                                                log_err(file_jvl, ruta_fin, id_rv);
                                            }
                                            var dir_ext = Directory.GetDirectories(dir_p.ToString());
                                            foreach (var dir_n in dir_ext)
                                            {
                                                var lis_jvl = Directory.GetFiles(dir_n, "*jvl");
                                                if (lis_jvl.Length > 0)
                                                {
                                                    string ff = lis_jvl[0].ToString();
                                                    log_err_ext(ff, ruta_fin, id_rv);
                                                }
                                                else
                                                {

                                                }
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Sin acceso a rutas de red, favor de revisar o contactar a soporte");
                                }
                            }
                        }
                        else
                        {
                            using (var md_ft_f = new bd_tsEntities())
                            {
                                var i_ft_f = (from c in md_ft_f.inf_master_jvl
                                           
                                            select c).ToList();

                                if (i_ft_f.Count == 0)
                                {

                                }
                                else
                                {
                                    foreach (var f_rv in i_rv)
                                    {
                                        ruta_ini = f_rv.desc_ruta_ini;
                                        usr_ini = f_rv.ruta_user_ini;
                                        clv_ini = f_rv.ruta_pass_ini;
                                        ruta_fin = f_rv.desc_ruta_fin;
                                        id_rv = f_rv.id_ruta_videos;

                                        var networkPath = ruta_ini;
                                        var credentials = new NetworkCredential(usr_ini, clv_ini);
                                        try
                                        {
                                            using (new networkconnection(networkPath, credentials))
                                            {
                                                var dir_list = Directory.GetDirectories(networkPath);
                                                foreach (var dir in dir_list)
                                                {
                                                    DirectoryInfo dir_p = new DirectoryInfo(dir.ToString());
                                                    foreach (FileInfo fil_p in dir_p.GetFiles("*.jvl"))
                                                    {
                                                        file_jvl = fil_p.FullName.ToString();

                                                        p_nuevos(file_jvl, ruta_fin, id_rv);
                                                    }
                                                }
                                            }
                                        }
                                        catch
                                        {
                                            Console.WriteLine("Sin acceso a rutas de red, favor de revisar o contactar a soporte");
                                        }
                                    }
                                }
                            }
                                
                        }
                    }
                }
            }
        }

        private static void log_err_ext(string file_jvl, string ruta_fin, int id_rv)
        {
            string dir_p, dir_m, jvl_nom, err_estructura = null;
            Guid str_MasterId, str_SessionId, str_EventId;
            int err_c = 0;
            string str_Title = null, str_Department, str_IsSealed, str_Exhibits, str_StartDate, str_EndDate, str_Location, str_Type, str_MasterGroups, str_Name, str_TimeStamp, str_Type_Event, str_TypeId, str_TypeCategoryId, str_IsSystemEvent, str_IsPrivate, str_Identifier, str_EventNotes, str_Path, str_DeviceId, str_Height, str_Width, str_Incomplete;

            FileInfo fi_jvl = new FileInfo(file_jvl);
            jvl_nom = Path.GetFileNameWithoutExtension(file_jvl); ;
            dir_p = fi_jvl.Directory.Name;

            DataSet dataSet = new DataSet();
            int num = (int)dataSet.ReadXml(file_jvl);

            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = new DataTable();
            DataTable dataTable3 = new DataTable();
            DataTable dataTable4 = new DataTable();

            DataTable table = dataSet.Tables["Master"];
            Guid str_idcontrol = Guid.NewGuid();

            foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
            {
                str_MasterId = Guid.Parse(row[0].ToString());
                str_Id = row[1].ToString();
                str_Title = row[2].ToString();
                str_Department = row[3].ToString();
                str_IsSealed = row[4].ToString();
                str_Exhibits = row[6].ToString();
            }

            if (str_Id == jvl_nom)
            {
            }
            else
            {
                err_c = 1;
            }

            if (str_Id == dir_p)
            {
            }
            else
            {
                err_c = err_c + 2 + 1;
            }

            var dir_list = Directory.GetDirectories(Path.GetDirectoryName(file_jvl));

            foreach (var dir in dir_list)
            {
                var list = Directory.GetFiles(dir, "*.wmv");
                if (list.Length > 0)
                {
                    DirectoryInfo di = new DirectoryInfo(dir);
                    dir_m = di.Name;

                    if (str_Id == dir_m)
                    {
                    }
                    else
                    {
                        err_c = err_c + 1;
                    }
                }
            }
            switch (err_c)
            {
                case 1:
                    err_estructura = "err_nom_jvl";
                    break;

                case 2:
                    err_estructura = "err_dir_princial";
                    break;

                case 3:
                    err_estructura = "err_dir_media";
                    break;

                case 4:
                    err_estructura = "err_nom_jvl / err_dir_princial";
                    break;

                case 5:
                    err_estructura = "err_nom_jvl / err_dir_princial / err_dir_media";
                    break;

                default:
                    err_estructura = "Sin Errores";
                    break;
            }

            if (err_estructura == "Sin Errores")
            {
                string str_parent =  fi_jvl.Directory.Parent.ToString();
                using (var edm_master = new bd_tsEntities())
                {
                    var i_master = (from c in edm_master.inf_master_jvl
                                    where c.sesion == str_parent
                                    select c).ToList();
                    str_idcontrol = i_master[0].id_control_exp;
                    if (i_master.Count == 0)
                    {
                        inf_master_jvl infMaster = new inf_master_jvl()
                        {
                            id_control_exp = str_idcontrol,
                            sesion = str_Id,
                            titulo = str_Title,
                            err_carga = err_estructura,
                            id_estatus_exp = 2,
                            id_estatus_qa = 1,
                            id_ruta_videos = id_rv,
                            fecha_registro = DateTime.Now
                        };
                        edm_master.inf_master_jvl.Add(infMaster);
                        edm_master.SaveChanges();

                        string dir_c = Path.GetDirectoryName(file_jvl);
                        string dir_root = ruta_fin + "\\" + jvl_nom;

                        if (Directory.Exists(dir_root))
                        {
                            Directory.CreateDirectory(dir_root);
                            file_library.CopyDirectory(dir_c, dir_root, false);
                        }
                        else
                        {
                            FFMpegConverter ffMpegConverter = new FFMpegConverter();
                            FFProbe ffProbe = new FFProbe();

                            Directory.CreateDirectory(dir_root);
                            file_library.CopyDirectory(dir_c, dir_root, false);

                            var dir_wmv = Directory.GetDirectories(dir_root);

                            foreach (var dir in dir_wmv)
                            {
                                var lis_wmv = Directory.GetFiles(dir, "*.wmv");
                                if (lis_wmv.Length > 0)
                                {
                                    foreach (var l_wmv in lis_wmv)
                                    {
                                        FileInfo fi_wmv = new FileInfo(l_wmv);

                                        var videoInfo = ffProbe.GetMediaInfo(fi_wmv.ToString());

                                        string dur_wmv = videoInfo.Duration.Hours + ":" + videoInfo.Duration.Minutes + ":" + videoInfo.Duration.Seconds;

                                        var i_media = (from c in edm_master.inf_exp_mat
                                                       where c.nom_archivo == fi_wmv.Name
                                                       select c).ToList();

                                        if (i_media.Count == 0)
                                        {
                                            var g_media = new inf_exp_mat

                                            {
                                                ruta_archivo = fi_wmv.ToString().Replace(".wmv", ".mp4"),

                                                duracion = dur_wmv,
                                                nom_archivo = fi_wmv.Name.Replace(".wmv", ""),
                                                id_est_mat = 2,
                                                id_control_exp = str_idcontrol,
                                                fecha_registro = DateTime.Now,
                                            };

                                            edm_master.inf_exp_mat.Add(g_media);
                                            edm_master.SaveChanges();

                                            try
                                            {
                                                ffMpegConverter.ConvertMedia(fi_wmv.ToString(), fi_wmv.ToString().Replace(".wmv", ".mp4"), Format.mp4);
                                                File.Delete(fi_wmv.ToString());

                                                using (var act_media = new bd_tsEntities())
                                                {
                                                    var a_exp = (from c in act_media.inf_master_jvl
                                                                 where c.id_control_exp == str_idcontrol
                                                                 select c).FirstOrDefault();

                                                    a_exp.id_estatus_exp = 1;

                                                    act_media.SaveChanges();

                                                    var a_media = (from c in act_media.inf_exp_mat
                                                                   where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                                   select c).FirstOrDefault();

                                                    a_media.id_est_mat = 1;

                                                    act_media.SaveChanges();
                                                }
                                            }
                                            catch
                                            {
                                                using (var act_media = new bd_tsEntities())
                                                {
                                                    var a_media = (from c in act_media.inf_exp_mat
                                                                   where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                                   select c).FirstOrDefault();

                                                    a_media.id_est_mat = 3;

                                                    act_media.SaveChanges();
                                                }
                                            }
                                        }
                                    }
                                }

                                var dir_ext = Directory.GetDirectories(dir);
                                foreach (var dir_f in dir_ext)
                                {
                                    var lis_pdf = Directory.GetFiles(dir_f, "*.pdf");
                                    if (lis_pdf.Length > 0)
                                    {
                                        FileInfo fi_pdf = new FileInfo(lis_pdf[0].ToString());

                                        using (var act_media = new bd_tsEntities())
                                        {
                                            var a_media = (from c in act_media.inf_exp_mat
                                                           where c.id_control_exp == str_idcontrol
                                                           select c).ToList();

                                            foreach (var i_jsf in a_media)
                                            {
                                                i_jsf.ruta_ext = fi_pdf.FullName;
                                                act_media.SaveChanges();
                                            }
                                        }
                                    }
                                }


                            }

                        }
                    }
                    else
                    {
                        string dir_c = Path.GetDirectoryName(file_jvl);
                        string dir_root = ruta_fin +"\\"+ str_parent + "\\" + jvl_nom;

                        if (Directory.Exists(dir_root))
                        {
                            Directory.CreateDirectory(dir_root);
                            file_library.CopyDirectory(dir_c, dir_root, false);
                            FFMpegConverter ffMpegConverter = new FFMpegConverter();
                            FFProbe ffProbe = new FFProbe();

                            var dir_wmv = Directory.GetDirectories(dir_root);

                            foreach (var dir in dir_wmv)
                            {
                                var lis_wmv = Directory.GetFiles(dir, "*.wmv");
                                if (lis_wmv.Length > 0)
                                {
                                    foreach (var l_wmv in lis_wmv)
                                    {
                                        FileInfo fi_wmv = new FileInfo(l_wmv);

                                        var videoInfo = ffProbe.GetMediaInfo(fi_wmv.ToString());

                                        string dur_wmv = videoInfo.Duration.Hours + ":" + videoInfo.Duration.Minutes + ":" + videoInfo.Duration.Seconds;

                                        var i_media = (from c in edm_master.inf_exp_mat
                                                       where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                       select c).ToList();

                                        if (i_media.Count == 0)
                                        {
                                            var g_media = new inf_exp_mat

                                            {
                                                ruta_archivo = fi_wmv.ToString().Replace(".wmv", ".mp4"),

                                                duracion = dur_wmv,
                                                nom_archivo = fi_wmv.Name.Replace(".wmv", ""),
                                                id_est_mat = 2,
                                                id_control_exp = str_idcontrol,
                                                fecha_registro = DateTime.Now,
                                            };

                                            edm_master.inf_exp_mat.Add(g_media);
                                            edm_master.SaveChanges();

                                            try
                                            {
                                                ffMpegConverter.ConvertMedia(fi_wmv.ToString(), fi_wmv.ToString().Replace(".wmv", ".mp4"), Format.mp4);
                                                File.Delete(fi_wmv.ToString());

                                                using (var act_media = new bd_tsEntities())
                                                {
                                                    var a_media = (from c in act_media.inf_exp_mat
                                                                   where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                                   select c).FirstOrDefault();

                                                    a_media.id_est_mat = 1;

                                                    act_media.SaveChanges();
                                                }
                                            }
                                            catch
                                            {
                                                using (var act_media = new bd_tsEntities())
                                                {
                                                    var a_media = (from c in act_media.inf_exp_mat
                                                                   where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                                   select c).FirstOrDefault();

                                                    a_media.id_est_mat = 3;

                                                    act_media.SaveChanges();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var g_media = new inf_exp_mat

                                            {
                                                ruta_archivo = fi_wmv.ToString().Replace(".wmv", ".mp4"),

                                                duracion = dur_wmv,
                                                nom_archivo = fi_wmv.Name.Replace(".wmv", ""),
                                                id_est_mat = 2,
                                                id_control_exp = str_idcontrol,
                                                fecha_registro = DateTime.Now,
                                            };

                                            edm_master.inf_exp_mat.Add(g_media);
                                            edm_master.SaveChanges();

                                            try
                                            {
                                                ffMpegConverter.ConvertMedia(fi_wmv.ToString(), fi_wmv.ToString().Replace(".wmv", ".mp4"), Format.mp4);
                                                File.Delete(fi_wmv.ToString());

                                                using (var act_media = new bd_tsEntities())
                                                {
                                                    var a_media = (from c in act_media.inf_exp_mat
                                                                   where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                                   select c).FirstOrDefault();

                                                    a_media.id_est_mat = 1;

                                                    act_media.SaveChanges();
                                                }
                                            }
                                            catch
                                            {
                                                using (var act_media = new bd_tsEntities())
                                                {
                                                    var a_media = (from c in act_media.inf_exp_mat
                                                                   where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                                   select c).FirstOrDefault();

                                                    a_media.id_est_mat = 3;

                                                    act_media.SaveChanges();
                                                }
                                            }
                                        }
                                        var dir_ext = Directory.GetDirectories(dir);
                                        foreach (var dir_f in dir_ext)
                                        {
                                            var lis_pdf = Directory.GetFiles(dir_f, "*.pdf");
                                            if (lis_pdf.Length > 0)
                                            {
                                                FileInfo fi_pdf = new FileInfo(lis_pdf[0].ToString());

                                                using (var act_media = new bd_tsEntities())
                                                {
                                                    var a_media = (from c in act_media.inf_exp_mat
                                                                   where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                                   select c).ToList();

                                                    foreach (var i_jsf in a_media)
                                                    {
                                                        i_jsf.ruta_ext = fi_pdf.FullName;
                                                        act_media.SaveChanges();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                               
                            }
                        }
                        else
                        {
                            Directory.CreateDirectory(dir_root);
                            file_library.CopyDirectory(dir_c, dir_root, false);
                            FFMpegConverter ffMpegConverter = new FFMpegConverter();
                            FFProbe ffProbe = new FFProbe();

                            var dir_wmv = Directory.GetDirectories(dir_c);

                            foreach (var dir in dir_wmv)
                            {
                                var lis_wmv = Directory.GetFiles(dir, "*.wmv");
                                if (lis_wmv.Length > 0)
                                {
                                    foreach (var l_wmv in lis_wmv)
                                    {
                                        FileInfo fi_wmv = new FileInfo(l_wmv);

                                        var videoInfo = ffProbe.GetMediaInfo(fi_wmv.ToString());

                                        string dur_wmv = videoInfo.Duration.Hours + ":" + videoInfo.Duration.Minutes + ":" + videoInfo.Duration.Seconds;

                                        var i_media = (from c in edm_master.inf_exp_mat
                                                       where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                       select c).ToList();

                                        if (i_media.Count == 0)
                                        {
                                            var g_media = new inf_exp_mat

                                            {
                                                ruta_archivo = fi_wmv.ToString().Replace(".wmv", ".mp4"),

                                                duracion = dur_wmv,
                                                nom_archivo = fi_wmv.Name.Replace(".wmv", ""),
                                                id_est_mat = 2,
                                                id_control_exp = str_idcontrol,
                                                fecha_registro = DateTime.Now,
                                            };

                                            edm_master.inf_exp_mat.Add(g_media);
                                            edm_master.SaveChanges();

                                            try
                                            {
                                                ffMpegConverter.ConvertMedia(fi_wmv.ToString(), fi_wmv.ToString().Replace(".wmv", ".mp4"), Format.mp4);
                                                File.Delete(fi_wmv.ToString());

                                                using (var act_media = new bd_tsEntities())
                                                {
                                                    var a_media = (from c in act_media.inf_exp_mat
                                                                   where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                                   select c).FirstOrDefault();

                                                    a_media.id_est_mat = 1;

                                                    act_media.SaveChanges();
                                                }
                                            }
                                            catch
                                            {
                                                using (var act_media = new bd_tsEntities())
                                                {
                                                    var a_media = (from c in act_media.inf_exp_mat
                                                                   where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                                   select c).FirstOrDefault();

                                                    a_media.id_est_mat = 3;

                                                    act_media.SaveChanges();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var g_media = new inf_exp_mat

                                            {
                                                ruta_archivo = fi_wmv.ToString().Replace(".wmv", ".mp4"),

                                                duracion = dur_wmv,
                                                nom_archivo = fi_wmv.Name.Replace(".wmv", ""),
                                                id_est_mat = 2,
                                                id_control_exp = str_idcontrol,
                                                fecha_registro = DateTime.Now,
                                            };

                                            edm_master.inf_exp_mat.Add(g_media);
                                            edm_master.SaveChanges();

                                            try
                                            {
                                                ffMpegConverter.ConvertMedia(fi_wmv.ToString(), fi_wmv.ToString().Replace(".wmv", ".mp4"), Format.mp4);
                                                File.Delete(fi_wmv.ToString());

                                                using (var act_media = new bd_tsEntities())
                                                {
                                                    var a_media = (from c in act_media.inf_exp_mat
                                                                   where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                                   select c).FirstOrDefault();

                                                    a_media.id_est_mat = 1;

                                                    act_media.SaveChanges();
                                                }
                                            }
                                            catch
                                            {
                                                using (var act_media = new bd_tsEntities())
                                                {
                                                    var a_media = (from c in act_media.inf_exp_mat
                                                                   where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                                   select c).FirstOrDefault();

                                                    a_media.id_est_mat = 3;

                                                    act_media.SaveChanges();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                using (var edm_master = new bd_tsEntities())
                {
                    var i_master = (from c in edm_master.inf_master_jvl
                                    where c.sesion == jvl_nom
                                    select c).ToList();

                    if (i_master.Count == 0)
                    {
                        inf_master_jvl infMaster = new inf_master_jvl()
                        {
                            id_control_exp = str_idcontrol,
                            sesion = str_Id,
                            titulo = str_Title,
                            err_carga = err_estructura,
                            id_estatus_exp = 3,
                            id_estatus_qa = 1,
                            id_ruta_videos = id_rv,
                            fecha_registro = DateTime.Now
                        };
                        edm_master.inf_master_jvl.Add(infMaster);
                        edm_master.SaveChanges();
                    }
                    else
                    {
                    }
                }
            }
        }

        private static void p_nuevos(string file_jvl, string ruta_fin, int id_rv)
        {
            string dir_p, dir_m, jvl_nom, err_estructura = null;
            Guid str_MasterId, str_SessionId, str_EventId;
            int err_c = 0;
            string str_Title = null, str_Department, str_IsSealed, str_Exhibits, str_StartDate, str_EndDate, str_Location, str_Type, str_MasterGroups, str_Name, str_TimeStamp, str_Type_Event, str_TypeId, str_TypeCategoryId, str_IsSystemEvent, str_IsPrivate, str_Identifier, str_EventNotes, str_Path, str_DeviceId, str_Height, str_Width, str_Incomplete;
            Guid str_idcontrol, str_idcontrolf;
            FileInfo fi_jvl = new FileInfo(file_jvl);
            jvl_nom = Path.GetFileNameWithoutExtension(file_jvl); ;
            dir_p = fi_jvl.Directory.Name.ToString();

            DataSet dataSet = new DataSet();
            int num = (int)dataSet.ReadXml(file_jvl);

            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = new DataTable();
            DataTable dataTable3 = new DataTable();
            DataTable dataTable4 = new DataTable();

            DataTable table = dataSet.Tables["Master"];


            foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
            {
                str_MasterId = Guid.Parse(row[0].ToString());
                str_Id = row[1].ToString();
                str_Title = row[2].ToString();
                str_Department = row[3].ToString();
                str_IsSealed = row[4].ToString();
                str_Exhibits = row[6].ToString();
            }

            if (str_Id == jvl_nom)
            {
            }
            else
            {
                err_c = 1;
            }

            if (str_Id == dir_p)
            {
            }
            else
            {
                err_c = err_c + 2 + 1;
            }

            var dir_list = Directory.GetDirectories(Path.GetDirectoryName(file_jvl));

            foreach (var dir in dir_list)
            {
                var list = Directory.GetFiles(dir, "*.wmv");
                if (list.Length > 0)
                {
                    DirectoryInfo di = new DirectoryInfo(dir);
                    dir_m = di.Name;

                    if (str_Id == dir_m)
                    {
                    }
                    else
                    {
                        err_c = err_c + 1;
                    }
                }
            }
            switch (err_c)
            {
                case 1:
                    err_estructura = "err_nom_jvl";
                    break;

                case 2:
                    err_estructura = "err_dir_princial";
                    break;

                case 3:
                    err_estructura = "err_dir_media";
                    break;

                case 4:
                    err_estructura = "err_nom_jvl / err_dir_princial";
                    break;

                case 5:
                    err_estructura = "err_nom_jvl / err_dir_princial / err_dir_media";
                    break;

                default:
                    err_estructura = "Sin Errores";
                    break;
            }

            if (err_estructura == "Sin Errores")
            {
               
                using (var edm_master = new bd_tsEntities())
                {
                    var i_master = (from c in edm_master.inf_master_jvl
                                    where c.sesion == str_Id
                                    select c).ToList();

                    if (i_master.Count == 0)
                    {
                        str_idcontrol = Guid.NewGuid(); 

                        inf_master_jvl infMaster = new inf_master_jvl()
                        {
                            id_control_exp = str_idcontrol,
                            sesion = str_Id,
                            titulo = str_Title,
                            err_carga = err_estructura,
                            id_estatus_exp = 4,
                            id_estatus_qa = 1,
                            id_ruta_videos = id_rv,
                            fecha_registro = DateTime.Now
                        };
                        edm_master.inf_master_jvl.Add(infMaster);
                        edm_master.SaveChanges();

                        string dir_c = Path.GetDirectoryName(file_jvl);
                        string dir_root = ruta_fin + "\\" + jvl_nom;

                        if (Directory.Exists(dir_root))
                        {
                            Directory.CreateDirectory(dir_root);
                            file_library.CopyDirectory(dir_c, dir_root, false);
                        }
                        else
                        {
                            FFMpegConverter ffMpegConverter = new FFMpegConverter();
                            FFProbe ffProbe = new FFProbe();

                            Directory.CreateDirectory(dir_root);
                            file_library.CopyDirectory(dir_c, dir_root, false);

                            var dir_wmv = Directory.GetDirectories(dir_root);

                            foreach (var dir in dir_wmv)
                            {
                                var lis_wmv = Directory.GetFiles(dir, "*.wmv");
                                if (lis_wmv.Length > 0)
                                {
                                    foreach (var l_wmv in lis_wmv)
                                    {
                                        FileInfo fi_wmv = new FileInfo(l_wmv);

                                        var videoInfo = ffProbe.GetMediaInfo(fi_wmv.ToString());

                                        string dur_wmv = videoInfo.Duration.Hours + ":" + videoInfo.Duration.Minutes + ":" + videoInfo.Duration.Seconds;

                                        var i_media = (from c in edm_master.inf_exp_mat
                                                       where c.nom_archivo == fi_wmv.Name
                                                       select c).ToList();

                                        if (i_media.Count == 0)
                                        {
                                            var g_media = new inf_exp_mat

                                            {
                                                ruta_archivo = fi_wmv.ToString().Replace(".wmv", ".mp4"),

                                                duracion = dur_wmv,
                                                nom_archivo = fi_wmv.Name.Replace(".wmv", ""),
                                                id_est_mat = 4,
                                                id_control_exp = str_idcontrol,
                                                fecha_registro = DateTime.Now,
                                            };

                                            edm_master.inf_exp_mat.Add(g_media);
                                            edm_master.SaveChanges();
                                        }
                                    }
                                }

                                var dir_ext = Directory.GetDirectories(dir);
                                foreach (var dir_f in dir_ext)
                                {
                                    var lis_pdf = Directory.GetFiles(dir_f, "*.pdf");
                                    if (lis_pdf.Length > 0)
                                    {
                                        FileInfo fi_pdf = new FileInfo(lis_pdf[0].ToString());

                                        using (var act_media = new bd_tsEntities())
                                        {
                                            var a_media = (from c in act_media.inf_exp_mat
                                                           where c.id_control_exp == str_idcontrol
                                                           select c).ToList();

                                            foreach (var i_jsf in a_media)
                                            {
                                                i_jsf.ruta_ext = fi_pdf.FullName;
                                                act_media.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                        str_idcontrol = i_master[0].id_control_exp;
                        string dir_c = Path.GetDirectoryName(file_jvl);
                        string dir_root = ruta_fin + "\\" + jvl_nom;
                        if (Directory.Exists(dir_root))
                        {
                        
                        }
                        else
                        {
                            var a_media = (from c in edm_master.inf_master_jvl
                                           where c.sesion == str_Id
                                           select c).FirstOrDefault();
                            a_media.err_carga = err_estructura;
                            a_media.id_estatus_exp = 2;

                            edm_master.SaveChanges();

                            Directory.CreateDirectory(dir_root);
                            file_library.CopyDirectory(dir_c, dir_root, false);
                            FFMpegConverter ffMpegConverter = new FFMpegConverter();
                            FFProbe ffProbe = new FFProbe();

                            var dir_wmv = Directory.GetDirectories(dir_root);

                            foreach (var dir in dir_wmv)
                            {
                                var lis_wmv = Directory.GetFiles(dir, "*.wmv");
                                if (lis_wmv.Length > 0)
                                {
                                    foreach (var l_wmv in lis_wmv)
                                    {
                                        FileInfo fi_wmv = new FileInfo(l_wmv);

                                        var videoInfo = ffProbe.GetMediaInfo(fi_wmv.ToString());

                                        string dur_wmv = videoInfo.Duration.Hours + ":" + videoInfo.Duration.Minutes + ":" + videoInfo.Duration.Seconds;
                                        string f_name = fi_wmv.Name.Replace(".wmv", "");
                                        var i_media = (from c in edm_master.inf_exp_mat
                                                       where c.nom_archivo == f_name
                                                       select c).ToList();

                                        if (i_media.Count == 0)
                                        {
                                            var g_media = new inf_exp_mat

                                            {
                                                ruta_archivo = fi_wmv.ToString().Replace(".wmv", ".mp4"),

                                                duracion = dur_wmv,
                                                nom_archivo = fi_wmv.Name.Replace(".wmv", ""),
                                                id_est_mat = 2,
                                                id_control_exp = str_idcontrol,
                                                fecha_registro = DateTime.Now,
                                            };

                                            edm_master.inf_exp_mat.Add(g_media);
                                            edm_master.SaveChanges();

                                        }
                                        else
                                        {
                                        }
                                    }
                                }
                                var dir_ext = Directory.GetDirectories(dir);
                                foreach (var dir_f in dir_ext)
                                {
                                    var lis_pdf = Directory.GetFiles(dir_f, "*.pdf");
                                    if (lis_pdf.Length > 0)
                                    {
                                        FileInfo fi_pdf = new FileInfo(lis_pdf[0].ToString());

                                        using (var act_media = new bd_tsEntities())
                                        {
                                            var aa_m = (from c in act_media.inf_exp_mat
                                                           where c.id_control_exp == str_idcontrol
                                                           select c).ToList();

                                            foreach (var i_jsf in aa_m)
                                            {
                                                i_jsf.ruta_ext = fi_pdf.FullName;
                                                act_media.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                using (var edm_master = new bd_tsEntities())
                {
                    var i_master = (from c in edm_master.inf_master_jvl
                                    where c.sesion == jvl_nom
                                    select c).ToList();

                    if (i_master.Count == 0)
                    {
                        str_idcontrol = Guid.NewGuid();
                        inf_master_jvl infMaster = new inf_master_jvl()
                        {
                            id_control_exp = str_idcontrol,
                            sesion = str_Id,
                            titulo = str_Title,
                            err_carga = err_estructura,
                            id_estatus_exp = 3,
                            id_estatus_qa = 1,
                            id_ruta_videos = id_rv,
                            fecha_registro = DateTime.Now
                        };
                        edm_master.inf_master_jvl.Add(infMaster);
                        edm_master.SaveChanges();
                    }
                    else
                    {
                    }
                }
            }
        }

        private static void caducidad_mat()
        {
            DateTime dt_fr, dt_frc;
            int int_dcv;

            using (var act_cv = new bd_tsEntities())
            {
                var i_cv = (from c in act_cv.inf_caducidad_videos
                            select c).ToList();

                if (i_cv.Count == 0)
                {
                    //al no haber registros en la tabla inf_caducidad_videos no se genera ningun procedimiento
                    Console.WriteLine("Sin registros en los días de respaldo, favor de agregar");
                }
                else
                {
                    //si existe un registro en la tabla inf_caducidad_videos se busca en la tabla inf_exp_mat los registros que apartir de la fecha en que se registro, cumplen con los dias que marca el campo dias_caducidad, si los cumple, se deben de borrar fisicamente estos archivos ademas de cambiar el estatus de cada uno, cuando sea correcto este procedimiento.
                    int_dcv = int.Parse(i_cv[0].dias_caducidad.ToString());
                    var i_em = (from c in act_cv.inf_exp_mat
                                select c).ToList();

                    foreach (var f_em in i_em)
                    {
                        dt_fr = DateTime.Parse(f_em.fecha_registro.ToString());
                        dt_frc = dt_fr.AddDays(int_dcv);

                        if (dt_fr == dt_frc)
                        {
                            Console.WriteLine("Expediente coincide con los días límite para su respaldo");
                        }
                        else
                        {
                            Console.WriteLine("Ningún expediente coincide con los días de respaldo");
                        }
                    }
                }
            }
        }

        private static void log_err(string file_jvl, string ruta_fin, int id_rv)
        {
            string dir_p, dir_m, jvl_nom, err_estructura = null;
            Guid str_MasterId, str_SessionId, str_EventId;
            int err_c = 0;
            string str_Title = null, str_Department, str_IsSealed, str_Exhibits, str_StartDate, str_EndDate, str_Location, str_Type, str_MasterGroups, str_Name, str_TimeStamp, str_Type_Event, str_TypeId, str_TypeCategoryId, str_IsSystemEvent, str_IsPrivate, str_Identifier, str_EventNotes, str_Path, str_DeviceId, str_Height, str_Width, str_Incomplete;

            FileInfo fi_jvl = new FileInfo(file_jvl);
            jvl_nom = Path.GetFileNameWithoutExtension(file_jvl); ;
            dir_p = fi_jvl.Directory.Name;

            DataSet dataSet = new DataSet();
            int num = (int)dataSet.ReadXml(file_jvl);

            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = new DataTable();
            DataTable dataTable3 = new DataTable();
            DataTable dataTable4 = new DataTable();

            DataTable table = dataSet.Tables["Master"];
            Guid str_idcontrol = Guid.NewGuid();

            foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
            {
                str_MasterId = Guid.Parse(row[0].ToString());
                str_Id = row[1].ToString();
                str_Title = row[2].ToString();
                str_Department = row[3].ToString();
                str_IsSealed = row[4].ToString();
                str_Exhibits = row[6].ToString();
            }

            if (str_Id == jvl_nom)
            {
            }
            else
            {
                err_c = 1;
            }

            if (str_Id == dir_p)
            {
            }
            else
            {
                err_c = err_c + 2 + 1;
            }

            var dir_list = Directory.GetDirectories(Path.GetDirectoryName(file_jvl));

            foreach (var dir in dir_list)
            {
                var list = Directory.GetFiles(dir, "*.wmv");
                if (list.Length > 0)
                {
                    DirectoryInfo di = new DirectoryInfo(dir);
                    dir_m = di.Name;

                    if (str_Id == dir_m)
                    {
                    }
                    else
                    {
                        err_c = err_c + 1;
                    }
                }
            }
            switch (err_c)
            {
                case 1:
                    err_estructura = "err_nom_jvl";
                    break;

                case 2:
                    err_estructura = "err_dir_princial";
                    break;

                case 3:
                    err_estructura = "err_dir_media";
                    break;

                case 4:
                    err_estructura = "err_nom_jvl / err_dir_princial";
                    break;

                case 5:
                    err_estructura = "err_nom_jvl / err_dir_princial / err_dir_media";
                    break;

                default:
                    err_estructura = "Sin Errores";
                    break;
            }

            if (err_estructura == "Sin Errores")
            {
                using (var edm_master = new bd_tsEntities())
                {
                    var i_master = (from c in edm_master.inf_master_jvl
                                    where c.sesion == jvl_nom
                                    select c).ToList();

                    if (i_master.Count == 0)
                    {
                        inf_master_jvl infMaster = new inf_master_jvl()
                        {
                            id_control_exp = str_idcontrol,
                            sesion = str_Id,
                            titulo = str_Title,
                            err_carga = err_estructura,
                            id_estatus_exp = 2,
                            id_estatus_qa = 1,
                            id_ruta_videos = id_rv,
                            fecha_registro = DateTime.Now
                        };
                        edm_master.inf_master_jvl.Add(infMaster);
                        edm_master.SaveChanges();

                        string dir_c = Path.GetDirectoryName(file_jvl);
                        string dir_root = ruta_fin + "\\" + jvl_nom;

                        if (Directory.Exists(dir_root))
                        {
                            Directory.CreateDirectory(dir_root);
                            file_library.CopyDirectory(dir_c, dir_root, false);
                        }
                        else
                        {
                            FFMpegConverter ffMpegConverter = new FFMpegConverter();
                            FFProbe ffProbe = new FFProbe();

                            Directory.CreateDirectory(dir_root);
                            file_library.CopyDirectory(dir_c, dir_root, false);

                            var dir_wmv = Directory.GetDirectories(dir_root);

                            foreach (var dir in dir_wmv)
                            {
                                var lis_wmv = Directory.GetFiles(dir, "*.wmv");
                                if (lis_wmv.Length > 0)
                                {
                                    foreach (var l_wmv in lis_wmv)
                                    {
                                        FileInfo fi_wmv = new FileInfo(l_wmv);

                                        var videoInfo = ffProbe.GetMediaInfo(fi_wmv.ToString());

                                        string dur_wmv = videoInfo.Duration.Hours + ":" + videoInfo.Duration.Minutes + ":" + videoInfo.Duration.Seconds;

                                        var i_media = (from c in edm_master.inf_exp_mat
                                                       where c.nom_archivo == fi_wmv.Name
                                                       select c).ToList();

                                        if (i_media.Count == 0)
                                        {
                                            var g_media = new inf_exp_mat

                                            {
                                                ruta_archivo = fi_wmv.ToString().Replace(".wmv", ".mp4"),

                                                duracion = dur_wmv,
                                                nom_archivo = fi_wmv.Name.Replace(".wmv", ""),
                                                id_est_mat = 2,
                                                id_control_exp = str_idcontrol,
                                                fecha_registro = DateTime.Now,
                                            };

                                            edm_master.inf_exp_mat.Add(g_media);
                                            edm_master.SaveChanges();

                                            try
                                            {
                                                ffMpegConverter.ConvertMedia(fi_wmv.ToString(), fi_wmv.ToString().Replace(".wmv", ".mp4"), Format.mp4);
                                                File.Delete(fi_wmv.ToString());

                                                using (var act_media = new bd_tsEntities())
                                                {
                                                    var a_exp = (from c in act_media.inf_master_jvl
                                                                 where c.id_control_exp == str_idcontrol
                                                                 select c).FirstOrDefault();

                                                    a_exp.id_estatus_exp = 1;

                                                    act_media.SaveChanges();

                                                    var a_media = (from c in act_media.inf_exp_mat
                                                                   where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                                   select c).FirstOrDefault();

                                                    a_media.id_est_mat = 1;

                                                    act_media.SaveChanges();
                                                }
                                            }
                                            catch
                                            {
                                                using (var act_media = new bd_tsEntities())
                                                {
                                                    var a_media = (from c in act_media.inf_exp_mat
                                                                   where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                                   select c).FirstOrDefault();

                                                    a_media.id_est_mat = 3;

                                                    act_media.SaveChanges();
                                                }
                                            }
                                        }
                                    }
                                }

                                var dir_ext = Directory.GetDirectories(dir);
                                foreach (var dir_f in dir_ext)
                                {
                                    var lis_pdf = Directory.GetFiles(dir_f, "*.pdf");
                                    if (lis_pdf.Length > 0)
                                    {
                                        FileInfo fi_pdf = new FileInfo(lis_pdf[0].ToString());

                                        using (var act_media = new bd_tsEntities())
                                        {
                                            var a_media = (from c in act_media.inf_exp_mat
                                                           where c.id_control_exp == str_idcontrol
                                                           select c).ToList();

                                            foreach (var i_jsf in a_media)
                                            {
                                                i_jsf.ruta_ext = fi_pdf.FullName;
                                                act_media.SaveChanges();
                                            }
                                        }
                                    }
                                }

                    
                            }
                           
                        }
                    }
                    else
                    {
                        string dir_c = Path.GetDirectoryName(file_jvl);
                        string dir_root = ruta_fin + "\\" + jvl_nom;

                        FFMpegConverter ffMpegConverter = new FFMpegConverter();
                        FFProbe ffProbe = new FFProbe();

                        var dir_wmv = Directory.GetDirectories(dir_c);

                        foreach (var dir in dir_wmv)
                        {
                            var lis_wmv = Directory.GetFiles(dir, "*.wmv");
                            if (lis_wmv.Length > 0)
                            {
                                foreach (var l_wmv in lis_wmv)
                                {
                                    FileInfo fi_wmv = new FileInfo(l_wmv);

                                    var videoInfo = ffProbe.GetMediaInfo(fi_wmv.ToString());

                                    string dur_wmv = videoInfo.Duration.Hours + ":" + videoInfo.Duration.Minutes + ":" + videoInfo.Duration.Seconds;

                                    var i_media = (from c in edm_master.inf_exp_mat
                                                   where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                   select c).ToList();

                                    if (i_media.Count == 0)
                                    {
                                    }
                                    else
                                    {
                                        var g_media = new inf_exp_mat

                                        {
                                            ruta_archivo = fi_wmv.ToString().Replace(".wmv", ".mp4"),

                                            duracion = dur_wmv,
                                            nom_archivo = fi_wmv.Name.Replace(".wmv", ""),
                                            id_est_mat = 2,
                                            id_control_exp = str_idcontrol,
                                            fecha_registro = DateTime.Now,
                                        };

                                        edm_master.inf_exp_mat.Add(g_media);
                                        edm_master.SaveChanges();

                                        try
                                        {
                                            ffMpegConverter.ConvertMedia(fi_wmv.ToString(), fi_wmv.ToString().Replace(".wmv", ".mp4"), Format.mp4);
                                            File.Delete(fi_wmv.ToString());

                                            using (var act_media = new bd_tsEntities())
                                            {
                                                var a_media = (from c in act_media.inf_exp_mat
                                                               where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                               select c).FirstOrDefault();

                                                a_media.id_est_mat = 1;

                                                act_media.SaveChanges();
                                            }
                                        }
                                        catch
                                        {
                                            using (var act_media = new bd_tsEntities())
                                            {
                                                var a_media = (from c in act_media.inf_exp_mat
                                                               where c.nom_archivo == fi_wmv.Name.Replace(".wmv", "")
                                                               select c).FirstOrDefault();

                                                a_media.id_est_mat = 3;

                                                act_media.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                using (var edm_master = new bd_tsEntities())
                {
                    var i_master = (from c in edm_master.inf_master_jvl
                                    where c.sesion == jvl_nom
                                    select c).ToList();

                    if (i_master.Count == 0)
                    {
                        inf_master_jvl infMaster = new inf_master_jvl()
                        {
                            id_control_exp = str_idcontrol,
                            sesion = str_Id,
                            titulo = str_Title,
                            err_carga = err_estructura,
                            id_estatus_exp = 3,
                            id_estatus_qa = 1,
                            id_ruta_videos = id_rv,
                            fecha_registro = DateTime.Now
                        };
                        edm_master.inf_master_jvl.Add(infMaster);
                        edm_master.SaveChanges();
                    }
                    else
                    {
                    }
                }
            }
        }

  
    }
}