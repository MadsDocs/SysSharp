using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Win32;
using System.Management;

using System.IO;

using System.Diagnostics;
using System.Reflection;
using System.Timers;

using System.Net;
using System.Net.NetworkInformation;

using System.Text.RegularExpressions;

namespace SysSharp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        klassen.funktionen funk = new klassen.funktionen();
        string win_dir = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        
        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                

                klassen.funktionen funk = new klassen.funktionen();
                funk.syssharp_init();


                if (!Directory.Exists(appdata + @"\SysSharp\"))
                {
                    Directory.CreateDirectory(appdata + @"\SysSharp\");
                }

                //funk.lies_settings();

                Directory.CreateDirectory(appdata + @"\SysSharp\extend\");
                
                klassen.funktionen fun = new klassen.funktionen();
                fun.ramauslastung();
                

                klassen.funktionen fu = new klassen.funktionen();


                #region sperren_der_textboxen&steuerelementen
                txtbox_username.IsEnabled = false;
                txtbx_netbios.IsEnabled = false;
                txtbx_sp.IsEnabled = false;
                txtbx_winver.IsEnabled = false;
                txtbx_clrver.IsEnabled = false;
                txtbx_domainusr.IsEnabled = false;
                txtbx_windir.IsEnabled = false;
                txtbx_cpuproc.IsEnabled = false;
                
                txtbx_freespace.IsEnabled = false;
                txtbx_screenres.IsEnabled = false;


                txtbx_deviceID.IsEnabled = false;
                txtbx_name.IsEnabled = false;
                txtbx_ram.IsEnabled = false;
                txtbx_driverver.IsEnabled = false;
                txtbx_videoproc.IsEnabled = false;
                txtbx_videoupdate.IsEnabled = false;

                lst_user.IsEnabled = false;

                #endregion
                #region download_extend

                
                fu._logger("Starte Download des Zusatzprogramms", 1);
                           
              

               #endregion
                #region altewindowserkennung

                ManagementObjectSearcher os_searcher =
                   new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT * FROM Win32_OperatingSystem");

                foreach (ManagementObject _os in os_searcher.Get())
                {
                    Object win_ver = _os["Caption"];
                    Object build_nr = _os["Version"];
                    Object build_type = _os["BuildType"];
                    Object architecture = _os["OSArchitecture"];

                    txtbx_winver.Text = win_ver.ToString() + "(" + build_nr.ToString() + ")" + "\t" + architecture.ToString();
	


                }

                #endregion
                #region is_sp
                lbl_winsp.Content = "Service Pack: ";
                if (Environment.OSVersion.ServicePack == "")
                {
                    txtbx_sp.Text = "Kein Service Pack installiert";
                }
                else
                {
                    txtbx_sp.Text = Environment.OSVersion.ServicePack;
                }
                txtbx_sp.Text = Environment.OSVersion.ServicePack;
                #endregion
                #region benutzer_und_systemvars

                string pc_name = Environment.MachineName;
                string user = Environment.UserName;
                string domain_user = Environment.UserDomainName;
                string CLR_Version = Environment.Version.ToString();
                string is_64 = Environment.Is64BitOperatingSystem.ToString();
                string processor_count = Environment.ProcessorCount.ToString();

                lbl_pcname.Content = "Computername: ";
                lbl_winuser.Content = "Angemledeter User: ";

                txtbox_username.Text = user;
                txtbx_netbios.Text = pc_name;

                lbl_CLRVersion.Content = "CLR Version: ";
                txtbx_clrver.Text = Environment.Version.ToString();

                lbl_domainuser.Content = "Domain User: ";
                txtbx_domainusr.Text = domain_user;

                lbl_cpuproc.Content = "Anzahl der Prozessorkerne: ";
                txtbx_cpuproc.Text = processor_count;

                lbl_WinDir.Content = "Win32 Verzeichnis: ";
                txtbx_windir.Text = Environment.GetFolderPath(Environment.SpecialFolder.Windows);

                lbl_screenres.Content = "Desktopauflösung: ";

                //Ausgabe der Desktopauflösung:
                ManagementObjectSearcher searchedesk =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_DesktopMonitor");

                foreach (ManagementObject obj in searchedesk.Get())
                {
                    Object screenheight = obj["ScreenHeight"];
                    Object screenwidth = obj["ScreenWidth"];
                    txtbx_screenres.Text = screenwidth + "x" + screenheight;



                }

                
                //Ausgabe aller Benutzer auf dem System:
                ManagementObjectSearcher searchusr =
                  new ManagementObjectSearcher("root\\CIMV2",
                  "SELECT * FROM Win32_UserAccount");

                lst_user.Items.Add("Verfügbare Benuter auf System (" + pc_name + ")");

                foreach (ManagementObject usr in searchusr.Get())
                {
                    
                    lst_user.Items.Add(usr["Caption"]);

                }

                ManagementObjectSearcher win_searcher =
                  new ManagementObjectSearcher("root\\CIMV2",
                  "SELECT * FROM Win32_OperatingSystem");

                foreach (ManagementObject queryObj in win_searcher.Get())
                {
                    Object oslang = queryObj["OSLanguage"];

                    if (oslang.ToString() == "1031")
                    {
                        lbl_win_properties.Content = "Derzeitige Windows Sprache: " + oslang + "( Deutsch )";
                    }
                }

                //Ab hier speichern wir alles in eine HTML Datei...
            
                

                #endregion
                #region cpu&biosinformationen

                fu._logger("Start (BIOS & CPU INFORMATIONEN): " + DateTime.Now, 1);
                // CPU Informationen:
                ManagementObjectSearcher cpusearcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Processor");

                foreach (ManagementObject cpu in cpusearcher.Get())
                {
                    Object caption = cpu["Caption"];
                    Object name = cpu["Name"];
                    Object currentclockspeed = cpu["CurrentClockSpeed"];
                    Object datawidth = cpu["DataWidth"];
                    Object L2Cache = cpu["L2CacheSize"];
                    Object L3Cache = cpu["L3CacheSize"];
                    Object deviceID = cpu["DeviceID"];
                    Object max_clock_speed = cpu["MaxClockSpeed"];
                    Object processorID = cpu["ProcessorId"];
                    Object revision = cpu["Revision"];
                    Object socket = cpu["SocketDesignation"];


                    txtbx_cpu_name.Text = name + " ( " + caption + " )";
                    lbl_cpu_currclockspeed.Content = "Currenct ClockSpeed: ";
                    lbl_cpu_datawidth.Content = "Datenbreite: ";
                    lbl_cpu_L2Cache.Content = "L2Cache: ";
                    lbl_cpu_deviceid.Content = "Device ID: ";
                    lbl_cpu_L3Cache.Content = "L3Cache: ";
                    lbl_cpu_processorID.Content = "Processor ID: ";
                    lbl_cpu_revision.Content = "Revision: ";
                    lbl_cpu_socketdesignation.Content = "Socket Designation: ";

                    //Um zu verhindern das SysSharp einen L3 Cache über 1GB anzeigt, wandeln wir das ganze in KB um.


                    txtbx_cpu_currclockspeed.Text = currentclockspeed.ToString() + " MHz";
                    txtbx_cpu_datawidth.Text = datawidth.ToString() + " KB";
                    txtbx_cpu_deviceID.Text = deviceID.ToString();
                    txtbx_cpu_L2Cache.Text = L2Cache.ToString() + " KB";
                    txtbx_cpu_L3Cache.Text = L3Cache.ToString() + " KB";
                    txtbx_cpu_processorID.Text = processorID.ToString();
                    txtbx_cpu_revision.Text = revision.ToString();
                    txtbx_cpu_socket.Text = socket.ToString();
                    
                }

               

                //Bios Imformationen:
                ManagementObjectSearcher bios_searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_BIOS");

                foreach (ManagementObject _bios in bios_searcher.Get())
                {
                    //Object code_set = _bios["CodeSet"];
                    Object descrip = _bios["Description"];
                    //Object install_date = _bios["InstallDate"];
                    Object manufactur = _bios["Manufacturer"];
                    Object releasedate = _bios["ReleaseDate"];
                    Object name = _bios["Name"];
                    Object lang = _bios["CurrentLanguage"];
                    Object status = _bios["Status"];
                    Object lang_ver = _bios["ListOfLanguages"];

                    //lbl_bios_name.Content = "BIOS Name: ";
                    lbl_bios_install.Content = "Install Datum: ";
                    lbl_bios_manufactur.Content = "Hersteller: ";
                    lbl_bios_releasedate.Content = "Veröffentlichungsdatum:";
                    lbl_bios_speech.Content = "Sprache (BIOS): ";
                    lbl_bios_codeset.Content = "Codeset: ";
                    lbl_bios_status.Content = "Status: ";
                  
                    /*if (code_set == null)
                    {
                        txtbx_bios_name.Text = "BIOS CodeSet nicht gefunden!";
                        fu._logger("\t[DEBUG] BIOS Codeset konnte nicht ausgelesen werden!", 1);
                    }
                    else
                    {
                        txtbx_bios_codeset.Text = code_set.ToString();
                    }

                    if (install_date == null)
                    {
                        txtbx_bios_installdate.Text = "Installationsdatum des Bios konnte nicht ausgelesen werden!";
                        fu._logger("\t[DEBUG] Installation Datum des BIOS konnte nicht ausgelesen werden!", 1);
                    }
                    else
                    {
                        txtbx_bios_installdate.Text = install_date.ToString();
                    }*/

                    txtbx_bios_name.Text = descrip.ToString();
                    //txtbx_bios_sprache.Text = lang.ToString();
                    txtbx_bios_manufactur.Text = manufactur.ToString();
                    txtbx_bios_release.Text = releasedate.ToString();
                    txtbx_bios_status.Text = status.ToString();

                }

                

                fu._logger("Beendet (BIOS & CPU): " + DateTime.Now, 1);

                #endregion
                #region grafikkarteninformationen

                ManagementObjectSearcher searcher =
                  new ManagementObjectSearcher("root\\CIMV2",
                  "SELECT * FROM Win32_VideoController");

                foreach (ManagementObject query in searcher.Get())
                {
                    Object name = query["Caption"];
                    Object ram = query["AdapterRAM"];
                    Object deviceID = query["DeviceID"];
                    Object driverdate = query["DriverDate"];
                    Object driverversion = query["DriverVersion"];
                    Object videoprocessor = query["VideoProcessor"];
                    Object refreshrate = query["CurrentRefreshRate"];
                    Object status = query["Status"];

                    //lbl_graka_name.Content = "Name: ";
                    lbl_graka_ram.Content = "RAM: ";
                    lbl_graka_deviceID.Content = "Deivce ID: ";

                    lbl_driverver.Content = "Video Treiber Version: ";
                    lbl_videoupdate.Content = "Treiber Datum: ";
                    lbl_videoproc.Content = "Videoprocessor: ";

                    lbl_refreshrate.Content = "Aktuelle Bildwiederholungsrate: ";
                    lbl_status.Content = "Status: ";

                    if (ram == null && videoprocessor == null && refreshrate == null)
                    {
                        txtbx_ram.Text = null;
                        txtbx_refreshrate.Text = null;
                        txtbx_videoproc.Text = null;

                        fu._logger("RAM der Grafikkarte konnte nicht ausgelesen werden!", 1);
                        fu._logger("Graphikprozessor nicht gefunden!", 1);
                        fu._logger("Die Bildwiederholungsrate konnte nicht ausgelesen werden!", 1);


                    }
                    else
                    {
                        long graka_ram = Convert.ToInt64(ram);
                        long neuerram = graka_ram / 1024 / 1024;

                        txtbx_name.Text = name.ToString();
                        txtbx_ram.Text = neuerram.ToString() + "MB";
                        txtbx_deviceID.Text = deviceID.ToString();
                        //txtbx_videoupdate.Text = driverdate.ToString();
                        //txtbx_videoproc.Text = videoprocessor.ToString();
                        //txtbx_driverver.Text = driverversion.ToString();

                        string winver = Environment.OSVersion.Version.ToString();

                        if (winver != "6.2.9200.0")
                        {
                            txtbx_refreshrate.Text = refreshrate.ToString() + "Hz";
                        }
                        else
                        {
                            txtbx_refreshrate.Width = 350;
                            txtbx_refreshrate.Text = "DEBUG: Refreshrate konnte nicht ausgelesen werden!";
                            
                        }

                        
                        txtbx_status.Text = status.ToString();





                    }

                    /*_writeashtml.WriteLine("<p style=strong> Grafikkarten Informationen</p>");
                    _writeashtml.WriteLine("<ul>");
                    _writeashtml.WriteLine("<li> Name: " + name + "</li>");
                    _writeashtml.WriteLine("<li> Grafikspeicher: " + ram + "</li>");
                    _writeashtml.WriteLine("<li> Device ID: " + deviceID + "</li>");
                    _writeashtml.WriteLine("<li> Treiberdatum: " + driverdate + "</li>");
                    _writeashtml.WriteLine("<li> Treiberversion: " + driverversion + "</li>");
                    _writeashtml.WriteLine("<li> Videoprocessor: " + videoprocessor + "</li>");
                    _writeashtml.WriteLine("<li> Refreshrate: " + refreshrate + "</li>");
                    _writeashtml.WriteLine("<li> Status: " + status + "</li>");
                    _writeashtml.WriteLine("</ul>");
                    _writeashtml.WriteLine("<!-- Grafikinformation Report Created at " + DateTime.Now + "-->");
                    _writeashtml.Flush();
                    _writeashtml.Close();*/


                }



                #endregion
                #region festplatteninfos
                
                lbl_freedisk.Content = "Freier Speicherplatz (C:)";
                ManagementObject disk = new ManagementObject("Win32_LogicalDisk.DeviceID=\"C:\"");
                disk.Get();

                txtbx_freespace.Text = disk["FreeSpace"].ToString();

                double freespacecalc = Convert.ToDouble(txtbx_freespace.Text);

                if (freespacecalc / 1024 > 1 || freespacecalc / 1024 == 1 || freespacecalc / 1024 < 1)
                {
                    freespacecalc = freespacecalc / 1024;
                    if (freespacecalc / 1024 < 1)
                    {
                        freespacecalc = freespacecalc / 1;
                        freespacecalc = Math.Round(freespacecalc, 2);
                        txtbx_freespace.Text = freespacecalc.ToString();
                        txtbx_freespace.Text += " BiByte";
                    }
                    else
                    {
                        freespacecalc = freespacecalc / 1024;
                        if (freespacecalc / 1024 < 1)
                        {
                            freespacecalc = Math.Round(freespacecalc, 2);
                            txtbx_freespace.Text = freespacecalc.ToString();
                            txtbx_freespace.Text += " KiBiByte";
                        }
                        else
                        {
                            freespacecalc = freespacecalc / 1024;
                            if (freespacecalc / 1024 < 1)
                            {
                                freespacecalc = Math.Round(freespacecalc, 2);
                                txtbx_freespace.Text = freespacecalc.ToString();
                                txtbx_freespace.Text += " GiBiByte";
                            }
                            else
                            {
                                freespacecalc = freespacecalc / 1024;
                                if (freespacecalc / 1024 < 1 || freespacecalc / 1024 == 1 || freespacecalc / 1024 > 1)
                                {
                                    freespacecalc = Math.Round(freespacecalc, 2);
                                    txtbx_freespace.Text = freespacecalc.ToString();
                                    txtbx_freespace.Text += " TiBiByte";
                                }
                            }
                        }
                    }
                }

                ManagementObjectSearcher dsk_searcher =
                   new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT * FROM Win32_DiskDrive"); 

                foreach (ManagementObject dsk in dsk_searcher.Get())
                {
                    Object caption = dsk["Caption"];
                    Object dsk_dvID = dsk["DeviceID"];
                    Object dsk_firmware = dsk["FirmwareRevision"];
                    Object dsk_manufactor = dsk["Manufacturer"];
                    Object dsk_blocksize = dsk["MinBlockSize"];
                    Object dsk_model = dsk["Model"];
                    //Object dsk_partitions = disk["Partitions"];
                    //Object dsk_sn = dsk["SerialNumber"];
                    Object dsk_size = dsk["Size"];
                    Object dsk_total_cylinders = dsk["TotalCylinders"];
                    Object dsk_total_header = dsk["TotalHeads"];
                    Object dsk_total_sections = dsk["TotalSectors"];
                    Object dsk_total_tracks = dsk["TotalTracks"];

                    string cpt = caption.ToString();
                    string dvID = dsk_dvID.ToString();
                    string firmware = dsk_firmware.ToString();
                    string manufatur = dsk_manufactor.ToString();
                    string modell = dsk_model.ToString();
                    //byte part = Convert.ToByte(dsk_partitions);
                    //long sn = Convert.ToInt64(dsk_sn);
                    long size = Convert.ToInt64(dsk_size);
                    long cylinders = Convert.ToInt64(dsk_total_cylinders);
                    byte heads = Convert.ToByte(dsk_total_header);
                    long sectors = Convert.ToInt64(dsk_total_sections);
                    long tracks = Convert.ToInt64(dsk_total_tracks);

                    lstbx_hdds.Items.Add("Name: " + cpt +"\n" + "DeviceID: " + dvID +"\n" + "Firmware: " + firmware + "\n" + "Hersteller: " + manufatur + "\n" 
                                        + "Modell: " + modell + "\n" + "Größe: " + size + "\n" 
                                        + "Cylinders: " + cylinders + "\n" + "Heads: " + heads + "\n" + "Sectoren: " + sectors + "\n" + "Tracks: " + tracks + "\r\n");

                }
                
                #endregion
                #region netzwerk
                
                lbl_adapter.Content = "Verfügbare Adapter";

                ManagementObjectSearcher network_searcher =
                new ManagementObjectSearcher("root\\CIMV2",
                "SELECT * FROM Win32_NetworkAdapterConfiguration");

                foreach (ManagementObject query in network_searcher.Get())
                {
                    if (query["IPAddress"] == null)
                    {
                        lst_bx_adaptername.Items.Add(query["IPAddress"]);
                    }
                    else
                    {

                        String[] arrIP = (String[])query["IPAddress"];
                        String[] subnet = (String[])query["IPSubnet"];

                        lst_bx_adaptername.Items.Add(query["Caption"]);

                        foreach (string arrValue in arrIP)
                        {
                            lst_bx_adaptername.Items.Add("IP Adresse: " + arrValue);
                            foreach (String arrValuesub in subnet)
                            {
                                lst_bx_adaptername.Items.Add("Subnetmask: " + arrValuesub);
                            }
                            lst_bx_adaptername.Items.Add("Default TTL: " + query["DefaultTTL"]);
                            lst_bx_adaptername.Items.Add("MAC Adress: " + query["MACAddress"]);
                            lst_bx_adaptername.Items.Add("DNS Hostname:  " + (query["DNSHostName"]));
                            lst_bx_adaptername.Items.Add("DHCP Server: " + query["DHCPServer"]);
                            lst_bx_adaptername.Items.Add("                      ");

                        }
                    }


                
                }

               
                #endregion
                #region ram
                //fu._logger("Start (RAM): " + DateTime.Now, 1);


                ManagementObjectSearcher ramsearcher =
                  new ManagementObjectSearcher("root\\CIMV2",
                  "SELECT * FROM Win32_PhysicalMemory");

                
                foreach (ManagementObject ram in ramsearcher.Get())
                {
                    Object name = ram["Tag"];
                    Object capacity = ram["Capacity"];
                    Object data_width = ram["DataWidth"];
                    Object bank_name = ram["BankLabel"];
                    Object speed = ram["Speed"];
                    Object manufactur = ram["Manufacturer"];
                    Object serialnummer = ram["SerialNumber"];
                    Object typedetail = ram["TypeDetail"];
                    

                    if (capacity == null)
                    {
                        funk._logger("\t[DEBUG]\tArbeitsspeicher Größe konnte nicht ausgelesen werden!", 2);

                        //Tja dann brauchen wir eine veränderte ausgabe...

                    }

                    long cap = Convert.ToInt64(capacity);
                    long new_cap = cap / 1024 / 1024 / 1024;

                    long outtypedetail = Convert.ToInt64(typedetail);

                    switch (outtypedetail)
	                {
                        case 1:
                            fu._logger("OutofTypeDetail ist 1, RAM ist reserviert..",1);
                            lstbx_ram.Items.Add("| " + name + " | " + new_cap + " | " + data_width + " | " + bank_name + " | " + speed + " | " + manufactur + " | " + serialnummer +
                                        " | " + typedetail + "(RESERVERD)");
                            break;
                        case 2:
                            fu._logger("OutofTypeDetail ist 2, RAM ist OTHER..", 1);
                            lstbx_ram.Items.Add("| " + name + " | " + new_cap + " | " + data_width + " | " + bank_name + " | " + speed + " | " + manufactur + " | " + serialnummer +
                                        " | " + typedetail + "(OHTER)");
                            break;
                        case 4:
                            fu._logger("OutofTypeDetail ist 4, RAM ist UNKNOW..", 1);
                            lstbx_ram.Items.Add("| " + name + " | " + new_cap + " | " + data_width + " | " + bank_name + " | " + speed + " | " + manufactur + " | " + serialnummer +
                                        " | " + typedetail + "(UNKNOW)");
                            break;
                        case 8:
                            fu._logger("OutofTypeDetail ist 8, RAM ist PAGED..", 1);
                            lstbx_ram.Items.Add("| " + name + " | " + new_cap + " | " + data_width + " | " + bank_name + " | " + speed + " | " + manufactur + " | " + serialnummer +
                                        " | " + typedetail + "(FAST - PAGED)");
                            break;
                        case 16:
                            fu._logger("OutofTypeDetail ist 16, RAM ist STATIC COLUMN..", 1);
                            lstbx_ram.Items.Add("| " + name + " | " + new_cap + " | " + data_width + " | " + bank_name + " | " + speed + " | " + manufactur + " | " + serialnummer +
                                        " | " + typedetail + "(STATIC COLUMN)");
                            break;
                        case 32:
                            fu._logger("OutofTypeDetail ist 32, RAM ist PSEUDO STATIC..", 1);
                            lstbx_ram.Items.Add("| " + name + " | " + new_cap + " | " + data_width + " | " + bank_name + " | " + speed + " | " + manufactur + " | " + serialnummer +
                                        " | " + typedetail + "(PSEUDO STATIC)");
                            break;
                        case 64:
                            fu._logger("OutofTypeDetail ist 64, RAM ist RAMBUS..", 1);
                            lstbx_ram.Items.Add("| " + name + " | " + new_cap + " | " + data_width + " | " + bank_name + " | " + speed + " | " + manufactur + " | " + serialnummer +
                                        " | " + typedetail + "(RAMBUS)");
                            break;
                        case 128:
                            fu._logger("OutofTypeDetail ist 128, RAM ist SYNCHRONUS..", 1);
                            lstbx_ram.Items.Add("| " + name + " | " + new_cap + " | " + data_width + " | " + bank_name + " | " + speed + " | " + manufactur + " | " + serialnummer +
                                        " | " + typedetail + "(SYNCHRONUS)");
                            break;
                        case 256:
                            fu._logger("OutofTypeDetail ist 256, RAM ist CMOS..", 1);
                            lstbx_ram.Items.Add("| " + name + " | " + new_cap + " | " + data_width + " | " + bank_name + " | " + speed + " | " + manufactur + " | " + serialnummer +
                                        " | " + typedetail + "(CMOS)");
                            break;
                        case 512:
                            fu._logger("OutofTypeDetail ist 512, RAM ist EDO..", 1);
                            lstbx_ram.Items.Add("| " + name + " | " + new_cap + " | " + data_width + " | " + bank_name + " | " + speed + " | " + manufactur + " | " + serialnummer +
                                        " | " + typedetail + "(EDO)");
                            break;
                        case 1024:
                            fu._logger("OutofTypeDetail ist 1024, RAM ist WINDOWS DRAM..", 1);
                            lstbx_ram.Items.Add("| " + name + " | " + new_cap + " | " + data_width + " | " + bank_name + " | " + speed + " | " + manufactur + " | " + serialnummer +
                                        " | " + typedetail + "(WINDOWS DRAM)");
                            break;
                        case 2048:
                            fu._logger("OutofTypeDetail ist 2048, RAM ist CACHE DRAM..", 1);
                            lstbx_ram.Items.Add("| " + name + " | " + new_cap + " | " + data_width + " | " + bank_name + " | " + speed + " | " + manufactur + " | " + serialnummer +
                                        " | " + typedetail + "(CACHE DRAM)");
                            break;
                        case 4096:
                            fu._logger("OutofTypeDetail ist 4096, RAM ist NOVOLATILE..", 1);
                            lstbx_ram.Items.Add("| " + name + " | " + new_cap + " | " + data_width + " | " + bank_name + " | " + speed + " | " + manufactur + " | " + serialnummer +
                                        " | " + typedetail + "(NONVOLATILE)");
                            break;
		                default:
                            fu._logger("TypeDetail konnte nicht ausgelesen werden...", 2);
                            lstbx_ram.Items.Add("| " + name + " | " + new_cap + " | " + data_width + " | " + bank_name + " | " + speed + " | " + manufactur + " | " + serialnummer);
                        break;
	                }

                    


                }
                #endregion
                #region peripherietab
                ManagementObjectSearcher search_pr =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Printer");

                lbl_drucker.Content = "Verfügbare Drucker: ";
                foreach (ManagementObject queryObj in search_pr.Get())
                {
                    lstbx_drucker.IsEnabled = false;
                    lstbx_drucker.Items.Add(queryObj["Caption"]);

                }

                #endregion
                #region keyboard

                

                lbl_keyboard.Content = "Tastaturen: ";
                lstbx_keyboard.IsEnabled = false;
                ManagementObjectSearcher search_key =
                 new ManagementObjectSearcher("root\\CIMV2",
                 "SELECT * FROM Win32_Keyboard");

                foreach (ManagementObject queryObj in search_key.Get())
                {
                    lstbx_keyboard.Items.Add(queryObj["Caption"]);

                }

                
                #endregion
                #region cdrom
                
                lbl_CDROM.Content = "Installierte Laufwerke: ";
                lst_bx_cdrom.IsEnabled = false;
                ManagementObjectSearcher search_cd =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_CDROMDrive");

                foreach (ManagementObject queryObj in search_cd.Get())
                {
                    lst_bx_cdrom.Items.Add(queryObj["Caption"]);
                    lst_bx_cdrom.Items.Add(queryObj["DeviceID"]);


                }
                
                #endregion
                #region oeffnenvonappdata
                //Process.Start("explorer.exe", appdata + @"\SysSharp\");
                #endregion
                #region winsat
                /*
                lbl_win_sat.Content = "WIN SAT Ergebnisse:";

                ManagementObjectSearcher search_sat =
                   new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT * FROM Win32_WinSAT");

                foreach (ManagementObject _sat in search_sat.Get())
                {
                    Object cpuscore = _sat["CPUScore"];
                    Object directxscore = _sat["D3DScore"];
                    Object diskscore = _sat["DiskScore"];
                    Object ramscore = _sat["MemoryScore"];
                    Object grakascore = _sat["GraphicsScore"];

                    lbl_sat_cpu.Content = "CPU Score:\t" + cpuscore.ToString();
                    lbl_sat_d3.Content = "D3D Score:\t" + directxscore.ToString();
                    lbl_sat_dsksc.Content = "Diskscore:\t " + diskscore.ToString();
                    lbl_sat_ram.Content = "RAM Score:\t" + ramscore.ToString();
                    lbl_sat_grafik.Content = "Grafikkarte Score:\t" + grakascore.ToString();

                }
                */
                #endregion
                #region timezone
                ManagementObjectSearcher search_time =
                  new ManagementObjectSearcher("root\\CIMV2",
                  "SELECT * FROM Win32_TimeZone");

                foreach (ManagementObject _time in search_time.Get())
                {
                    Object current_time_zone = _time["Caption"];
                    //lbl_timezone_current.Content = current_time_zone;

                }

                #endregion
                #region version
                fu.version();
                string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                lbl_ver.Content = version;
                #endregion
                #region verfuegbare_treiber
                //Liest alle verfügbare Treiber aus.. Kann eine Zeit dauern bis das fertig ist :)
                ManagementObjectSearcher drv_searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_SystemDriver");

                foreach (ManagementObject treiber in drv_searcher.Get())
                {
                    Object treiber_name = treiber["Caption"];
                    //Object treiber_install_date = treiber["InstallDate"];
                    Object service_type = treiber["ServiceType"];
                    Object started = treiber["Started"];
                    Object state = treiber["State"];
                    Object status = treiber["Status"];
                    Object tagID = treiber["TagId"];

                    //bool outstarted;
                    //int out_tagID;

                    string treibername = treiber_name.ToString();
                    //DateTime installdate = DateTime.Parse(treiber_install_date.ToString());
                    /*string drv_service_type = service_type.ToString();
                    bool started_drv = bool.TryParse(started.ToString(), out outstarted);
                    string drv_state = state.ToString();
                    string drv_status = status.ToString();
                    int drv_tagID = Convert.ToInt32(tagID);*/

                    lstbx_treiber.Items.Add(treibername);

                    
                }

                #endregion
               

                fun._logger("App Done at: " + DateTime.Now, 2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fehler beim Initialisieren der Anwendung!");
                MessageBox.Show(ex.StackTrace, "Folgende Informationen sind wichtig!");
                klassen.funktionen funkt = new klassen.funktionen();
                funkt.logger(ex);

                Environment.Exit(0);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            try
            {
                Process.Start(appdata + @"\SysSharp\extend\SysSharp_Con.exe");
            }
            catch (Exception ex)
            {
                klassen.funktionen fun = new klassen.funktionen();
                fun.logger(ex);
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            //int os_version = Environment.OSVersion.Version.Major;
            string os_version = Environment.OSVersion.VersionString;

            if (os_version != "6")
            {
                funk._logger("\t[DEBUG]\t Gefundenes OS: " + os_version, 1);
                MessageBox.Show("Die Leistungsüberwachung ist nur auf Windows Vista oder höher verfügbar!");

                //Ereignisbutton ausschalten:
                btn_mmc_dienste.IsEnabled = false;
                btn_mmc_ereignis.IsEnabled = false;
                btn_mmc_fw.IsEnabled = false;
                btn_mmc_leistung.IsEnabled = false;
            }
            else
            {
                try
                {
                    //Alles okay, wir haben Vista oder Win7 oder Win8...
                    funk._logger("Starte nun die MMC mit der Leistungsüberwachung!", 1);

                    //Um der MMC die Leistungsüberwachung anzudrohen brauchen wir folgenden Befehl:
                    // %windir%\system32\perfmon.msc /s

                    //Daher müssen wir erstmal wieder herausfinden, wo %WINDIR% überhaupt ist...


                    //dann setzen wir das ganze mal zusammen:
                    //string start = win_dir + @"\system32\perfmon.msc /s";

                    //und dann... übergeben wir dies dem Process.Start:
                    string start = win_dir + @"\System32\perfmon.msc";
                    Process.Start(start);
                }
                catch (Exception ex)
                {
                    funk.logger(ex);
                }
            }

            
            
        }

        private void btn_mmc_fw_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string start = win_dir + @"\System32\WF.msc";
                Process.Start(start);
            }
            catch (Exception ex)
            {
                funk.logger(ex);
            }
        }

        private void btn_mmc_dienste_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string start = win_dir + @"\System32\services.msc";
                Process.Start(start);
            }
            catch (Exception ex)
            {
                funk.logger(ex);
            }
        }

        private void btn_mmc_ereignis_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string start = win_dir + @"\System32\eventvwr.msc";
                Process.Start(start);
            }
            catch (Exception ex)
            {
                funk.logger(ex);
            }
        }


    }
}
