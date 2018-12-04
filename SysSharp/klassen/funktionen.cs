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
using System.Reflection;

using System.Net;
using System.Diagnostics;
using System.Xml;

using System.Security.Cryptography;
using System.Collections;


namespace SysSharp.klassen
{
    class funktionen
    {
        string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string temp = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        Process syssharp = Process.GetCurrentProcess();

        public void syssharp_init()
        {
            try
            {
                //IST DIE ERSTE METHODE DIE AUFGERUFEN WERDEN MUSS!!!
                if (!Directory.Exists(appdata + @"\SysSharp\"))
                {
                    //_________________SysSharp INIT________________________________
                    Directory.CreateDirectory(appdata + @"\SysSharp\");
                    Directory.CreateDirectory(appdata + @"\SysSharp\Logs\");
                    _logger("Hauptordner und Log Ordner sind erstellt...", 1);
                    //______________________________________________________________


                    //_________________SysSharp Updates____________________________
                    _logger("Erstelle nun Update Ordner...", 1);
                    Directory.CreateDirectory(appdata + @"\SysSharp\updates\");
                    Directory.CreateDirectory(appdata + @"\SysSharp\updates\backup\");
                    //_______________________________________________________________

                    //_______________SysSharp Zusatzprogramme_______________________
                    _logger("Erstelle Zusatzprogramme!", 1);
                    Directory.CreateDirectory(appdata + @"\SysSharp\Plugins\");
                    Directory.CreateDirectory(appdata + @"\SysSharp\Plugins\outdated\");
                    //_____________________________________________________________




                }
                else
                {
                    _logger("SysSharp Ordner vorhanden! Aborting...", 1);
                }

                if (!File.Exists(appdata + @"\SysSharp\Logs\currdir.txt"))
                {

                    FileInfo fi = new FileInfo(Assembly.GetEntryAssembly().Location);
                    _logger("Akutelles WorkingDirectory: " + fi.Directory, 1);

                    StreamWriter _curr_dir = new StreamWriter(appdata + @"\SysSharp\Logs\currdir.txt", true, ASCIIEncoding.UTF8, 12);
                    _curr_dir.WriteLine(fi.Directory);
                    _curr_dir.Flush();
                    _curr_dir.Close();
                }
                else
                {
                    _logger("curr_dir ist gesetzt!", 1);
                }
                
            }
            catch (Exception ex)
            {
                logger(ex);
            }
        }


        public void logger(Exception ex)
        {
            //-- Dies ist der Standart Logger:
            //Kann und wird verwendet wenn es keine spezielle Exception gibt!

            if (!Directory.Exists(appdata + @"\SysSharp\"))
            {
                Directory.CreateDirectory(temp + @"\SysSharp\"); 

                //-- Danach erstelle eine Log Datei:
                StreamWriter _logger = new StreamWriter(temp + @"\SysSharp\system.log", true, ASCIIEncoding.UTF8, 100);
                _logger.WriteLine(DateTime.Now + "\t" + @"| INFO | Verzeichnis SysSharp unter %APPDATA% erstellt!");
                _logger.WriteLine(DateTime.Now + "\t" + @"| ERROR | Eine Exception wurde geworfen: ");
                _logger.WriteLine(DateTime.Now + "\t" + @"| ERROR | Exception: {0} ", ex.Message);
                _logger.WriteLine(DateTime.Now + "\t" + @"| ERROR | STACK TRACE: {0} ", ex.StackTrace);
                _logger.WriteLine(DateTime.Now + "\t" + @"| ERROR | Inner Exception: {0} ", ex.InnerException);
                _logger.WriteLine("\r\n");
                _logger.Flush();
                _logger.Close();

                ProcessStartInfo _notepad = new ProcessStartInfo("notepad.exe", temp + @"\SysSharp\system.log");
                Process.Start(_notepad);
                

            }
            else
            {
                //-- Okay, der Ordner "SysSharp" ist unter %APPDATA% erstellt worden.. nun können wir ja auch so loggen ;)
                // Der Ordner SysSharp wird eigentl. schon im Hauptprogramm erstellt, nur kann es sein das durch
                // fehlende Berechtigungen dieser Ordner nicht erstellt werden konnte, und wir wollen ja nicht
                // das unser eigenes Fehlerprotokoll einen Fehler verursacht (wäre aber ein cooles
                // Paradoxum :>)

                /*FileInfo _log = new FileInfo(appdata + @"\SysSharp\system.log");

                if (_log.Length == 1000)
                {
                    File.Delete(appdata + @"\SysSharp\system.log");
                }*/

                StreamWriter _logger = new StreamWriter(appdata + @"\SysSharp\Logs\system.log", true, ASCIIEncoding.UTF8, 12);
                _logger.WriteLine(DateTime.Now + "\t" +  @"| ERROR | Eine Exception wurde geworfen: ");
                _logger.WriteLine(DateTime.Now + "\t" +  @"| ERROR | Exception: {0} ", ex.Message);
                _logger.WriteLine(DateTime.Now + "\t" +  @"| ERROR | STACK TRACE: {0} ", ex.StackTrace);
                _logger.WriteLine(DateTime.Now + "\t" +  @"| ERROR | Inner Exception: {0} ", ex.InnerException);
                _logger.WriteLine("                                                             ");
                _logger.Flush();
                _logger.Close();
            }

            //EventLog.CreateEventSource(ex.Message, "SysSharp");
        }
        public void _logger(string Message, int Ereigniszahl)
        {
            /*FileInfo _log = new FileInfo(appdata + @"\SysSharp\logger.log");

            if (_log.Length == 1000)
            {
                File.Delete(appdata + @"\SysSharp\logger.log");
            }*/

            if (Message == string.Empty && Ereigniszahl < 0)
            {
                MessageBox.Show("DEBUG: Keine Meldung und keine Ereignisszahl gesetzt!");
            }
            else if (Ereigniszahl < 0 || Ereigniszahl == 3)
            {
                MessageBox.Show("Der Normale Logger darf entweder keine .NET Exception enthalten oder aber auch nicht Null sein!");
            }

            //Umprogrammierung 16.10:
            //Absofort werde im Logger keine Zahlen mehr verwendet, außer halt im Quellcode (da sollte sich der Programmierer dann ja auch kennen welche Zahl für was steht ;))
            if (Ereigniszahl == 1)
            {
                StreamWriter ownlogger = new StreamWriter(appdata + @"\SysSharp\Logs\logger.log", true, ASCIIEncoding.UTF8, 12);
                ownlogger.WriteLine(DateTime.Now +"\t"  + "| INFO | \t" + Message);
                ownlogger.Flush();
                ownlogger.Close();
            }
            else if (Ereigniszahl == 2)
            {
                StreamWriter ownlogger = new StreamWriter(appdata + @"\SysSharp\Logs\logger.log", true, ASCIIEncoding.UTF8, 12);
                ownlogger.WriteLine(DateTime.Now + "\t" + "| MSG  | \t" + Message);
                ownlogger.Flush();
                ownlogger.Close();
            }
            
        }
        
        public void ramauslastung()
        {
            _logger("Start (RAMAUSLASTUNG): " + DateTime.Now, 1);

            try
            {
                //Mit dieser Methode wollen wir herausfinden wieviel RAM unser Programm benötigt:
                //Dazu müssen wir erst einmal herausfinden welche PID unser Programm hat:
                

                int syssharp_pid = syssharp.SessionId;
                
                long verbrauchterram = syssharp.WorkingSet64 /1024 /1024;
                TimeSpan proctime = syssharp.TotalProcessorTime;

                _logger("\t|DEBUG|\t" + "Der Prozess hat nun die PID: " + syssharp_pid, 1);
                _logger("\t|DEBUG|\t" + "Belegter Speicherplatz " + verbrauchterram + "MB", 1);
                _logger("\t|DEBUG|\t" + "Prozessor Zeit: " + proctime, 1);
                _logger("\t[DEBUG]\t" + "StartTime: " + syssharp.StartTime, 1);
                _logger("\t[DEBUG]\t" + "Start Ordner: " + syssharp.StartInfo.WorkingDirectory, 1);
                _logger("\t[DEBUG]\t" + "Programmname: " + syssharp.ProcessName, 1);
                _logger("\t[DEBUG]\t" + "Windows Style: " + syssharp.StartInfo.WindowStyle, 1);
                
            }
            catch (Exception ex)
            {
                logger(ex);
            }

            _logger("Beendet (RAMAUSLASTUNG): " + DateTime.Now, 1);
        }


        public void saveinfoasxml(Object daten1, Object daten2, Object daten3)
        {
            XmlTextWriter _xml = new XmlTextWriter(appdata + @"\SysSharp\infos.xml", Encoding.UTF8);
        }

        public void version()
        {
            _logger("Start (VERSION): " + DateTime.Now, 1);

            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //Schreibt eine Version.txt Datei ins %APPDaTA% Verzeichnis:
            StreamWriter _version = new StreamWriter(appdata + @"\SysSharp\version.txt", true, ASCIIEncoding.UTF8, 12);
            _version.Write(version);
            _version.Flush();
            _version.Close();
            _logger("\t[DEBUG]\t" + "Derzeitige SysSharp Version: " + version, 1);

            _logger("Beendet (VERSION: " + DateTime.Now, 1);
        }

        public void current_dir()
        {
            _logger("Start (CURR_DIR): " + DateTime.Now, 1);

            if (!File.Exists(appdata + @"\SysSharp\update_dir.txt"))
            {
                string current_dir = Environment.CurrentDirectory;
                StreamWriter reader = new StreamWriter(appdata + @"\SysSharp\update_dir.txt", true, ASCIIEncoding.UTF8, 12);
                reader.Write(current_dir);
                reader.Flush();
                reader.Close();

                _logger("Derzeitiger Arbeitsordner: " + current_dir, 1);
            }
            else
            {
                _logger("[DEBUG] Pfadangabe komplett!", 1);
            }

            _logger("Beendet (CURR_DIR): " + DateTime.Now, 1);
            
        }

        public void write_setting()
        {
            /*
             *  Diese Methode schreibt eine Settingsdatei in den %APPDATA% Ordner..
             *  
             *  Einstellbare Einstellungen:
             *          1) write_hardware_to_disk | 0 oder 1
             *              Schreibt alle Hardwarespezifikationen in eine Datei die man unter %APPDATA% abrufen kann
             *          2) update_server | <url>
             *              Mit dieser Einstellung kann man den Update Server wechseln, standartmäßig holt sich
             *              SysSharp das Update über den Sourceforge Server
             *          3) debug 0 | 1
             *             
             * 
             */

            if (File.Exists(appdata + @"\SysSharp\settings.settings"))
            {
                bool write_hardware_to_disk = false;
                string update_server = " ";
                bool debug = false;

                //Auslesen der Settingsdatei:
                StreamReader _settings = new StreamReader(appdata + @"\SysSharp\settings.settings");
                string inhalt = _settings.ReadToEnd();

            }
            else
            {

                StreamWriter _settings = new StreamWriter(appdata + @"\SysSharp\settings.settings", true, ASCIIEncoding.UTF8, 12);
                _settings.Write("_write_hardware_to_disk = 0");
                _settings.Write("\r\n");
                _settings.Write("update_server = ");
                _settings.Write("\r\n");
                _settings.Write("debug = 0");
                _settings.Write("\r\n");
                _settings.Flush();
                _settings.Close();
            }
        }

        public void lies_settings()
        {
            /*
             * 
             * Diese Methode liest die Settings Datei unter %APPDATA% aus..
             * Da diese Methode sehr früh eigentl. schon gelöst wird, müssen wir hier überprüfen ob
             * der Ordner SysSharp schon angelegt worden ist!
             * 
             * Sobald die Einstellungen ausgelesen sind (was warscheinlichst dank der Lahmigkeit von .NET [Ja, lahmigkeit] eh etwas dauer dürft)
             * werden diese in Variablen geschrieben und ausgewertet..
             * 
             * Gültige Werte sind neben 0 | 1 eigentl. derweil keine anderen. Sollten andere Werte als 0 | 1 gegeben sein, werden die Einstellungen
             * einfach ignoriert!
             * 
             * (Wird echt Zeit mal ein Initalisierungsprogramm zu schreiben...)
             * 
             */

            try
            {
                //Dir ist nicht vorhanden!
                if (!Directory.Exists(appdata + @"\SysSharp\"))
                {
                    MessageBox.Show("Konnte keinen SysSharp Ordner finden! Bitte legen Sie diesen manuell an oder starten Sie SysSharp mit Adminrechten!");
                }
                // Dir ist vorhanden aber die Datei nicht!
                else if (!File.Exists(appdata + @"\SysSharp\settings.settings"))
                {
                    MessageBox.Show("Konnte keine Settings Datei lesen!");
                    _logger("Fehlende Settings Datei! Bitte starten Sie SysSharp mit Administrationrechten!", 1);
                             
                }
                //Dir ist vorhanden UND die Settingsdatei ist da :3
                else
                {
                    //Zuallerst lesen wir mal den kompletten inhalt ein, sollte dieser NULL sein, scheint iwas bei der Erstellung kaputt gegangen zu sein :)
                    string inhalt = " ";
                    StreamReader _settings = new StreamReader(appdata + @"\SysSharp\settings.settings");
                    inhalt = _settings.ReadToEnd();

                    if (inhalt == string.Empty)
                    {
                        MessageBox.Show("Settings Datei ist korrupt!");
                        _logger("Settings Datei hat einen ungültigen Wert, nämlich NULL. Führen Sie SysSharp einfach im Adminmodus aus!", 2);
                    }
                    else
                    {
                       /*/
                        * 
                        * 
                        * Na endlich.. wir haben es geschafft, die SettingsDatei ist nicht NULL oder null oder sonst iwas... dann können wir ja auslesen :)
                        * Zuerst speichern wir alles in eine ArrayList, also den inhalt mit einer FOREACH Schleife durch gehen den aktuellen inhalt in
                        * das Array speichern und dann wieder von vorne.
                        * 
                        * 
                        */

                        ArrayList __settings = new ArrayList(20);
                        string line = " ";

                        while ((line = _settings.ReadLine()) != null)
                        {
                            __settings.Add(line);
                            _logger("Einstellung: " + line, 1);
                            
                        }
                       
                        
                    }
                }
            }
            catch (Exception ex)
            {
                logger(ex);
            }

        }

       
    
    }
}
